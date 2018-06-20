

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Common.Logging;
using Common.Logging.Simple;

namespace Oragon.Spring.Aspects.Exceptions
{
    public class CaptureOutputLoggerFactoryAdapter : IDisposable
    {
        private class CapturingTraceListener : TraceListener
        {
            private readonly CaptureOutputLoggerFactoryAdapter adapter;

            public CapturingTraceListener(CaptureOutputLoggerFactoryAdapter adapter)
            {
                this.adapter = adapter;
            }

            public override void Write(string message)
            {
                adapter.LogMessages.Add(message);
            }

            public override void WriteLine(string message)
            {
                this.Write(message);
            }
        }

        private readonly CapturingTraceListener listener;

        public CaptureOutputLoggerFactoryAdapter()
        {
            listener = new CapturingTraceListener(this);
            System.Diagnostics.Trace.Listeners.Add(listener);            
        }

        public void Dispose()
        {
            System.Diagnostics.Trace.Listeners.Remove(listener);
        }

        private IList<string> logMessages = new List<string>();

        public IList<string> LogMessages
        {
            get { return logMessages; }
            set { logMessages = value; }
        }

        #region ILoggerFactoryAdapter Members

       

        #endregion
    }
}