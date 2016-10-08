﻿namespace NechritoRiven.Event.OrbwalkingModes
{
    #region

    using Core;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Orbwalking = Orbwalking;

    #endregion

    internal class Mixed : Core
    {
        #region Public Methods and Operators

        public static void Harass()
        {
            var target = TargetSelector.GetTarget(400, TargetSelector.DamageType.Physical);

            if (target == null)
            {
                return;
            }

            if (Spells.Q.IsReady() && Spells.W.IsReady() && Qstack == 1)
            {
               CastQ(target);
            }

            if (Spells.W.IsReady())
            {
                CastW(target);
            }

            if (!Spells.Q.IsReady()
                || !Spells.E.IsReady()
                || Qstack != 3
                || Orbwalking.CanAttack()
                || !Orbwalking.CanMove(5))
            {
                return;
            }

            Spells.E.Cast(Game.CursorPos);
            
            Utility.DelayAction.Add(190, () => Spells.Q.Cast(target.Position));
        }

        #endregion
    }
}
