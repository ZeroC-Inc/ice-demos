//
// Copyright (c) ZeroC, Inc. All rights reserved.
//

#include "Hello.h"
#include <Ice/Ice.h>
#include <iostream>

using namespace std;
using namespace Demo;

int run(const shared_ptr<Ice::Communicator>&);

int
main(int argc, char* argv[])
{
    int status = 0;

    try
    {
        //
        // CommunicatorHolder's ctor initializes an Ice communicator,
        // and its dtor destroys this communicator.
        //
        const Ice::CommunicatorHolder ich(argc, argv, "config.client");

        //
        // The communicator initialization removes all Ice-related arguments from argc/argv
        //
        if (argc > 1)
        {
            cerr << argv[0] << ": too many arguments" << endl;
            status = 1;
        }
        else
        {
            status = run(ich.communicator());
        }
    }
    catch (const std::exception& ex)
    {
        cerr << argv[0] << ": " << ex.what() << endl;
        status = 1;
    }

    return status;
}

void menu();

int
run(const shared_ptr<Ice::Communicator>& communicator)
{
    auto hello = Ice::checkedCast<HelloPrx>(communicator->propertyToProxy("Hello.Proxy"));
    if (!hello)
    {
        cerr << "invalid proxy" << endl;
        return 1;
    }

    menu();

    char c = 'x';
    do
    {
        try
        {
            cout << "==> ";
            cin >> c;
            if (c == 't')
            {
                hello->sayHello();
            }
            else if (c == 's')
            {
                hello->shutdown();
            }
            else if (c == 'x')
            {
                // Nothing to do
            }
            else if (c == '?')
            {
                menu();
            }
            else
            {
                cout << "unknown command `" << c << "'" << endl;
                menu();
            }
        }
        catch (const Ice::Exception& ex)
        {
            cerr << ex << endl;
        }
    } while (cin.good() && c != 'x');

    return 0;
}

void
menu()
{
    cout << "usage:\n"
            "t: send greeting\n"
            "s: shutdown server\n"
            "x: exit\n"
            "?: help\n";
}
