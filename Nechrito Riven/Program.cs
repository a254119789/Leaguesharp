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
                return;
            }

            Load.LoadAssembly();
        }

        #endregion
    }
}