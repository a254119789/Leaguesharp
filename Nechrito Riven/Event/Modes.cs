﻿#region

using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using NechritoRiven.Core;
using NechritoRiven.Menus;

#endregion

namespace NechritoRiven.Event
{
    internal class Modes : Core.Core
    {
        // Jungle, Combo etc.
        public static void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;

            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
            {
                if (args.Target is Obj_AI_Minion)
                {
                    var minions = MinionManager.GetMinions(Player.AttackRange + 380);

                    if (minions == null)
                    {
                        return;
                    }

                    foreach (var m in minions)
                    {
                        if (!Spells.Q.IsReady()
                            || !MenuConfig.LaneQ
                            || m.UnderTurret(true))
                        {
                            continue;
                        }

                        ForceItem();
                       
                        ForceCastQ(m);
                    }
                }

                var objAiTurret = args.Target as Obj_AI_Turret;
                if (objAiTurret != null)
                {
                    if (objAiTurret.IsValid && Spells.Q.IsReady() && MenuConfig.LaneQ)
                    {
                        Spells.Q.Cast(objAiTurret.Position - 250);
                    }
                }

                var mobs = MinionManager.GetMinions(Player.Position, 600f, MinionTypes.All, MinionTeam.Neutral);

                if (mobs == null) return;

                foreach (var m in mobs)
                {
                    if (m.Health < Player.GetAutoAttackDamage(m))
                    {
                        continue;
                    }

                    if (Spells.Q.IsReady() && MenuConfig.JnglQ)
                    {
                        ForceItem();
                        ForceCastQ(m);
                    }

                    else if (!Spells.W.IsReady() || !MenuConfig.JnglW) return;

                    ForceItem();
                    Spells.W.Cast(m);
                }
            }

            var a = HeroManager.Enemies.Where(x => x.IsValidTarget(Player.AttackRange + 360));

            var targets = a as Obj_AI_Hero[] ?? a.ToArray();

            foreach (var target in targets)
            {
                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
                {
                    if (!Spells.Q.IsReady()) return;

                    ForceItem();
                    Usables.CastYoumoo();
                    if (!target.IsFacing(Player) && target.IsMoving)
                    {
                        Spells.Q.Cast(Player.Position.Extend(target.Position, -125)); // This is for magnet mode kiting :p
                    }
                    else
                    {
                        ForceCastQ(target);
                    }
                }

                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
                {
                    if (Qstack == 2)
                    {
                        ForceItem();
                        ForceCastQ(target);
                    }
                }

                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.FastHarass)
                {
                    if (Spells.Q.IsReady() && InQRange(target))
                    {
                        ForceCastQ(target);
                    }
                    if (Spells.W.IsReady() && !Spells.Q.IsReady() && InWRange(target))
                    {
                        Spells.W.Cast(target);
                    }
                }

                if (Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Burst) return;

                if (!InWRange(target)) return;

                if (Spells.W.IsReady())
                {
                    Spells.W.Cast(target);
                }

                ForceItem();
                ForceCastQ(target);

                if (Spells.R.IsReady() && Spells.R.Instance.Name == IsSecondR)
                {
                    Spells.R.Cast(target.Position);
                }
            }
        }

        public static void QMove()
        {
            if (!MenuConfig.QMove || !Spells.Q.IsReady())
            {
                return;
            }

            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
           
            Utility.DelayAction.Add(Game.Ping + 2, () => Spells.Q.Cast(Player.Position -15));
        }

        public static void Jungleclear()
        {
            var mobs = MinionManager.GetMinions(Player.Position, 600f, MinionTypes.All, MinionTeam.Neutral);

            if (mobs == null) return;

            foreach (var m in mobs)
            {
                if (!m.IsValid) return;

                if (Spells.E.IsReady() && MenuConfig.JnglE && !Player.IsWindingUp)
                {
                    Spells.E.Cast(m.Position);
                }
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
                if (m.UnderTurret()) continue;

                if (Spells.E.IsReady() && MenuConfig.LaneE)
                {
                    Spells.E.Cast(m);
                }

                if(!Spells.W.IsReady() || !MenuConfig.LaneW || !InWRange(m) || Player.IsWindingUp || m.Health > Spells.W.GetDamage(m)) continue;

                Spells.W.Cast(m);
            }
        }

        public static void Combo()
        {
            var target = TargetSelector.GetTarget(Player.AttackRange + 310, TargetSelector.DamageType.Physical);

            if(target == null || target.IsDead || !target.IsValidTarget() || target.IsInvulnerable) return;

            if (Spells.R.IsReady() && Spells.R.Instance.Name == IsSecondR)
            {
                var pred = Spells.R.GetPrediction(target);

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

            if (Spells.W.IsReady() && Spells.R.IsReady() && Spells.R.Instance.Name == IsFirstR && (MenuConfig.AlwaysR || Dmg.GetComboDamage(target) > target.Health))
            {
                if (InWRange(target)) return;

                Spells.E.Cast(target.Position);
                ForceR();
                Utility.DelayAction.Add(190, ForceW);
            }

           else if (Spells.W.IsReady() && Spells.Q.IsReady() && Spells.E.IsReady())
            {
                Usables.CastYoumoo();

                Utility.DelayAction.Add(10, ForceItem);
                Utility.DelayAction.Add(190, () => Spells.W.Cast());
            }

            else if (Spells.W.IsReady() && InWRange(target))
            {
                ForceW();
            }
        }

        public static void Burst()
        {
            var target = TargetSelector.GetSelectedTarget();

            if (target == null || !target.IsValidTarget(425 + Spells.W.Range) || target.IsInvulnerable) return;

            if (!Spells.Flash.IsReady()) return;

            if (!(target.Health < Dmg.GetComboDamage(target)) && !MenuConfig.AlwaysF) return;

            if (Player.Distance(target.Position) < 585) return;

            if (!Spells.R.IsReady() || !Spells.E.IsReady() || !Spells.W.IsReady() || Spells.R.Instance.Name != IsFirstR) return;

            Usables.CastYoumoo();
            Spells.E.Cast(target.Position);
            ForceR();
            Utility.DelayAction.Add(170 + Game.Ping / 2, FlashW);
            ForceItem();
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

        public static void Harass()
        {
            var target = TargetSelector.GetTarget(400, TargetSelector.DamageType.Physical);

            if (Spells.Q.IsReady() && Spells.W.IsReady() && Spells.E.IsReady() && Qstack == 1)
            {
                if (target.IsValidTarget() && !target.IsZombie)
                {
                    ForceCastQ(target);
                    ForceW();
                }
            }

            if (!Spells.Q.IsReady() || !Spells.E.IsReady() || Qstack != 3 || Orbwalking.CanAttack() || !Orbwalking.CanMove(5)) return;

            var epos = Player.ServerPosition + (Player.ServerPosition - target.ServerPosition).Normalized() * 300;

            Spells.E.Cast(epos);
            Utility.DelayAction.Add(190, () => Spells.Q.Cast(epos));
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
                { Spells.Q.Cast(Game.CursorPos); }


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
                var enemy = HeroManager.Enemies.Where(target => target.IsValidTarget(Player.HasBuff("RivenFengShuiEngine")
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
    }
}
