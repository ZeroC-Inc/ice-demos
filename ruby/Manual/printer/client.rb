#!/usr/bin/env ruby
# Copyright (c) ZeroC, Inc.

require 'Ice'
Ice::loadSlice('Printer.ice')

Ice::initialize(ARGV) do |communicator|
    base = communicator.stringToProxy("SimplePrinter:default -h localhost -p 10000")
    printer = Demo::PrinterPrx::checkedCast(base)
    if not printer
        raise "Invalid proxy"
    end

    printer.printString("Hello World!")
end
