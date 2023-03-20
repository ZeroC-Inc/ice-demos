//
// Copyright (c) ZeroC, Inc. All rights reserved.
//

using Demo;
using System;
using System.Threading;

internal class CalculatorI : CalculatorDisp_
{
    private readonly ManualResetEventSlim _shutdownRequested;

    public override int add(int x, int y, Ice.Current current)
    {
        return x + y;
    }

    public override int subtract(int x, int subtrahend, Ice.Current current)
    {
        return x - subtrahend;
    }

    public override int divide(int numerator, int denominator, out int remainder, Ice.Current current)
    {
        if (denominator == 0)
        {
            throw new Demo.DivideByZeroException();
        }
        remainder = numerator % denominator;
        return numerator / denominator;
    }

    public override int square(int x, Ice.Current current)
    {
        return x * x;
    }

    public override double squareRoot(int x, Ice.Current current)
    {
        if (x < 0)
        {
            throw new NegativeRootException();
        }
        return Math.Sqrt(x);
    }

    public override void shutdown(Ice.Current current)
    {
        _shutdownRequested.Set();
    }

    internal CalculatorI(ManualResetEventSlim shutdownRequested)
    {
        _shutdownRequested = shutdownRequested;
    }
}
