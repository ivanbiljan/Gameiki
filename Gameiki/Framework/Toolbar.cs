using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Gameiki.Patcher.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace Gameiki.Framework {
    internal sealed class ToolbarItem {
        public ToolbarItem(Texture2D texture, Action callback, string tooltip = null) {
            Texture = texture;
            Callback = callback;
            Tooltip = tooltip;
        }
        
        public Action Callback { get; }
        public Texture2D Texture { get; }
        public string Tooltip { get; }
    }
    
    internal sealed class Toolbar : UserInterfaceItem {
        // private readonly List<Texture2D> _buttons = new List<Texture2D> {
        //     Main.itemTexture[ItemID.SilverAxe],
        //     Main.itemTexture[ItemID.Toolbelt],
        //     Main.itemTexture[ItemID.AngelWings],
        //     Main.itemTexture[ItemID.TrashCan],
        //     Main.mapIconTexture[0],
        //     Main.itemTexture[ItemID.Teleporter],
        //     Main.sun3Texture,
        //     Main.npcHeadTexture[2]
        // };

        private readonly List<ToolbarItem> _items = new List<ToolbarItem> {
            new ToolbarItem(Main.itemTexture[ItemID.SilverAxe], () => {}, "Tools Lookup"),
            new ToolbarItem(Main.itemTexture[ItemID.Toolbelt], () => {}, "Accessories"),
            new ToolbarItem(Main.itemTexture[ItemID.AngelWings], () => {}, "Wings"),
            new ToolbarItem(Main.itemTexture[ItemID.TrashCan], () => { }, "Recycle Bin"),
            new ToolbarItem(Main.mapIconTexture[0], () => RevealMap(), "Map Reveal"),
            new ToolbarItem(Main.itemTexture[ItemID.Teleporter], () => {}, "Waypoints"),
            new ToolbarItem(Main.sun3Texture, () => {}, "Time"),
            new ToolbarItem(Main.npcHeadTexture[2], () => {}, "Town NPCs")
        };

        public Toolbar() {
            var width = _items.Sum(i => i.Texture.Width + 20) + 10;
            Boundaries = new Rectangle(Main.screenWidth / 2 - width / 2, Main.screenHeight - 45, width, 45);
        }

        public override AnchorPosition AnchorPosition { get; } = AnchorPosition.Bottom;
        public override Rectangle Boundaries { get; }

        public override void Draw(object sender, DrawEventArgs args) {
            // if (!Gameiki.MyPlayer.active || Main.drawingPlayerChat || Main.editChest || Main.editSign || Main.playerInventory || Main.mapFullscreen) {
            //     return;
            // }
            
            Main.spriteBatch.Draw(Main.chatBackTexture, Boundaries, Color.White);

            var offset = 0;
            string cursorText = null;
            for (var i = 0; i < _items.Count; ++i) {
                var item = _items[i];
                var position = new Vector2(Boundaries.X + 10 + offset, Boundaries.Y + (Boundaries.Height - item.Texture.Height) / 2);
                var rectangle = new Rectangle(0, 0, item.Texture.Width, item.Texture.Height);
                if (Main.mouseX >= position.X && Main.mouseX <= position.X + rectangle.Width && Main.mouseY >= position.Y && Main.mouseY <= position.Y + rectangle.Height) {
                    Main.spriteBatch.Draw(item.Texture, position, rectangle, Color.White, 0, new Vector2(), 1.15f, SpriteEffects.None, 0);
                    if (Main.mouseLeft) {
                        item.Callback();
                    }
                    
                    cursorText = item.Tooltip;
                }
                else {
                    Main.spriteBatch.Draw(item.Texture, position, rectangle, Color.White, 0, new Vector2(), 0.9f, SpriteEffects.None, 0);
                }
                
                offset += item.Texture.Width + 20; // Give everything a 20 pixel margin
            }

            // Hacky, find a clever way around this
            Main.DrawInterface_36_Cursor();
            if (cursorText != null) {
                Main.instance.MouseText(cursorText);
            }
        }

        private static Task RevealMap() {
            return Task.Factory.StartNew(() => {
                for (var x = 0; x < Main.Map.MaxWidth; ++x) {
                    for (var y = 0; y < Main.Map.MaxHeight; ++y) {
                        Main.Map.UpdateLighting(x, y, 255);
                    }
                }

                Main.refreshMap = true;
            });
        }
    }
}