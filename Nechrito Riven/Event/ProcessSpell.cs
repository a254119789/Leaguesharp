namespace NechritoRiven.Event
{
    #region

    using Core;

    using LeagueSharp;
    using LeagueSharp.Common;

    #endregion

    internal class ProcessSpell : Core
    {
        #region Public Methods and Operators

        public static void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsEnemy || Player.Distance(sender.ServerPosition) > args.SData.CastRange) return;

            if (Spells.E.IsReady())
            {
                if (EAntiSpell.Contains(args.SData.Name) || (TargetedAntiSpell.Contains(args.SData.Name) && args.Target.IsMe))
                {
                    Spells.E.Cast(Game.CursorPos);
                }
            }

            if (!WAntiSpell.Contains(args.SData.Name) || !Spells.W.IsReady() || !InWRange(sender))
            {
                return;
            }

            Spells.W.Cast();
        }

        #endregion
    }
}