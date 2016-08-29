using Dark_Star_Thresh.Core;
using LeagueSharp;
using LeagueSharp.Common;
using System;

namespace Dark_Star_Thresh.Update
{
    internal class Misc : Core.Core
    {
        public static void Skinchanger(EventArgs args)
        {
            Player.SetSkin(Player.CharData.BaseSkinName, MenuConfig.UseSkin ? MenuConfig.Config.Item("Skin").GetValue<StringList>().SelectedIndex : Player.BaseSkinId);
        }

        public static void OnInterruptableTarget(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (!MenuConfig.Interrupt
                || sender.IsInvulnerable
                || !sender.IsValidTarget(Spells.E.Range)
                || !Spells.E.IsReady())
            {
                return;
            }

                Spells.E.Cast(sender);
        }

        public static void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (!MenuConfig.Gapcloser) return;

            var sender = gapcloser.Sender;

            if (!sender.IsEnemy || !Spells.E.IsReady() || !sender.IsValidTarget(Spells.E.Range)) return;

            Spells.E.Cast(sender);
        }
    }
}
