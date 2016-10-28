namespace NechritoRiven.Event
{
    #region

    using System;
    using System.Linq;

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

            var target = TargetSelector.GetTarget(ObjectManager.Player.AttackRange + 50, TargetSelector.DamageType.Physical);

            var mob = MinionManager.GetMinions(
              ObjectManager.Player.AttackRange + 50,
              MinionTypes.All,
              MinionTeam.Neutral).FirstOrDefault();

            var isMoving = (target != null && target.IsMoving) || (mob != null && mob.IsMoving);

            var delay = Ping();

            switch (args.Animation)
            {
                case "Spell1a":
                    LastQ = Utils.GameTimeTickCount;
                    Qstack = 2;
                    if (SafeReset())
                    {
                        if (isMoving)
                        {
                            Utility.DelayAction.Add((int)((MenuConfig.Qd + delay)* 1.18), Reset);
                            Console.WriteLine("Q1 Slow Delay: " + (MenuConfig.Qd + delay) * 1.18);
                        }
                        else
                        {
                            Utility.DelayAction.Add(MenuConfig.Qd + delay, Reset);
                            Console.WriteLine("Q1 Fast Delay: " + (MenuConfig.Qd + delay));
                        }
                    }

                    break;
                case "Spell1b":
                    LastQ = Utils.GameTimeTickCount;
                    Qstack = 3;
                    if (SafeReset())
                    {
                        if (isMoving)
                        {
                            Utility.DelayAction.Add((int)((MenuConfig.Q2D + delay)* 1.18), Reset);
                            Console.WriteLine("Q2 Slow Delay: " + (MenuConfig.Q2D + delay)* 1.18);
                        }
                        else
                        {
                            Utility.DelayAction.Add(MenuConfig.Q2D + delay, Reset);
                            Console.WriteLine("Q2 Fast Delay: " + (delay + MenuConfig.Q2D));
                        }
                    }

                    break;
                case "Spell1c":
                    LastQ = Utils.GameTimeTickCount;
                    Qstack = 1;
                    if (SafeReset())
                    {
                        if (isMoving)
                        {
                            Utility.DelayAction.Add((int)((MenuConfig.Qld + delay)* 1.18), Reset);
                            Console.WriteLine("Q3 Slow Delay: " + (MenuConfig.Qld + delay)* 1.18);
                            Console.WriteLine(">----END----<");

                        }
                        else
                        {
                            Utility.DelayAction.Add(MenuConfig.Qld + delay, Reset);
                            Console.WriteLine("Q3 Fast Delay: " + (MenuConfig.Qld + delay));
                            Console.WriteLine(">----END----<");
                        }
                    }

                    break;
            }
        }

        #endregion

        #region Methods

        private static void Emotes()
        {
            if (ObjectManager.Player.HasBuffOfType(BuffType.Stun) || ObjectManager.Player.HasBuffOfType(BuffType.Snare))
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

       // private static int AtkSpeed => (int)(1400 / Player.AttackSpeedMod * 3.75);

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
            Orbwalking.ResetAutoAttackTimer();
            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
           // Player.IssueOrder(GameObjectOrder.AttackTo, Game.CursorPos);
        }

        private static bool SafeReset()
        {
            return Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None || MenuConfig.AnimSemi;
        }

        #endregion
    }
}