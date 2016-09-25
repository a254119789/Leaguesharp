namespace NechritoRiven.Event.OrbwalkingModes
{
    #region

    using Core;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Menus;

    #endregion

    internal class BurstMode : Core
    {
        #region Public Methods and Operators

        public static void Burst()
        {
            if (Spells.Flash.IsReady()
                && Spells.R.IsReady()
                && Spells.W.IsReady()
                && MenuConfig.AlwaysF)
            {
                var target = TargetSelector.GetSelectedTarget();

                if (target == null
                    || !target.IsValidTarget(425 + Spells.W.Range)
                    || target.IsInvulnerable
                    || Player.Distance(target.Position) < 540)
                {
                    return;
                }

                Usables.CastYoumoo();

                if (Spells.E.IsReady())
                {
                    Spells.E.Cast(target.Position);
                }
               
                Spells.R.Cast();
                Spells.W.Cast();
                Utility.DelayAction.Add(10, () => Player.Spellbook.CastSpell(Spells.Flash, target.Position));
            }
            else
            {
                var target = TargetSelector.GetTarget(Player.AttackRange + 330, TargetSelector.DamageType.Physical);

                if (!target.IsValidTarget() || target == null) return;

                if (Spells.R.IsReady() && Spells.R.Instance.Name == IsSecondR)
                {
                    var pred = Spells.R.GetPrediction(target, true, collisionable: new[] { CollisionableObjects.YasuoWall });

                    if (pred.Hitchance < HitChance.High)
                    {
                        return;
                    }

                    Spells.R.Cast(pred.CastPosition);
                }

                if (Qstack == 3
                    && target.Distance(Player) >= Player.AttackRange
                    && target.Distance(Player) <= 600 
                    && Spells.R.IsReady()
                    && Spells.R.Instance.Name == IsFirstR
                    && MenuConfig.Q3Wall
                    && Spells.E.IsReady())
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

                if (Spells.E.IsReady())
                {
                    Spells.E.Cast(target.Position);
                }

                if (Spells.R.IsReady() && Spells.R.Instance.Name == IsFirstR)
                {
                    Spells.R.Cast();
                }

                if (Spells.W.IsReady())
                {
                   CastW(target);
                }
            }
        }

        #endregion
    }
}
