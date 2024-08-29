﻿/*!
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
        var sanitizedStack = stackTrace.Replace("<", "&lt;").Replace(">", "&gt;");

        var lines = sanitizedStack.Split('\n');
        var lang = string.Empty;
        var clone = new StringBuilder();

        // search for language
        foreach (var line in lines.Where(_ => lang == string.Empty))
        {
            if (Regex.IsMatch(line, @"(\s+)at .*\)"))
            {
                lang = "english";
            }
            else if (Regex.IsMatch(line, @"(\s+)ved .*\)"))
            {
                lang = "danish";
            }
            else if (Regex.IsMatch(line, @"(\s+)bei .*\)"))
            {
                lang = "german";
            }
            else if (Regex.IsMatch(line, @"(\s+)в .*\)"))
            {
                lang = "russian";
            }
            else if (Regex.IsMatch(line, @"(\s+)在 .*\)"))
            {
                lang = "chinese";
            }
        }

        if (lang == string.Empty)
        {
            return clone.ToString();
        }

        this._selectedLanguage = Search(lang, this._languages);

        // Pretty print result if is set to true
        if (this._options.PrettyPrint)
        {
            sanitizedStack = FormatException(sanitizedStack, this._selectedLanguage.At);
            lines = sanitizedStack.Split('\n');
        }

        for (int i = 0, j = lines.Length; i < j; ++i)
        {
            var line = lines[i];
            var li = line;

            var hli = new Regex($@"(\S*){this._selectedLanguage.At} .*\)");

            if (hli.IsMatch(line))
            {
                // Frame
                var regFrame = new Regex($@"(\S*){this._selectedLanguage.At} .*\)");
                var partsFrame = regFrame.Match(line).Value;

                partsFrame = partsFrame.Replace($"{this._selectedLanguage.At} ", string.Empty);

                // Frame -> ParameterList
                var regParamList = new Regex(@"\(.*\)");

                var partsParamList = regParamList.Match(line).Value;

                // Frame -> Params
                var partsParams = partsParamList.Replace("(", string.Empty).Replace(")", string.Empty);
                var arrParams = partsParams.Split(',');
                var parameterList = new StringBuilder();

                for (var index = 0; index < arrParams.Length; index++)
                {
                    var parameter = arrParams[index];
                    if (string.IsNullOrEmpty(parameter))
                    {
                        continue;
                    }

                    var cleanedParameter = parameter.TrimStart().Split(' ');
                    
                    var paramType = cleanedParameter[0];
                    if (string.IsNullOrEmpty(cleanedParameter[0]))
                    {
                        continue;
                    }

                    var theParam = $"<span class=\"{this._options.ParamTypeCssClass}\">{paramType}</span>";
                    if (cleanedParameter.Length > 1)
                    {
                        var paramName = cleanedParameter[1];
                        theParam += $" <span class=\"{this._options.ParamNameCssClass}\">{paramName}</span>";";
                    }

                    parameterList.Append(index + 1 < arrParams.Length ? $"{theParam}, " : $"{theParam}");
                }

                var stringParamComplete = $"<span class=\"{this._options.ParamsListCssClass}\">({parameterList})</span>";

                // Frame -> Type & Method
                var partsTypeMethod = partsFrame.Replace(partsParamList, string.Empty).Replace("\r", string.Empty);
                var arrTypeMethod = partsTypeMethod.Split('.').ToList();
                var method = arrTypeMethod[arrTypeMethod.Count -1];

                var type = partsTypeMethod.Replace($".{method}", string.Empty);
                var stringTypeMethod =
                    $"<span class=\"{this._options.TypeCssClass}\">{type}</span>.<span class=\"{this._options.MethodCssClass}\">{method}</span>";

                // Construct Frame
                var newPartsFrame = partsFrame.Replace(partsParamList, stringParamComplete)
                    .Replace(partsTypeMethod, stringTypeMethod);

                // Line
                var regLine = new Regex($"(:{this._selectedLanguage.Line}.*)");

                var partsLine = regLine.Match(line).Value;
                partsLine = partsLine.Replace(":", string.Empty).Replace("\r", string.Empty);

                // File => (!) text requires multiline to exec regex, otherwise it will return null.
                var regFile = new Regex($"({this._selectedLanguage.In}\\s.*)", RegexOptions.Multiline);
                var partsFile = regFile.Match(line).Value;
                partsFile = partsFile.Replace($"{this._selectedLanguage.In} ", string.Empty)
                    .Replace($":{partsLine}", string.Empty);

                li = li.Replace(partsFrame, $"<span class=\"{this._options.FrameCssClass}\">{newPartsFrame}</span>");

                if (!string.IsNullOrEmpty(partsFile))
                {
                    li = li.Replace(partsFile, $"<span class=\"{this._options.FileCssClass}\">{partsFile}</span>");
                }

                if (!string.IsNullOrEmpty(partsLine))
                {
                    li = li.Replace(partsLine, $"<span class=\"{this._options.LineCssClass}\">{partsLine}</span>");
                }

                li = li.Replace("&lt;", "<span>&lt;</span>").Replace("&gt;", "<span>&gt;</span>");

                clone.Append(lines.Length - 1 == i ? li : $"{li}\n");
            }
            else
            {
                if (string.IsNullOrEmpty(line.Trim()))
                {
                    continue;
                }

                li = line;

                clone.Append(lines.Length - 1 == i ? li : $"{li}\n");
            }
        }

        return clone.ToString();
    }

    /// <summary>
    /// Get the Current selected language
    /// </summary>
    /// <returns>
    /// Returns the selected language
    /// </returns>
    public string GetLanguage()
    {
        return this._selectedLanguage?.Name;
    }

    /// <summary>
    /// Get the Language
    /// </summary>
    /// <param name="languageName">
    /// The language name.
    /// </param>
    /// <param name="languages">
    /// The languages.
    /// </param>
    /// <returns>
    /// The <see cref="Language"/>.
    /// </returns>
    private static Language Search(string languageName, IReadOnlyCollection<Language> languages)
    {
        var language = languages.FirstOrDefault(x => x.Name == languageName);

        return language ?? languages.FirstOrDefault(x => x.Name == "english");
    }

    /// <summary>
    /// Format exception.
    /// </summary>
    /// <param name="exceptionMessage">
    /// The exception message.
    /// </param>
    /// <param name="languageAt">
    /// The language At.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    private static string FormatException(string exceptionMessage, string languageAt)
    {
        var regex = new Regex(@"(-{3}\s)(.*?)(-{3})");
        var regex2 = new Regex($@"(\s){languageAt} ([^-:]*?)\((.*?)\)");

        var result = regex.IsMatch(exceptionMessage) ? regex.Replace(exceptionMessage, string.Empty) : exceptionMessage;

        if (regex2.IsMatch(result))
        {
            result = regex.Replace(result, string.Empty);
        }

        return result;
    }

    private static List<Language> InitializeLanguages()
    {
        return
        [
            new Language { Name = "english", At = "at", In = "in", Line = "line" },
            new Language { Name = "danish", At = "ved", In = "i", Line = "linje" },
            new Language { Name = "german", At = "bei", In = "in", Line = "Zeile" },
            new Language { Name = "russian", At = "в", In = "в", Line = "строка" },
            new Language { Name = "chinese", At = "在", In = "位置", Line = "行号" },
        ];
    }
}
