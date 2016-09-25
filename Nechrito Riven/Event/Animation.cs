namespace NechritoRiven.Event
{
    #region

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using NechritoRiven.Core;
    using NechritoRiven.Menus;

    using Orbwalking = NechritoRiven.Orbwalking;

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

            //Game.PrintChat((Ping() + MenuConfig.Qd - AtkSpeed()).ToString());

            switch (args.Animation)
            {
                case "Spell1a":
                    LastQ = Utils.GameTimeTickCount;
                    Qstack = 2;
                    if (SafeReset())
                    {
                        Utility.DelayAction.Add(Ping() + MenuConfig.Qd - AtkSpeed(), Reset);
                    }

                    break;
                case "Spell1b":
                    LastQ = Utils.GameTimeTickCount;
                    Qstack = 3;
                    if (SafeReset())
                    {
                        Utility.DelayAction.Add(Ping() + MenuConfig.Q2D - AtkSpeed(), Reset);
                    }

                    break;
                case "Spell1c":
                    LastQ = Utils.GameTimeTickCount;
                    Qstack = 1;
                    if (SafeReset())
                    {
                        Utility.DelayAction.Add(Ping() + MenuConfig.Qld - AtkSpeed(), Reset);
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

         private static int AtkSpeed()
        {
            return (int)((Player.Level + Player.AttackSpeedMod) * 0.625);
        }

        private static int Ping()
        {
            int ping;

            if (!MenuConfig.CancelPing)
            {
                ping = 5;
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
            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos, true);
            Orbwalking.LastAaTick = 0;
        }

        private static bool SafeReset()
        {
            return Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None || MenuConfig.AnimSemi;
        }

        #endregion
    }
}