namespace Dark_Star_Thresh
{
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    internal class Program
    {
        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
        }

        private static void OnLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != "Thresh")
            {
                Game.PrintChat("Could not load Dark Star Thresh");
                return;
            }

            Load.LoadAssembly();
        }
    }
}
