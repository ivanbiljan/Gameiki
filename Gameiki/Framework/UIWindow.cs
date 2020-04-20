using Gameiki.Patcher.Events;
using Microsoft.Xna.Framework;

namespace Gameiki.Framework {
    internal sealed class UIWindow : UserInterfaceItem {
        public override AnchorPosition AnchorPosition { get; protected set; } = AnchorPosition.Float;
        public override Rectangle Boundaries { get; protected set; }
        public override void Draw(object sender, DrawEventArgs args) {
        }
    }
}