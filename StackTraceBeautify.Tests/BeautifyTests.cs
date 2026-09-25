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

using AwesomeAssertions;

namespace StackTraceBeautify.Tests;

using NUnit.Framework;

/// <summary>
/// The beautify tests.
/// </summary>
public class BeautifyTests
{
    /// <summary>
    /// Beautify Test with English Stack Trace
    /// </summary>
    [Test]
    public void StackTraceEnglishTest1()
    {
        const string expected = """
                                System.FormatException: Input string was not in a correct format.
                                   at <span class="st-frame"><span class="st-type">System.Number</span>.<span class="st-method">ThrowOverflowOrFormatException</span><span class="st-frame-params">(<span class="st-param-type">ParsingStatus</span> <span class="st-param-name">status</span>, <span class="st-param-type">TypeCode</span> <span class="st-param-name">type</span>)</span></span>
                                   at <span class="st-frame"><span class="st-type">System.Number</span>.<span class="st-method">ParseInt32</span><span class="st-frame-params">(<span class="st-param-type">ReadOnlySpan`1</span> <span class="st-param-name">value</span>, <span class="st-param-type">NumberStyles</span> <span class="st-param-name">styles</span>, <span class="st-param-type">NumberFormatInfo</span> <span class="st-param-name">info</span>)</span></span>
                                   at <span class="st-frame"><span class="st-type">System.Int32</span>.<span class="st-method">Parse</span><span class="st-frame-params">(<span class="st-param-type">String</span> <span class="st-param-name">s</span>)</span></span>
                                   at <span class="st-frame"><span class="st-type">MyNamespace.IntParser</span>.<span class="st-method">Parse</span><span class="st-frame-params">(<span class="st-param-type">String</span> <span class="st-param-name">s</span>)</span></span> in <span class="st-file">C:\apps\MyNamespace\IntParser.cs</span>:<span class="st-line">line 11</span>
                                   at <span class="st-frame"><span class="st-type">MyNamespace.Program</span>.<span class="st-method">Main</span><span class="st-frame-params">(<span class="st-param-type">String[]</span> <span class="st-param-name">args</span>)</span></span> in <span class="st-file">C:\apps\MyNamespace\Program.cs</span>:<span class="st-line">line 12</span>
                                """;

        const string stack = """
                             System.FormatException: Input string was not in a correct format.
                                at System.Number.ThrowOverflowOrFormatException(ParsingStatus status, TypeCode type)
                                at System.Number.ParseInt32(ReadOnlySpan`1 value, NumberStyles styles, NumberFormatInfo info)
                                at System.Int32.Parse(String s)
                                at MyNamespace.IntParser.Parse(String s) in C:\apps\MyNamespace\IntParser.cs:line 11
                                at MyNamespace.Program.Main(String[] args) in C:\apps\MyNamespace\Program.cs:line 12
                             """;

        var beautify = new StackTraceBeautify();

        var result = beautify.Beautify(stack);

        beautify.GetLanguage().Should().BeEquivalentTo("english");

        result.Should().BeEquivalentTo(expected);
    }

    /// <summary>
    /// Beautify Test with English Stack Trace
    /// </summary>
    [Test]
    public void StackTraceEnglishTest2()
    {
        const string expected = """
                                Elmah.TestException: This is a test exception that can be safely ignored. at Elmah.ErrorLogPageFactory.FindHandler(String name) in C:\ELMAH\src\Elmah\ErrorLogPageFactory.cs:line 126 at Elmah.ErrorLogPageFactory.GetHandler(HttpContext context, String requestType, String url, String pathTranslated) in C:\ELMAH\src\Elmah\ErrorLogPageFactory.cs:line 66 at System.Web.HttpApplication.MapHttpHandler(HttpContext context, String requestType, VirtualPath path, String pathTranslated, Boolean useAppConfig) at System.Web.HttpApplication.MapHandlerExecutionStep.System.Web.HttpApplication.IExecutionStep.Execute() at System.Web.HttpApplication.ExecuteStep(IExecutionStep step, Boolean& completedSynchronously)
                                """;

        const string stack = """
                             Elmah.TestException: This is a test exception that can be safely ignored. at Elmah.ErrorLogPageFactory.FindHandler(String name) in C:\ELMAH\src\Elmah\ErrorLogPageFactory.cs:line 126 at Elmah.ErrorLogPageFactory.GetHandler(HttpContext context, String requestType, String url, String pathTranslated) in C:\ELMAH\src\Elmah\ErrorLogPageFactory.cs:line 66 at System.Web.HttpApplication.MapHttpHandler(HttpContext context, String requestType, VirtualPath path, String pathTranslated, Boolean useAppConfig) at System.Web.HttpApplication.MapHandlerExecutionStep.System.Web.HttpApplication.IExecutionStep.Execute() at System.Web.HttpApplication.ExecuteStep(IExecutionStep step, Boolean& completedSynchronously)
                             """;

        var beautify = new StackTraceBeautify();

        var result = beautify.Beautify(stack);

        result.Should().BeEquivalentTo(expected);
    }

    /// <summary>
    /// Beautify Test with English.
    /// </summary>
    [Test]
    public void StackTraceEnglishTest3()
    {
        const string expected = """
                                Azure.Messaging.ServiceBus.ServiceBusException: The lock supplied is invalid. Either the lock expired, or the message has already been removed from the queue, or was received by a different receiver instance. (MessageLockLost). For troubleshooting information, see https://aka.ms/azsdk/net/servicebus/exceptions/troubleshoot.
                                at Azure.Messaging.ServiceBus.Amqp.AmqpReceiver.ThrowLockLostException() at Azure.Messaging.ServiceBus.Amqp.AmqpReceiver.DisposeMessageAsync(Guid lockToken, Outcome outcome, TimeSpan timeout) at Azure.Messaging.ServiceBus.Amqp.AmqpReceiver.CompleteInternalAsync(Guid lockToken, TimeSpan timeout) at Azure.Messaging.ServiceBus.Amqp.AmqpReceiver.<span>&lt;</span><span>&gt;</span>c.<span>&lt;</span><span>&lt;</span>CompleteAsync<span>&gt;</span>b__43_0<span>&gt;</span>d.MoveNext()  at Azure.Messaging.ServiceBus.ServiceBusRetryPolicy.<span>&lt;</span><span>&gt;</span>c__22`1.<span>&lt;</span><span>&lt;</span>RunOperation<span>&gt;</span>b__22_0<span>&gt;</span>d.MoveNext()  at Azure.Messaging.ServiceBus.ServiceBusRetryPolicy.RunOperation[T1,TResult](Func`4 operation, T1 t1, TransportConnectionScope scope, CancellationToken cancellationToken, Boolean logRetriesAsVerbose) at Azure.Messaging.ServiceBus.ServiceBusRetryPolicy.RunOperation[T1](Func`4 operation, T1 t1, TransportConnectionScope scope, CancellationToken cancellationToken) at Azure.Messaging.ServiceBus.Amqp.AmqpReceiver.CompleteAsync(Guid lockToken, CancellationToken cancellationToken) at Azure.Messaging.ServiceBus.ServiceBusReceiver.CompleteMessageAsync(ServiceBusReceivedMessage message, CancellationToken cancellationToken) at Azure.Messaging.ServiceBus.ReceiverManager.ProcessOneMessage(ServiceBusReceivedMessage triggerMessage, CancellationToken cancellationToken)
                                """;

        const string stack = """
                             Azure.Messaging.ServiceBus.ServiceBusException: The lock supplied is invalid. Either the lock expired, or the message has already been removed from the queue, or was received by a different receiver instance. (MessageLockLost). For troubleshooting information, see https://aka.ms/azsdk/net/servicebus/exceptions/troubleshoot.
                             at Azure.Messaging.ServiceBus.Amqp.AmqpReceiver.ThrowLockLostException() at Azure.Messaging.ServiceBus.Amqp.AmqpReceiver.DisposeMessageAsync(Guid lockToken, Outcome outcome, TimeSpan timeout) at Azure.Messaging.ServiceBus.Amqp.AmqpReceiver.CompleteInternalAsync(Guid lockToken, TimeSpan timeout) at Azure.Messaging.ServiceBus.Amqp.AmqpReceiver.&lt;&gt;c.&lt;&lt;CompleteAsync&gt;b__43_0>d.MoveNext() --- End of stack trace from previous location --- at Azure.Messaging.ServiceBus.ServiceBusRetryPolicy.&lt;&gt;c__22`1.&lt;&lt;RunOperation&gt;b__22_0&gt;d.MoveNext() --- End of stack trace from previous location --- at Azure.Messaging.ServiceBus.ServiceBusRetryPolicy.RunOperation[T1,TResult](Func`4 operation, T1 t1, TransportConnectionScope scope, CancellationToken cancellationToken, Boolean logRetriesAsVerbose) at Azure.Messaging.ServiceBus.ServiceBusRetryPolicy.RunOperation[T1](Func`4 operation, T1 t1, TransportConnectionScope scope, CancellationToken cancellationToken) at Azure.Messaging.ServiceBus.Amqp.AmqpReceiver.CompleteAsync(Guid lockToken, CancellationToken cancellationToken) at Azure.Messaging.ServiceBus.ServiceBusReceiver.CompleteMessageAsync(ServiceBusReceivedMessage message, CancellationToken cancellationToken) at Azure.Messaging.ServiceBus.ReceiverManager.ProcessOneMessage(ServiceBusReceivedMessage triggerMessage, CancellationToken cancellationToken)
                             """;

        var beautify = new StackTraceBeautify();

        var result = beautify.Beautify(stack);

        result.Should().BeEquivalentTo(expected);
    }

    /// <summary>
    /// Beautify Test with German Stack Trace
    /// </summary>
    [Test]
    public void StackTraceGermanTest1()
    {
        const string expected = """
                                System.ApplicationException: Etwas hier ---<span>&gt;</span> System.FormatException: Die Eingabezeichenfolge wurde nicht richtig formatiert. bei System.Number.ThrowOverflowOrFormatException(ParsingStatus status, TypeCode type) bei System.Number.ParseInt32(ReadOnlySpan`1 value, NumberStyles styles, NumberFormatInfo info) bei System.Int32.Parse(String s) bei MyNamespace.IntParser.Execute(String s) in C:\apps\MyNamespace\IntParser.cs:Zeile 13  bei Elmah.Io.App.Controllers.AccountController.ChangeEmail(String secret) in x:\agent\_work\94\s\src\Elmah.Io.App\Controllers\AccountController.cs:Zeile 45 bei System.Convert.FromBase64CharPtr(Char* inputPtr, Int32 inputLength)  bei MyNamespace.IntParser.Execute(String s) in C:\apps\MyNamespace\IntParser.cs:Zeile 17 bei MyNamespace.Program.Main(String[] args) in C:\apps\MyNamespace\Program.cs:Zeile 13
                                """;

        const string stack = """
                             System.ApplicationException: Etwas hier ---> System.FormatException: Die Eingabezeichenfolge wurde nicht richtig formatiert. bei System.Number.ThrowOverflowOrFormatException(ParsingStatus status, TypeCode type) bei System.Number.ParseInt32(ReadOnlySpan`1 value, NumberStyles styles, NumberFormatInfo info) bei System.Int32.Parse(String s) bei MyNamespace.IntParser.Execute(String s) in C:\apps\MyNamespace\IntParser.cs:Zeile 13 --- Ende des Stack-Trace vom vorherigen Ort, an dem eine Ausnahme ausgelöst wurde --- bei Elmah.Io.App.Controllers.AccountController.ChangeEmail(String secret) in x:\agent\_work\94\s\src\Elmah.Io.App\Controllers\AccountController.cs:Zeile 45 bei System.Convert.FromBase64CharPtr(Char* inputPtr, Int32 inputLength) --- Ende des Stack-Trace vom vorherigen Ort, an dem eine Ausnahme ausgelöst wurde --- bei MyNamespace.IntParser.Execute(String s) in C:\apps\MyNamespace\IntParser.cs:Zeile 17 bei MyNamespace.Program.Main(String[] args) in C:\apps\MyNamespace\Program.cs:Zeile 13
                             """;

        var beautify = new StackTraceBeautify();

        var result = beautify.Beautify(stack);

        result.Should().BeEquivalentTo(expected);
    }

    /// <summary>
    /// Beautify Test with Danish Stack Trace
    /// </summary>
    [Test]
    public void StackTraceDanishTest1()
    {
        const string expected = """
                                System.ApplicationException: Kørselsfejl ---<span>&gt;</span> System.FormatException: Inputstrengen blev ikke formateret korrekt. ved System.Number.ThrowOverflowOrFormatException(ParsingStatus status, TypeCode type) ved System.Number.ParseInt32(ReadOnlySpan`1 value, NumberStyles styles, NumberFormatInfo info) ved System.Int32.Parse(String s) ved MyNamespace.IntParser.Execute(String s) i C:\apps\MyNamespace\IntParser.cs:linje 13  ved Elmah.Io.App.Controllers.AccountController.ChangeEmail(String secret) i x:\agent\_work\94\s\src\Elmah.Io.App\Controllers\AccountController.cs:linje 45 ved System.Convert.FromBase64CharPtr(Char* inputPtr, Int32 inputLength)  ved MyNamespace.IntParser.Execute(String s) i C:\apps\MyNamespace\IntParser.cs:linje 17 ved MyNamespace.Program.Main(String[] args) i C:\apps\MyNamespace\Program.cs:linje 13
                                """;

        const string stack = """
                             System.ApplicationException: Kørselsfejl ---> System.FormatException: Inputstrengen blev ikke formateret korrekt. ved System.Number.ThrowOverflowOrFormatException(ParsingStatus status, TypeCode type) ved System.Number.ParseInt32(ReadOnlySpan`1 value, NumberStyles styles, NumberFormatInfo info) ved System.Int32.Parse(String s) ved MyNamespace.IntParser.Execute(String s) i C:\apps\MyNamespace\IntParser.cs:linje 13 --- Slutning af stackspor fra tidligere sted, hvor undtagelse blev kastet --- ved Elmah.Io.App.Controllers.AccountController.ChangeEmail(String secret) i x:\agent\_work\94\s\src\Elmah.Io.App\Controllers\AccountController.cs:linje 45 ved System.Convert.FromBase64CharPtr(Char* inputPtr, Int32 inputLength) --- Slutning af stackspor fra tidligere sted, hvor undtagelse blev kastet --- ved MyNamespace.IntParser.Execute(String s) i C:\apps\MyNamespace\IntParser.cs:linje 17 ved MyNamespace.Program.Main(String[] args) i C:\apps\MyNamespace\Program.cs:linje 13
                             """;

        var beautify = new StackTraceBeautify();

        var result = beautify.Beautify(stack);

        result.Should().BeEquivalentTo(expected);
    }

    /// <summary>
    /// Beautify Test with Spanish Stack Trace (one frame per line).
    /// </summary>
    [Test]
    public void StackTraceSpanishTest1()
    {
        const string expected = """
                                System.FormatException: La cadena de entrada no tenía el formato correcto.
                                   en <span class="st-frame"><span class="st-type">System.Number</span>.<span class="st-method">ThrowOverflowOrFormatException</span><span class="st-frame-params">(<span class="st-param-type">ParsingStatus</span> <span class="st-param-name">status</span>, <span class="st-param-type">TypeCode</span> <span class="st-param-name">type</span>)</span></span>
                                   en <span class="st-frame"><span class="st-type">System.Number</span>.<span class="st-method">ParseInt32</span><span class="st-frame-params">(<span class="st-param-type">ReadOnlySpan`1</span> <span class="st-param-name">value</span>, <span class="st-param-type">NumberStyles</span> <span class="st-param-name">styles</span>, <span class="st-param-type">NumberFormatInfo</span> <span class="st-param-name">info</span>)</span></span>
                                   en <span class="st-frame"><span class="st-type">System.Int32</span>.<span class="st-method">Parse</span><span class="st-frame-params">(<span class="st-param-type">String</span> <span class="st-param-name">s</span>)</span></span>
                                   en <span class="st-frame"><span class="st-type">MyNamespace.IntParser</span>.<span class="st-method">Parse</span><span class="st-frame-params">(<span class="st-param-type">String</span> <span class="st-param-name">s</span>)</span></span> en <span class="st-file">C:\apps\MyNamespace\IntParser.cs</span>:<span class="st-line">línea 11</span>
                                   en <span class="st-frame"><span class="st-type">MyNamespace.Program</span>.<span class="st-method">Main</span><span class="st-frame-params">(<span class="st-param-type">String[]</span> <span class="st-param-name">args</span>)</span></span> en <span class="st-file">C:\apps\MyNamespace\Program.cs</span>:<span class="st-line">línea 12</span>
                                """;

        const string stack = """
                             System.FormatException: La cadena de entrada no tenía el formato correcto.
                                en System.Number.ThrowOverflowOrFormatException(ParsingStatus status, TypeCode type)
                                en System.Number.ParseInt32(ReadOnlySpan`1 value, NumberStyles styles, NumberFormatInfo info)
                                en System.Int32.Parse(String s)
                                en MyNamespace.IntParser.Parse(String s) en C:\apps\MyNamespace\IntParser.cs:línea 11
                                en MyNamespace.Program.Main(String[] args) en C:\apps\MyNamespace\Program.cs:línea 12
                             """;

        var beautify = new StackTraceBeautify();

        var result = beautify.Beautify(stack);

        beautify.GetLanguage().Should().BeEquivalentTo("spanish");

        result.Should().BeEquivalentTo(expected);
    }

    /// <summary>
    /// Beautify Test with Spanish Stack Trace (single concatenated line with inner-exception markers).
    /// </summary>
    [Test]
    public void StackTraceSpanishTest2()
    {
        const string expected = """
                                System.ApplicationException: Algo aquí ---<span>&gt;</span> System.FormatException: La cadena de entrada no tenía el formato correcto. en System.Number.ThrowOverflowOrFormatException(ParsingStatus status, TypeCode type) en System.Number.ParseInt32(ReadOnlySpan`1 value, NumberStyles styles, NumberFormatInfo info) en System.Int32.Parse(String s) en MyNamespace.IntParser.Execute(String s) en C:\apps\MyNamespace\IntParser.cs:línea 13  en Elmah.Io.App.Controllers.AccountController.ChangeEmail(String secret) en x:\agent\_work\94\s\src\Elmah.Io.App\Controllers\AccountController.cs:línea 45 en System.Convert.FromBase64CharPtr(Char* inputPtr, Int32 inputLength)  en MyNamespace.IntParser.Execute(String s) en C:\apps\MyNamespace\IntParser.cs:línea 17 en MyNamespace.Program.Main(String[] args) en C:\apps\MyNamespace\Program.cs:línea 13
                                """;

        const string stack = """
                             System.ApplicationException: Algo aquí ---> System.FormatException: La cadena de entrada no tenía el formato correcto. en System.Number.ThrowOverflowOrFormatException(ParsingStatus status, TypeCode type) en System.Number.ParseInt32(ReadOnlySpan`1 value, NumberStyles styles, NumberFormatInfo info) en System.Int32.Parse(String s) en MyNamespace.IntParser.Execute(String s) en C:\apps\MyNamespace\IntParser.cs:línea 13 --- Fin del seguimiento de la pila del lugar anterior donde se produjo la excepción --- en Elmah.Io.App.Controllers.AccountController.ChangeEmail(String secret) en x:\agent\_work\94\s\src\Elmah.Io.App\Controllers\AccountController.cs:línea 45 en System.Convert.FromBase64CharPtr(Char* inputPtr, Int32 inputLength) --- Fin del seguimiento de la pila del lugar anterior donde se produjo la excepción --- en MyNamespace.IntParser.Execute(String s) en C:\apps\MyNamespace\IntParser.cs:línea 17 en MyNamespace.Program.Main(String[] args) en C:\apps\MyNamespace\Program.cs:línea 13
                             """;

        var beautify = new StackTraceBeautify();

        var result = beautify.Beautify(stack);

        result.Should().BeEquivalentTo(expected);
    }

    /// <summary>
    /// Beautify Test with Russian Stack Trace
    /// </summary>
    [Test]
    public void StackTraceRussianTest1()
    {
        const string expected = """
                                System.ApplicationException: Ошибка в ходе выполнения ---<span>&gt;</span> System.FormatException: Входная строка имела неверный формат. в System.Number.ThrowOverflowOrFormatException(ParsingStatus status, TypeCode type) в System.Number.ParseInt32(ReadOnlySpan`1 value, NumberStyles styles, NumberFormatInfo info) в System.Int32.Parse(String s) в MyNamespace.IntParser.Execute(String s) в C:\apps\MyNamespace\IntParser.cs:строка 13  в Elmah.Io.App.Controllers.AccountController.ChangeEmail(String secret) в x:\agent\_work\94\s\src\Elmah.Io.App\Controllers\AccountController.cs:строка 45 в System.Convert.FromBase64CharPtr(Char* inputPtr, Int32 inputLength)  в MyNamespace.IntParser.Execute(String s) в C:\apps\MyNamespace\IntParser.cs:строка 17 в MyNamespace.Program.Main(String[] args) в C:\apps\MyNamespace\Program.cs:строка 13
                                """;

        const string stack = """
                             System.ApplicationException: Ошибка в ходе выполнения ---> System.FormatException: Входная строка имела неверный формат. в System.Number.ThrowOverflowOrFormatException(ParsingStatus status, TypeCode type) в System.Number.ParseInt32(ReadOnlySpan`1 value, NumberStyles styles, NumberFormatInfo info) в System.Int32.Parse(String s) в MyNamespace.IntParser.Execute(String s) в C:\apps\MyNamespace\IntParser.cs:строка 13 --- Конец трассировка стека из предыдущего расположения, где возникло исключение --- в Elmah.Io.App.Controllers.AccountController.ChangeEmail(String secret) в x:\agent\_work\94\s\src\Elmah.Io.App\Controllers\AccountController.cs:строка 45 в System.Convert.FromBase64CharPtr(Char* inputPtr, Int32 inputLength) --- End of stack trace from previous location where exception was thrown --- в MyNamespace.IntParser.Execute(String s) в C:\apps\MyNamespace\IntParser.cs:строка 17 в MyNamespace.Program.Main(String[] args) в C:\apps\MyNamespace\Program.cs:строка 13
                             """;

        var beautify = new StackTraceBeautify();

        var result = beautify.Beautify(stack);

        result.Should().BeEquivalentTo(expected);
    }

    /// <summary>
    /// Beautify Test with Chinese Stack Trace
    /// </summary>
    [Test]
    public void StackTraceChineseTest1()
    {
        const string expected = "System.Exception: Could not load file or assembly 'netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'. 系统找不到指定的文件。 在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw() 在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task) 在 ClrCustomVisualizerVSHost.VisualizerTargetInternal.<span>&lt;</span>RequestDataAsync<span>&gt;</span>d__10.MoveNext()  在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw() 在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task) 在 Microsoft.VisualStudio.OutOfProcessVisualizers.VisualizerTarget.<span>&lt;</span>RequestDataAsync<span>&gt;</span>d__10.MoveNext()";

        const string stack = "System.Exception: Could not load file or assembly 'netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'. 系统找不到指定的文件。 在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw() 在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task) 在 ClrCustomVisualizerVSHost.VisualizerTargetInternal.&lt;RequestDataAsync&gt;d__10.MoveNext() --- 引发异常的上一位置中堆栈跟踪的末尾 --- 在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw() 在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task) 在 Microsoft.VisualStudio.OutOfProcessVisualizers.VisualizerTarget.&lt;RequestDataAsync&gt;d__10.MoveNext()";

        var beautify = new StackTraceBeautify();

        var result = beautify.Beautify(stack);

        result.Should().BeEquivalentTo(expected);
    }

    /// <summary>
    /// Beautify Test with French Stack Trace (one frame per line).
    /// </summary>
    [Test]
    public void StackTraceFrenchTest1()
    {
        const string expected = """
                                System.FormatException: La chaîne d'entrée était dans un format incorrect.
                                   à <span class="st-frame"><span class="st-type">System.Number</span>.<span class="st-method">ThrowOverflowOrFormatException</span><span class="st-frame-params">(<span class="st-param-type">ParsingStatus</span> <span class="st-param-name">status</span>, <span class="st-param-type">TypeCode</span> <span class="st-param-name">type</span>)</span></span>
                                   à <span class="st-frame"><span class="st-type">System.Number</span>.<span class="st-method">ParseInt32</span><span class="st-frame-params">(<span class="st-param-type">ReadOnlySpan`1</span> <span class="st-param-name">value</span>, <span class="st-param-type">NumberStyles</span> <span class="st-param-name">styles</span>, <span class="st-param-type">NumberFormatInfo</span> <span class="st-param-name">info</span>)</span></span>
                                   à <span class="st-frame"><span class="st-type">System.Int32</span>.<span class="st-method">Parse</span><span class="st-frame-params">(<span class="st-param-type">String</span> <span class="st-param-name">s</span>)</span></span>
                                   à <span class="st-frame"><span class="st-type">MyNamespace.IntParser</span>.<span class="st-method">Parse</span><span class="st-frame-params">(<span class="st-param-type">String</span> <span class="st-param-name">s</span>)</span></span> dans <span class="st-file">C:\apps\MyNamespace\IntParser.cs</span>:<span class="st-line">ligne 11</span>
                                   à <span class="st-frame"><span class="st-type">MyNamespace.Program</span>.<span class="st-method">Main</span><span class="st-frame-params">(<span class="st-param-type">String[]</span> <span class="st-param-name">args</span>)</span></span> dans <span class="st-file">C:\apps\MyNamespace\Program.cs</span>:<span class="st-line">ligne 12</span>
                                """;

        const string stack = """
                             System.FormatException: La chaîne d'entrée était dans un format incorrect.
                                à System.Number.ThrowOverflowOrFormatException(ParsingStatus status, TypeCode type)
                                à System.Number.ParseInt32(ReadOnlySpan`1 value, NumberStyles styles, NumberFormatInfo info)
                                à System.Int32.Parse(String s)
                                à MyNamespace.IntParser.Parse(String s) dans C:\apps\MyNamespace\IntParser.cs:ligne 11
                                à MyNamespace.Program.Main(String[] args) dans C:\apps\MyNamespace\Program.cs:ligne 12
                             """;

        var beautify = new StackTraceBeautify();

        var result = beautify.Beautify(stack);

        beautify.GetLanguage().Should().BeEquivalentTo("french");

        result.Should().BeEquivalentTo(expected);
    }

    /// <summary>
    /// Beautify Test with Japanese Stack Trace (one frame per line).
    /// </summary>
    [Test]
    public void StackTraceJapaneseTest1()
    {
        const string expected = """
                                System.FormatException: 入力文字列の形式が正しくありません。
                                   場所 <span class="st-frame"><span class="st-type">System.Number</span>.<span class="st-method">ThrowOverflowOrFormatException</span><span class="st-frame-params">(<span class="st-param-type">ParsingStatus</span> <span class="st-param-name">status</span>, <span class="st-param-type">TypeCode</span> <span class="st-param-name">type</span>)</span></span>
                                   場所 <span class="st-frame"><span class="st-type">System.Number</span>.<span class="st-method">ParseInt32</span><span class="st-frame-params">(<span class="st-param-type">ReadOnlySpan`1</span> <span class="st-param-name">value</span>, <span class="st-param-type">NumberStyles</span> <span class="st-param-name">styles</span>, <span class="st-param-type">NumberFormatInfo</span> <span class="st-param-name">info</span>)</span></span>
                                   場所 <span class="st-frame"><span class="st-type">System.Int32</span>.<span class="st-method">Parse</span><span class="st-frame-params">(<span class="st-param-type">String</span> <span class="st-param-name">s</span>)</span></span>
                                   場所 <span class="st-frame"><span class="st-type">MyNamespace.IntParser</span>.<span class="st-method">Parse</span><span class="st-frame-params">(<span class="st-param-type">String</span> <span class="st-param-name">s</span>)</span></span> 場所 <span class="st-file">C:\apps\MyNamespace\IntParser.cs</span>:<span class="st-line">行 11</span>
                                   場所 <span class="st-frame"><span class="st-type">MyNamespace.Program</span>.<span class="st-method">Main</span><span class="st-frame-params">(<span class="st-param-type">String[]</span> <span class="st-param-name">args</span>)</span></span> 場所 <span class="st-file">C:\apps\MyNamespace\Program.cs</span>:<span class="st-line">行 12</span>
                                """;

        const string stack = """
                             System.FormatException: 入力文字列の形式が正しくありません。
                                場所 System.Number.ThrowOverflowOrFormatException(ParsingStatus status, TypeCode type)
                                場所 System.Number.ParseInt32(ReadOnlySpan`1 value, NumberStyles styles, NumberFormatInfo info)
                                場所 System.Int32.Parse(String s)
                                場所 MyNamespace.IntParser.Parse(String s) 場所 C:\apps\MyNamespace\IntParser.cs:行 11
                                場所 MyNamespace.Program.Main(String[] args) 場所 C:\apps\MyNamespace\Program.cs:行 12
                             """;

        var beautify = new StackTraceBeautify();

        var result = beautify.Beautify(stack);

        beautify.GetLanguage().Should().BeEquivalentTo("japanese");

        result.Should().BeEquivalentTo(expected);
    }

    /// <summary>
    /// Beautify Test with Italian Stack Trace
    /// </summary>
    [Test]
    public void StackTraceUnknownLanguageTest1()
    {
        const string expected = """
                                System.FormatException: Formato della stringa di input non corretto.
                                   in <span class="st-frame"><span class="st-type">System.Int32</span>.<span class="st-method">Parse</span><span class="st-frame-params">(<span class="st-param-type">String</span> <span class="st-param-name">s</span>)</span></span>
                                   in <span class="st-frame"><span class="st-type">MyNamespace.Program</span>.<span class="st-method">Main</span><span class="st-frame-params">(<span class="st-param-type">String[]</span> <span class="st-param-name">args</span>)</span></span> in <span class="st-file">C:\apps\MyNamespace\Program.cs</span>:<span class="st-line">riga 12</span>
                                """;

        const string stack = """
                             System.FormatException: Formato della stringa di input non corretto.
                                in System.Int32.Parse(String s)
                                in MyNamespace.Program.Main(String[] args) in C:\apps\MyNamespace\Program.cs:riga 12
                             """;

        var beautify = new StackTraceBeautify();

        var result = beautify.Beautify(stack);

        beautify.GetLanguage().Should().BeEquivalentTo("italian");

        result.Should().BeEquivalentTo(expected);
    }

    /// <summary>
    /// Beautify Test with constructors, generic methods and linux paths
    /// </summary>
    [Test]
    public void StackTraceConstructorAndGenericsTest1()
    {
        const string expected = """
                                System.InvalidOperationException: Failed
                                   at <span class="st-frame"><span class="st-type">MyNamespace.Worker</span>.<span class="st-method">.ctor</span><span class="st-frame-params">(<span class="st-param-type">String</span> <span class="st-param-name">name</span>)</span></span> in <span class="st-file">/src/app/Worker.cs</span>:<span class="st-line">line 7</span>
                                   at <span class="st-frame"><span class="st-type">MyNamespace.Worker</span>.<span class="st-method">Run[System.String]</span><span class="st-frame-params">(<span class="st-param-type">Dictionary`2[System.String,System.Int32]</span> <span class="st-param-name">map</span>)</span></span>
                                """;

        const string stack = """
                             System.InvalidOperationException: Failed
                                at MyNamespace.Worker..ctor(String name) in /src/app/Worker.cs:line 7
                                at MyNamespace.Worker.Run[System.String](Dictionary`2[System.String,System.Int32] map)
                             """;

        var result = new StackTraceBeautify().Beautify(stack);

        result.Should().BeEquivalentTo(expected);
    }

    /// <summary>
    /// Parsing and language detection with the keywords of all .NET Framework translations
    /// (mscorlib resources "Word_At" and "StackTrace_InFileLineNumber").
    /// </summary>
    [TestCase("at", "in {0}:line {1}", "line", "english")]
    [TestCase("عند", "في {0}:السطر {1}", "السطر", "arabic")]
    [TestCase("在", "位置 {0}:行号 {1}", "行号", "chinese")]
    [TestCase("於", "於 {0}: 行 {1}", "行", "chinese-traditional")]
    [TestCase("v", "v {0}:řádek {1}", "řádek", "czech")]
    [TestCase("ved", "i {0}:linje {1}", "linje", "danish")]
    [TestCase("bij", "in {0}:regel {1}", "regel", "dutch")]
    [TestCase("kohteessa", "tiedostossa {0}:rivillä {1}", "rivillä", "finnish")]
    [TestCase("à", "dans {0}:ligne {1}", "ligne", "french")]
    [TestCase("bei", "in {0}:Zeile {1}.", "Zeile", "german")]
    [TestCase("σε", "στο {0}:γραμμή {1}", "γραμμή", "greek")]
    [TestCase("ב- ", "ב- {0}:שורה {1}", "שורה", "hebrew")]
    [TestCase("a következő helyen:", "hely: {0}, sor: {1}", "sor:", "hungarian")]
    [TestCase("in", "in {0}:riga {1}", "riga", "italian")]
    [TestCase("場所", "場所 {0}:行 {1}", "行", "japanese")]
    [TestCase("위치:", "파일 {0}:줄 {1}", "줄", "korean")]
    [TestCase("ved", "i {0}:linje {1}", "linje", "danish")] // Norwegian
    [TestCase("w", "w {0}:wiersz {1}", "wiersz", "polish")]
    [TestCase("em", "na {0}:linha {1}", "linha", "portuguese")] // Portuguese (Brazil)
    [TestCase("em", "em {0}:line {1}", "line", "portuguese")] // Portuguese (Portugal)
    [TestCase("в", "в {0}:строка {1}", "строка", "russian")]
    [TestCase("en", "en {0}:línea {1}", "línea", "spanish")]
    [TestCase("vid", "i {0}:rad {1}", "rad", "swedish")]
    [TestCase("konum:", "{0} içinde: satır {1}", "satır", "turkish")]
    [TestCase("xyz", "abc {0}:foo {1}", "foo", null)]
    public void LanguageTest(string at, string fileLineFormat, string lineWord, string language)
    {
        const string file = @"C:\apps\My Namespace\Program.cs";

        // Same composition as the .NET Framework: "   " + Word_At + " " + method + " " + StackTrace_InFileLineNumber
        var stack = $"System.Exception: Error\n   {at} MyNamespace.Worker.Run()\n   {at} MyNamespace.Program.Main(String[] args) {string.Format(fileLineFormat, file, 12)}";

        var expectedFileLine = string.Format(
            fileLineFormat.Replace($"{lineWord} {{1}}", "{1}"),
            $"<span class=\"st-file\">{file}</span>",
            $"<span class=\"st-line\">{lineWord} 12</span>");

        var expected = $"System.Exception: Error\n   {at} <span class=\"st-frame\"><span class=\"st-type\">MyNamespace.Worker</span>.<span class=\"st-method\">Run</span><span class=\"st-frame-params\">()</span></span>\n   {at} <span class=\"st-frame\"><span class=\"st-type\">MyNamespace.Program</span>.<span class=\"st-method\">Main</span><span class=\"st-frame-params\">(<span class=\"st-param-type\">String[]</span> <span class=\"st-param-name\">args</span>)</span></span> {expectedFileLine}";

        var beautify = new StackTraceBeautify();

        var result = beautify.Beautify(stack);

        beautify.GetLanguage().Should().Be(language);

        result.Should().Be(expected);
    }
}