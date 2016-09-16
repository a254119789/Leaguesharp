namespace NechritoRiven.Event.OrbwalkingModes
{
    #region

    using Core;

    using LeagueSharp.Common;

    using Menus;

    #endregion

    internal class ComboMode : Core
    {
        #region Public Methods and Operators

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

                if ((!MenuConfig.OverKillCheck && Qstack > 1) || (MenuConfig.OverKillCheck && !Spells.Q.IsReady() && Qstack == 1))
                {
                    Spells.R.Cast(pred.CastPosition);
                }
            }

            if (Spells.E.IsReady())
            {
                Spells.E.Cast(target.Position);
            }

            if ((Spells.Q.IsReady()
                || Player.HasBuff("RivenFeint")
                || target.Health < Dmg.GetComboDamage(target))
                && MenuConfig.AlwaysR
                && Spells.R.IsReady()
                && Spells.R.Instance.Name == IsFirstR)
            {
                ForceR();
            }

            if (!Spells.W.IsReady() || !InWRange(target)) return;

            if (!MenuConfig.NechLogic && (!Player.HasBuff("RivenFeint") || !target.IsFacing(Player)))
            {
                Spells.W.Cast();
            }
        }

        #endregion
    }
}
