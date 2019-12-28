using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BasicFunction.Helper
{
    public class Win32Helper
    {
        /// <summary>
        /// 单个按键
        /// </summary>
        /// <param name="key"></param>
        public static void SendKeyByVirtualSoftKeyboard(Key key)
        {
            keybd_event((byte)KeyInterop.VirtualKeyFromKey(key), 0, 0, 0);//按下
            keybd_event((byte)KeyInterop.VirtualKeyFromKey(key), 0, 2, 0);//松开
        }

        /// <summary>
        /// SendKeyPressDown和SendKeyPressUp 相互配对 用于组合键
        /// </summary>
        /// <param name="key"></param>
        public static void SendKeyPressDown(Key key)
        {
            keybd_event((byte)KeyInterop.VirtualKeyFromKey(key), 0, 0, 0);//按下

        }

        /// <summary>
        /// SendKeyPressDown和SendKeyPressUp 相互配对 用于组合键
        /// </summary>
        /// <param name="key"></param>
        public static void SendKeyPressUp(Key key)
        {
            keybd_event((byte)KeyInterop.VirtualKeyFromKey(key), 0, 2, 0);//松开
        }

        /// <summary>
        /// true是出于大写状态 false是小写状态
        /// </summary>
        public static bool CapsLockStatus
        {
            get
            {
                var bs = new byte[256];
                GetKeyboardState(bs);
                return bs[0x14] == 1;
            }
        }

        /// <summary>
        /// 获取由key转化的字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static char GetCharFromKey(Key key)
        {
            char ch = ' ';

            int virtualKey = KeyInterop.VirtualKeyFromKey(key);
            var keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            uint scanCode = MapVirtualKey((uint)virtualKey, MapType.MAPVK_VK_TO_VSC);
            var stringBuilder = new StringBuilder(2);

            int result = ToUnicode((uint)virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0);
            switch (result)
            {
                case -1:
                    break;
                case 0:
                    break;
                case 1:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
                default:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
            }
            return ch;
        }

        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        [DllImport("user32.dll", EntryPoint = "GetKeyboardState")]
        private static extern int GetKeyboardState(byte[] pbKeyState);
        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, MapType uMapType);
        [DllImport("user32.dll")]
        private static extern int ToUnicode(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [Out, MarshalAs( UnmanagedType.LPWStr, SizeParamIndex = 4 )]
            StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags);
        public enum MapType : uint
        {
            MAPVK_VK_TO_VSC = 0x0,
            MAPVK_VSC_TO_VK = 0x1,
            MAPVK_VK_TO_CHAR = 0x2,
            MAPVK_VSC_TO_VK_EX = 0x3,
        }
    }
}
