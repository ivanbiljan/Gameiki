using System;
using System.Runtime.Remoting.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;

namespace XnaGui {
    public sealed class TextBox : Control {
        private readonly DynamicSpriteFont _font;

        private KeyboardState _currentKeyboard;
        private int _cursorBlinkCounter;
        private int _cursorPosition;

        private bool _hasFocus;
        private bool _isCursorBlinking;
        private DateTime _lastUpdate;
        private KeyboardState _previousKeyboard;

        public TextBox(Control parent, int x, int y, int width, int height) : base(parent, x, y, width, height) {
        }

        public TextBox(Control parent, int x, int y, int width, int height, DynamicSpriteFont font) : base(parent, x, y, width, height) {
            _font = font ?? throw new ArgumentNullException(nameof(font));
        }

        /// <summary>
        ///     Gets or sets the placeholder text.
        /// </summary>
        public string PlaceholderText { get; set; }

        /// <summary>
        ///     Gets or sets the textbox contents.
        /// </summary>
        public string Text { get; set; } = "";

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            base.Draw(gameTime, spriteBatch);

            spriteBatch.Draw(XnaGui.WhiteTexture, BoundBox, BackgroundColor);
            if (!_hasFocus) {
                if (string.IsNullOrWhiteSpace(Text)) {
                    if (!string.IsNullOrWhiteSpace(PlaceholderText)) {
                        spriteBatch.DrawString(_font, PlaceholderText, Position, ForegroundColor);
                    }
                }
            }
            else {
                if (++_cursorBlinkCounter > 20) {
                    _cursorBlinkCounter = 0;
                    _isCursorBlinking = !_isCursorBlinking;
                }

                if (_isCursorBlinking && _hasFocus) {
                    spriteBatch.DrawString(_font, Text.Insert(_cursorPosition, "|"), Position,
                        ForegroundColor);
                }
                else {
                    spriteBatch.DrawString(_font, Text, Position,
                        ForegroundColor);
                }
            }
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            if (!_hasFocus) {
                return;
            }

            // Lets not do this too frequently
            if ((DateTime.Now - _lastUpdate).TotalMilliseconds < 125) {
                return;
            }

            _previousKeyboard = _currentKeyboard;
            _currentKeyboard = Keyboard.GetState();
            if (_currentKeyboard.IsKeyDown(Keys.Escape)) {
                _hasFocus = false;
                return;
            }
            
            if (_currentKeyboard.IsKeyDown(Keys.Left)) {
                _cursorPosition = Math.Max(_cursorPosition - 1, 0);
                return;
            }

            if (_currentKeyboard.IsKeyDown(Keys.Right)) {
                _cursorPosition = Math.Max(_cursorPosition + 1, Text.Length);
                return;
            }

            if (_currentKeyboard.IsKeyDown(Keys.Back) && Text.Length != 0) {
                Text = Text.Remove(Math.Max(Text.Length - 1, 0), 1);
                return; 
            }

            if (_currentKeyboard.IsKeyDown(Keys.Delete) && Text.Length != 0) {
                Text = Text.Remove(_cursorPosition, 1);
                return;
            }

            foreach (var key in _currentKeyboard.GetPressedKeys()) {
                // Damn
                if (key == Keys.LeftShift || key == Keys.RightShift || key == Keys.LeftControl || key == Keys.RightControl ||
                    key == Keys.LeftAlt || key == Keys.RightAlt || key == Keys.LeftWindows || key == Keys.RightWindows) {
                    continue;
                }
                
                if (_previousKeyboard.IsKeyDown(key) && (DateTime.Now - _lastUpdate).TotalMilliseconds >= 125) {
                    Text += key;
                    ++_cursorPosition;
                    _lastUpdate = DateTime.Now;
                }
            }
        }

        protected override void OnClicked(object sender, EventArgs args) {
            _hasFocus = !_hasFocus;
            base.OnClicked(sender, args);
        }
    }
}