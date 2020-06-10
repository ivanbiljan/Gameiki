// using System;
// using System.Collections.Generic;
// using System.ComponentModel;
// using System.Linq;
// using System.Threading.Tasks;
// using Gameiki.Extensions;
// using Gameiki.Patcher.Events;
// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
// using Terraria;
// using Terraria.ID;
//
// namespace Gameiki.Framework {
//     internal sealed class ToolbarItem {
//         public ToolbarItem(Texture2D texture, Action callback, string tooltip = null) {
//             Texture = texture;
//             Callback = callback;
//             Tooltip = tooltip;
//         }
//
//         public Action Callback { get; }
//
//         public float ScaleOverride { get; set; } = 0.9f;
//         public Texture2D Texture { get; }
//         public string Tooltip { get; }
//     }
//
//     internal sealed class Toolbar {
//         private static Toolbar _instance;
//
//         private readonly Rectangle _boundaries;
//
//         private readonly List<ToolbarItem> _items = new List<ToolbarItem> {
//             new ToolbarItem(Main.itemTexture[ItemID.SilverAxe], () => { }, "Tools Lookup"),
//             new ToolbarItem(Main.itemTexture[ItemID.Toolbelt], () => { }, "Accessories"),
//             new ToolbarItem(Main.itemTexture[ItemID.AngelWings], () => { }, "Wings"),
//             new ToolbarItem(Main.itemTexture[ItemID.TrashCan], () => { }, "Recycle Bin"),
//             new ToolbarItem(Main.mapIconTexture[0], RevealMap, "Map Reveal"),
//             new ToolbarItem(Main.itemTexture[ItemID.Teleporter], () => { }, "Waypoints"),
//             new ToolbarItem(Main.sunTexture, SetTime, "Noon") {ScaleOverride = 0.5f},
//             new ToolbarItem(Main.npcHeadTexture[2], () => { }, "Town NPCs"),
//             new ToolbarItem(Main.instance.OurLoad<Texture2D>("Images\\Gameiki\\Other\\music"),
//                 () => PlayMusic(), "Plays music"),
//             new ToolbarItem(Main.instance.OurLoad<Texture2D>("Images\\Gameiki\\Other\\flashlight"), Fullbright, "Fullbright")
//                 {ScaleOverride = 0.6f},
//             new ToolbarItem(Main.instance.OurLoad<Texture2D>("Images\\Gameiki\\Other\\godmode"), Godmode, "Godmode")
//                 {ScaleOverride = 0.5f}
//         };
//
//         private readonly Button _toggleButton;
//
//         private Texture2D _pointerTexture;
//         private bool _visible = true;
//
//         private Toolbar() {
//             var width = (int) _items.Sum(i => i.Texture.Width * i.ScaleOverride + 20);
//             _boundaries = new Rectangle(Main.screenWidth / 2 - width / 2, Main.screenHeight - 45, width, 45);
//
//             var toggleTextSize = Main.fontMouseText.MeasureString("Toggle");
//             _toggleButton = new Button(Main.screenWidth / 2 - (int) (toggleTextSize.X + 16) / 2,
//                 Main.screenHeight - 45 - (int) (toggleTextSize.Y + 16), (int) toggleTextSize.X + 16, (int) toggleTextSize.Y + 16,
//                 "Toggle", ToggleHotbar);
//         }
//
//         public static Toolbar Instance => _instance ?? (_instance = new Toolbar());
//
//         private static void Fullbright() {
//             var session = Gameiki.MyPlayer.GetData<Session>("session");
//             session.IsFullbright = !session.IsFullbright;
//             Gameiki.MyPlayer.SendGameikiMessage(
//                 $"Fullbright is now {(session.IsFullbright ? GameikiUtils.GetColoredString("on", Color.LimeGreen) : GameikiUtils.GetColoredString("off", Color.Red))}.");
//         }
//
//         private static void Godmode() {
//             var session = Gameiki.MyPlayer.GetData<Session>("session");
//             session.IsGodmode = !session.IsGodmode;
//             Gameiki.MyPlayer.SendGameikiMessage(
//                 $"Godmode is now {(session.IsGodmode ? GameikiUtils.GetColoredString("on", Color.LimeGreen) : GameikiUtils.GetColoredString("off", Color.Red))}.");
//         }
//
//         private static void PlayMusic() {
//             Main.spriteBatch.End();
//             SongPlayer.Instance.PlayRandom();
//         }
//
//         private static void RevealMap() {
//             new TaskFactory().StartNew(() => {
//                 for (var x = 0; x < Main.Map.MaxWidth; ++x) {
//                     for (var y = 0; y < Main.Map.MaxHeight; ++y) {
//                         Main.Map.UpdateLighting(x, y, 255);
//                     }
//                 }
//
//                 Main.refreshMap = true;
//             });
//
//             Gameiki.MyPlayer.SendGameikiMessage(GameikiUtils.GetColoredString("The map is now revealed.", Color.LimeGreen));
//         }
//
//         private static void SetTime() {
//             GameikiUtils.SetTime(12, 0);
//             Gameiki.MyPlayer.SendGameikiMessage(GameikiUtils.GetColoredString("Time set to 12:00", Color.Yellow));
//         }
//
//         public void Initialize() {
//             _pointerTexture = Main.instance.OurLoad<Texture2D>("Images\\Gameiki\\Cursor\\cursornew");
//             _toggleButton.Initialize();
//
//             Hooks.PreCursorDraw += OnPreCursorDraw;
//         }
//
//         private void OnPreCursorDraw(object sender, HandledEventArgs args) {
//             if (!_visible || Main.drawingPlayerChat) {
//                 return;
//             }
//
//             Utils.DrawInvBG(Main.spriteBatch, _boundaries);
//
//             var offset = 0;
//             string cursorText = null;
//             for (var i = 0; i < _items.Count; ++i) {
//                 var item = _items[i];
//                 var position = new Vector2(_boundaries.X + 10 + offset,
//                     _boundaries.Y + (_boundaries.Height - item.Texture.Height * item.ScaleOverride) / 2);
//                 var rectangle = new Rectangle(0, 0, item.Texture.Width, item.Texture.Height);
//                 if (Main.mouseX >= position.X && Main.mouseX <= position.X + rectangle.Width && Main.mouseY >= position.Y &&
//                     Main.mouseY <= position.Y + rectangle.Height) {
//                     Main.spriteBatch.Draw(item.Texture, position, rectangle, Color.White, 0, new Vector2(),
//                         item.ScaleOverride + item.ScaleOverride * 0.15f, SpriteEffects.None, 0);
//                     Main.spriteBatch.Draw(_pointerTexture, new Vector2(Main.mouseX - 12, Main.mouseY + 5), new Rectangle?(), Color.White,
//                         0f,
//                         new Vector2(0.1f) * _pointerTexture.Size(), Main.cursorScale * 1.1f, SpriteEffects.None, 0.0f);
//
//                     Gameiki.MyPlayer.mouseInterface = true;
//                     if (Main.mouseLeft && Main.mouseLeftRelease) {
//                         item.Callback();
//                     }
//
//                     cursorText = item.Tooltip;
//                     args.Handled = true;
//                 }
//                 else {
//                     Main.spriteBatch.Draw(item.Texture, position, rectangle, Color.White, 0, new Vector2(), item.ScaleOverride,
//                         SpriteEffects.None, 0);
//                 }
//
//                 offset += (int) (item.Texture.Width * item.ScaleOverride) + 20; // Give everything a 20 pixel margin
//             }
//
//             if (!string.IsNullOrWhiteSpace(cursorText)) {
//                 Main.instance.MouseText(cursorText);
//             }
//         }
//
//         private void ToggleHotbar() {
//             _visible = !_visible;
//             if (!_visible) {
//                 _toggleButton.SetPosition(_toggleButton.Position.X, Main.screenHeight - _toggleButton.Dimensions.Y);
//             }
//             else {
//                 var toggleTextSize = Main.fontMouseText.MeasureString("Toggle");
//                 _toggleButton.SetPosition(_toggleButton.Position.X, Main.screenHeight - 45 - (int) (toggleTextSize.Y + 10));
//             }
//         }
//     }
// }