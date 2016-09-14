﻿namespace NechritoRiven.Event
{
    #region

    using LeagueSharp;
    using LeagueSharp.Common;

    using NechritoRiven.Core;
    using NechritoRiven.Menus;

    using Orbwalking = NechritoRiven.Orbwalking;

    #endregion

    internal class Anim : Core
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
            }
        }

        private static int Ping()
        {
            int ping;

            if (!MenuConfig.CancelPing)
            {
                ping = 0;
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
            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos + 10, false);
            Orbwalking.LastAaTick = 0;
        }

        private static bool SafeReset()
        {
            return Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None;
        }

        #endregion
    }
}