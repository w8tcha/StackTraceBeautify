/*!
 * .NET Port of jQuery Plugin netStack
 * License : Apache 2
 * Author : Ingo Herbote
 * Url: https://github.com/elmahio/netStack.js
 *
 *
 * Original 
 * A simple and easy jQuery plugin for highlighting .NET stack traces
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
    /// Matches a complete stack frame line, independent of the language of the stack trace:
    /// "{at} Type.Method(params)", optionally followed by the file and line information in one of the
    /// formats used by the .NET translations (see mscorlib "Word_At" and "StackTrace_InFileLineNumber"):
    /// " {in} file:{line} number" (most languages, German adds a trailing dot),
    /// " {in} file, {line} number" (Hungarian: "hely: {0}, sor: {1}") or
    /// " file {in}: {line} number" (Turkish: "{0} içinde: satır {1}").
    /// </summary>
    private static readonly Regex FrameRegex = new(
        @"^\s*(?<at>\S+|\S+(?:\s+\S+){1,2}:)\s+"
        + @"(?<frame>(?<typeMethod>[^\s()]*\.[^\s()]+)\((?<params>[^()]*)\))"
        + @"(?:\s+[^\s\\/.]+\s+(?<file>.+?)[:,]\s*(?<line>[^\s:,]+:?\s+\d+)\.?"
        + @"|\s+(?<file>.+?)\s+[^\s\\/.]+:\s*(?<line>[^\s:,]+\s+\d+)\.?)?\s*$",
        RegexOptions.Compiled);

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
                               LineCssClass = "st-line"
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
    /// The language is not needed for parsing, it is for information only.
    /// </summary>
    /// <returns>
    /// Returns the language name, or <c>null</c> if the language is unknown or no frame was recognized
    /// </returns>
    public string GetLanguage()
    {
        return this._selectedLanguage?.Name;
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
        var match = FrameRegex.Match(line);

        if (!match.Success)
        {
            return line;
        }

        var at = Regex.Replace(match.Groups["at"].Value, @"\s+", " ");

        this._selectedLanguage ??= this._languages.FirstOrDefault(x => x.At == at);

        var typeMethod = match.Groups["typeMethod"];
        var parameters = match.Groups["params"];
        var file = match.Groups["file"];
        var lineNumber = match.Groups["line"];

        var result = new StringBuilder();

        result.Append(line, 0, typeMethod.Index);
        result.Append($"<span class=\"{this._options.FrameCssClass}\">");
        result.Append(this.FormatTypeMethod(typeMethod.Value));
        result.Append($"<span class=\"{this._options.ParamsListCssClass}\">({this.FormatParameters(parameters.Value)})</span>");
        result.Append("</span>");

        var position = match.Groups["frame"].Index + match.Groups["frame"].Length;

        if (file.Success)
        {
            result.Append(line, position, file.Index - position);
            result.Append($"<span class=\"{this._options.FileCssClass}\">{file.Value}</span>");
            result.Append(line, file.Index + file.Length, lineNumber.Index - file.Index - file.Length);
            result.Append($"<span class=\"{this._options.LineCssClass}\">{lineNumber.Value}</span>");

            position = lineNumber.Index + lineNumber.Length;
        }

        result.Append(line, position, line.Length - position);

        return result.ToString();
    }

    /// <summary>
    /// Highlights the type and method name of a frame (e.g. "System.Int32.Parse").
    /// </summary>
    /// <param name="typeMethod">
    /// The type and method name.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    private string FormatTypeMethod(string typeMethod)
    {
        // Find the last dot that is not part of a generic argument list (e.g. "Method[System.String]")
        var depth = 0;
        var separator = -1;

        for (var i = typeMethod.Length - 1; i >= 0 && separator < 0; i--)
        {
            switch (typeMethod[i])
            {
                case ']':
                    depth++;
                    break;
                case '[':
                    depth--;
                    break;
                case '.' when depth == 0:
                    separator = i;
                    break;
            }
        }

        // Constructors (e.g. "System.Object..ctor") keep the leading dot in the method name
        if (separator > 0 && typeMethod[separator - 1] == '.')
        {
            separator--;
        }

        var type = typeMethod.Substring(0, separator);
        var method = typeMethod.Substring(separator + 1);

        return
            $"<span class=\"{this._options.TypeCssClass}\">{type}</span>.<span class=\"{this._options.MethodCssClass}\">{method}</span>";
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
