using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RemoteMouseServer
{
    class RemoteKeybd
    {
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
        public const int KEYEVENTF_EXTENDEDKEY = 0x0001; //Key down flag
        public const int KEYEVENTF_KEYUP = 0x0002; //Key up flag
        public static string KeybdButton
        {
            set
            {
                string[] keyState;
                keyState = value.Split(';');
                if(keyState[1] == "down")
                {
                    keybd_event((byte)Int32.Parse(keyState[2]), 0, KEYEVENTF_EXTENDEDKEY, 0);
                }
                else
                {
                    keybd_event((byte)Int32.Parse(keyState[2]), 0, KEYEVENTF_KEYUP, 0);
                }
            }
        }
        }
}
