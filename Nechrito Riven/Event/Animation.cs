namespace NechritoRiven.Event
{
    #region

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Core;
    using Menus;

    using Orbwalking = Orbwalking;

    #endregion

    internal class Animation : Core
    {
        #region Public Methods and Operators

        public static void OnPlay(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            switch (args.Animation)
            {
                case "Spell1a":
                    LastQ = Utils.GameTimeTickCount;
                    Qstack = 2;
                    if (SafeReset())
                    {
                        Utility.DelayAction.Add(MenuConfig.Qd + Ping(), Reset);
                    }

                    break;
                case "Spell1b":
                    LastQ = Utils.GameTimeTickCount;
                    Qstack = 3;
                    if (SafeReset())
                    {
                        Utility.DelayAction.Add(MenuConfig.Q2D + Ping(), Reset);
                    }

                    break;
                case "Spell1c":
                    LastQ = Utils.GameTimeTickCount;
                    Qstack = 1;
                    if (SafeReset())
                    {
                        Utility.DelayAction.Add(MenuConfig.Qld + Ping(), Reset);
                    }

                    break;
            }
        }

        #endregion

        #region Methods

        private static void Emotes()
        {
            switch (MenuConfig.EmoteList.SelectedIndex)
            {
                case 0:
                    Game.SendEmote(Emote.Laugh);
                    break;
                case 1:
                    Game.SendEmote(Emote.Taunt);
                    break;
                case 2:
                    Game.SendEmote(Emote.Joke);
                    break;
                case 3:
                    Game.SendEmote(Emote.Dance);
                    break;
                case 4:
                    Console.WriteLine("Nechrito Riven: Q AA SLOW: No Emote!");
                    break;
            }
        }

        private static int Ping()
        {
            int ping;

            if (!MenuConfig.CancelPing)
            {
                ping = 5; // gg wp humanized
            }
            else
            {
                ping = Game.Ping / 2;
            }

            return ping;
        }

        private static void Reset()
        {
            Emotes();
            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos + 10, true);
            Orbwalking.LastAaTick = 0;
        }

        private static bool SafeReset()
        {
            // gg wp inheritance
            return Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None;
        }

        #endregion
    }
}