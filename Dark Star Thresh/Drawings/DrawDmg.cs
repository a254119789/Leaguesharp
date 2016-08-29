using SharpDX;
using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using Dark_Star_Thresh.Core;

namespace Dark_Star_Thresh.Drawings
{
    internal class DrawDmg : Core.Core
    {
        private static readonly HpBarIndicator Indicator = new HpBarIndicator();

        public static void OnEndScene(EventArgs args)
        {
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(ene => ene.IsValidTarget(Spells.Q.Range) && !ene.IsZombie))
            {
                if (!MenuConfig.DrawDmg) continue;

                Indicator.unit = enemy;
                Indicator.drawDmg(Dmg.Damage(enemy), new ColorBGRA(255, 204, 0, 170));
            }
        }
    }
}
