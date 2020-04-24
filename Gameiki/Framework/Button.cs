using System;
using System.ComponentModel;
using System.Diagnostics;
using Gameiki.Patcher.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;

namespace Gameiki.Framework {
    /// <summary>
    ///     Represents a button control.
    /// </summary>
    public class Button : Control {
        private readonly Action _callback;
        private readonly string _text;
        private Texture2D _pointerTexture;

        public Button(int x, int y, int width, int height) : base(x, y, width, height) {
        }

        public Button(int x, int y, int width, int height, string text = null, Action callback = null) : base(x, y, width, height) {
            _text = text;
            _callback = callback;
        }

        public override void Initialize() {
            base.Initialize();

            // Setup hooks
            Patcher.Events.Hooks.PreCursorDraw += OnPreCursorDraw;
            MouseClick += OnMouseClick;

            // Setup everything else
            _pointerTexture = Main.instance.OurLoad<Texture2D>("Images\\Gameiki\\Cursor\\cursornew");
        }

        private void OnMouseClick(object sender, EventArgs e) {
            _callback?.Invoke();
        }

        private void OnPreCursorDraw(object sender, HandledEventArgs args) {
            Utils.DrawInvBG(Main.spriteBatch, Position.X, Position.Y, Dimensions.X, Dimensions.Y);
            Main.spriteBatch.DrawString(Main.fontMouseText, _text,
                new Vector2(Position.X + (Dimensions.X - Main.fontMouseText.MeasureString(_text).X) / 2,
                    Position.Y + (Dimensions.Y - Main.fontMouseText.LineSpacing) / 2), Color.White);

            if (!IsHoveredOver()) {
                return;
            }

            Gameiki.MyPlayer.mouseInterface = true;
            Main.spriteBatch.Draw(_pointerTexture, new Vector2(Main.mouseX - 12, Main.mouseY + 5), new Rectangle?(), Color.White, 0f,
                new Vector2(0.1f) * _pointerTexture.Size(), Main.cursorScale * 1.1f, SpriteEffects.None, 0.0f);
            Main.spriteBatch.DrawString(Main.fontMouseText, _text,
                new Vector2(Position.X + (Dimensions.X - Main.fontMouseText.MeasureString(_text).X) / 2,
                    Position.Y + (Dimensions.Y - Main.fontMouseText.LineSpacing) / 2), Main.OurFavoriteColor);

            args.Handled = true;
        }
    }
}