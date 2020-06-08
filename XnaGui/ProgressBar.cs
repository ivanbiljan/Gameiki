using System;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaGui.Extensions;

namespace XnaGui {
    public class ProgressBar : RangeControlBase {
        private int _step;

        public ProgressBar(Control parent, int x, int y, int width, int height) : base(parent, x, y, width, height) {
        }

        public int Step {
            get => _step;
            set {
                if (value < 0 || value > Maximum) {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                
                _step = value;
            }
        }

        public void Increment(int step) {
            if (Value + step > Maximum) {
                Value = Maximum;
            } else if (Value + step < Minimum) {
                Value = Minimum;
            }
            else {
                Value += step;
            }
        }

        public void PerformStep() {
            Increment(Step);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            base.Draw(gameTime, spriteBatch);
            
            spriteBatch.Begin();

            var filledRectangle = new Rectangle((int) Position.X, (int) Position.Y, BoundBox.Width / Maximum * Value, BoundBox.Height);
            spriteBatch.DrawRectangle(BoundBox, ForegroundColor);
            spriteBatch.FillRectangle(filledRectangle, ForegroundColor);

            spriteBatch.End();
        }
    }
}