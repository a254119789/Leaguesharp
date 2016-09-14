using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.SDK;
using Reforged_Riven.Extras;
using Reforged_Riven.Menu;
using SharpDX;

namespace Reforged_Riven.Draw
{
    internal class DrawDmg : Core
    {
        private static readonly HpBarIndicator Indicator = new HpBarIndicator();

        public static void DmgDraw(EventArgs args)
        {
            if (!MenuConfig.Dind) return;

            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(ene => ene.IsValidTarget(1500) && !ene.IsDead && ene.IsVisible))
            {
                Indicator.Unit = enemy;
                Indicator.DrawDmg(Dmg.GetComboDamage(enemy), Color.LawnGreen);
            }
        }
    }
}
