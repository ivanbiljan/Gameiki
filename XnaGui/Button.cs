using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;

namespace XnaGui {
    public sealed class Button : Control {
        private DynamicSpriteFont _font;
        
        public Button(Control parent, int x, int y, int width, int height) : base(parent, x, y, width, height) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class with the specified properties.
        /// </summary>
        /// <param name="parent">The parent control, if any.</param>
        /// <param name="x">The X coordinate of the upper left corner.</param>
        /// <param name="y">The Y coordinate of the upper left corner.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="font">The font used to print the text.</param>
        /// <param name="text">The button's display text.</param>
        public Button(Control parent, int x, int y, int width, int height, DynamicSpriteFont font, string text) : base(parent, x, y, width, height) {
            _font = font ?? throw new ArgumentNullException(nameof(font));
            Text = text;
        }
        
        /// <summary>
        /// Gets the display text.
        /// </summary>
        public string Text { get; set; }
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            base.Draw(gameTime, spriteBatch);
            
            spriteBatch.Draw(XnaGui.WhiteTexture, BoundBox, BackgroundColor);
            if (!string.IsNullOrWhiteSpace(Text)) {
                DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, _font, Text, Position, ForegroundColor);
            }
        }
    }
}