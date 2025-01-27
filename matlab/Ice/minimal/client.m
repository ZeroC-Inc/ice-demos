% Copyright (c) ZeroC, Inc.

function client(args)
    addpath('generated');
    if ~libisloaded('ice')
        loadlibrary('ice', @iceproto);
    end

    import Demo.*

    if nargin == 0
        args = {};
    end

    try
        communicator = Ice.initialize(args);
        cleanup = onCleanup(@() communicator.destroy());
        hello = Demo.HelloPrx.checkedCast(communicator.stringToProxy('hello:default -h localhost -p 10000'));
        hello.sayHello();
    catch ex
        fprintf('%s\n', getReport(ex));
    end
    rmpath('generated');
end
