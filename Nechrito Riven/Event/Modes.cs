﻿namespace NechritoRiven.Event
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

    internal class Modes : Core
    {
        #region Public Methods and Operators

        public static void Burst()
        {
            if (Spells.R.IsReady() && Spells.R.Instance.Name == IsSecondR && !MenuConfig.DisableR2)
            {
                var target = TargetSelector.GetTarget(450 + 70, TargetSelector.DamageType.Physical);
                var pred = Spells.R.GetPrediction(target);

                if (pred.Hitchance < HitChance.High)
                {
                    return;
                }

                if ((!MenuConfig.OverKillCheck && Qstack > 1) || (MenuConfig.OverKillCheck && !Spells.Q.IsReady() && Qstack == 1))
                {
                    Console.WriteLine("Casting R");
                    Spells.R.Cast(pred.CastPosition);
                }
            }

            if (Spells.Flash.IsReady() && Spells.Q.IsReady() && Spells.R.IsReady() && Spells.R.Instance.Name == IsFirstR
                && MenuConfig.AlwaysF)
            {
                var target = TargetSelector.GetSelectedTarget();

                if (target == null || !target.IsValidTarget(425 + Spells.W.Range) || target.IsInvulnerable
                    || !Spells.E.IsReady() || !Spells.W.IsReady() || Player.Distance(target.Position) < 580)
                {
                    return;
                }

                Usables.CastYoumoo();
                Spells.E.Cast(target.Position);
                Spells.R.Cast();
                Utility.DelayAction.Add(180, FlashW);
                ForceW();
            }
            else
            {
                var target = TargetSelector.GetTarget(450 + 70, TargetSelector.DamageType.Physical);
                if (!target.IsValidTarget() || target == null) return;

                if (Spells.E.IsReady())
                {
                    Spells.E.Cast(target.Position);
                }

                if (Spells.R.IsReady() && Spells.R.Instance.Name == IsFirstR)
                {
                    Spells.R.Cast();
                }

                if (Spells.W.IsReady() && InWRange(target))
                {
                    Spells.W.Cast();
                }
            }
        }

        public static void Combo()
        {
            var target = TargetSelector.GetTarget(Player.AttackRange + 310, TargetSelector.DamageType.Physical);

            if (target == null || target.IsDead || !target.IsValidTarget() || target.IsInvulnerable) return;

            if (Spells.R.IsReady() && Spells.R.Instance.Name == IsSecondR && !MenuConfig.DisableR2)
            {
                var pred = Spells.R.GetPrediction(target);

                if (pred.Hitchance < HitChance.High)
                {
                    return;
                }

                if (Qstack > 1 && !MenuConfig.OverKillCheck)
                {
                    Spells.R.Cast(pred.CastPosition);
                }

                if (MenuConfig.OverKillCheck && !Spells.Q.IsReady() && Qstack == 1)
                {
                    Spells.R.Cast(pred.CastPosition);
                }
            }

            if (Spells.E.IsReady())
            {
                Spells.E.Cast(target.Position);
            }

            if ((Spells.Q.IsReady() || Player.HasBuff("RivenFeint") || target.Health < Dmg.GetComboDamage(target))
                && MenuConfig.AlwaysR && Spells.R.IsReady() && Spells.R.Instance.Name == IsFirstR)
            {
                ForceR();
            }

            if (!Spells.W.IsReady() || !InWRange(target)) return;

            if (!MenuConfig.NechLogic && (!Player.HasBuff("RivenFeint") || !target.IsFacing(Player)))
            {
                Spells.W.Cast();
            }
        }

        public static void FastHarass()
        {
            if (!Spells.Q.IsReady() || !Spells.E.IsReady()) return;

            var target = TargetSelector.GetTarget(450 + Player.AttackRange + 70, TargetSelector.DamageType.Physical);

            if (!target.IsValidTarget() || target.IsZombie) return;

            if (!Orbwalking.InAutoAttackRange(target) && !InWRange(target))
            {
                Spells.E.Cast(target.Position);
            }

            Utility.DelayAction.Add(10, ForceItem);
            Utility.DelayAction.Add(170, () => ForceCastQ(target));
        }

        public static void Flee()
        {
            if (MenuConfig.WallFlee)
            {
                var end = Player.ServerPosition.Extend(Game.CursorPos, Spells.Q.Range);
                var isWallDash = FleeLogic.IsWallDash(end, Spells.Q.Range);

                var eend = Player.ServerPosition.Extend(Game.CursorPos, Spells.E.Range);
                var wallE = FleeLogic.GetFirstWallPoint(Player.ServerPosition, eend);
                var wallPoint = FleeLogic.GetFirstWallPoint(Player.ServerPosition, end);
                Player.GetPath(wallPoint);

                if (Spells.Q.IsReady() && Qstack < 3)
                {
                    Spells.Q.Cast(Game.CursorPos);
                }

                if (!isWallDash || Qstack != 3 || !(wallPoint.Distance(Player.ServerPosition) <= 800)) return;

                ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, wallPoint);

                if (!(wallPoint.Distance(Player.ServerPosition) <= 600)) return;

                ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, wallPoint);

                if (!(wallPoint.Distance(Player.ServerPosition) <= 45)) return;

                if (Spells.E.IsReady())
                {
                    Spells.E.Cast(wallE);
                }

                if (Qstack != 3 || !(end.Distance(Player.Position) <= 260) || !wallPoint.IsValid()) return;

                Player.IssueOrder(GameObjectOrder.MoveTo, wallPoint);
                Spells.Q.Cast(wallPoint);
            }
            else
            {
                var enemy =
                    HeroManager.Enemies.Where(
                        target =>
                        target.IsValidTarget(
                            Player.HasBuff("RivenFengShuiEngine")
                                ? 70 + 195 + Player.BoundingRadius
                                : 70 + 120 + Player.BoundingRadius) && Spells.W.IsReady());

                var x = Player.Position.Extend(Game.CursorPos, 300);
                var objAitargetes = enemy as Obj_AI_Hero[] ?? enemy.ToArray();

                if (Spells.W.IsReady() && objAitargetes.Any()) foreach (var target in objAitargetes) if (InWRange(target)) Spells.W.Cast();

                if (Spells.Q.IsReady() && !Player.IsDashing()) Spells.Q.Cast(Game.CursorPos);

                if (MenuConfig.FleeYomuu)
                {
                    Usables.CastYoumoo();
                }

                if (Spells.E.IsReady() && !Player.IsDashing()) Spells.E.Cast(x);
            }
        }

        public static void Harass()
        {
            var target = TargetSelector.GetTarget(400, TargetSelector.DamageType.Physical);

            if (target == null)
            {
                return;
            }

            if (Spells.Q.IsReady() && Spells.W.IsReady() && Spells.E.IsReady() && Qstack == 1)
            {
                if (target.IsValidTarget() && !target.IsZombie)
                {
                    ForceCastQ(target);
                    ForceW();
                }
            }

            if (!Spells.Q.IsReady() || !Spells.E.IsReady() || Qstack != 3 || Orbwalking.CanAttack()
                || !Orbwalking.CanMove(5)) return;

            var epos = Player.ServerPosition + (Player.ServerPosition - target.ServerPosition).Normalized() * 300;

            Spells.E.Cast(epos);
            Utility.DelayAction.Add(190, () => Spells.Q.Cast(epos));
        }

        public static void Jungleclear()
        {
            var mobs = MinionManager.GetMinions(
                Player.Position,
                Player.AttackRange + Spells.E.Range + 50,
                MinionTypes.All,
                MinionTeam.Neutral);

            if (mobs == null) return;

            foreach (var m in mobs)
            {
                if (!m.IsValid || !Spells.E.IsReady() || !MenuConfig.JnglE || Player.IsWindingUp) return;

                Spells.E.Cast(m.Position);
                Utility.DelayAction.Add(10, ForceItem);
            }
        }

        public static void Laneclear()
        {
            var minions = MinionManager.GetMinions(Player.AttackRange + 380);

            if (minions == null)
            {
                return;
            }

            foreach (var m in minions)
            {
                if (m.UnderTurret(true)) return;

                if (Spells.E.IsReady() && MenuConfig.LaneE)
                {
                    Spells.E.Cast(m);
                }

                if (!Spells.W.IsReady()
                    || !MenuConfig.LaneW
                    || !InWRange(m)
                    || Player.IsWindingUp
                    || m.Health > Spells.W.GetDamage(m))
                {
                    return;
                }

                Spells.W.Cast(m);
            }
        }

        // Jungle, Combo etc.
        public static void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;

            var a = HeroManager.Enemies.Where(x => x.IsValidTarget(Player.AttackRange + 360));

            var targets = a as Obj_AI_Hero[] ?? a.ToArray();

            foreach (var target in targets)
            {
                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
                {
                    if (Spells.Q.IsReady())
                    {
                        ForceItem();
                        Utility.DelayAction.Add(1, () => ForceCastQ(target)); // Else Q AA wont go off at all times, cause of tiamat.
                    }

                    if (MenuConfig.NechLogic && (Qstack != 1 || !Spells.Q.IsReady()))
                    {
                        ForceW();
                        return;
                    }
                }

                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
                {
                    if (Qstack == 2)
                    {
                        ForceItem();
                        Utility.DelayAction.Add(1, () => ForceCastQ(target));
                    }
                }

                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.FastHarass)
                {
                    if (Spells.Q.IsReady() && InQRange(target))
                    {
                        Utility.DelayAction.Add(1, () => ForceCastQ(target));
                    }

                    if (Spells.W.IsReady() && !Spells.Q.IsReady() && InWRange(target))
                    {
                        Spells.W.Cast(target);
                    }
                }

                if (Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Burst) return;

                if (!Spells.Q.IsReady()) return;

                ForceItem();
                Utility.DelayAction.Add(1, () => ForceCastQ(target));
            }

            if (Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear) return;

            if (args.Target is Obj_AI_Minion)
            {
                var minions = MinionManager.GetMinions(Player.AttackRange + 450);

                if (minions == null)
                {
                    return;
                }

                foreach (var m in minions)
                {
                    if (!Spells.Q.IsReady() || !MenuConfig.LaneQ || m.UnderTurret(true))
                    {
                        return;
                    }

                    ForceItem();
                    Utility.DelayAction.Add(1, () => ForceCastQ(m));
                }
            }

            var mobs = MinionManager.GetMinions(Player.Position, 400f, MinionTypes.All, MinionTeam.Neutral);

            if (mobs == null) return;

            foreach (var m in mobs)
            {
               
                if (Spells.Q.IsReady() && MenuConfig.JnglQ)
                {
                    ForceItem();
                    Utility.DelayAction.Add(1, () => ForceCastQ(m));
                }

                if (!Spells.W.IsReady() || !MenuConfig.JnglW || Player.HasBuff("RivenFeint") || Qstack > 2) return;

                ForceItem();
                Spells.W.Cast(m);
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

        public static void QMove()
        {
            if (!MenuConfig.QMove || !Spells.Q.IsReady())
            {
                return;
            }

            Utility.DelayAction.Add(Game.Ping + 2, () => Spells.Q.Cast(Player.Position - 15));
        }

        #endregion
    }
}