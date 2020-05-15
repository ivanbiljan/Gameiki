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

        private string _text = "";
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
        public string Text {
            get => _text;
            set => _text = value;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            base.Draw(gameTime, spriteBatch);

            // Draw the bounding box
            //spriteBatch.Draw(XnaGui.WhiteTexture, BoundBox, BackgroundColor);
            // Enable scissor rectangles so we can clip the text to the bounding rectangle
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, new RasterizerState {ScissorTestEnable = true});

            spriteBatch.GraphicsDevice.ScissorRectangle = BoundBox;
            spriteBatch.Draw(XnaGui.WhiteTexture, spriteBatch.GraphicsDevice.ScissorRectangle, BackgroundColor);
            if (!string.IsNullOrWhiteSpace(Text)) {
                spriteBatch.DrawString(_font, Text, Position, ForegroundColor);
            }

            // Restore the old rasterizer state
            spriteBatch.End();
            spriteBatch.Begin();
            
            // Draw the text
            if (!_hasFocus) {
                if (string.IsNullOrWhiteSpace(Text)) {
                    if (!string.IsNullOrWhiteSpace(PlaceholderText)) {
                        spriteBatch.DrawString(_font, PlaceholderText, Position, ForegroundColor);
                    }
                }
            }
            else {
                if (++_cursorBlinkCounter > 50) {
                    _cursorBlinkCounter = 0;
                    _isCursorBlinking = !_isCursorBlinking;
                }

                if (_isCursorBlinking && _hasFocus) {
                    spriteBatch.DrawString(_font, $"{Text}|", Position,
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
            
            UserInputHandler.Update(gameTime);
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
            
            // if (_currentKeyboard.IsKeyDown(Keys.Left)) {
            //     _cursorPosition = Math.Max(_cursorPosition - 1, 0);
            //     return;
            // }
            //
            // if (_currentKeyboard.IsKeyDown(Keys.Right)) {
            //     _cursorPosition = Math.Max(_cursorPosition + 1, Text.Length);
            //     return;
            // }
            
            UserInputHandler.GetTextInput(ref _text);
        }

        protected override void OnClicked(object sender, EventArgs args) {
            _hasFocus = !_hasFocus;
            base.OnClicked(sender, args);
        }
    }
}