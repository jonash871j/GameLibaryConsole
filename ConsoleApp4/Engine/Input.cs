using System.Runtime.InteropServices;

namespace Engine
{
    public static class Input
    {
        [DllImport("user32.dll")]
        private static extern int GetAsyncKeyState(short key);

        private const byte NOT_PRESSED = 0;
        private const byte IS_PRESSED = 1;
        private const byte FIRST_PRESS = 2;
        private const byte IS_RELEASED = 3;

        private enum InputState
        {
            Unlocked = 0,
            Triggered,
            Locked,
        }

        private static InputState[] keyMap = new InputState[255];

        /// <summary>
        /// Gets keystate for a virtual key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true if keystate is true</returns>
        public static bool KeyState(Key key)
        {
            if (GetAsyncKeyState((short)key) >= 0x8000)
                return true;
            else return false;
        }
        public static bool KeyAnyState()
        {
            for (int i = 0; i < 255; i++)
                if (KeyState((Key)i))
                    return true;
            return false;
        }

        /// <summary>
        /// Gets pressed state for a virtual key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true if key is pressed</returns>
        public static bool KeyPressed(Key key)
        {
            if (KeyState(key))
            {
                if (keyMap[(short)key] != InputState.Locked)
                {
                    keyMap[(short)key] = InputState.Triggered;
                    return true;
                }
            }
            else
            {
                keyMap[(short)key] = InputState.Unlocked;
            }
            return false;
        }
        public static bool KeyAnyPressed()
        {
            for (int i = 0; i < 255; i++)
                if (KeyPressed((Key)i))
                    return true;
            return false;
        }

        /// <summary>
        /// Gets released state for a virtual key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true if key is released</returns>
        public static bool KeyReleased(Key key)
        {
            //if ((GetAsyncKeyState((short)key) >= 0x8000) && (keyMap[(short)key] == NOT_PRESSED))
            //    keyMap[(short)key] = IS_PRESSED;

            //if ((GetAsyncKeyState((short)key) == 0) && (keyMap[(short)key] == IS_PRESSED))
            //    keyMap[(short)key] = IS_RELEASED;


            //if (keyMap[(short)key] == IS_RELEASED)
            //{
            //    keyMap[(short)key] = NOT_PRESSED;
            //    return true;
            //}
            return false;
        }
        public static bool KeyAnyReleased()
        {
            for (int i = 0; i < 255; i++)
                if (KeyReleased((Key)i))
                    return true;
            return false;
        }

        /// <summary>
        /// Reads string
        /// </summary>
        public static string Read(string text)
        {
            for (int i = 65; i <= 90; i++)
            {
                if ((KeyState(Key.SHIFT)) && (KeyPressed((Key)i)))
                    return text + (char)i;

                if (KeyPressed((Key)i))
                    return text + (char)(i + 32);
            }

            for (int i = 48; i <= 57; i++)
                if (KeyPressed((Key)i))
                    return text + (char)i;

            if ((KeyPressed(Key.BACK)) && (text.Length >= 1))
                return text.Remove(text.Length - 1);

            if (KeyPressed(Key.SPACE))
                return text + " ";

            if (KeyPressed(Key.RETURN))
                return text + '\n';

            return text;
        }

        internal static void Update()
        {
            for (int i = 0; i < keyMap.Length; i++)
            {
                if (keyMap[i] == InputState.Triggered)
                    keyMap[i] = InputState.Locked;
            }
        }
    }
    public enum Key
    {
        LBUTTON = 0x01,
        RBUTTON = 0x02,
        CANCEL = 0x03,
        MBUTTON = 0x04,
        XBUTTON1 = 0x05,
        XBUTTON2 = 0x06,
        BACK = 0x08,
        TAB = 0x09,
        CLEAR = 0x0C,
        RETURN = 0x0D,
        SHIFT = 0x10,
        CONTROL = 0x11,
        MENU = 0x12,
        PAUSE = 0x13,
        CAPITAL = 0x14,
        KANA = 0x15,
        HANGEUL = 0x15,
        HANGUL = 0x15,
        JUNJA = 0x17,
        FINAL = 0x18,
        HANJA = 0x19,
        KANJI = 0x19,
        ESCAPE = 0x1B,
        CONVERT = 0x1C,
        NONCONVERT = 0x1D,
        ACCEPT = 0x1E,
        MODECHANGE = 0x1F,
        SPACE = 0x20,
        PRIOR = 0x21,
        NEXT = 0x22,
        END = 0x23,
        HOME = 0x24,
        LEFT = 0x25,
        UP = 0x26,
        RIGHT = 0x27,
        DOWN = 0x28,
        SELECT = 0x29,
        PRINT = 0x2A,
        EXECUTE = 0x2B,
        SNAPSHOT = 0x2C,
        INSERT = 0x2D,
        DELETE = 0x2E,
        HELP = 0x2F,
        LWIN = 0x5B,
        RWIN = 0x5C,
        APPS = 0x5D,
        SLEEP = 0x5F,
        NUMPAD0 = 0x60,
        NUMPAD1 = 0x61,
        NUMPAD2 = 0x62,
        NUMPAD3 = 0x63,
        NUMPAD4 = 0x64,
        NUMPAD5 = 0x65,
        NUMPAD6 = 0x66,
        NUMPAD7 = 0x67,
        NUMPAD8 = 0x68,
        NUMPAD9 = 0x69,
        MULTIPLY = 0x6A,
        ADD = 0x6B,
        SEPARATOR = 0x6C,
        SUBTRACT = 0x6D,
        DECIMAL = 0x6E,
        DIVIDE = 0x6F,
        F1 = 0x70,
        F2 = 0x71,
        F3 = 0x72,
        F4 = 0x73,
        F5 = 0x74,
        F6 = 0x75,
        F7 = 0x76,
        F8 = 0x77,
        F9 = 0x78,
        F10 = 0x79,
        F11 = 0x7A,
        F12 = 0x7B,
        F13 = 0x7C,
        F14 = 0x7D,
        F15 = 0x7E,
        F16 = 0x7F,
        F17 = 0x80,
        F18 = 0x81,
        F19 = 0x82,
        F20 = 0x83,
        F21 = 0x84,
        F22 = 0x85,
        F23 = 0x86,
        F24 = 0x87,
        NAVIGATION_VIEW = 0x88,
        NAVIGATION_MENU = 0x89,
        NAVIGATION_UP = 0x8A,
        NAVIGATION_DOWN = 0x8B,
        NAVIGATION_LEFT = 0x8C,
        NAVIGATION_RIGHT = 0x8D,
        NAVIGATION_ACCEPT = 0x8E,
        NAVIGATION_CANCEL = 0x8F,
        NUMLOCK = 0x90,
        SCROLL = 0x91,
        OEM_NEC_EQUAL = 0x92,
        OEM_FJ_JISHO = 0x92,
        OEM_FJ_MASSHOU = 0x93,
        OEM_FJ_TOUROKU = 0x94,
        OEM_FJ_LOYA = 0x95,
        OEM_FJ_ROYA = 0x96,
        LSHIFT = 0xA0,
        RSHIFT = 0xA1,
        LCONTROL = 0xA2,
        RCONTROL = 0xA3,
        LMENU = 0xA4,
        RMENU = 0xA5,
        BROWSER_BACK = 0xA6,
        BROWSER_FORWARD = 0xA7,
        BROWSER_REFRESH = 0xA8,
        BROWSER_STOP = 0xA9,
        BROWSER_SEARCH = 0xAA,
        BROWSER_FAVORITES = 0xAB,
        BROWSER_HOME = 0xAC,
        VOLUME_MUTE = 0xAD,
        VOLUME_DOWN = 0xAE,
        VOLUME_UP = 0xAF,
        MEDIA_NEXT_TRACK = 0xB0,
        MEDIA_PREV_TRACK = 0xB1,
        MEDIA_STOP = 0xB2,
        MEDIA_PLAY_PAUSE = 0xB3,
        LAUNCH_MAIL = 0xB4,
        LAUNCH_MEDIA_SELECT = 0xB5,
        LAUNCH_APP1 = 0xB6,
        LAUNCH_APP2 = 0xB7,
        OEM_1 = 0xBA,
        OEM_PLUS = 0xBB,
        OEM_COMMA = 0xBC,
        OEM_MINUS = 0xBD,
        OEM_PERIOD = 0xBE,
        OEM_2 = 0xBF,
        OEM_3 = 0xC0,
        GAMEPAD_A = 0xC3,
        GAMEPAD_B = 0xC4,
        GAMEPAD_X = 0xC5,
        GAMEPAD_Y = 0xC6,
        GAMEPAD_RIGHT_SHOULDER = 0xC7,
        GAMEPAD_LEFT_SHOULDER = 0xC8,
        GAMEPAD_LEFT_TRIGGER = 0xC9,
        GAMEPAD_RIGHT_TRIGGER = 0xCA,
        GAMEPAD_DPAD_UP = 0xCB,
        GAMEPAD_DPAD_DOWN = 0xCC,
        GAMEPAD_DPAD_LEFT = 0xCD,
        GAMEPAD_DPAD_RIGHT = 0xCE,
        GAMEPAD_MENU = 0xCF,
        GAMEPAD_VIEW = 0xD0,
        GAMEPAD_LEFT_THUMBSTICK_BUTTON = 0xD1,
        GAMEPAD_RIGHT_THUMBSTICK_BUTTON = 0xD2,
        GAMEPAD_LEFT_THUMBSTICK_UP = 0xD3,
        GAMEPAD_LEFT_THUMBSTICK_DOWN = 0xD4,
        GAMEPAD_LEFT_THUMBSTICK_RIGHT = 0xD5,
        GAMEPAD_LEFT_THUMBSTICK_LEFT = 0xD6,
        GAMEPAD_RIGHT_THUMBSTICK_UP = 0xD7,
        GAMEPAD_RIGHT_THUMBSTICK_DOWN = 0xD8,
        GAMEPAD_RIGHT_THUMBSTICK_RIGHT = 0xD9,
        GAMEPAD_RIGHT_THUMBSTICK_LEFT = 0xDA,
        OEM_4 = 0xDB,
        OEM_5 = 0xDC,
        OEM_6 = 0xDD,
        OEM_7 = 0xDE,
        OEM_8 = 0xDF,
        OEM_AX = 0xE1,
        OEM_102 = 0xE2,
        ICO_HELP = 0xE3,
        ICO_00 = 0xE4,
        PROCESSKEY = 0xE5,
        ICO_CLEAR = 0xE6,
        PACKET = 0xE7,
        OEM_RESET = 0xE9,
        OEM_JUMP = 0xEA,
        OEM_PA1 = 0xEB,
        OEM_PA2 = 0xEC,
        OEM_PA3 = 0xED,
        OEM_WSCTRL = 0xEE,
        OEM_CUSEL = 0xEF,
        OEM_ATTN = 0xF0,
        OEM_FINISH = 0xF1,
        OEM_COPY = 0xF2,
        OEM_AUTO = 0xF3,
        OEM_ENLW = 0xF4,
        OEM_BACKTAB = 0xF5,
        ATTN = 0xF6,
        CRSEL = 0xF7,
        EXSEL = 0xF8,
        EREOF = 0xF9,
        PLAY = 0xFA,
        ZOOM = 0xFB,
        NONAME = 0xFC,
        PA1 = 0xFD,
        OEM_CLEAR = 0xFE,
    };
}