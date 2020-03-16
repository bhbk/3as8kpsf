using System;
using System.Diagnostics;
using System.Collections;
using System.Text;

namespace Bhbk.Lib.Msft.Win.Sys.Log
{
    public class security
    {
        public static void write(String executingassembly, String method, Exception ex)
        {
            EventLog evtLog = new EventLog("Security", ".", executingassembly);
            StringBuilder evtEntry = new StringBuilder(executingassembly);
            evtEntry.Append(Environment.NewLine + method);
            evtEntry.Append(Environment.NewLine + Environment.NewLine + "Exception: " + ex.Message);
            evtEntry.Append(Environment.NewLine + Environment.NewLine + "Stack Trace: " + ex.StackTrace);
            evtLog.WriteEntry(evtEntry.ToString(), EventLogEntryType.Error);
            evtLog.Close();
        }
    }
}
