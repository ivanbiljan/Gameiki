using System;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Clipboard = XnaGui.Native.Clipboard;

namespace XnaGui {
    public class XnaGui {
        private static Game _game;

        /// <summary>
        ///     Gets the content manager.
        /// </summary>
        public static ContentManager ContentManager => _game.Content;

        /// <summary>
        ///     Gets the graphics device.
        /// </summary>
        public static GraphicsDevice GraphicsDevice => _game.GraphicsDevice;

        /// <summary>
        ///     Gets the value indicating whether the game is active.
        /// </summary>
        public static bool IsGameActive => _game.IsActive;

        /// <summary>
        ///     Gets a generic 1x1 white texture.
        /// </summary>
        public static Texture2D WhiteTexture { get; private set; }

        /// <summary>
        ///     Gets the game window.
        /// </summary>
        public static GameWindow Window => _game.Window;

        /// <summary>
        ///     Initializes the GUI for the specified game.
        /// </summary>
        /// <param name="game">The game, which must not be <c>null</c>.</param>
        public static void Initialize(Game game) {
            MessageBox.Show(Clipboard.GetText());
            _game = game ?? throw new ArgumentNullException(nameof(game));

            WhiteTexture = new Texture2D(_game.GraphicsDevice, 1, 1);
            WhiteTexture.SetData(new[] {Color.White});
        }
    }
}