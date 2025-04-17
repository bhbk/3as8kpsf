using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace Bhbk.Lib.Msft.Proc.Trampoline.Client
{
    public static class connect
    {
        // GetSystemMetrics() constants.
        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;
        private const uint OPEN_EXISTING = 0x00000003;
        private const int ERROR_PIPE_BUSY = 0x00000231;
        private const int ERROR_MORE_DATA = 0x00000234;
        private const int NMPWAIT_USE_DEFAULT_WAIT = 0x00000000;
        private const int PIPE_READMODE_MESSAGE = 0x00000002;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hSnapshot);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFile(String lpFileName,
            uint dwDesiredAccess,
            uint dwSharedMode,
            uint lpSecurityAttributes,
            uint dwCreateDisposition,
            uint dwFlagsAndAttributes,
            uint hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern uint GetLastError();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetNamedPipeHandleState(IntPtr hNamedPipe,
            ref uint lpMode,
            uint lpMaxCollectionCount,
            uint lpCollectDataTimeout);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadFile(IntPtr hFile,
            System.Text.StringBuilder lpBuffer,
            uint nNumberOfBytesToRead,
            ref uint nNumberOfBytesRead,
            uint lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WaitNamedPipe(String lpNamedPipeName,
            uint nTimeout);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteFile(IntPtr hFile,
            System.Text.StringBuilder lpBuffer,
            uint nNumberOfBytesToWrite,
            ref uint nNumberOfBytesWritten,
            uint lpOverlapped);

        public static String Client(String pipePath, UInt32 pipeBufferSize, String pbTargetExecutable)
        {
            // http://msdn.microsoft.com/en-us/library/aa365592(VS.85).aspx
            StringBuilder chBuf = new StringBuilder((Int32)pipeBufferSize);
            StringBuilder pszMsg = new StringBuilder(pbTargetExecutable);
            IntPtr hPipe;
            bool fSuccess;
            uint cbRead = 0;
            uint cbWritten = 0;
            uint dwMode;

            // Try to open a named pipe; wait for it, if necessary. */
            for (; ;)
            {
                hPipe = CreateFile(
                    pipePath,                       // pipe name.
                    GENERIC_READ | GENERIC_WRITE,   // read and write access.
                    0,                              // no sharing.
                    0,                              // default security attributes.
                    OPEN_EXISTING,                  // opens existing pipe.
                    0,                              // default attributes.
                    0);                             // no template file.

                // Break if the pipe handle is valid.
                if (hPipe != IntPtr.Zero)
                {
                    break;
                }

                // Exit if an error other than ERROR_PIPE_BUSY occurs.
                if (GetLastError() != ERROR_PIPE_BUSY)
                {
                    return "<ERROR>";
                }

                // All pipe instances are busy, so wait for 20 seconds.
                if (!WaitNamedPipe(pipePath, NMPWAIT_USE_DEFAULT_WAIT))
                {
                    return "<ERROR>";
                }
            }

            // The pipe connected; change to message-read mode.
            dwMode = PIPE_READMODE_MESSAGE;

            fSuccess = SetNamedPipeHandleState(
                hPipe,                              // pipe handle.
                ref dwMode,                         // new pipe mode.
                0,                                  // don't set maximum bytes.
                0);                                 // don't set maximum time.

            if (!fSuccess)
            {
                return "<ERROR>";
            }

            // Make sure we don't send any messages larger than the buffer size.
            if (pszMsg.Length < pipeBufferSize - 1)
            {
                // Send a message to the pipe server. */
                fSuccess = WriteFile(
                    hPipe,                          // pipe handle.
                    pszMsg,                         // message.
                    (UInt32)(pszMsg.Length + 1),    // number of bytes to written to file.
                    ref cbWritten,                  // bytes written.
                    0);                             // not overlapped.

                if (!fSuccess)
                {
                    return "<ERROR>";
                }

                do
                {
                    // Read from the pipe.
                    fSuccess = ReadFile(
                        hPipe,                      // pipe handle.
                        chBuf,                      // buffer to receive reply.
                        pipeBufferSize,             // number of bytes to be read from file.
                        ref cbRead,                 // number of bytes read.
                        0);                         // not overlapped.


                    if (!fSuccess && GetLastError() != ERROR_MORE_DATA)
                    {
                        break;
                    }
                // repeat loop if ERROR_MORE_DATA.
                } while (!fSuccess);
            }
            CloseHandle(hPipe);

            return chBuf.ToString();
        }
    }
}
