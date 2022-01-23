using Funkin.Game.API;
using osu.Framework.Logging;

namespace Funkin.Game.Mod.Mania
{
    public sealed class ManiaMod : IMod
    {
        public void Load()
        {
            Logger.Log("TEST FROM MANIAMOD");
        }

        public Identifier MakeIdentifier(string content) => new(Constants.MANIA_NAME, content);
    }
}
