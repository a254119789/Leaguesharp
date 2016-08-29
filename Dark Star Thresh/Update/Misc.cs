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
            if (!MenuConfig.UseSkin)
            {
                Player.SetSkin(Player.CharData.BaseSkinName, Player.BaseSkinId);
                return;
            }
            Player.SetSkin(Player.CharData.BaseSkinName, MenuConfig.Config.Item("Skin").GetValue<StringList>().SelectedIndex);
        }

        public static void OnInterruptableTarget(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (!MenuConfig.Interrupt || sender.IsInvulnerable) return;

            if (sender.IsValidTarget(Spells.E.Range))
            {
                if (Spells.E.IsReady())
                {
                    Spells.E.Cast(sender);
                }
            }
        }

        public static void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (!MenuConfig.Gapcloser) return;

            var sender = gapcloser.Sender;
            if (sender.IsEnemy && Spells.E.IsReady() && sender.IsValidTarget())
            {
                if (sender.IsValidTarget(Spells.E.Range))
                {
                    Spells.E.Cast(sender);
                }
            }
        }
    }
}
