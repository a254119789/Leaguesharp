namespace NechritoRiven.Event.OrbwalkingModes
{
    #region

    using Core;

    using LeagueSharp;
    using LeagueSharp.Common;

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
                var selectedTarget = TargetSelector.GetSelectedTarget();

                if (selectedTarget == null 
                    || !selectedTarget.IsValidTarget(410 + Spells.W.Range)
                    || Player.Distance(selectedTarget.Position) < 400)
                {
                    return;
                }

                Usables.CastYoumoo();

                Spells.E.Cast(selectedTarget.Position);
                Spells.R.Cast();
                Utility.DelayAction.Add(170, FlashW);
            }
            else
            {
                var target = TargetSelector.GetTarget(Player.AttackRange + 360, TargetSelector.DamageType.Physical);

                if (target == null) return;

                if (Spells.R.IsReady() && Spells.R.Instance.Name == IsSecondR && Qstack > 2)
                {
                    var pred = Spells.R.GetPrediction(
                        target,
                        true,
                        collisionable: new[] { CollisionableObjects.YasuoWall });

                    if (pred.Hitchance < HitChance.High)
                    {
                        return;
                    }

                    Spells.R.Cast(pred.CastPosition);
                }

                if (Spells.E.IsReady())
                {
                    Spells.E.Cast(target.Position);
                }
                else if (Spells.R.IsReady() && Spells.R.Instance.Name == IsFirstR)
                {
                    Spells.R.Cast();
                }
                else if (Spells.W.IsReady())
                {
                    CastW(target);
                    DoubleCastQ(target);
                }
            }
        }

        #endregion
    }
}
