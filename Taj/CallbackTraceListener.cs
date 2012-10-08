using System;
using System.Diagnostics;

namespace Taj
{
    internal class CallbackTraceListener : DefaultTraceListener
    {
        private readonly Action<string> toCall;
        internal CallbackTraceListener(Action<string> onTraceEvent)
        {
            toCall = onTraceEvent;
        }

        public override void WriteLine(string message)
        {
            toCall(message);
            base.WriteLine(message);
        }
    }
}