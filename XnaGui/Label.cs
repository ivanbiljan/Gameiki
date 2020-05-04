using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;

namespace XnaGui {
    public sealed class Label : Control {
        private readonly DynamicSpriteFont _font;
        private readonly string _text;
        
        public Label(Control parent, int x, int y, int width, int height) : base(parent, x, y, width, height) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class with the specified dimensions, font and text.
        /// </summary>
        /// <param name="parent">The parent control, if any.</param>
        /// <param name="x">The X coordinate of the upper left corner.</param>
        /// <param name="y">The Y coordinate of the upper left corner.</param>
        /// <param name="font">The font used to draw the label.</param>
        /// <param name="text">The text.</param>
        public Label(Control parent, int x, int y, DynamicSpriteFont font, string text) : base(parent, x, y, 0, 0) {
            _font = font ?? throw new ArgumentNullException(nameof(font));
            _text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            base.Draw(gameTime, spriteBatch);
            DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, _font, _text, Position, ForegroundColor);
        }
    }
}