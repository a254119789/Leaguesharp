namespace NechritoRiven.Event.OrbwalkingModes
{
    #region

    using Core;

    using LeagueSharp;
    using LeagueSharp.Common;

    #endregion

    internal class Combos : Core
    {
        private static Obj_AI_Hero Target => TargetSelector.GetTarget(375, TargetSelector.DamageType.Physical);

        private static Obj_AI_Hero SelectedTarget => TargetSelector.GetSelectedTarget();

        public static void Burst()
        {
            if (Spells.R.IsReady() && Spells.R.Instance.Name == IsSecondR)
            {
                BurstCastR2(Target);
            }
           else if (Spells.Flash.IsReady() && MenuConfig.AlwaysF)
            {
                if (SelectedTarget == null
                    || !SelectedTarget.IsValidTarget(Player.AttackRange + 625)
                    || Player.Distance(SelectedTarget.Position) < Player.AttackRange
                    || (MenuConfig.Flash && SelectedTarget.Health > Dmg.GetComboDamage(SelectedTarget) && !Spells.R.IsReady() && Spells.R.Instance.Name != IsSecondR)
                    || (!MenuConfig.Flash && !Spells.W.IsReady()))
                {
                    return;
                }

                Usables.CastYoumoo();
                Spells.E.Cast(SelectedTarget.Position);
                Spells.R.Cast();
                Utility.DelayAction.Add(170, BackgroundData.FlashW);
            }
            else
            {
                Combo();
            }
        }

        public static void Combo()
        {
            if (Target == null)
            {
                return;
            }

            if (Spells.R.IsReady() && Spells.R.Instance.Name == IsSecondR)
            {
                ComboCastR2(Target);
            }

           if (Spells.E.IsReady() && MenuConfig.Q3Wall && Qstack >= 3)
            {
                Q3Wall(Target);
            }

            if (Spells.E.IsReady())
            {
                Spells.E.Cast(Target.Position);
            }

            if (Spells.R.IsReady() && Spells.R.Instance.Name == IsFirstR && MenuConfig.UseR1)
            {
                if (MenuConfig.SafeR1 && Target.HealthPercent < 15 && Spells.Q.IsReady())
                {
                    return;
                }
                Spells.R.Cast();
            }

            if (!Spells.W.IsReady() || !BackgroundData.InRange(Target))
            {
                return;
            }

            DoublecastWQ(Target);
        }

        private static void ComboCastR2(Obj_AI_Hero target)
        {
            var pred = Spells.R.GetPrediction(target, true, collisionable: new[] { CollisionableObjects.YasuoWall });

            if (pred.Hitchance < HitChance.High || target.HasBuff(BackgroundData.InvulnerableList.ToString()) || Player.IsWindingUp)
            {
                return;
            }

            if ((!MenuConfig.OverKillCheck && Qstack > 1)
                || MenuConfig.OverKillCheck
                && (target.HealthPercent <= 40
                && !Spells.Q.IsReady() && Qstack == 1
                || target.Distance(Player) >= Player.AttackRange + 310))
            {
                Spells.R.Cast(pred.CastPosition);
            }
        }

        private static void BurstCastR2(Obj_AI_Hero target)
        {
            var pred = Spells.R.GetPrediction(target, true, collisionable: new[] { CollisionableObjects.YasuoWall });

            if (pred.Hitchance < HitChance.High 
                || target.HasBuff(BackgroundData.InvulnerableList.ToString())
                || Player.IsWindingUp
                || Qstack == 1)
            {
                return;
            }

            Spells.R.Cast(pred.CastPosition);
        }

        private static void DoublecastWQ(Obj_AI_Hero target)
        {
            if (MenuConfig.Doublecast && Spells.Q.IsReady() && Qstack != 2)
            {
                BackgroundData.CastW(target);
                BackgroundData.DoubleCastQ(target);
            }
            else
            {
                BackgroundData.CastW(target);
            }
        }

        private static void Q3Wall(Obj_AI_Hero target)
        {
            if (target.Distance(Player) < Player.AttackRange || target.Distance(Player) > 650)
            {
                return;
            }

            var wallPoint = FleeLogic.GetFirstWallPoint(Player.Position, Player.Position.Extend(target.Position, 650));
            Player.GetPath(wallPoint);

            if (!Spells.E.IsReady()
                || wallPoint.Distance(Player.Position) > Spells.E.Range
                || !wallPoint.IsValid())
            {
                return;
            }

            Spells.E.Cast(wallPoint);

            if (Spells.R.IsReady() && Spells.R.Instance.Name == IsFirstR)
            {
                Spells.R.Cast();
            }

            Utility.DelayAction.Add(190, () => Spells.Q.Cast(wallPoint));

            if (wallPoint.Distance(Player.Position) <= 100)
            {
                Spells.Q.Cast(wallPoint);
            }
        }
    }
}
