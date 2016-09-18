namespace NechritoRiven.Event.OrbwalkingModes
{
    #region

    using Core;

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
                Utility.DelayAction.Add(180, FlashW);
                ForceW(); // Just in case.
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

        #endregion
    }
}
