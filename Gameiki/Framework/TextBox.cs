// using System;
// using System.ComponentModel;
// using Gameiki.Patcher.Events;
// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
// using ReLogic.Graphics;
// using Terraria;
// using Terraria.GameInput;
//
// namespace Gameiki.Framework {
//     /// <summary>
//     ///     Represents a textbox control.
//     /// </summary>
//     public class TextBox : Control {
//         private Texture2D _generalTexture;
//         private bool _isCursorBlinking;
//         private int _mouseBlinkCounter;
//
//         public TextBox(int x, int y, int width, int height) : base(x, y, width, height) {
//         }
//
//         /// <summary>
//         ///     Gets a value indicating whether the textbox has focus.
//         /// </summary>
//         public bool HasFocus { get; set; }
//
//         /// <summary>
//         ///     Gets the placeholder text.
//         /// </summary>
//         public string PlaceholderText { get; } = "Type here...";
//
//         /// <summary>
//         ///     Gets the current text.
//         /// </summary>
//         public string Text { get; private set; }
//
//         public override void Initialize() {
//             base.Initialize();
//
//             _generalTexture = new Texture2D(Main.instance.GraphicsDevice, 1, 1);
//             _generalTexture.SetData(new[] {Color.White});
//
//             Hooks.PreCursorDraw += OnPreCursorDraw;
//             MouseClick += OnMouseClick;
//         }
//
//         private void OnMouseClick(object sender, EventArgs e) {
//             HasFocus = true;
//         }
//
//         private void OnPreCursorDraw(object sender, HandledEventArgs e) {
//             Main.spriteBatch.Draw(_generalTexture, Position, new Rectangle(0, 0, (int) Dimensions.X, (int) Dimensions.Y), Color.White);
//             if (!HasFocus) {
//                 if (string.IsNullOrWhiteSpace(Text)) {
//                     Main.spriteBatch.DrawString(Main.fontMouseText, PlaceholderText, new Vector2(Position.X + 3, Position.Y + 3),
//                         Color.Black * 0.2f);
//                 }
//             }
//             else {
//                 Text = Main.GetInputText(Text);
//                 if (Main.inputTextEscape) {
//                     HasFocus = false;
//                 }
//
//                 if (_mouseBlinkCounter++ == 20) {
//                     _mouseBlinkCounter = 0;
//                     _isCursorBlinking = !_isCursorBlinking;
//                 }
//
//                 PlayerInput.WritingText = HasFocus;
//                 Main.instance.HandleIME();
//                 Main.spriteBatch.DrawString(Main.fontMouseText, $"{Text}{(_isCursorBlinking ? "|" : string.Empty)}",
//                     new Vector2(Position.X + 3, Position.Y + 3), Color.Black);
//             }
//         }
//     }
// }