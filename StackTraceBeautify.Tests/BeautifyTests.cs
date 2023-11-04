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

namespace StackTraceBeautify.Tests;

using Xunit;

/// <summary>
    /// The beautify tests.
    /// </summary>
    public class BeautifyTests
    {
        /// <summary>
        /// Beautify Test with English Stack Trace
        /// </summary>
        [Fact]
        public void EnglishTest1()
        {
            const string Expected = "System.FormatException: Input string was not in a correct format.\r\n   at <span class=\"st-frame\"><span class=\"st-type\">System.Number</span>.<span class=\"st-method\">ThrowOverflowOrFormatException</span><span class=\"st-frame-params\">(<span class=\"st-param-type\">ParsingStatus</span> <span class=\"st-param-name\">status</span>, <span class=\"st-param-type\">TypeCode</span> <span class=\"st-param-name\">type</span>)</span></span>\r\n   at <span class=\"st-frame\"><span class=\"st-type\">System.Number</span>.<span class=\"st-method\">ParseInt32</span><span class=\"st-frame-params\">(<span class=\"st-param-type\">ReadOnlySpan`1</span> <span class=\"st-param-name\">value</span>, <span class=\"st-param-type\">NumberStyles</span> <span class=\"st-param-name\">styles</span>, <span class=\"st-param-type\">NumberFormatInfo</span> <span class=\"st-param-name\">info</span>)</span></span>\r\n   at <span class=\"st-frame\"><span class=\"st-type\">System.Int32</span>.<span class=\"st-method\">Parse</span><span class=\"st-frame-params\">(<span class=\"st-param-type\">String</span> <span class=\"st-param-name\">s</span>)</span></span>\r\n   at <span class=\"st-frame\"><span class=\"st-type\">MyNamespace.IntParser</span>.<span class=\"st-method\">Parse</span><span class=\"st-frame-params\">(<span class=\"st-param-type\">String</span> <span class=\"st-param-name\">s</span>)</span></span> in C:\\apps\\MyNamespace\\IntParser.cs:<span class=\"st-line\">line 11</span>\r\n   at <span class=\"st-frame\"><span class=\"st-type\">MyNamespace.Program</span>.<span class=\"st-method\">Main</span><span class=\"st-frame-params\">(<span class=\"st-param-type\">String[]</span> <span class=\"st-param-name\">args</span>)</span></span> in <span class=\"st-file\">C:\\apps\\MyNamespace\\Program.cs</span>:<span class=\"st-line\">line 12</span>";

            const string Stack = """
                                 System.FormatException: Input string was not in a correct format.
                                    at System.Number.ThrowOverflowOrFormatException(ParsingStatus status, TypeCode type)
                                    at System.Number.ParseInt32(ReadOnlySpan`1 value, NumberStyles styles, NumberFormatInfo info)
                                    at System.Int32.Parse(String s)
                                    at MyNamespace.IntParser.Parse(String s) in C:\apps\MyNamespace\IntParser.cs:line 11
                                    at MyNamespace.Program.Main(String[] args) in C:\apps\MyNamespace\Program.cs:line 12
                                 """;

            var beautify = new StackTraceBeautify();

            var result = beautify.Beautify(Stack);

            Assert.Equal("english", beautify.GetLanguage());

            Assert.Equal(Expected, result);
        }
    }