namespace NechritoRiven.Draw
{
    #region

    using System;
    using System.Drawing;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Core;
    using Event;
    using NechritoRiven.Menus;

    #endregion

    internal class DrawWallSpot : Core
    {
        #region Public Methods and Operators

        public static void WallDraw(EventArgs args)
        {
            var end = Player.ServerPosition.Extend(Game.CursorPos, Spells.Q.Range);
            var isWallDash = FleeLogic.IsWallDash(end, Spells.Q.Range);

            var wallPoint = FleeLogic.GetFirstWallPoint(Player.ServerPosition, end);

            if (isWallDash && MenuConfig.FleeSpot)
            {
                if (wallPoint.Distance(Player.ServerPosition) <= 600)
                {
                    Render.Circle.DrawCircle(wallPoint, 60, Color.White);
                    Render.Circle.DrawCircle(end, 60, Color.Green);
                }
            }
        }

        #endregion
    }
}