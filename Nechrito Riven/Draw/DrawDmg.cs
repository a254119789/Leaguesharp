using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using NechritoRiven.Core;
using NechritoRiven.Menus;
using SharpDX;

namespace NechritoRiven.Draw
{
    internal class DrawDmg
    {
        private static readonly HpBarIndicator Indicator = new HpBarIndicator();
        public static void DmgDraw(EventArgs args)
        {
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(ene => ene.IsValidTarget(1500)))
            {
                if (!MenuConfig.Dind) continue;

                Indicator.Unit = enemy;

                Indicator.DrawDmg(Dmg.GetComboDamage(enemy), enemy.Health <= Dmg.GetComboDamage(enemy)*.65 ? Color.LawnGreen : Color.Yellow);
            }
        }
    }
}
