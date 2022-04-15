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

/// <summary>
/// The language.
/// </summary>
public class Options
{
    /// <summary>
    /// Gets or sets a value indicating whether pretty the output.
    /// </summary>
    public bool PrettyPrint { get; set; }

    /// <summary>
    /// Gets or sets the frame CSS class.
    /// </summary>
    public string FrameCssClass { get; set; }

    /// <summary>
    /// Gets or sets the method type CSS class.
    /// </summary>
    public string TypeCssClass { get; set; }

    /// <summary>
    /// Gets or sets the method name CSS class.
    /// </summary>
    public string MethodCssClass { get; set; }

    /// <summary>
    /// Gets or sets the method parameter list CSS class.
    /// </summary>
    public string ParamsListCssClass { get; set; }

    /// <summary>
    /// Gets or sets the method parameter type CSS class.
    /// </summary>
    public string ParamTypeCssClass { get; set; }

    /// <summary>
    /// Gets or sets the method parameter name CSS class.
    /// </summary>
    public string ParamNameCssClass { get; set; }

    /// <summary>
    /// Gets or sets the source file CSS class.
    /// </summary>
    public string FileCssClass { get; set; }

    /// <summary>
    /// Gets or sets the source line CSS class.
    /// </summary>
    public string LineCssClass { get; set; }
}