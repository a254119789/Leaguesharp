#region

using System;
using LeagueSharp.SDK;

#endregion

namespace Reforged_Riven
{
    internal class Program : Core
    {
        private static void Main(string[] args)
        {
            Bootstrap.Init(args);
            Events.OnLoad += Load;
        }

        private static void Load(object sender, EventArgs e)
        {
            if (GameObjects.Player.ChampionName != "Riven")
            {
                return;
            }
            Reforged_Riven.Load.LoadAssembly();
        }
    }
}
