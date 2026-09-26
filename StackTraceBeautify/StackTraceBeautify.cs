/*!
 * .NET Port of tje JavaScript library for highlighting .NET stack traces
 * License : Apache 2
 * Author : https://elmah.io
 * Url: https://github.com/elmahio/netStack.js
 *
 */

namespace StackTraceBeautify;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// The stack trace beautify.
/// </summary>
public class StackTraceBeautify
{
    /// <summary>
    /// Matches a .NET stack frame line, independent of the language of the stack trace:
    /// "{at} Type.Method(params)", optionally followed by the file and line information in one of the
    /// formats used by the .NET translations (see mscorlib "Word_At" and "StackTrace_InFileLineNumber"):
    /// " {in} file:{line} number" (most languages, German adds a trailing dot),
    /// " {in} file, {line} number" (Hungarian: "hely: {0}, sor: {1}") or
    /// " file {in}: {line} number" (Turkish: "{0} içinde: satır {1}").
    /// </summary>
    private static readonly Regex DotNetFrameRegex = new(
        @"^\s*(?<at>\S+|\S+(?:\s+\S+){1,2}:)\s+"
        + @"(?<typeMethod>[^\s()]*\.[^\s()]+)\((?<params>[^()]*)\)"
        + @"(?:\s+[^\s\\/.]+\s+(?<file>.+?)[:,]\s*(?<line>[^\s:,]+:?\s+\d+)\.?"
        + @"|\s+(?<file>.+?)\s+[^\s\\/.]+:\s*(?<line>[^\s:,]+\s+\d+)\.?)?\s*$",
        RegexOptions.Compiled);

    /// <summary>
    /// Matches a Java (JVM) stack frame line: "at [module/]Type.method(File.java:12)", where the location can also be
    /// "Native Method", "Unknown Source" or "SourceFile:12" (Android), optionally followed by logback's "~[app.jar:1.0]".
    /// </summary>
    private static readonly Regex JavaFrameRegex = new(
        @"^\s*at\s+(?:[^\s()/]*/){0,2}"
        + @"(?<typeMethod>[^\s()/]+\.[^\s()/]+)"
        + @"\((?:(?<file>[^\s():]+\.(?:java|kt|kts|scala|groovy|clj|cljc)|SourceFile)(?::(?<line>\d+))?"
        + @"|Native Method|Unknown Source(?::(?<line>\d+))?)\)"
        + @"(?:\s+~?\[[^\]]*\])?\s*$",
        RegexOptions.Compiled);

    /// <summary>
    /// Matches a V8 (Chrome, Edge, Node.js) stack frame line: "at [new |async ]Type.method [as alias] (file.js:12:5)",
    /// "at Type.method (native)" or "at file.js:12:5" (anonymous function).
    /// </summary>
    private static readonly Regex V8FrameRegex = new(
        @"^\s*at\s+(?:(?:new|async)\s+)?"
        + @"(?:(?<typeMethod>[^\s()]+(?:\s\[as\s[^\s\]]+\])?)\s+"
        + @"\((?:(?<file>[^()]+?):(?<line>\d+)(?::(?<column>\d+))?|native|&lt;anonymous&gt;|index\s\d+)\)"
        + @"|(?<file>[^\s()]+?):(?<line>\d+)(?::(?<column>\d+))?)\s*$",
        RegexOptions.Compiled);

    /// <summary>
    /// Matches a Firefox or Safari stack frame line: "Type.method@file.js:12:5", "@file.js:12:5" (anonymous function),
    /// "global code@file.js:12:5" or "method@[native code]".
    /// </summary>
    private static readonly Regex GeckoFrameRegex = new(
        @"^\s*(?<typeMethod>[^@\s]*|(?:global|module|eval)\scode)@"
        + @"(?:(?<file>.+?):(?<line>\d+):(?<column>\d+)|\[native code\])\s*$",
        RegexOptions.Compiled);

    /// <summary>
    /// Matches a Python stack frame line: File "file.py", line 12, in function
    /// (the source code line that follows is left unchanged).
    /// </summary>
    private static readonly Regex PythonFrameRegex = new(
        @"^\s*File\s+""(?<file>[^""]+)"",\s+line\s+(?<line>\d+)(?:,\s+in\s+(?<typeMethod>\S+))?\s*$",
        RegexOptions.Compiled);

    /// <summary>
    /// Matches a PHP stack frame line: "#0 file.php(12): Type->method(args)", "#0 file.php(12): Type::method(args)"
    /// or "#0 [internal function]: function(args)".
    /// </summary>
    private static readonly Regex PhpFrameRegex = new(
        @"^\s*#\d+\s+(?:(?<file>[^()]+?)\((?<line>\d+)\)|\[internal function\]):\s+"
        + @"(?<typeMethod>(?:[^\s(){}]|\{[^{}]*\})+)\((?<args>.*)\)\s*$",
        RegexOptions.Compiled);

    /// <summary>
    /// Matches a Ruby stack frame line: "[from ]file.rb:12:in `method'" (Ruby up to 3.3) or
    /// "[from ]file.rb:12:in 'Type#method'" (Ruby 3.4+), optionally followed by the exception message.
    /// </summary>
    private static readonly Regex RubyFrameRegex = new(
        @"^\s*(?:from\s+)?(?<file>\S.*?):(?<line>\d+):in\s+[`']"
        + @"(?:(?:block|rescue|ensure)(?:\s\(\d+\slevels\))?\sin\s)?(?<typeMethod>[^`'\s]+)'",
        RegexOptions.Compiled);

    /// <summary>
    /// Matches the function line of a Go stack frame: "pkg.function(args)", "pkg.(*Type).Method(args)",
    /// "panic(args)" or "created by pkg.function[ in goroutine 1]".
    /// </summary>
    private static readonly Regex GoFunctionRegex = new(
        @"^(?:(?<typeMethod>panic|(?=[^\s(]*\.)(?:[^\s()]|\(\*?[^\s()]+\))+)\((?<args>[^()]*)\)"
        + @"|created\sby\s(?<typeMethod>\S+)(?:\sin\sgoroutine\s\d+)?)\s*$",
        RegexOptions.Compiled);

    /// <summary>
    /// Matches the location line of a Go stack frame (follows the function line): "	/path/file.go:12 +0x1d".
    /// </summary>
    private static readonly Regex GoLocationRegex = new(
        @"^\s+(?<file>\S+\.(?:go|s)):(?<line>\d+)(?:\s+(?:\+0x[0-9a-f]+|\w+=0x[0-9a-f]+))*\s*$",
        RegexOptions.Compiled);

    /// <summary>
    /// The frame patterns and type/method separators of each runtime, in detection order.
    /// Java comes before .NET, because Java frames also have the structure of a .NET frame (the location
    /// would be taken as the parameter list). Go comes last, because its function lines are the least specific.
    /// </summary>
    private static readonly (StackTraceRuntime Runtime, string[] Separators, Regex[] Patterns)[] Parsers =
    [
        (StackTraceRuntime.Java, ["."], [JavaFrameRegex]),
        (StackTraceRuntime.JavaScript, ["."], [V8FrameRegex, GeckoFrameRegex]),
        (StackTraceRuntime.Python, ["."], [PythonFrameRegex]),
        (StackTraceRuntime.Php, ["-&gt;", "::"], [PhpFrameRegex]),
        (StackTraceRuntime.Ruby, ["#", "."], [RubyFrameRegex]),
        (StackTraceRuntime.DotNet, ["."], [DotNetFrameRegex]),
        (StackTraceRuntime.Go, ["."], [GoFunctionRegex, GoLocationRegex])
    ];

    /// <summary>
    /// The options.
    /// </summary>
    private readonly Options _options;

    /// <summary>
    /// The languages.
    /// </summary>
    private readonly List<Language> _languages;

    /// <summary>
    /// The selected language.
    /// </summary>
    private Language _selectedLanguage;

    /// <summary>
    /// The selected runtime.
    /// </summary>
    private StackTraceRuntime? _selectedRuntime;

    /// <summary>
    /// Initializes a new instance of the <see cref="StackTraceBeautify"/> class.
    /// </summary>
    public StackTraceBeautify()
    {
        this._options = new Options
                           {
                               PrettyPrint = true,
                               FrameCssClass = "st-frame",
                               TypeCssClass = "st-type",
                               MethodCssClass = "st-method",
                               ParamsListCssClass = "st-frame-params",
                               ParamTypeCssClass = "st-param-type",
                               ParamNameCssClass = "st-param-name",
                               FileCssClass = "st-file",
                               LineCssClass = "st-line",
                               ColumnCssClass = "st-column"
                           };

        this._languages = InitializeLanguages();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StackTraceBeautify"/> class.
    /// </summary>
    public StackTraceBeautify(Options option)
    {
        this._options = option;

        this._languages = InitializeLanguages();
    }

    /// <summary>
    /// Converts a Stack Trace (string) in to a html highlighted (beautified) string
    /// </summary>
    /// <param name="stackTrace">
    /// The stack trace.
    /// </param>
    /// <returns>
    /// Returns an Html String
    /// </returns>
    public string Beautify(string stackTrace)
    {
        if (stackTrace is null)
        {
            throw new ArgumentNullException(nameof(stackTrace));
        }

        var sanitizedStack = stackTrace.Trim().Replace("<", "&lt;").Replace(">", "&gt;");

        // Pretty print result if is set to true
        if (this._options.PrettyPrint)
        {
            sanitizedStack = FormatException(sanitizedStack);
        }

        // Trim empty lines
        var lines = sanitizedStack.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

        this._selectedLanguage = null;
        this._selectedRuntime = this._options.Runtime == StackTraceRuntime.Auto ? null : this._options.Runtime;

        var clone = new StringBuilder();

        for (int i = 0, j = lines.Length; i < j; ++i)
        {
            var li = this.FormatLine(lines[i]);

            li = li.Replace("&lt;", "<span>&lt;</span>").Replace("&gt;", "<span>&gt;</span>");

            clone.Append(lines.Length - 1 == i ? li : $"{li}\n");
        }

        return clone.ToString();
    }

    /// <summary>
    /// Get the language of the stack trace, based on the keyword used in the first recognized frame.
    /// The language is not needed for parsing, it is for information only. Only .NET stack traces are
    /// translated, Java and JavaScript stack traces always return <c>null</c>.
    /// </summary>
    /// <returns>
    /// Returns the language name, or <c>null</c> if the language is unknown or no frame was recognized
    /// </returns>
    public string GetLanguage()
    {
        return this._selectedLanguage?.Name;
    }

    /// <summary>
    /// Get the runtime of the stack trace: the runtime set in <see cref="Options.Runtime"/>, or the runtime
    /// detected from the first recognized frame.
    /// </summary>
    /// <returns>
    /// Returns the runtime, or <c>null</c> if the runtime is detected and no frame was recognized
    /// </returns>
    public StackTraceRuntime? GetRuntime()
    {
        return this._selectedRuntime;
    }

    /// <summary>
    /// Highlights a single line, if it is a stack frame. Any other line is returned unchanged.
    /// </summary>
    /// <param name="line">
    /// The line.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    private string FormatLine(string line)
    {
        var (match, separators) = this.MatchFrame(line);

        if (match is null)
        {
            return line;
        }

        if (match.Groups["at"].Success)
        {
            var at = Regex.Replace(match.Groups["at"].Value, @"\s+", " ");

            this._selectedLanguage ??= this._languages.FirstOrDefault(x => x.At == at);
        }

        // The highlighted parts, sorted by their position in the line (Python has the file before the function)
        var parts = new List<(int Index, int Length, string Html)>();

        var typeMethod = match.Groups["typeMethod"];

        if (typeMethod.Length > 0)
        {
            parts.Add(this.FormatFrame(line, typeMethod, match.Groups["params"], match.Groups["args"], separators));
        }

        AddPart(parts, match.Groups["file"], this._options.FileCssClass);
        AddPart(parts, match.Groups["line"], this._options.LineCssClass);
        AddPart(parts, match.Groups["column"], this._options.ColumnCssClass);

        var result = new StringBuilder();
        var position = 0;

        foreach (var (index, length, html) in parts.OrderBy(part => part.Index))
        {
            result.Append(line, position, index - position);
            result.Append(html);

            position = index + length;
        }

        result.Append(line, position, line.Length - position);

        return result.ToString();
    }

    /// <summary>
    /// Highlights the frame: the type and method name, followed by the parameter list (.NET: typed parameters,
    /// PHP and Go: argument values), if there is one.
    /// </summary>
    private (int Index, int Length, string Html) FormatFrame(
        string line,
        Group typeMethod,
        Group parameters,
        Group arguments,
        string[] separators)
    {
        var html = new StringBuilder();
        var end = typeMethod.Index + typeMethod.Length;

        html.Append($"<span class=\"{this._options.FrameCssClass}\">");
        html.Append(this.FormatTypeMethod(typeMethod.Value, separators));

        if (parameters.Success)
        {
            html.Append($"<span class=\"{this._options.ParamsListCssClass}\">({this.FormatParameters(parameters.Value)})</span>");

            // Include the parens around the parameter list
            end = parameters.Index + parameters.Length + 1;
        }
        else if (arguments.Success)
        {
            html.Append($"<span class=\"{this._options.ParamsListCssClass}\">({arguments.Value})</span>");

            end = arguments.Index + arguments.Length + 1;
        }

        html.Append("</span>");

        return (typeMethod.Index, end - typeMethod.Index, html.ToString());
    }

    /// <summary>
    /// Finds the stack frame pattern of the selected runtime that matches the line. If the runtime is not
    /// selected yet, the patterns of all runtimes are tried and the runtime of the first match is selected.
    /// </summary>
    /// <param name="line">
    /// The line.
    /// </param>
    /// <returns>
    /// The <see cref="Match"/> and the type/method separators of the runtime, or <c>null</c> if the line is not a stack frame.
    /// </returns>
    private (Match Match, string[] Separators) MatchFrame(string line)
    {
        foreach (var (runtime, separators, patterns) in Parsers)
        {
            if (this._selectedRuntime is not null && this._selectedRuntime != runtime)
            {
                continue;
            }

            foreach (var pattern in patterns)
            {
                var match = pattern.Match(line);

                if (match.Success)
                {
                    this._selectedRuntime = runtime;
                    return (match, separators);
                }
            }
        }

        return (null, null);
    }

    /// <summary>
    /// Adds the highlighted group to the parts (if the group was matched).
    /// </summary>
    private static void AddPart(List<(int Index, int Length, string Html)> parts, Group group, string cssClass)
    {
        if (group.Success)
        {
            parts.Add((group.Index, group.Length, $"<span class=\"{cssClass}\">{group.Value}</span>"));
        }
    }

    /// <summary>
    /// Highlights the type and method name of a frame (e.g. "System.Int32.Parse" or PHP "App\Service-&gt;run").
    /// </summary>
    /// <param name="typeMethod">
    /// The type and method name.
    /// </param>
    /// <param name="separators">
    /// The separators between type and method name of the runtime (e.g. "." or PHP "-&gt;" and "::").
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    private string FormatTypeMethod(string typeMethod, string[] separators)
    {
        // Find the last separator that is not part of a generic argument list (e.g. "Method[System.String]")
        // or of a PHP closure name (e.g. "{closure:App\Service::run():12}")
        var depth = 0;
        var separator = -1;
        var separatorLength = 0;

        for (var i = typeMethod.Length - 1; i >= 0 && separator < 0; i--)
        {
            switch (typeMethod[i])
            {
                case ']' or '}':
                    depth++;
                    break;
                case '[' or '{':
                    depth--;
                    break;
                default:
                    if (depth == 0)
                    {
                        var found = separators.FirstOrDefault(
                            s => i + 1 >= s.Length && string.CompareOrdinal(typeMethod, i + 1 - s.Length, s, 0, s.Length) == 0);

                        if (found is not null)
                        {
                            separator = i + 1 - found.Length;
                            separatorLength = found.Length;
                        }
                    }

                    break;
            }
        }

        // Plain function names (e.g. JavaScript "main") have no type
        if (separator < 0)
        {
            return $"<span class=\"{this._options.MethodCssClass}\">{typeMethod}</span>";
        }

        // Constructors (e.g. "System.Object..ctor") keep the leading dot in the method name
        if (separator > 0 && typeMethod[separator] == '.' && typeMethod[separator - 1] == '.')
        {
            separator--;
        }

        var type = typeMethod.Substring(0, separator);
        var separatorText = typeMethod.Substring(separator, separatorLength);
        var method = typeMethod.Substring(separator + separatorLength);

        return
            $"<span class=\"{this._options.TypeCssClass}\">{type}</span>{separatorText}<span class=\"{this._options.MethodCssClass}\">{method}</span>";
    }

    /// <summary>
    /// Highlights the parameter list of a frame (without the outer parens).
    /// </summary>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    private string FormatParameters(string parameters)
    {
        var arrParams = SplitParameterList(parameters);
        var parameterList = new StringBuilder();

        for (var index = 0; index < arrParams.Length; index++)
        {
            var cleanedParameter = arrParams[index].Trim().Split(' ');

            var paramType = cleanedParameter[0];
            if (string.IsNullOrEmpty(paramType))
            {
                continue;
            }

            var theParam = $"<span class=\"{this._options.ParamTypeCssClass}\">{paramType}</span>";
            if (cleanedParameter.Length > 1)
            {
                var paramName = cleanedParameter[1];
                theParam += $" <span class=\"{this._options.ParamNameCssClass}\">{paramName}</span>";
            }

            parameterList.Append(index + 1 < arrParams.Length ? $"{theParam}, " : $"{theParam}");
        }

        return parameterList.ToString();
    }

    /// <summary>
    /// Format exception.
    /// </summary>
    /// <param name="exceptionMessage">
    /// The exception message.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    private static string FormatException(string exceptionMessage)
    {
        var regex = new Regex(@"(-{3}\s)(.*?)(-{3})");

        return regex.IsMatch(exceptionMessage) ? regex.Replace(exceptionMessage, string.Empty) : exceptionMessage;
    }

    /// <summary>
    /// Splits a parameter list on commas, without splitting inside nested brackets/parens
    /// or HTML-entity-escaped generic type argument lists (e.g. "Dictionary`2[System.String,System.Int32]"
    /// or "Func&amp;lt;T1,T2,TResult&amp;gt;").
    /// </summary>
    /// <param name="parameters">
    /// The raw, comma-separated parameter list text (already stripped of the outer parens).
    /// </param>
    /// <returns>
    /// The individual parameter substrings.
    /// </returns>
    private static string[] SplitParameterList(string parameters)
    {
        var result = new List<string>();
        var current = new StringBuilder();
        var depth = 0;

        for (var i = 0; i < parameters.Length; i++)
        {
            var c = parameters[i];

            if (c is '(' or '[')
            {
                depth++;
            }
            else if (c is ')' or ']')
            {
                depth--;
            }
            else if (depth == 0 && c == ',')
            {
                result.Add(current.ToString());
                current.Clear();
                continue;
            }
            else if (c == '&' && string.CompareOrdinal(parameters, i, "&lt;", 0, 4) == 0)
            {
                depth++;
            }
            else if (c == '&' && string.CompareOrdinal(parameters, i, "&gt;", 0, 4) == 0)
            {
                depth--;
            }

            current.Append(c);
        }

        result.Add(current.ToString());

        return [.. result];
    }

    private static List<Language> InitializeLanguages()
    {
        return
        [
            // Keywords of the .NET Framework translations (mscorlib resources "Word_At" and "StackTrace_InFileLineNumber").
            // Norwegian uses the same keywords as Danish and is detected as Danish.
            new Language { Name = "english", At = "at", In = "in", Line = "line" },
            new Language { Name = "arabic", At = "عند", In = "في", Line = "السطر" },
            new Language { Name = "chinese", At = "在", In = "位置", Line = "行号" },
            new Language { Name = "chinese-traditional", At = "於", In = "於", Line = "行" },
            new Language { Name = "czech", At = "v", In = "v", Line = "řádek" },
            new Language { Name = "danish", At = "ved", In = "i", Line = "linje" },
            new Language { Name = "dutch", At = "bij", In = "in", Line = "regel" },
            new Language { Name = "finnish", At = "kohteessa", In = "tiedostossa", Line = "rivillä" },
            new Language { Name = "french", At = "à", In = "dans", Line = "ligne" },
            new Language { Name = "german", At = "bei", In = "in", Line = "Zeile" },
            new Language { Name = "greek", At = "σε", In = "στο", Line = "γραμμή" },
            new Language { Name = "hebrew", At = "ב-", In = "ב-", Line = "שורה" },
            new Language { Name = "hungarian", At = "a következő helyen:", In = "hely:", Line = "sor:" },
            new Language { Name = "italian", At = "in", In = "in", Line = "riga" },
            new Language { Name = "japanese", At = "場所", In = "場所", Line = "行" },
            new Language { Name = "korean", At = "위치:", In = "파일", Line = "줄" },
            new Language { Name = "polish", At = "w", In = "w", Line = "wiersz" },
            new Language { Name = "portuguese", At = "em", In = "na", Line = "linha" },
            new Language { Name = "russian", At = "в", In = "в", Line = "строка" },
            new Language { Name = "spanish", At = "en", In = "en", Line = "línea" },
            new Language { Name = "swedish", At = "vid", In = "i", Line = "rad" },
            new Language { Name = "turkish", At = "konum:", In = "içinde:", Line = "satır" }
        ];
    }
}
