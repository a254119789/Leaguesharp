namespace NechritoRiven.Event.OrbwalkingModes
{
    #region

    using Core;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Orbwalking = NechritoRiven.Orbwalking;

    #endregion

    internal class FastHarassMode : Core
    {
        #region Public Methods and Operators

        public static void FastHarass()
        {
            var target = TargetSelector.GetTarget(400, TargetSelector.DamageType.Physical);

            if (target == null || !Spells.Q.IsReady() || !Spells.E.IsReady())
            {
                return;
            }

            if (Spells.Q.IsReady() && Spells.W.IsReady() && Spells.E.IsReady() && Qstack == 1)
            {
                BackgroundData.CastQ(target);
            }

            if (Qstack == 3 && !Orbwalking.CanAttack() && Orbwalking.CanMove(5))
            {
                Spells.E.Cast(Game.CursorPos);

                Utility.DelayAction.Add(170, () => Spells.W.Cast());

                Utility.DelayAction.Add(190, () => Spells.Q.Cast(Game.CursorPos));
            }
        }

        #endregion
    }
}
