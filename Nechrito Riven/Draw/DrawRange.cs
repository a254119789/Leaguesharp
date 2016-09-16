namespace NechritoRiven.Draw
{
    #region

    using System;
    using System.Drawing;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Core;
    using Menus;

    #endregion

    internal class DrawRange : Core
    {
        #region Public Methods and Operators

        public static void RangeDraw(EventArgs args)
        {
            if (Player.IsDead)
            {
                return;
            }

            if (MenuConfig.DrawCb)
            {
                if (Spells.E.IsReady())
                {
                    Render.Circle.DrawCircle(
                        Player.Position,
                        310 + Player.AttackRange,
                        Spells.Q.IsReady()
                        ? Color.LightBlue
                        : Color.DarkSlateGray);
                }
                else
                {
                    Render.Circle.DrawCircle(
                        Player.Position,
                        Player.AttackRange,
                        Spells.Q.IsReady() 
                        ? Color.LightBlue 
                        : Color.DarkSlateGray);
                }
            }

            if (MenuConfig.DrawBt && Spells.Flash != SpellSlot.Unknown)
            {
                Render.Circle.DrawCircle(
                    Player.Position,
                    750,
                    Spells.R.IsReady() 
                    && Spells.Flash.IsReady()
                    ? Color.LightBlue
                    : Color.DarkSlateGray);
            }

            if (MenuConfig.DrawFh)
            {
                Render.Circle.DrawCircle(
                    Player.Position,
                    450 + Player.AttackRange + 70,
                    Spells.E.IsReady() && Spells.Q.IsReady()
                    ? Color.LightBlue 
                    : Color.DarkSlateGray);
            }

            if (MenuConfig.DrawHs)
            {
                Render.Circle.DrawCircle(
                    Player.Position,
                    400,
                    Spells.Q.IsReady() && Spells.W.IsReady()
                    ? Color.LightBlue 
                    : Color.DarkSlateGray);
            }

            var pos = Drawing.WorldToScreen(Player.Position);

            if (MenuConfig.DrawAlwaysR)
            {
                Drawing.DrawText(pos.X - 20, pos.Y + 20, Color.Cyan, "Use R1  (     )");
                Drawing.DrawText(
                    pos.X + 43,
                    pos.Y + 20,
                    MenuConfig.AlwaysR ? Color.White : Color.Red,
                    MenuConfig.AlwaysR ? "On" : "Off");
            }

            if (!MenuConfig.ForceFlash)
            {
                return;
            }

            Drawing.DrawText(pos.X - 20, pos.Y + 40, Color.Cyan, "Use Flash  (     )");

            Drawing.DrawText(
                pos.X + 64,
                pos.Y + 40,
                MenuConfig.AlwaysF ? Color.White : Color.Red, 
                MenuConfig.AlwaysF ? "On" : "Off");
        }

        #endregion
    }
}