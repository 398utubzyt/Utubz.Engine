using Utubz.Async;

namespace Utubz
{
    /// <summary>
    /// Get <see cref="Key"/> states from your <see cref="Component"/>'s current <see cref="Utubz.Window"/> <see cref="InputContext"/>.
    /// <para/>Or, get them from the asynchronous <see cref="InputContext"/> which listens to input events from every <see cref="Utubz.Window"/>.
    /// </summary>
    public static class Input
    {
        internal static InputContext winCtx;
        internal static InputContext asyncCtx;

        /// <summary>
        /// A single-threaded <see cref="InputContext"/> that corresponds with the <see cref="Utubz.Window"/> the <see cref="Component"/> is calling from.
        /// </summary>
        public static InputContext Window => winCtx;
        /// <summary>
        /// An asynchronous <see cref="InputContext"/> for situations where multi-threaded input is required.
        /// </summary>
        public static InputContext Asynchronous => asyncCtx;

        /// <summary>
        /// Get/set whether the engine updates the <see cref="Utubz.Window"/> <see cref="InputContext"/> automatically every frame, 
        /// or if the <see cref="Input"/> will be updated manually (through <see cref="Update"/>).
        /// </summary>
        /// <remarks>This does not affect the asynchronous <see cref="InputContext"/>, as it should always be handled manually.</remarks>
        public static bool AutoUpdate { get; set; } = true;

        /// <summary>
        /// Gets if the <see cref="Key"/> is held down from the <see cref="Utubz.Window"/>'s <see cref="InputContext"/>.
        /// </summary>
        /// <param name="key">The <see cref="Key"/> to check.</param>
        /// <returns>The held state of the <see cref="Key"/>.</returns>
        public static bool KeyHeld(Key key)
            => winCtx.KeyHeld(key);

        /// <summary>
        /// Gets if the <see cref="Key"/> was pressed down this frame from the <see cref="Utubz.Window"/>'s <see cref="InputContext"/>.
        /// </summary>
        /// <param name="key">The <see cref="Key"/> to check.</param>
        /// <returns>The pressed state of the <see cref="Key"/>.</returns>
        public static bool KeyDown(Key key)
            => winCtx.KeyDown(key);

        /// <summary>
        /// Gets if the <see cref="Key"/> was released this frame from the <see cref="Utubz.Window"/>'s <see cref="InputContext"/>.
        /// </summary>
        /// <param name="key">The <see cref="Key"/> to check.</param>
        /// <returns>The released state of the <see cref="Key"/>.</returns>
        public static bool KeyUp(Key key)
            => winCtx.KeyUp(key);

        /// <summary>
        /// The current position of the mouse in screen coordinates starting from the top left of the screen.
        /// </summary>
        public static Vector2 MousePosition
            => winCtx.MousePos();

        /// <summary>
        /// The change in position from the last update of the mouse in screen coordinates.
        /// </summary>
        public static Vector2 MouseDelta
            => winCtx.MouseDelta();

        /// <summary>
        /// The scrollwheel's change since the last update.
        /// </summary>
        public static Vector2 MouseScroll
            => winCtx.MouseScroll();

        /// <summary>
        /// Pushes any changes made to the <see cref="Utubz.Window"/>'s <see cref="InputContext"/>.
        /// </summary>
        public static void Update()
        {
            winCtx.Update();
        }

        /// <summary>
        /// Gets if the <see cref="Key"/> is held down from the asynchronous <see cref="InputContext"/>.
        /// </summary>
        /// <param name="key">The <see cref="Key"/> to check.</param>
        /// <returns>The held state of the <see cref="Key"/>.</returns>
        public static bool KeyHeldAsync(Key key)
            => asyncCtx.KeyHeld(key);

        /// <summary>
        /// Gets if the <see cref="Key"/> was pressed down this frame from the asynchronous <see cref="InputContext"/>.
        /// </summary>
        /// <param name="key">The <see cref="Key"/> to check.</param>
        /// <returns>The pressed state of the <see cref="Key"/>.</returns>
        public static bool KeyDownAsync(Key key)
            => asyncCtx.KeyDown(key);

        /// <summary>
        /// Gets if the <see cref="Key"/> was released this frame from the asynchronous <see cref="InputContext"/>.
        /// </summary>
        /// <param name="key">The <see cref="Key"/> to check.</param>
        /// <returns>The released state of the <see cref="Key"/>.</returns>
        public static bool KeyUpAsync(Key key)
            => asyncCtx.KeyUp(key);

        public static float KeyAxis(Key pos, Key neg)
        {
            float axis = 0f;

            if (KeyHeld(pos))
                axis += 1f;

            if (KeyHeld(neg))
                axis -= 1f;

            return axis;
        }

        public static Vector2 KeyPad(Key xpos, Key xneg, Key ypos, Key yneg)
        {
            return new Vector2(KeyAxis(xpos, xneg), KeyAxis(ypos, yneg));
        }

        public static Vector3 KeyPadXZ(Key xpos, Key xneg, Key ypos, Key yneg)
        {
            return new Vector3(KeyAxis(xpos, xneg), 0f, KeyAxis(ypos, yneg));
        }

        public static Vector3 KeyDirection(Key xpos, Key xneg, Key ypos, Key yneg, Key zpos, Key zneg)
        {
            return new Vector3(KeyAxis(xpos, xneg), KeyAxis(ypos, yneg), KeyAxis(zpos, zneg));
        }

        /// <summary>
        /// Pushes any changes made to the asynchronous <see cref="InputContext"/>.
        /// </summary>
        public static void UpdateAsync()
        {
            asyncCtx.Update();
        }
    }
}
