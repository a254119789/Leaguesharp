namespace NechritoRiven.Event.OrbwalkingModes
{
    #region

    using LeagueSharp.Common;

    using Core;

    using Orbwalking = Orbwalking;

    #endregion

    internal class FastHarassMode : Core
    {
        #region Public Methods and Operators

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

        #endregion
    }
}
