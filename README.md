# StackTraceBeautify

.NET Port of [netStack.js](https://github.com/elmahio/netStack.js)

[![NuGet](https://img.shields.io/nuget/v/StackTraceBeautify.svg)](https://nuget.org/packages/StackTraceBeautify) [![license](https://img.shields.io/hexpm/l/plug.svg)](#)

![build status](https://github.com/w8tcha/StackTraceBeautify/actions/workflows/build.yml/badge.svg)

A simple Library for highlighting .NET stack traces. It converts a Stack Trace (String) in to an html highlighted (beautified) String

#### Stacktrace - Language support
The parser does not depend on the language of the stack trace. Frames are recognized by their structure
(`<at> Type.Method(params) [<in> file:<line> number]`), so stack traces in any language are supported,
e.g. Danish, English, French, Japanese, German, Spanish, Russian, Chinese, ...

`GetLanguage()` returns the detected language (for information only) for all .NET Framework translations:
English, Arabic, Chinese (Simplified and Traditional), Czech, Danish, Dutch, Finnish, French, German, Greek, Hebrew,
Hungarian, Italian, Japanese, Korean, Polish, Portuguese, Russian, Spanish, Swedish and Turkish
(Norwegian uses the same keywords as Danish and is detected as Danish).

#### Demo
[Stack Trace Formatter - Online pretty print of .NET stack traces](https://elmah.io/tools/stack-trace-formatter/)

#### Initialization
```c#
var bs = new StackTraceBeautify()
bs.Beautify(input);
```

#### Options
```c#
var bs = new StackTraceBeautify(new Options
                           {
                               PrettyPrint = false,
                               FrameCssClass = "st-frame",
                               TypeCssClass = "st-type",
                               MethodCssClass = "st-method",
                               ParamsListCssClass = "st-frame-params",
                               ParamTypeCssClass = "st-param-type",
                               ParamNameCssClass = "st-param-name",
                               FileCssClass = "st-file",
                               LineCssClass = "st-line"
                           });
```

#### Ready to go css
```css
pre, code {background-color:#333; color: #ffffff;}
.st-type {color: #0a8472; font-weight: bolder;}
.st-method {color: #70c9ba; font-weight: bolder;}
.st-frame-params {color: #ffffff; font-weight: normal;}
.st-param-type {color: #0a8472;}
.st-param-name {color: #ffffff;}
.st-file {color:#f8b068;}
.st-line {color:#ff4f68;}
```

---
### Acknowledgments

* [@elmahio](https://github.com/elmahio)
* [@IgoR-NiK](https://github.com/IgoR-NiK)