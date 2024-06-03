// Copyright (c) ZeroC, Inc.

using Demo;

public class DiscoverReplyI : DiscoverReplyDisp_
{
    public override void
    reply(Ice.ObjectPrx obj, Ice.Current current)
    {
        lock (this)
        {
            if (_obj == null)
            {
                _obj = obj;
            }
            Monitor.Pulse(this);
        }
    }

    public Ice.ObjectPrx
    WaitReply(long timeout)
    {
        lock (this)
        {
            long end = (System.DateTime.Now.Ticks / 1000) + timeout;
            while (_obj == null)
            {
                int delay = (int)(end - (System.DateTime.Now.Ticks / 1000));
                if (delay > 0)
                {
                    Monitor.Wait(this, delay);
                }
                else
                {
                    break;
                }
            }
            return _obj;
        }
    }

    private Ice.ObjectPrx _obj;
}
