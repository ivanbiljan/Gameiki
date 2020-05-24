using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaGui.Extensions;

namespace XnaGui {
    public sealed class CheckBox : Control {
        public CheckBox(Control parent, int x, int y, int width, int height) : base(parent, x, y, width, height) {
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the box is checked.
        /// </summary>
        public bool IsChecked { get; set; }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            base.Draw(gameTime, spriteBatch);
            if (IsChecked) {
                spriteBatch.FillRectangle(BoundBox, BackgroundColor);
            }
            else {
                spriteBatch.DrawRectangle(Position, BoundBox.Width, BoundBox.Height, BackgroundColor);
            }
        }

        protected override void OnClicked(object sender, EventArgs args) {
            base.OnClicked(sender, args);
            IsChecked = !IsChecked;
        }
    }
}