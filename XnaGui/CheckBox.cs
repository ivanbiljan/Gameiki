using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using XnaGui.Extensions;
using XnaGui.Input;

namespace XnaGui {
    public sealed class CheckBox : Control {
        private readonly DynamicSpriteFont _font;
        private readonly Rectangle _tickBox;
        
        private string _text;
        private TextAlign _textAlign;
        private Vector2 _textPosition;
        private Vector2 _textSize;

        public CheckBox(Control parent, int x, int y, int width, int height) : base(parent, x, y, width, height) {
        }

        public CheckBox(Control parent, int x, int y, int width, int height, string text, DynamicSpriteFont font) : base(parent, x, y, width, height) {
            _text = text ?? "CheckBox";
            _textSize = font.MeasureString(_text);
            _font = font;
            _tickBox = new Rectangle(x, y + height / 2 - 5, 10, 10);
        }

        /// <summary>
        ///     Gets or sets the checkbox's text.
        /// </summary>
        public string Description {
            get => _text;
            set {
                if (_text == value) {
                    return;
                }

                _text = value;
                _textSize = _font.MeasureString(value);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the box is checked.
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        ///     Gets or sets the text alignment.
        /// </summary>
        public TextAlign TextAlign {
            get => _textAlign;
            set {
                if (_textAlign == value) {
                    return;
                }

                _textAlign = value;
                switch (value) {
                    case TextAlign.BottomLeft:
                        _textPosition = new Vector2(BoundBox.X + 10 + 2, BoundBox.Y + BoundBox.Height - _textSize.Y);
                        break;
                    case TextAlign.CenterLeft:
                        _textPosition = new Vector2(Position.X + 10 + 5, Math.Max(BoundBox.Y, (BoundBox.Y + BoundBox.Height) / 2 - _textSize.Y));
                        break;
                    case TextAlign.TopLeft:
                        _textPosition = new Vector2(Position.X + 10 + 15, BoundBox.Y + 5);
                        break;
                    case TextAlign.TopCenter:
                        _textPosition = new Vector2(Math.Max(BoundBox.X + 10 + 2, BoundBox.X + BoundBox.Width / 2 - _textSize.X / 2),
                            BoundBox.Y + 5);
                        break;
                    case TextAlign.Center:
                        _textPosition = new Vector2(Math.Max(BoundBox.X + 10 + 2, BoundBox.X + BoundBox.Width / 2 - _textSize.X / 2),
                            Math.Max(BoundBox.Y + _textSize.Y, BoundBox.Y + BoundBox.Height / 2 - _textSize.Y));
                        break;
                    case TextAlign.BottomCenter:
                        _textPosition = new Vector2(Math.Max(BoundBox.X + 10 + 2, BoundBox.X + BoundBox.Width / 2 - _textSize.X / 2),
                            Math.Max(BoundBox.Y + 2, BoundBox.Y + BoundBox.Height - _textSize.Y));
                        break;
                    case TextAlign.TopRight:
                        _textPosition = new Vector2(BoundBox.X + BoundBox.Width - _textSize.X, BoundBox.Y + 5);
                        break;
                    case TextAlign.CenterRight:
                        _textPosition = new Vector2(BoundBox.X + BoundBox.Y - _textSize.X, Math.Max(BoundBox.Y, (BoundBox.Y + BoundBox.Height) / 2 - _textSize.Y));
                        break;
                    case TextAlign.BottomRight:
                        _textPosition = new Vector2(Math.Max(BoundBox.X + 10 + 2, BoundBox.X + BoundBox.Width - _textSize.X),
                            BoundBox.Y + BoundBox.Height - _textSize.Y);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            base.Draw(gameTime, spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, DepthStencilState.Default,
                new RasterizerState {ScissorTestEnable = true});
            spriteBatch.GraphicsDevice.ScissorRectangle = BoundBox;
            if (IsChecked) {
                spriteBatch.FillRectangle(_tickBox, BackgroundColor);
            }
            else {
                spriteBatch.DrawRectangle(_tickBox, BackgroundColor);
            }

            spriteBatch.DrawString(_font, _text, _textPosition, Color.White);
            spriteBatch.End();
        }

        protected override void OnClicked(object sender, EventArgs args) {
            base.OnClicked(sender, args);
            if (!_tickBox.Contains((int) MouseManager.MousePosition.X, (int) MouseManager.MousePosition.Y)) {
                return;
            }

            IsChecked = !IsChecked;
        }
    }
}