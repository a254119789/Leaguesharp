namespace Dark_Star_Thresh.Drawings
{
    using System;
    using System.Drawing;

    using Dark_Star_Thresh.Core;
    using Dark_Star_Thresh.Update;

    using LeagueSharp;
    using LeagueSharp.Common;

    internal class DrawRange : Core
    {
        public static void OnDraw(EventArgs args)
        {
            if (Player.IsDead) return;

            var pos = Drawing.WorldToScreen(Player.Position);

            if (MenuConfig.DrawQ)
            {
                Render.Circle.DrawCircle(
                    Player.Position,
                    Spells.Q.Range,
                    Spells.Q.IsReady() ? Color.White : Color.DarkSlateGray);
            }

            if (MenuConfig.DrawW)
            {
                Render.Circle.DrawCircle(
                    Player.Position,
                    Spells.W.Range,
                    Spells.W.IsReady() ? Color.White : Color.DarkSlateGray);
            }

            if (MenuConfig.DrawE)
            {
                Render.Circle.DrawCircle(
                    Player.Position,
                    Spells.E.Range,
                    Spells.E.IsReady() ? Color.White : Color.DarkSlateGray);
            }

            if (MenuConfig.DrawR)
            {
                Render.Circle.DrawCircle(
                    Player.Position,
                    Spells.R.Range,
                    Spells.R.IsReady() ? Color.White : Color.DarkSlateGray);
            }

            if (MenuConfig.DrawPred)
            {
                var target = TargetSelector.GetSelectedTarget();
                var pred = Spells.Q.GetPrediction(target);

                if (target != null && !target.IsDead && target.IsValidTarget(Spells.Q.Range))
                {
                    Render.Circle.DrawCircle(pred.UnitPosition, 50, Color.GhostWhite);
                }
            }
        }
    }
}
