using System;
using SharpDX;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using Dark_Star_Thresh.Core;

namespace Dark_Star_Thresh.Drawings
{
    internal class DrawDmg : Core.Core
    {
        private static readonly HpBarIndicator Indicator = new HpBarIndicator();
        private static readonly Dmg Dmg = new Dmg();

        public static void OnEndScene(EventArgs args)
        {
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(ene => ene.IsValidTarget(1350)))
            {
                if (!MenuConfig.DrawDmg) continue;

                Indicator.Unit = enemy;
                Indicator.DrawDmg(Dmg.Damage(enemy), Color.LawnGreen);
            }
        }
    }
}
