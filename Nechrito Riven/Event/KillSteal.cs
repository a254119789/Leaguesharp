namespace NechritoRiven.Event
{
    #region

    using System;

    using Core;

    using LeagueSharp.Common;

    using Menus;

    #endregion

    internal class KillSteal : Core
    {
        #region Public Methods and Operators

        public static void Update(EventArgs args)
        {
            var hero = TargetSelector.GetTarget(Spells.R.Range, TargetSelector.DamageType.Physical);

            if (hero == null
                || hero.HasBuff("kindrednodeathbuff")
                || hero.HasBuff("Undying Rage")
                || hero.HasBuff("JudicatorIntervention"))
            {
                return;
            }

            if (Spells.W.IsReady() && InRange(hero) && MenuConfig.KsW)
            {
                if (hero.Health <= Spells.W.GetDamage(hero))
                {
                    Spells.W.Cast();
                }
            }

            if (Spells.R.IsReady() && Spells.R.Instance.Name == IsSecondR && MenuConfig.KsR2)
            {
                if (hero.Health < Dmg.RDmg(hero))
                {
                    var pred = Spells.R.GetPrediction(hero);

                    Spells.R.Cast(pred.CastPosition);
                }
            }

            if (hero.Health < Spells.Q.GetDamage(hero))
            {
                Spells.Q.Cast(hero);
            }

            if (!Spells.Ignite.IsReady() || !MenuConfig.Ignite)
            {
                return;
            }

            if (hero.IsValidTarget(600f) && Dmg.IgniteDamage(hero) >= hero.Health)
            {
                Player.Spellbook.CastSpell(Spells.Ignite, hero);
            }
        }

        #endregion
    }
}