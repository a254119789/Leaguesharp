namespace NechritoRiven.Event
{
    #region

    using LeagueSharp.Common;

    using NechritoRiven.Core;

    #endregion

    internal class Gapclose : Core
    {
        #region Public Methods and Operators

        public static void Gapcloser(ActiveGapcloser gapcloser)
        {
            var t = gapcloser.Sender;
            if (t.IsEnemy && Spells.W.IsReady() && t.IsValidTarget() && !t.IsZombie)
            {
                if (t.IsValidTarget(Spells.W.Range + t.BoundingRadius))
                {
                    Spells.W.Cast(t);
                }
            }
        }

        #endregion
    }
}