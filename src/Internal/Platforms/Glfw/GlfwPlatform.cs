using Utubz.Internal.Native;
using Utubz.Internal.Native.Glfw;
using Utubz.Internal.Native.Glad;
using Utubz.Graphics;

using System;
using System.Collections.Generic;

namespace Utubz.Internal.Platforms.Glfw
{
    public sealed class GlfwPlatform : Platform
    {
		#region Glfw -> SDL Key Lookup Table

		private Dictionary<int, int> GlfwKeyLookup = new Dictionary<int, int>()
		{
			{ -1, 0 },

			{ 65, 4 },
			{ 66, 5 },
			{ 67, 6 },
			{ 68, 7 },
			{ 69, 8 },
			{ 70, 9 },
			{ 71, 10 },
			{ 72, 11 },
			{ 73, 12 },
			{ 74, 13 },
			{ 75, 14 },
			{ 76, 15 },
			{ 77, 16 },
			{ 78, 17 },
			{ 79, 18 },
			{ 80, 19 },
			{ 81, 20 },
			{ 82, 21 },
			{ 83, 22 },
			{ 84, 23 },
			{ 85, 24 },
			{ 86, 25 },
			{ 87, 26 },
			{ 88, 27 },
			{ 89, 28 },
			{ 90, 29 },

			{ 49, 30 },
			{ 50, 31 },
			{ 51, 32 },
			{ 52, 33 },
			{ 53, 34 },
			{ 54, 35 },
			{ 55, 36 },
			{ 56, 37 },
			{ 57, 38 },
			{ 48, 39 },

			{ 257, 40 },
			{ 256, 41 },
			{ 259, 42 },
			{ 258, 43 },
			{ 32, 44 },

			{ 45, 45 },
			{ 61, 46 },
			{ 91, 47 },
			{ 93, 48 },
			{ 92, 49 },
			{ NONUSHASH, 50 },
			{ 59, 51 },
			{ 39, 52 },
			{ 96, 53 },
			{ 44, 54 },
			{ 46, 55 },
			{ 47, 56 },

			{ 280, 57 },

			{ 290, 58 },
			{ 291, 59 },
			{ 292, 60 },
			{ 293, 61 },
			{ 294, 62 },
			{ 295, 63 },
			{ 296, 64 },
			{ 297, 65 },
			{ 298, 66 },
			{ 299, 67 },
			{ 300, 68 },
			{ 301, 69 },

			{ PRINTSCREEN, 70 },
			{ SCROLLLOCK, 71 },
			{ PAUSE, 72 },
			{ INSERT, 73 },
			{ HOME, 74 },
			{ PAGEUP, 75 },
			{ DELETE, 76 },
			{ END, 77 },
			{ PAGEDOWN, 78 },
			{ RIGHT, 79 },
			{ LEFT, 80 },
			{ DOWN, 81 },
			{ UP, 82 },

			{ NUMLOCKCLEAR, 83 },
			{ KP_DIVIDE, 84 },
			{ KP_MULTIPLY, 85 },
			{ KP_MINUS, 86 },
			{ KP_PLUS, 87 },
			{ KP_ENTER, 88 },
			{ KP_1, 89 },
			{ KP_2, 90 },
			{ KP_3, 91 },
			{ KP_4, 92 },
			{ KP_5, 93 },
			{ KP_6, 94 },
			{ KP_7, 95 },
			{ KP_8, 96 },
			{ KP_9, 97 },
			{ KP_0, 98 },
			{ KP_PERIOD, 99 },

			{ NONUSBACKSLASH, 100 },
			{ APPLICATION, 101 },
			{ POWER, 102 },
			{ KP_EQUALS, 103 },
			{ 302, 104 },
			{ 303, 105 },
			{ 304, 106 },
			{ 305, 107 },
			{ 306, 108 },
			{ 307, 109 },
			{ 308, 110 },
			{ 309, 111 },
			{ 310, 112 },
			{ 311, 113 },
			{ 312, 114 },
			{ 313, 115 },
			{ EXECUTE, 116 },
			{ HELP, 117 },
			{ MENU, 118 },
			{ SELECT, 119 },
			{ STOP, 120 },
			{ AGAIN, 121 },
			{ UNDO, 122 },
			{ CUT, 123 },
			{ COPY, 124 },
			{ PASTE, 125 },
			{ FIND, 126 },
			{ MUTE, 127 },
			{ VOLUMEUP, 128 },
			{ VOLUMEDOWN, 129 },
			/* not sure whether there's a reason to enable these */
			/*	{ LOCKINGCAPSLOCK, 130 }, */
			/*	{ LOCKINGNUMLOCK, 131 }, */
			/*	{ LOCKINGSCROLLLOCK, 132 }, */
			{ KP_COMMA, 133 },
			{ KP_EQUALSAS400, 134 },

			{ INTERNATIONAL1, 135 },
			{ INTERNATIONAL2, 136 },
			{ INTERNATIONAL3, 137 },
			{ INTERNATIONAL4, 138 },
			{ INTERNATIONAL5, 139 },
			{ INTERNATIONAL6, 140 },
			{ INTERNATIONAL7, 141 },
			{ INTERNATIONAL8, 142 },
			{ INTERNATIONAL9, 143 },
			{ LANG1, 144 },
			{ LANG2, 145 },
			{ LANG3, 146 },
			{ LANG4, 147 },
			{ LANG5, 148 },
			{ LANG6, 149 },
			{ LANG7, 150 },
			{ LANG8, 151 },
			{ LANG9, 152 },

			{ ALTERASE, 153 },
			{ SYSREQ, 154 },
			{ CANCEL, 155 },
			{ CLEAR, 156 },
			{ PRIOR, 157 },
			{ RETURN2, 158 },
			{ SEPARATOR, 159 },
			{ OUT, 160 },
			{ OPER, 161 },
			{ CLEARAGAIN, 162 },
			{ CRSEL, 163 },
			{ EXSEL, 164 },

			{ KP_00, 176 },
			{ KP_000, 177 },
			{ THOUSANDSSEPARATOR, 178 },
			{ DECIMALSEPARATOR, 179 },
			{ CURRENCYUNIT, 180 },
			{ CURRENCYSUBUNIT, 181 },
			{ KP_LEFTPAREN, 182 },
			{ KP_RIGHTPAREN, 183 },
			{ KP_LEFTBRACE, 184 },
			{ KP_RIGHTBRACE, 185 },
			{ KP_TAB, 186 },
			{ KP_BACKSPACE, 187 },
			{ KP_A, 188 },
			{ KP_B, 189 },
			{ KP_C, 190 },
			{ KP_D, 191 },
			{ KP_E, 192 },
			{ KP_F, 193 },
			{ KP_XOR, 194 },
			{ KP_POWER, 195 },
			{ KP_PERCENT, 196 },
			{ KP_LESS, 197 },
			{ KP_GREATER, 198 },
			{ KP_AMPERSAND, 199 },
			{ KP_DBLAMPERSAND, 200 },
			{ KP_VERTICALBAR, 201 },
			{ KP_DBLVERTICALBAR, 202 },
			{ KP_COLON, 203 },
			{ KP_HASH, 204 },
			{ KP_SPACE, 205 },
			{ KP_AT, 206 },
			{ KP_EXCLAM, 207 },
			{ KP_MEMSTORE, 208 },
			{ KP_MEMRECALL, 209 },
			{ KP_MEMCLEAR, 210 },
			{ KP_MEMADD, 211 },
			{ KP_MEMSUBTRACT, 212 },
			{ KP_MEMMULTIPLY, 213 },
			{ KP_MEMDIVIDE, 214 },
			{ KP_PLUSMINUS, 215 },
			{ KP_CLEAR, 216 },
			{ KP_CLEARENTRY, 217 },
			{ KP_BINARY, 218 },
			{ KP_OCTAL, 219 },
			{ KP_DECIMAL, 220 },
			{ KP_HEXADECIMAL, 221 },

			{ LCTRL, 224 },
			{ LSHIFT, 225 },
			{ LALT, 226 },
			{ LGUI, 227 },
			{ RCTRL, 228 },
			{ RSHIFT, 229 },
			{ RALT, 230 },
			{ RGUI, 231 },

			{ MODE, 257 },

			/* These come from the USB consumer page (0x0C) */
			{ AUDIONEXT, 258 },
			{ AUDIOPREV, 259 },
			{ AUDIOSTOP, 260 },
			{ AUDIOPLAY, 261 },
			{ AUDIOMUTE, 262 },
			{ MEDIASELECT, 263 },
			{ WWW, 264 },
			{ MAIL, 265 },
			{ CALCULATOR, 266 },
			{ COMPUTER, 267 },
			{ AC_SEARCH, 268 },
			{ AC_HOME, 269 },
			{ AC_BACK, 270 },
			{ AC_FORWARD, 271 },
			{ AC_STOP, 272 },
			{ AC_REFRESH, 273 },
			{ AC_BOOKMARKS, 274 },

			/* These come from other sources }, and are mostly mac related */
			{ BRIGHTNESSDOWN, 275 },
			{ BRIGHTNESSUP, 276 },
			{ DISPLAYSWITCH, 277 },
			{ KBDILLUMTOGGLE, 278 },
			{ KBDILLUMDOWN, 279 },
			{ KBDILLUMUP, 280 },
			{ EJECT, 281 },
			{ SLEEP, 282 },

			{ APP1, 283 },
			{ APP2, 284 },

			/* These come from the USB consumer page (0x0C) */
			{ AUDIOREWIND, 285 },
			{ AUDIOFASTFORWARD, 286 },
		};

		#endregion

		internal static int winNextId;
        public override string Name => "GLFW";

        #region Initialization

        public override bool Init()
        {
            return glfw3.GlfwInit() == 0;
        }

        public override string Error()
        {
            string msg = string.Empty;
            glfw3.GlfwGetError(ref msg);
            return msg;
        }

        public override void Quit()
        {
        }

        #endregion

        #region GL

        private static IntPtr PtrGlfwGetProcAddr(string name) => System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(glfw3.GlfwGetProcAddress(name));

        public override unsafe void LoadGL()
        {
            GL.Load(PtrGlfwGetProcAddr);
        }

        #endregion

        #region Events

        public override void Poll()
        {
            glfw3.GlfwPollEvents();
        }

        #endregion

        #region Windowing

        public override int GetWindowId(Window window)
        {
            if (window is GlfwWindow win)
                return win.internalId;
            return -1;
        }

        protected override Window CreateWindow()
        {
            return new GlfwWindow();
        }

        #endregion

        #region Input

        public override int GetKey(int key)
        {
            return key;
        }

        #endregion
    }
}
