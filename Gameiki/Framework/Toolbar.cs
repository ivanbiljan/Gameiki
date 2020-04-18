using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Gameiki.Patcher.Events;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace Gameiki.Framework {
    internal sealed class Toolbar : UserInterfaceItem {
        private readonly List<Texture2D> _buttons = new List<Texture2D> {
            Main.itemTexture[ItemID.SilverAxe],
            Main.npcHeadTexture[2]
        };
        
        public Toolbar() {
            Boundaries = new Rectangle(Main.screenWidth / 2 - Main.chatBackTexture.Width / 2, Main.screenHeight - 45, Main.chatBackTexture.Width, 45);
        }
        
        public override AnchorPosition AnchorPosition { get; protected set; } = AnchorPosition.Bottom;
        public override Rectangle Boundaries { get; protected set; }

        public override void Draw(object sender, DrawEventArgs args) {
            Main.spriteBatch.Draw(Main.chatBackTexture, Boundaries, Color.White);

            var offset = 0;
            for (var i = 0; i < _buttons.Count; ++i) {
                var texture = _buttons[i];
                var rectangle = new Rectangle(Boundaries.X + 5, Boundaries.Y + (Boundaries.Height - texture.Height) / 2, texture.Width, texture.Height);
                
                if (i > 0) {
                    rectangle.X += offset;
                }
                
                Main.spriteBatch.Draw(texture, rectangle, Color.White);
                offset += texture.Width + 5; // Give everything a 5 pixel margin
            }
        }
    }
}