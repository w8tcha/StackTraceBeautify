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
    public void StackTraceEnglishTest1()
    {
        const string expected = """
                                System.FormatException: Input string was not in a correct format.
                                   at <span class="st-frame"><span class="st-type">System.Number</span>.<span class="st-method">ThrowOverflowOrFormatException</span><span class="st-frame-params">(<span class="st-param-type">ParsingStatus</span> <span class="st-param-name">status</span>, <span class="st-param-type">TypeCode</span> <span class="st-param-name">type</span>)</span></span>
                                   at <span class="st-frame"><span class="st-type">System.Number</span>.<span class="st-method">ParseInt32</span><span class="st-frame-params">(<span class="st-param-type">ReadOnlySpan`1</span> <span class="st-param-name">value</span>, <span class="st-param-type">NumberStyles</span> <span class="st-param-name">styles</span>, <span class="st-param-type">NumberFormatInfo</span> <span class="st-param-name">info</span>)</span></span>
                                   at <span class="st-frame"><span class="st-type">System.Int32</span>.<span class="st-method">Parse</span><span class="st-frame-params">(<span class="st-param-type">String</span> <span class="st-param-name">s</span>)</span></span>
                                   at <span class="st-frame"><span class="st-type">MyNamespace.IntParser</span>.<span class="st-method">Parse</span><span class="st-frame-params">(<span class="st-param-type">String</span> <span class="st-param-name">s</span>)</span></span> in C:\apps\MyNamespace\IntParser.cs:<span class="st-line">line 11</span>
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

        Assert.Equal("english", beautify.GetLanguage());

        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Beautify Test with English Stack Trace
    /// </summary>
    [Fact]
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

        Assert.Equal("english", beautify.GetLanguage());

        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Beautify Test with English.
    /// </summary>
    [Fact]
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

        Assert.Equal("english", beautify.GetLanguage());

        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Beautify Test with German Stack Trace
    /// </summary>
    [Fact]
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

        Assert.Equal("german", beautify.GetLanguage());

        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Beautify Test with Danish Stack Trace
    /// </summary>
    [Fact]
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

        Assert.Equal("danish", beautify.GetLanguage());

        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Beautify Test with Russian Stack Trace
    /// </summary>
    [Fact]
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

        Assert.Equal("russian", beautify.GetLanguage());

        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Beautify Test with Chinese Stack Trace
    /// </summary>
    [Fact]
    public void StackTraceChineseTest1()
    {
        const string expected = "System.Exception: Could not load file or assembly 'netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'. 系统找不到指定的文件。 在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw() 在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task) 在 ClrCustomVisualizerVSHost.VisualizerTargetInternal.<span>&lt;</span>RequestDataAsync<span>&gt;</span>d__10.MoveNext()  在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw() 在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task) 在 Microsoft.VisualStudio.OutOfProcessVisualizers.VisualizerTarget.<span>&lt;</span>RequestDataAsync<span>&gt;</span>d__10.MoveNext()";

        const string stack = "System.Exception: Could not load file or assembly 'netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'. 系统找不到指定的文件。 在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw() 在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task) 在 ClrCustomVisualizerVSHost.VisualizerTargetInternal.&lt;RequestDataAsync&gt;d__10.MoveNext() --- 引发异常的上一位置中堆栈跟踪的末尾 --- 在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw() 在 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task) 在 Microsoft.VisualStudio.OutOfProcessVisualizers.VisualizerTarget.&lt;RequestDataAsync&gt;d__10.MoveNext()";

        var beautify = new StackTraceBeautify();

        var result = beautify.Beautify(stack);

        Assert.Equal("chinese", beautify.GetLanguage());

        Assert.Equal(expected, result);
    }
}