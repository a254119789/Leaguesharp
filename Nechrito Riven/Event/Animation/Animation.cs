﻿namespace NechritoRiven.Event.Animation
{
    #region

    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using NechritoRiven.Core;

    using Orbwalking = Orbwalking;

    #endregion

    internal class Animation : Core
    {
        #region Public Methods and Operators

        private static readonly bool PingActive = MenuConfig.CancelPing;

        private static Obj_AI_Hero Target => TargetSelector.GetTarget(ObjectManager.Player.AttackRange + 50, TargetSelector.DamageType.Physical);

        private static Obj_AI_Minion Mob => (Obj_AI_Minion)MinionManager.GetMinions(ObjectManager.Player.AttackRange + 50, MinionTypes.All, MinionTeam.Neutral).FirstOrDefault();

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
                        Utility.DelayAction.Add(ResetDelay(PingActive, MenuConfig.Qd), Reset);

                        Console.WriteLine("Q1 Delay: " + ResetDelay(PingActive, MenuConfig.Qd));
                    }
                    break;
                case "Spell1b":
                    LastQ = Utils.GameTimeTickCount;
                    Qstack = 3;

                    if (SafeReset())
                    {
                        Utility.DelayAction.Add(ResetDelay(PingActive, MenuConfig.Q2D), Reset);

                        Console.WriteLine("Q2 Delay: " + ResetDelay(PingActive, MenuConfig.Q2D));
                    }
                    break;
                case "Spell1c":
                    LastQ = Utils.GameTimeTickCount;
                    Qstack = 1;

                    if (SafeReset())
                    {
                        Utility.DelayAction.Add(ResetDelay(PingActive, MenuConfig.Qld), Reset);

                        Console.WriteLine("Q3 Delay: " 
                         + ResetDelay( PingActive, MenuConfig.Qld)
                         + Environment.NewLine + ">----END----<");
                    }
                    break;
            }
        }

        #endregion

        #region Methods

        private static void Emotes()
        {
            if (ObjectManager.Player.HasBuffOfType(BuffType.Stun) 
                || ObjectManager.Player.HasBuffOfType(BuffType.Snare)
                || ObjectManager.Player.HasBuffOfType(BuffType.Knockback)
                || ObjectManager.Player.HasBuffOfType(BuffType.Knockup))
            {
                return;
            }

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
                    break;
            }
        }

        private static int ResetDelay(bool ping, int qDelay)
        {
            qDelay -= ObjectManager.Player.Level / 2;

            if (ping)
            {
                qDelay += Game.Ping / 2;
            }

            return (int)((Target != null && Target.IsMoving) || (Mob != null && Mob.IsMoving) 
                ? qDelay * 1.15 
                : qDelay);
        }
        
        private static void Reset()
        {
            Emotes();
            Orbwalking.ResetAutoAttackTimer();
            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
           // Player.IssueOrder(GameObjectOrder.AttackTo, ObjectManager.Player.Position.Extend(ObjectManager.Player.Direction, 50));
        }

        private static bool SafeReset()
        {
            return Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None || MenuConfig.AnimSemi;
        }

        #endregion
    }
}