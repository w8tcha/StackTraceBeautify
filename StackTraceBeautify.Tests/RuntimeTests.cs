/*!
 * .NET Port of tje JavaScript library for highlighting .NET stack traces
 * License : Apache 2
 * Author : https://elmah.io
 * Url: https://github.com/elmahio/netStack.js
 *
 */

using AwesomeAssertions;

namespace StackTraceBeautify.Tests;

using NUnit.Framework;

/// <summary>
/// The tests for Java and JavaScript stack traces and the runtime detection.
/// </summary>
public class RuntimeTests
{
    /// <summary>
    /// Beautify Test with Java Stack Trace
    /// </summary>
    [Test]
    public void StackTraceJavaTest1()
    {
        const string expected = """
                                java.lang.NumberFormatException: For input string: "abc"
                                	at <span class="st-frame"><span class="st-type">java.lang.NumberFormatException</span>.<span class="st-method">forInputString</span></span>(<span class="st-file">NumberFormatException.java</span>:<span class="st-line">67</span>)
                                	at <span class="st-frame"><span class="st-type">java.lang.Integer</span>.<span class="st-method">parseInt</span></span>(<span class="st-file">Integer.java</span>:<span class="st-line">580</span>)
                                	at <span class="st-frame"><span class="st-type">com.example.IntParser</span>.<span class="st-method">parse</span></span>(<span class="st-file">IntParser.java</span>:<span class="st-line">11</span>)
                                	at <span class="st-frame"><span class="st-type">com.example.Main</span>.<span class="st-method">main</span></span>(<span class="st-file">Main.java</span>:<span class="st-line">12</span>)
                                """;

        const string stack = """
                             java.lang.NumberFormatException: For input string: "abc"
                             	at java.lang.NumberFormatException.forInputString(NumberFormatException.java:67)
                             	at java.lang.Integer.parseInt(Integer.java:580)
                             	at com.example.IntParser.parse(IntParser.java:11)
                             	at com.example.Main.main(Main.java:12)
                             """;

        var beautify = new StackTraceBeautify();

        var result = beautify.Beautify(stack);

        beautify.GetRuntime().Should().Be(StackTraceRuntime.Java);
        beautify.GetLanguage().Should().BeNull();

        result.Should().Be(expected);
    }

    /// <summary>
    /// Beautify Test with Java Stack Trace, including modules, constructors, lambdas, native methods,
    /// unknown sources, logback jar info, "Caused by" and "... n more"
    /// </summary>
    [Test]
    public void StackTraceJavaTest2()
    {
        const string expected = """
                                java.lang.IllegalStateException: Failed to start
                                	at <span class="st-frame"><span class="st-type">com.example.App</span>.<span class="st-method"><span>&lt;</span>init<span>&gt;</span></span></span>(<span class="st-file">App.java</span>:<span class="st-line">25</span>)
                                	at <span class="st-frame"><span class="st-type">com.example.App</span>.<span class="st-method">lambda$main$0</span></span>(<span class="st-file">App.kt</span>:<span class="st-line">8</span>) ~[app.jar:1.0]
                                	at java.base/<span class="st-frame"><span class="st-type">java.lang.Thread</span>.<span class="st-method">run</span></span>(<span class="st-file">Thread.java</span>:<span class="st-line">833</span>)
                                Caused by: java.io.IOException: Stream closed
                                	at java.base/<span class="st-frame"><span class="st-type">java.io.FileInputStream</span>.<span class="st-method">readBytes</span></span>(Native Method)
                                	at app//<span class="st-frame"><span class="st-type">com.example.Reader$Inner</span>.<span class="st-method">read</span></span>(Unknown Source)
                                	... 5 more
                                """;

        const string stack = """
                             java.lang.IllegalStateException: Failed to start
                             	at com.example.App.<init>(App.java:25)
                             	at com.example.App.lambda$main$0(App.kt:8) ~[app.jar:1.0]
                             	at java.base/java.lang.Thread.run(Thread.java:833)
                             Caused by: java.io.IOException: Stream closed
                             	at java.base/java.io.FileInputStream.readBytes(Native Method)
                             	at app//com.example.Reader$Inner.read(Unknown Source)
                             	... 5 more
                             """;

        var beautify = new StackTraceBeautify();

        var result = beautify.Beautify(stack);

        beautify.GetRuntime().Should().Be(StackTraceRuntime.Java);

        result.Should().Be(expected);
    }

    /// <summary>
    /// Beautify Test with JavaScript Stack Trace from V8 (Node.js)
    /// </summary>
    [Test]
    public void StackTraceJavaScriptV8Test()
    {
        const string expected = """
                                TypeError: Cannot read properties of undefined (reading 'name')
                                    at <span class="st-frame"><span class="st-type">UserService</span>.<span class="st-method">getName</span></span> (<span class="st-file">/app/src/user.js</span>:<span class="st-line">12</span>:<span class="st-column">20</span>)
                                    at <span class="st-frame"><span class="st-type">Object</span>.<span class="st-method"><span>&lt;</span>anonymous<span>&gt;</span></span></span> (<span class="st-file">C:\app\index.js</span>:<span class="st-line">5</span>:<span class="st-column">1</span>)
                                    at new <span class="st-frame"><span class="st-method">Server</span></span> (<span class="st-file">file:///app/server.mjs</span>:<span class="st-line">3</span>:<span class="st-column">9</span>)
                                    at <span class="st-frame"><span class="st-type">Router</span>.<span class="st-method">handle [as dispatch]</span></span> (<span class="st-file">/app/node_modules/router/index.js</span>:<span class="st-line">47</span>:<span class="st-column">12</span>)
                                    at async <span class="st-frame"><span class="st-type">Promise</span>.<span class="st-method">all</span></span> (index 0)
                                    at <span class="st-frame"><span class="st-type">Array</span>.<span class="st-method">forEach</span></span> (<span>&lt;</span>anonymous<span>&gt;</span>)
                                    at <span class="st-file">/app/src/start.js</span>:<span class="st-line">2</span>:<span class="st-column">3</span>
                                    at <span class="st-frame"><span class="st-type">Module</span>.<span class="st-method">_compile</span></span> (<span class="st-file">node:internal/modules/cjs/loader</span>:<span class="st-line">1105</span>:<span class="st-column">14</span>)
                                """;

        const string stack = """
                             TypeError: Cannot read properties of undefined (reading 'name')
                                 at UserService.getName (/app/src/user.js:12:20)
                                 at Object.<anonymous> (C:\app\index.js:5:1)
                                 at new Server (file:///app/server.mjs:3:9)
                                 at Router.handle [as dispatch] (/app/node_modules/router/index.js:47:12)
                                 at async Promise.all (index 0)
                                 at Array.forEach (<anonymous>)
                                 at /app/src/start.js:2:3
                                 at Module._compile (node:internal/modules/cjs/loader:1105:14)
                             """;

        var beautify = new StackTraceBeautify();

        var result = beautify.Beautify(stack);

        beautify.GetRuntime().Should().Be(StackTraceRuntime.JavaScript);
        beautify.GetLanguage().Should().BeNull();

        result.Should().Be(expected);
    }

    /// <summary>
    /// Beautify Test with JavaScript Stack Trace from Firefox and Safari
    /// </summary>
    [Test]
    public void StackTraceJavaScriptFirefoxSafariTest()
    {
        const string expected = """
                                <span class="st-frame"><span class="st-type">UserService.prototype</span>.<span class="st-method">getName</span></span>@<span class="st-file">https://example.com/js/user.js</span>:<span class="st-line">12</span>:<span class="st-column">20</span>
                                <span class="st-frame"><span class="st-method">handler/<span>&lt;</span></span></span>@<span class="st-file">https://example.com/js/app.js</span>:<span class="st-line">30</span>:<span class="st-column">7</span>
                                @<span class="st-file">https://example.com/js/app.js</span>:<span class="st-line">1</span>:<span class="st-column">1</span>
                                <span class="st-frame"><span class="st-method">forEach</span></span>@[native code]
                                <span class="st-frame"><span class="st-method">global code</span></span>@<span class="st-file">https://example.com/</span>:<span class="st-line">4</span>:<span class="st-column">2</span>
                                """;

        const string stack = """
                             UserService.prototype.getName@https://example.com/js/user.js:12:20
                             handler/<@https://example.com/js/app.js:30:7
                             @https://example.com/js/app.js:1:1
                             forEach@[native code]
                             global code@https://example.com/:4:2
                             """;

        var beautify = new StackTraceBeautify();

        var result = beautify.Beautify(stack);

        beautify.GetRuntime().Should().Be(StackTraceRuntime.JavaScript);

        result.Should().Be(expected);
    }

    /// <summary>
    /// Beautify Test with Python Stack Trace
    /// </summary>
    [Test]
    public void StackTracePythonTest()
    {
        const string expected = """
                                Traceback (most recent call last):
                                  File "<span class="st-file">/app/main.py</span>", line <span class="st-line">12</span>, in <span class="st-frame"><span class="st-method"><span>&lt;</span>module<span>&gt;</span></span></span>
                                    main()
                                  File "<span class="st-file">C:\app\parser.py</span>", line <span class="st-line">8</span>, in <span class="st-frame"><span class="st-method">parse</span></span>
                                    return int(value)
                                           ^^^^^^^^^^
                                  File "<span class="st-file"><span>&lt;</span>stdin<span>&gt;</span></span>", line <span class="st-line">1</span>
                                ValueError: invalid literal for int() with base 10: 'abc'
                                """;

        const string stack = """
                             Traceback (most recent call last):
                               File "/app/main.py", line 12, in <module>
                                 main()
                               File "C:\app\parser.py", line 8, in parse
                                 return int(value)
                                        ^^^^^^^^^^
                               File "<stdin>", line 1
                             ValueError: invalid literal for int() with base 10: 'abc'
                             """;

        var beautify = new StackTraceBeautify();

        var result = beautify.Beautify(stack);

        beautify.GetRuntime().Should().Be(StackTraceRuntime.Python);

        result.Should().Be(expected);
    }

    /// <summary>
    /// Beautify Test with PHP Stack Trace
    /// </summary>
    [Test]
    public void StackTracePhpTest()
    {
        const string expected = """
                                PHP Fatal error:  Uncaught Exception: boom in /var/www/src/Service.php:12
                                Stack trace:
                                #0 <span class="st-file">/var/www/src/Controller.php</span>(<span class="st-line">20</span>): <span class="st-frame"><span class="st-type">App\Service</span>-<span>&gt;</span><span class="st-method">run</span><span class="st-frame-params">('abc', 5)</span></span>
                                #1 [internal function]: <span class="st-frame"><span class="st-type">App\Controller</span>::<span class="st-method">handle</span><span class="st-frame-params">(Object(App\Request))</span></span>
                                #2 <span class="st-file">/var/www/src/Router.php</span>(<span class="st-line">7</span>): <span class="st-frame"><span class="st-method">{closure:App\Router::dispatch():6}</span><span class="st-frame-params">(NULL)</span></span>
                                #3 <span class="st-file">C:\www\index.php</span>(<span class="st-line">5</span>): <span class="st-frame"><span class="st-method">array_map</span><span class="st-frame-params">(Object(Closure), Array)</span></span>
                                #4 {main}
                                  thrown in /var/www/src/Service.php on line 12
                                """;

        const string stack = """
                             PHP Fatal error:  Uncaught Exception: boom in /var/www/src/Service.php:12
                             Stack trace:
                             #0 /var/www/src/Controller.php(20): App\Service->run('abc', 5)
                             #1 [internal function]: App\Controller::handle(Object(App\Request))
                             #2 /var/www/src/Router.php(7): {closure:App\Router::dispatch():6}(NULL)
                             #3 C:\www\index.php(5): array_map(Object(Closure), Array)
                             #4 {main}
                               thrown in /var/www/src/Service.php on line 12
                             """;

        var beautify = new StackTraceBeautify();

        var result = beautify.Beautify(stack);

        beautify.GetRuntime().Should().Be(StackTraceRuntime.Php);

        result.Should().Be(expected);
    }

    /// <summary>
    /// Beautify Test with Ruby Stack Trace (Ruby 3.4 and older quoting)
    /// </summary>
    [Test]
    public void StackTraceRubyTest()
    {
        const string expected = """
                                <span class="st-file">app/models/user.rb</span>:<span class="st-line">12</span>:in '<span class="st-frame"><span class="st-type">Kernel</span>#<span class="st-method">Integer</span></span>': invalid value for Integer(): "abc" (ArgumentError)
                                	from <span class="st-file">app/models/user.rb</span>:<span class="st-line">8</span>:in 'block (2 levels) in <span class="st-frame"><span class="st-type">User</span>#<span class="st-method">age</span></span>'
                                	from <span class="st-file">app/models/user.rb</span>:<span class="st-line">4</span>:in '<span class="st-frame"><span class="st-type">User</span>.<span class="st-method">find</span></span>'
                                	from <span class="st-file"><span>&lt;</span>internal:kernel<span>&gt;</span></span>:<span class="st-line">187</span>:in '<span class="st-frame"><span class="st-method">loop</span></span>'
                                	from <span class="st-file">C:/app/main.rb</span>:<span class="st-line">3</span>:in `<span class="st-frame"><span class="st-method"><span>&lt;</span>main<span>&gt;</span></span></span>'
                                """;

        const string stack = """
                             app/models/user.rb:12:in 'Kernel#Integer': invalid value for Integer(): "abc" (ArgumentError)
                             	from app/models/user.rb:8:in 'block (2 levels) in User#age'
                             	from app/models/user.rb:4:in 'User.find'
                             	from <internal:kernel>:187:in 'loop'
                             	from C:/app/main.rb:3:in `<main>'
                             """;

        var beautify = new StackTraceBeautify();

        var result = beautify.Beautify(stack);

        beautify.GetRuntime().Should().Be(StackTraceRuntime.Ruby);

        result.Should().Be(expected);
    }

    /// <summary>
    /// Beautify Test with Go Stack Trace (panic)
    /// </summary>
    [Test]
    public void StackTraceGoTest()
    {
        const string expected = """
                                panic: runtime error: index out of range [5] with length 3
                                goroutine 1 [running]:
                                <span class="st-frame"><span class="st-method">panic</span><span class="st-frame-params">({0x4a5b40?, 0xc000012345?})</span></span>
                                	<span class="st-file">/usr/local/go/src/runtime/panic.go</span>:<span class="st-line">914</span> +0x21f
                                <span class="st-frame"><span class="st-type">github.com/acme/app/parser</span>.<span class="st-method">Parse</span><span class="st-frame-params">(...)</span></span>
                                	<span class="st-file">/app/parser/parser.go</span>:<span class="st-line">12</span>
                                <span class="st-frame"><span class="st-type">main.(*Server)</span>.<span class="st-method">handle</span><span class="st-frame-params">(0xc000010000, {0x4b2f40, 0x3})</span></span>
                                	<span class="st-file">/app/server.go</span>:<span class="st-line">25</span> +0x1d
                                created by <span class="st-frame"><span class="st-type">main</span>.<span class="st-method">main</span></span> in goroutine 1
                                	<span class="st-file">/app/main.go</span>:<span class="st-line">8</span> +0x45
                                exit status 2
                                """;

        const string stack = """
                             panic: runtime error: index out of range [5] with length 3

                             goroutine 1 [running]:
                             panic({0x4a5b40?, 0xc000012345?})
                             	/usr/local/go/src/runtime/panic.go:914 +0x21f
                             github.com/acme/app/parser.Parse(...)
                             	/app/parser/parser.go:12
                             main.(*Server).handle(0xc000010000, {0x4b2f40, 0x3})
                             	/app/server.go:25 +0x1d
                             created by main.main in goroutine 1
                             	/app/main.go:8 +0x45
                             exit status 2
                             """;

        var beautify = new StackTraceBeautify();

        var result = beautify.Beautify(stack);

        beautify.GetRuntime().Should().Be(StackTraceRuntime.Go);

        result.Should().Be(expected);
    }

    /// <summary>
    /// The runtime of a .NET stack trace is detected, and the language detection still works
    /// </summary>
    [Test]
    public void DetectDotNetRuntimeTest()
    {
        const string stack = """
                             System.FormatException: Input string was not in a correct format.
                                at System.Int32.Parse(String s)
                                at MyNamespace.Program.Main(String[] args) in C:\apps\MyNamespace\Program.cs:line 12
                             """;

        var beautify = new StackTraceBeautify();

        beautify.Beautify(stack);

        beautify.GetRuntime().Should().Be(StackTraceRuntime.DotNet);
        beautify.GetLanguage().Should().Be("english");
    }

    /// <summary>
    /// No runtime is detected, if no line is a stack frame
    /// </summary>
    [Test]
    public void NoRuntimeDetectedTest()
    {
        const string stack = "Something went wrong";

        var beautify = new StackTraceBeautify();

        var result = beautify.Beautify(stack);

        beautify.GetRuntime().Should().BeNull();

        result.Should().Be(stack);
    }

    /// <summary>
    /// A forced runtime only highlights frames of that runtime
    /// </summary>
    [Test]
    public void ForcedRuntimeTest()
    {
        const string stack = """
                             java.lang.RuntimeException: boom
                             	at com.example.Main.main(Main.java:12)
                             """;

        var beautify = new StackTraceBeautify(new Options { Runtime = StackTraceRuntime.JavaScript });

        var result = beautify.Beautify(stack);

        beautify.GetRuntime().Should().Be(StackTraceRuntime.JavaScript);

        result.Should().Be(stack);
    }

    /// <summary>
    /// The runtime detection is reset for each stack trace
    /// </summary>
    [Test]
    public void RuntimeResetBetweenStackTracesTest()
    {
        var beautify = new StackTraceBeautify();

        beautify.Beautify("\tat com.example.Main.main(Main.java:12)");
        beautify.GetRuntime().Should().Be(StackTraceRuntime.Java);

        beautify.Beautify("    at main (/app/index.js:1:1)");
        beautify.GetRuntime().Should().Be(StackTraceRuntime.JavaScript);
    }
}
