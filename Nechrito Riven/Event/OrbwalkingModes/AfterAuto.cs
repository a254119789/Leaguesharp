namespace NechritoRiven.Event.OrbwalkingModes
{
    #region

    using System;
    using System.Linq;

    using Core;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Menus;

    using Orbwalking = Orbwalking;

    #endregion

    internal class AfterAuto : Core
    {
        #region Public Methods and Operators

        // Jungle, Combo etc.
        public static void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe || !Orbwalking.IsAutoAttack(args.SData.Name)) return;

            var a = HeroManager.Enemies.Where(x => x.IsValidTarget(Player.AttackRange + 360));

            var targets = a as Obj_AI_Hero[] ?? a.ToArray();

            foreach (var target in targets)
            {
                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
                {
                    if (Spells.Q.IsReady())
                    {
                        CastQ(target);
                    }
                }

                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
                {
                    if (Qstack == 2)
                    {
                        CastQ(target);
                    }
                }

                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.FastHarass)
                {
                    if (Spells.Q.IsReady())
                    {
                        CastQ(target);
                    }

                    if (!Spells.Q.IsReady())
                    {
                       CastW(target);
                    }
                }

                if (Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Burst) return;

                if (Spells.R.IsReady() && Spells.R.Instance.Name == IsSecondR)
                {
                    var pred = Spells.R.GetPrediction(target);

                    if (pred.Hitchance < HitChance.High)
                    {
                        return;
                    }

                    Spells.R.Cast(pred.CastPosition);
                }

                if (Spells.Q.IsReady())
                {
                    CastQ(target);
                }
            }

            if (Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear)
            {
                return;
            }

            if (args.Target is Obj_AI_Minion)
            {
                var minions = MinionManager.GetMinions(Player.AttackRange + 450);

                if (minions != null)
                {
                    foreach (var m in minions)
                    {
                        if (!MenuConfig.LaneQ || m.UnderTurret(true))
                        {
                            return;
                        }

                        if (Spells.Q.IsReady())
                        {
                            CastQ(m);
                        }
                    }
                }

                var mobs = MinionManager.GetMinions(Player.Position, 400f, MinionTypes.All, MinionTeam.Neutral);

                if (mobs == null) return;

                foreach (var m in mobs)
                {
                    if (MenuConfig.JnglQ && Spells.Q.IsReady())
                    {
                        CastQ(m);
                    }

                    if (!Spells.W.IsReady()
                        || !MenuConfig.JnglW
                        || Player.HasBuff("RivenFeint")
                        || Qstack > 2)
                    {
                        return;
                    }

                    CastW(m);
                }
            }

            var inhib = args.Target as Obj_BarracksDampener;
            if (inhib != null)
            {
                if (inhib.IsValid && Spells.Q.IsReady() && MenuConfig.LaneQ)
                {
                    Spells.Q.Cast(inhib.Position - 350);
                }
            }

            var turret = args.Target as Obj_AI_Turret;

            if (turret == null) return;

            if (turret.IsValid && Spells.Q.IsReady() && MenuConfig.LaneQ)
            {
                Spells.Q.Cast(turret.Position - 250);
            }
        }

        #endregion
    }
}