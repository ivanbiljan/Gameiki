using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaGui.Extensions;

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
        public int LargeChange { get; set; } = 1;

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            base.Draw(gameTime, spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, DepthStencilState.Default,
                new RasterizerState {ScissorTestEnable = true});
            spriteBatch.GraphicsDevice.ScissorRectangle = BoundBox;
            
            var filledRectangle = new Rectangle(_trackbar.X, _trackbar.Y,  BoundBox.Width / Maximum * Value, _trackbar.Height);
            spriteBatch.DrawRectangle(_trackbar, Color.White);
            spriteBatch.FillRectangle(filledRectangle, Color.Chartreuse);

            var thumbRadius = 5;
            var thumbCenter = new Vector2(_trackbar.X + BoundBox.Width / Maximum * Value - thumbRadius / 2, _trackbar.Y + 2);
            spriteBatch.FillCircle(thumbCenter, thumbRadius, Color.Black);

            spriteBatch.End();
        }
    }
}