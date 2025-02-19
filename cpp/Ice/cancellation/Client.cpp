// Copyright (c) ZeroC, Inc.

#include "../../common/env.h"
#include "Greeter.h"

#include <chrono>
#include <cstdlib>
#include <iostream>

using namespace std;

int
main(int argc, char* argv[])
{
    // Helper function to get the username of the current user.
    const string name = common::getUsername();

    // Create an Ice communicator to initialize the Ice runtime. The CommunicatorHolder is a RAII helper that creates
    // the communicator in its constructor and destroys it when it goes out of scope.
    const Ice::CommunicatorHolder communicatorHolder{argc, argv};
    const Ice::CommunicatorPtr& communicator = communicatorHolder.communicator();

    // GreeterPrx is a class generated by the Slice compiler. We create a proxy from a communicator and a "stringified
    // proxy" with the address of the target object.
    // If you run the server on a different computer, replace localhost in the string below with the server's hostname
    // or IP address.
    VisitorCenter::GreeterPrx greeter{communicator, "greeter:tcp -h localhost -p 4061"};

    // Create a proxy to the slow greeter. It uses the same connection as the regular greeter.
    VisitorCenter::GreeterPrx slowGreeter{communicator, "slowGreeter:tcp -h localhost -p 4061"};

    // Send a request to the regular greeter and get the response.
    string greeting = greeter->greet(name);
    cout << greeting << endl;

    // Create another slow greeter proxy with an invocation timeout of 4 seconds (the default invocation timeout is
    // infinite).
    VisitorCenter::GreeterPrx slowGreeter4s = slowGreeter->ice_invocationTimeout(4s);

    // Send a request to the slow greeter with the 4-second invocation timeout.
    try
    {
        greeting = slowGreeter4s->greet("alice");
        cout << "Received unexpected greeting: " << greeting << endl;
    }
    catch (const Ice::InvocationTimeoutException& exception)
    {
        cout << "Caught InvocationTimeoutException, as expected: " << exception.what() << endl;
    }

    // Send a request to the slow greeter, and cancel this request after 4 seconds.
    function<void()> cancel = slowGreeter->greetAsync(
        name,
        [](const string& greeting) { cout << "Received unexpected greeting: " << greeting << endl; },
        [](exception_ptr exceptionPtr)
        {
            try
            {
                rethrow_exception(exceptionPtr);
            }
            catch (const Ice::InvocationCanceledException& ex)
            {
                cout << "Caught InvocationCanceledException, as expected: " << ex.what() << endl;
            }
        });

    this_thread::sleep_for(4s);
    cancel();

    // Verify the regular greeter still works.
    greeting = greeter->greet("carol");
    cout << greeting << endl;

    // Send a request to the slow greeter, and wait forever for the response.
    cout << "Please press Ctrl+C in the server's terminal to cancel the slow greeter dispatch." << endl;
    try
    {
        greeting = slowGreeter->greet("dave");
        cout << greeting << endl;
    }
    catch (const Ice::UnknownException& exception)
    {
        cout << "Caught UnknownException, as expected: " << exception.what() << endl;
    }

    return 0;
}
