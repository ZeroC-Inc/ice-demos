// Copyright (c) ZeroC, Inc. All rights reserved.

using Demo;
using System;
using System.Configuration;
using ZeroC.Ice;

try
{
    // using statement - communicator is automatically destroyed at the end of this statement
    await using var communicator = new Communicator(ref args, ConfigurationManager.AppSettings);
    await communicator.ActivateAsync();

    // Destroy the communicator on Ctrl+C or Ctrl+Break
    Console.CancelKeyPress += async (sender, eventArgs) => await communicator.DestroyAsync();

    if (args.Length > 0)
    {
        Console.Error.WriteLine("too many arguments");
        return 1;
    }

    ICallbackSenderPrx server = communicator.GetPropertyAsProxy("CallbackSender.Proxy", ICallbackSenderPrx.Factory) ??
        throw new ArgumentException("invalid proxy");

    // Create an object adapter with no name and no endpoints for receiving callbacks over bidirectional connections.
    ObjectAdapter adapter = communicator.CreateObjectAdapter();

    // Register the callback receiver servant with the object adapter
    ICallbackReceiverPrx proxy = adapter.AddWithUUID(new CallbackReceiver(), ICallbackReceiverPrx.Factory);

    // Associate the object adapter with the bidirectional connection.
    server.GetConnection()!.Adapter = adapter;

    // Provide the proxy of the callback receiver object to the server and wait for shutdown.
    server.AddClient(proxy);
    await communicator.WaitForShutdownAsync();
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex);
    return 1;
}
return 0;
