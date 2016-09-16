namespace NechritoRiven
{
    #region

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    #endregion

    public class Program
    {
        #region Methods

        private static void Main()
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
        }

        private static void OnLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != "Riven")
            {
                Game.PrintChat("Could not load Riven");
                return;
            }

            Console.WriteLine("Loading...");
            Load.Load.LoadAssembly();
        }

        #endregion
    }
}