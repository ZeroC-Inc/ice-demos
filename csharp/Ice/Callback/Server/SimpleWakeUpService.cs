// Copyright (c) ZeroC, Inc.

using EarlyRiser;

namespace Server;

/// <summary>SimpleWakeUpService is an Ice servant that implements Slice interface WakeUpService.</summary>
internal class SimpleWakeUpService : WakeUpServiceDisp_
{
    // Implements the abstract method WakeMeUp from the WakeUpServiceDisp_ class generated by the Slice compiler.
    public override void WakeMeUp(AlarmClockPrx? alarmClock, long timeStamp, Ice.Current current)
    {
        if (alarmClock is null)
        {
            // Should never happen, but in case it does, the Ice runtime will send an Ice.UnknownException to the
            // client.
            throw new ArgumentNullException(nameof(alarmClock));
        }

        var timeStampDateTime = new DateTime(timeStamp, DateTimeKind.Utc);

        Console.WriteLine(
            $"Dispatching wakeMeUp request {{ alarmClock = '{alarmClock}', timeStamp = '{timeStampDateTime.ToLocalTime()}' }}");

        // Start a background task to ring the alarm clock.
        _ = Task.Run(async () =>
        {
            // Wait until the specified time.
            TimeSpan delay = timeStampDateTime - DateTime.UtcNow;
            if (delay > TimeSpan.Zero)
            {
                await Task.Delay(delay);
            }

            // First ring.
            ButtonPressed buttonPressed = await alarmClock.RingAsync("It's time to wake up!");

            // Keep ringing every 5 seconds until the user presses the stop button.
            while (buttonPressed is ButtonPressed.Snooze)
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                buttonPressed = await alarmClock.RingAsync("No more snoozing!");
            }
        });
    }
}
