namespace NechritoRiven.Event
{
    #region

    using LeagueSharp;
    using LeagueSharp.Common;

    using Core;
    using Menus;

    #endregion

    internal class Interrupt2 : Core
    {
        #region Public Methods and Operators

        public static void OnInterruptableTarget(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (!MenuConfig.InterruptMenu || sender.IsInvulnerable) return;

            if (sender.IsValidTarget(Spells.W.Range))
            {
                if (Spells.W.IsReady())
                {
                    Spells.W.Cast(sender);
                }
            }
        }

        #endregion
    }
}