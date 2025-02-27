// Copyright (c) ZeroC, Inc.

using VisitorCenter;

namespace Server;

/// <summary>Chatbot is an Ice servant that implements Slice interface Greeter.</summary>
internal class Chatbot : GreeterDisp_
{
    private readonly string _greeterName;

    /// <inheritdoc/>
    // Implements the abstract method Greet from the GreeterDisp_ class generated by the Slice compiler.
    // This variant is the synchronous implementation.
    public override string Greet(string name, Ice.Current current)
    {
        Console.WriteLine($"Dispatching greet request {{ name = '{name}' }}");
        return $"Hello, {name}! My name is {_greeterName}. How are you doing today?";
    }

    /// <summary>Constructs a Chatbot servant.</summary>
    /// <param name="greeterName">The name of the greeter.</param>
    internal Chatbot(string greeterName) => _greeterName = greeterName;
}
