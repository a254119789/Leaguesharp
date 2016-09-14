namespace NechritoRiven.Event
{
    #region

    using Core;

    using LeagueSharp.Common;

    #endregion

    internal class Gapclose : Core
    {
        #region Public Methods and Operators

        public static void Gapcloser(ActiveGapcloser gapcloser)
        {
            var t = gapcloser.Sender;

            if (!t.IsEnemy || !Spells.W.IsReady())
            {
                return;
            }

            if (t.IsValidTarget(Spells.W.Range + t.BoundingRadius))
            {
                Spells.W.Cast(t);
            }
        }

        #endregion
    }
}