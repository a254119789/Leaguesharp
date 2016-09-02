#region

using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace NechritoRiven.Event
{
    using Core;

    internal class OnCasted 
    {
        public static void OnCasting(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsEnemy || sender.Type != Core.Player.Type) return;

            var epos = Core.Player.ServerPosition + (Core.Player.ServerPosition - sender.ServerPosition).Normalized() * 300;

            if (!(Core.Player.Distance(sender.ServerPosition) <= args.SData.CastRange)) return;

            if (Spells.E.IsReady())
            {
                if (Core.EAntiSpell.Contains(args.SData.Name) || (Core.TargetedAntiSpell.Contains(args.SData.Name) && args.Target.IsMe))

                    Spells.E.Cast(epos);
            }

            if (!Core.WAntiSpell.Contains(args.SData.Name) || !Spells.W.IsReady()) return;

            Spells.W.Cast();
        }
    }
}
