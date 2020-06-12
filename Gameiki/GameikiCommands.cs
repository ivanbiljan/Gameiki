using Gameiki.Extensions;
using Gameiki.Framework.Commands;

namespace Gameiki {
    internal sealed class GameikiCommands {
        [Command("godmode", Description = "Toggles godmode, which enables infinite resources, invincibility and other.")]
        public void Godmode() {
            var session = Gameiki.MyPlayer.GetData<Session>("__session");
            session.IsGodmode = !session.IsGodmode;
            Gameiki.MyPlayer.SendGameikiMessage($"Godmode is now ");
        }
    }
}