/*!
 * .NET Port of the JavaScript library for highlighting .NET stack traces
 * License : Apache 2
 * Author : https://elmah.io
 * Url: https://github.com/elmahio/netStack.js
 *
 */

namespace StackTraceBeautify;

/// <summary>
/// The runtime (platform) that produced a stack trace.
/// </summary>
public enum StackTraceRuntime
{
    /// <summary>
    /// Detect the runtime from the first recognized stack frame.
    /// </summary>
    Auto,

    /// <summary>
    /// .NET stack traces, e.g. "at Type.Method(String s) in file.cs:line 12".
    /// </summary>
    DotNet,

    /// <summary>
    /// Java (and other JVM languages) stack traces, e.g. "at com.example.Type.method(Type.java:12)".
    /// </summary>
    Java,

    /// <summary>
    /// JavaScript stack traces from V8 (Chrome, Edge, Node.js), e.g. "at Type.method (file.js:12:5)",
    /// or from Firefox and Safari, e.g. "method@file.js:12:5".
    /// </summary>
    JavaScript,

    /// <summary>
    /// Python stack traces, e.g. File "file.py", line 12, in function
    /// </summary>
    Python,

    /// <summary>
    /// PHP stack traces, e.g. "#0 file.php(12): Type->method('abc')".
    /// </summary>
    Php,

    /// <summary>
    /// Ruby stack traces, e.g. "from file.rb:12:in 'Type#method'".
    /// </summary>
    Ruby,

    /// <summary>
    /// Go stack traces (panics), e.g. "main.(*Type).Method(0x1)" followed by "	/path/file.go:12 +0x1d".
    /// </summary>
    Go
}
