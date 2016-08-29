using System;
using System.Collections.Generic;
using System.Linq;
using System;
using LeagueSharp;
using LeagueSharp.Common;
using Dark_Star_Thresh.Core;
using Dark_Star_Thresh.Update;

namespace Dark_Star_Thresh.Drawings
{
    internal class DrawRange : Core.Core
    {
        public static void OnDraw(EventArgs args)
        {
            if (Player.IsDead) return;

            var pos = Drawing.WorldToScreen(Player.Position);

            if (MenuConfig.DrawQ) { Render.Circle.DrawCircle(Player.Position, Spells.Q.Range,Spells.Q.IsReady() ? System.Drawing.Color.FromArgb(120, 0, 170, 255) : System.Drawing.Color.IndianRed); }

            if (MenuConfig.DrawW) { Render.Circle.DrawCircle(Player.Position, Spells.W.Range, Spells.W.IsReady() ? System.Drawing.Color.FromArgb(120, 0, 170, 255) : System.Drawing.Color.IndianRed); }

            if (MenuConfig.DrawE) { Render.Circle.DrawCircle(Player.Position, Spells.E.Range, Spells.E.IsReady() ? System.Drawing.Color.FromArgb(120, 0, 170, 255) : System.Drawing.Color.IndianRed); }

            if (MenuConfig.DrawR) { Render.Circle.DrawCircle(Player.Position, Spells.R.Range, Spells.R.IsReady() ? System.Drawing.Color.FromArgb(120, 0, 170, 255) : System.Drawing.Color.IndianRed); }

            if(MenuConfig.DrawPred)
            {
                var target = TargetSelector.GetSelectedTarget();

                if (target != null && !target.IsDead && target.IsValidTarget(Spells.Q.Range))
                {
                    Render.Circle.DrawCircle(Mode.QPred(target), 50, System.Drawing.Color.GhostWhite);
                }
            }
        }
    }
}
