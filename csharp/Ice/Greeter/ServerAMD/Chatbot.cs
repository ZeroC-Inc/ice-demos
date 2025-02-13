// Copyright (c) ZeroC, Inc.

using VisitorCenter;

namespace ServerAMD;

/// <summary>Chatbot is an Ice servant that implements Slice interface Greeter.</summary>
internal class Chatbot : GreeterDisp_
{
    /// <inheritdoc/>
    // Implements the abstract method GreetAsync from the GreeterDisp_ class generated by the Slice compiler.
    // This version uses the Asynchronous Method Dispatch (AMD) variant.
    public override async Task<string> GreetAsync(string name, Ice.Current current)
    {
        await Task.Delay(TimeSpan.FromSeconds(1)); // Simulate a delay

        // We're now in a .NET thread pool thread.
        Console.WriteLine($"Dispatching greet request {{ name = '{name}' }}");
        return $"Hello, {name}!";
    }
}
