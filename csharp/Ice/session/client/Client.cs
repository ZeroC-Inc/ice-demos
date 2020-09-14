//
// Copyright (c) ZeroC, Inc. All rights reserved.
//

using Demo;
using System;
using System.Configuration;
using System.Collections.Generic;
using ZeroC.Ice;

// The new communicator is automatically destroyed (disposed) at the end of the using statement
using var communicator = new Communicator(ref args, ConfigurationManager.AppSettings);

// The communicator initialization removes all Ice-related arguments from args
if(args.Length > 0)
{
    throw new ArgumentException("too many arguments");
}

string? name;
do
{
    Console.Out.Write("Please enter your name ==> ");
    Console.Out.Flush();

    name = Console.In.ReadLine();
    if (name == null)
    {
        return;
    }
    name = name.Trim();
}
while (name.Length == 0);

ISessionFactoryPrx factory = communicator.GetPropertyAsProxy("SessionFactory.Proxy", ISessionFactoryPrx.Factory) ??
    throw new ArgumentException("invalid proxy");

ISessionPrx session = factory.Create(name);

var hellos = new List<IHelloPrx>();

menu();

bool destroy = true;
bool shutdown = false;
while (true)
{
    Console.Out.Write("==> ");
    Console.Out.Flush();
    string line = Console.In.ReadLine();
    if(line == null)
    {
        break;
    }
    
    if (int.TryParse(line, out int index))
    {
        if (index < hellos.Count)
        {
            IHelloPrx hello = hellos[index];
            hello.SayHello();
        }
        else
        {
            Console.Out.WriteLine(
                $"Index is too high. {hellos.Count} hello objects exist so far.\nUse `c' to create a new hello object.");
        }
    }
    else if (line == "c")
    {
        hellos.Add(session.CreateHello());
        Console.Out.WriteLine($"Created hello object {(hellos.Count - 1)}");
    }
    else if (line == "s")
    {
        destroy = false;
        shutdown = true;
        break;
    }
    else if (line == "x")
    {
        break;
    }
    else if (line == "t")
    {
        destroy = false;
        break;
    }
    else if (line == "?")
    {
        menu();
    }
    else
    {
        Console.Out.WriteLine($"Unknown command `{line}'.");
        menu();
    }
}

if (destroy)
{
    session.Destroy();
}

if (shutdown)
{
    factory.Shutdown();
}

static void menu()
{
    Console.Out.WriteLine(
@"usage:
    c:     create a new per-client hello object
    0-9:   send a greeting to a hello object
    s:     shutdown the server and exit
    x:     exit
    t:     exit without destroying the session
    ?:     help
");
}
