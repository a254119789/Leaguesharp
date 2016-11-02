namespace NechritoRiven.Event.Interrupters_Etc
{
    #region

    using LeagueSharp;
    using LeagueSharp.Common;

    using NechritoRiven.Core;

    #endregion

    internal class ProcessSpell : Core
    {
        #region Public Methods and Operators

        public static void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsEnemy || !sender.IsValidTarget(1000))
            {
                return;
            }

            if (Spells.E.IsReady())
            {
                if (AntigapclosingSpells.Contains(args.SData.Name) || (TargetedSpells.Contains(args.SData.Name) && args.Target.IsMe))
                {
                    Utility.DelayAction.Add(90, ()=> Spells.E.Cast(Game.CursorPos));
                }
            }

            if (!InterrupterSpell.Contains(args.SData.Name) || !Spells.W.IsReady() || !InRange(sender))
            {
                return;
            }

            CastW(sender);
        }

        #endregion
    }
}