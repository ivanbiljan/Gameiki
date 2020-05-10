using System;
using System.ComponentModel;
using Gameiki.Patcher.Events;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria;

namespace Gameiki.Framework {
    public class Label : Control {
        public Label(int x, int y, int width, int height) : base(x, y, width, height) {
        }

        public Label(int x, int y, string text) : base(x, y, 0, 0) {
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public Color Color { get; set; } = Color.White;

        public string Text { get; }

        public override void Initialize() {
            base.Initialize();

            Hooks.PreCursorDraw += OnPreCursorDraw;
        }

        private void OnPreCursorDraw(object sender, HandledEventArgs e) {
            Main.spriteBatch.DrawString(Main.fontMouseText, Text, Position, Color);
        }
    }
}