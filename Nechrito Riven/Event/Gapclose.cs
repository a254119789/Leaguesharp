namespace NechritoRiven.Event
{
    #region

    using LeagueSharp.Common;

    using Core;

    #endregion

    internal class Gapclose : Core
    {
        #region Public Methods and Operators

        public static void Gapcloser(ActiveGapcloser gapcloser)
        {
            var t = gapcloser.Sender;

            if (!t.IsEnemy)
            {
                return;
            }

            if (t.IsValidTarget(Spells.W.Range + t.BoundingRadius) && Spells.W.IsReady())
            {
                Spells.W.Cast(t);
            }

            if (t.IsValidTarget(Spells.Q.Range + t.BoundingRadius) && Spells.Q.IsReady() && Qstack == 3)
            {
                Spells.Q.Cast(gapcloser.End);
            }
        }

        #endregion
    }
}