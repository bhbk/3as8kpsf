using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Bhbk.Lib.Msft.Win.Sys.Mouse
{
    public static class Click
    {
        public static void Move()
        {
            try
            {
                POINT clk = new POINT();
                float scrnwidth = 0, scrnheight = 0;
                float width = 0, height = 0;

                // Wait a moment before you simulate a mouse click.
                Thread.Sleep(333);

                // Save old cursor position.
                GetCursorPos(ref clk);

                // Get screen resolution.
                scrnwidth = GetSystemMetrics(SM_CXSCREEN);
                scrnheight = GetSystemMetrics(SM_CYSCREEN);

                width = SCREEN_SCALE / scrnwidth;
                height = SCREEN_SCALE / scrnheight;

                // Move mouse back to original position.
                SetCursorPos(clk.x + (int)width, clk.y + (int)height);
            }
            catch (Exception ex)
            {
                Bhbk.Lib.Msft.Win.Sys.Log.application.write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        /*
        public static void Single(IntPtr hwnd, Objects.XY xy)
        {
            try
            {
                Random r = new Random(hwnd.ToInt32());
                RECT pwin = new RECT();
                POINT clk = new POINT();
                INPUT[] input = new INPUT[2];
                float scrnwidth = 0, scrnheight = 0;
                float pwinleft = 0, pwintop = 0, pwinright = 0, pwinbottom = 0;
                float cwinleft = 0, cwintop = 0, cwinright = 0, cwinbottom = 0;

                // Wait a moment before you simulate a mouse click.
                Thread.Sleep(333);

                // Save old cursor position.
                GetCursorPos(ref clk);

                // Get screen resolution.
                scrnwidth = GetSystemMetrics(SM_CXSCREEN);
                scrnheight = GetSystemMetrics(SM_CYSCREEN);

                // Get client window dimensions.
                GetWindowRect(hwnd, ref pwin);

                // Map window location into same coordinate space used by mouse_event.
                pwinleft = ((float)pwin.Left / scrnwidth) * SCREEN_SCALE;
                pwintop = ((float)pwin.Top / scrnheight) * SCREEN_SCALE;
                pwinright = ((float)pwin.Right / scrnwidth) * SCREEN_SCALE;
                pwinbottom = ((float)pwin.Bottom / scrnheight) * SCREEN_SCALE;

                // Map button location relative to window position into coordinate space used by mouse_event.
                cwinleft = ((float)xy.x1 / scrnwidth) * SCREEN_SCALE;
                cwintop = ((float)xy.y1 / scrnheight) * SCREEN_SCALE;
                cwinright = ((float)xy.x2 / scrnwidth) * SCREEN_SCALE;
                cwinbottom = ((float)xy.y2 / scrnheight) * SCREEN_SCALE;

                if (pwinleft < 1
                    || pwintop < 1
                    || pwinright < 1
                    || pwinbottom < 1)
                {
                    //                    throw new Exception("Invalid screen coordinates for parent window (" + pwinleft + ":" + pwintop + ":" + pwinright + ":" + pwinbottom + ")");
                }
                if (cwinleft < 1
                    || cwintop < 1
                    || cwinright < 1
                    || cwinbottom < 1)
                {
                    //                    throw new Exception("Invalid screen coordinates for child window (" + cwinleft + ":" + cwintop + ":" + cwinright + ":" + cwinbottom + ")");
                }

                // Construct mouse move structures.
                input[0].type = INPUT_MOUSE;
                input[0].mi.dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE;
                input[0].mi.dx = (int)pwinleft + (int)cwinleft + (int)r.Next(1, (int)cwinright - (int)cwinleft);
                input[0].mi.dy = (int)pwintop + (int)cwintop + (int)r.Next(1, (int)cwinbottom - (int)cwintop);

                if (input[0].mi.dx < 1 || input[0].mi.dx > SCREEN_SCALE
                    || input[0].mi.dy < 1 || input[0].mi.dy > SCREEN_SCALE)
                {
                    throw new Exception("Invalid screen coordinates (" + pwinleft + ":" + pwintop + ":" + pwinright + ":" + pwinbottom + ")");
                }

                // Construct mouse click structures.
                input[1].type = INPUT_MOUSE;
                input[1].mi.dwFlags = MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP;

                // Send array of mouse events for processing.
                SendInput((uint)input.Length, input, Marshal.SizeOf(typeof(INPUT)));

                // Move mouse back to original position.
                SetCursorPos(clk.x, clk.y);
            }
            catch (Exception ex)
            {
                Bhbk.Lib.Msft.Win.Sys.Log.application.write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        */
        // GetSystemMetrics() constants.
        private const int SM_CXSCREEN = 0x00000000;
        private const int SM_CYSCREEN = 0x00000001;
        private const int SM_CXFULLSCREEN = 0x00000010;
        private const int SM_CYFULLSCREEN = 0x00000011;
        // Mouse_Event() & SendInput() sees xy coordinates as 0-65535.
        private const int SCREEN_SCALE = 0x0000FFFF;
        // SendInput() constants.
        private const int INPUT_KEYBOARD = 0x00000001;
        private const int INPUT_MOUSE = 0x00000000;
        private const int MOUSEEVENTF_MOVE = 0x00000001;
        private const int MOUSEEVENTF_LEFTDOWN = 0x00000002;
        private const int MOUSEEVENTF_LEFTUP = 0x00000004;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x00000008;
        private const int MOUSEEVENTF_RIGHTUP = 0x00000010;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x00000020;
        private const int MOUSEEVENTF_MIDDLEUP = 0x00000040;
        private const int MOUSEEVENTF_ABSOLUTE = 0x00008000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetCursorPos(ref POINT p);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetSystemMetrics(int smIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetCursorPos(int x, int y);

        [StructLayout(LayoutKind.Explicit)]
        private struct INPUT
        {
            [FieldOffset(0)]
            public int type;
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            [FieldOffset(4)]
            public KEYBDINPUT ki;
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}
