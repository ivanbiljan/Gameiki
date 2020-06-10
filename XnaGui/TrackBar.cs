using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaGui.Extensions;
using XnaGui.Input;

namespace XnaGui {

    public class TrackBar : RangeControlBase {
        private readonly Rectangle _trackbar;
        
        public TrackBar(Control parent, int x, int y, int width, int height) : base(parent, x, y, width, height) {
            _trackbar = new Rectangle(x + 5, y + 5, width - 5, 5);
        }

        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        public Orientation Orientation { get; set; } = Orientation.Horizontal;

        /// <summary>
        /// Gets or sets the value that denotes the number of positions to move by if keyboard arrows are used to move the slider.
        /// </summary>
        public int SmallChange { get; set; } = 1;

        /// <summary>
        /// Gets or sets the value that denotes the number of positions to move by if the trackbar is clicked on. 
        /// </summary>
        public int LargeChange { get; set; } = 10;

        public override void Update(GameTime gameTime) {
            MouseManager.Update();
            base.Update(gameTime);
        }

        protected override void OnLeftButtonDown(object sender, EventArgs args) {
            if (MouseManager.MousePosition.X > (int) (_trackbar.X + (float) _trackbar.Width / Maximum * Value) - 10 / 2) {
                Value += LargeChange;
            } 
            
            base.OnLeftButtonDown(sender, args);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, DepthStencilState.Default,
                new RasterizerState {ScissorTestEnable = true});
            spriteBatch.GraphicsDevice.ScissorRectangle = BoundBox;

            var filledRectangle = new Rectangle(_trackbar.X, _trackbar.Y,  BoundBox.Width / Maximum * Value, _trackbar.Height);
            spriteBatch.DrawRectangle(_trackbar, Color.White);
            spriteBatch.FillRectangle(filledRectangle, Color.Chartreuse);

            const int thumbSize = 10;
            var thumbRectangle = new Rectangle((int) (_trackbar.X + (float) _trackbar.Width / Maximum * Value) - thumbSize / 2,
                _trackbar.Y - 2, thumbSize, thumbSize);
            spriteBatch.FillRectangle(thumbRectangle, Color.Black);

            spriteBatch.End();
            
            base.Draw(gameTime, spriteBatch);
        }
    }
}