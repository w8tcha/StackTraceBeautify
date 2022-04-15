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
public class Language
{
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the at.
    /// </summary>
    public string At { get; set; }

    /// <summary>
    /// Gets or sets the in.
    /// </summary>
    public string In { get; set; }

    /// <summary>
    /// Gets or sets the line.
    /// </summary>
    public string Line { get; set; }
}