namespace NechritoRiven.Event.OrbwalkingModes
{
    #region

    using Core;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Menus;

    #endregion

    internal class ComboMode : Core
    {
        #region Public Methods and Operators

        private static void R2()
        {
           
            
                var target = TargetSelector.GetTarget(Player.AttackRange + 310, TargetSelector.DamageType.Physical);

                var pred = Spells.R.GetPrediction(target, true, collisionable: new[] { CollisionableObjects.YasuoWall });

                if (pred.Hitchance < HitChance.High)
                {
                    return;
                }

                if ((!MenuConfig.OverKillCheck && Qstack > 1)
                    || (MenuConfig.OverKillCheck && !Spells.Q.IsReady() && Qstack == 1 && target.Distance(Player) >= 315)
                    || pred.AoeTargetsHitCount > 2)
                {
                    Spells.R.Cast(pred.CastPosition);
                }
            
        }

        public static void Combo()
        {
            var target = TargetSelector.GetTarget(Player.AttackRange + 310, TargetSelector.DamageType.Physical);

            if (target == null || !target.IsValidTarget()) return;

            if (Spells.R.IsReady() && Spells.R.Instance.Name == IsSecondR)
            {
                R2();
            }

            if (Qstack == 3 && target.Distance(Player) >= Player.AttackRange && MenuConfig.Q3Wall)
            {
                var wallPoint = FleeLogic.GetFirstWallPoint(Player.Position, Player.Position.Extend(target.Position, 650));

                Player.GetPath(wallPoint);

                if (wallPoint.Distance(Player.Position) > 100)
                {
                    Player.IssueOrder(GameObjectOrder.MoveTo, wallPoint);
                }

                if (Spells.E.IsReady() && wallPoint.Distance(Player.Position) <= Spells.E.Range)
                {
                    Spells.E.Cast(wallPoint);

                    if (Spells.R.IsReady() && Spells.R.Instance.Name == IsFirstR)
                    {
                        Spells.R.Cast();
                    }

                    Utility.DelayAction.Add(190, () => Spells.Q.Cast(wallPoint));
                }

                if (wallPoint.Distance(Player.Position) <= 100)
                {
                    Spells.Q.Cast(wallPoint);
                }
            }

            if (Spells.E.IsReady() && !target.Position.IsWall())
            {
                Spells.E.Cast(target.Position);

                if (Spells.R.IsReady())
                {
                    return;
                }

                Utility.DelayAction.Add(10, Usables.CastHydra);
            }

            if (MenuConfig.AlwaysR
                && Spells.R.IsReady()
                && Spells.R.Instance.Name == IsFirstR)
            {
                Spells.R.Cast();
            }

            if (!Spells.W.IsReady() || !InRange(target))
            {
                return;
            }

            if (MenuConfig.NechLogic && (Qstack != 1 || !Spells.Q.IsReady()))
            {
                CastW(target);
            }

            if (!MenuConfig.NechLogic && (Player.HasBuff("RivenFeint") || target.IsFacing(Player)))
            {
                CastW(target);
            }
        }

        #endregion
    }
}
