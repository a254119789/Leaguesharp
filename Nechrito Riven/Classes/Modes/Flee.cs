﻿using System;
using LeagueSharp.Common;
using LeagueSharp;
using System.Linq;
using SharpDX;

namespace NechritoRiven
{
    class Flee : Program
    {
        private static void Game_OnUpdate(EventArgs args)
        {
            FleeLogic();
        }
        public static void FleeLogic()
        {
            if(!MenuConfig.WallFlee)
            {
                return;
            }
            var IsWallDash = FleeLOGIC.IsWallDash(Player.ServerPosition, 320);
            var end = Player.ServerPosition.Extend(Game.CursorPos, 320);
            var Eend = Player.ServerPosition.Extend(Game.CursorPos, 450);
            var WallE = FleeLOGIC.GetFirstWallPoint(Player.ServerPosition, Eend);
            var WallPoint = FleeLOGIC.GetFirstWallPoint(Player.ServerPosition, end);

            if (Spells._q.IsReady() && _qstack < 3)
            { Spells._q.Cast(Game.CursorPos); }

            if (IsWallDash)
            {
                ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, WallPoint);
            }
            if (IsWallDash && _qstack == 3 && WallPoint.Distance(Player.ServerPosition) <= 800)
            {
                if (WallPoint.Distance(Player.ServerPosition) <= 500)
                {
                    ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, WallPoint);
                    if (Spells._q.IsReady() && WallPoint.Distance(Player.ServerPosition) < 50)
                    {
                        if (Spells._e.IsReady())
                        {
                            Spells._e.Cast(WallE);
                        }
                        if(_qstack == 3)
                        {
                            Spells._q.Cast(WallPoint);
                        }
                      
                    }
                }
            }
            
            if (!IsWallDash && !MenuConfig.WallFlee)
            {
                var enemy =
             HeroManager.Enemies.Where(
                 hero =>
                     hero.IsValidTarget(Program.Player.HasBuff("RivenFengShuiEngine")
                         ? 70 + 195 + Program.Player.BoundingRadius
                         : 70 + 120 + Program.Player.BoundingRadius) && Spells._w.IsReady());
                var x = Program.Player.Position.Extend(Game.CursorPos, 300);
                var objAiHeroes = enemy as Obj_AI_Hero[] ?? enemy.ToArray();
                if (Spells._q.IsReady() && !Program.Player.IsDashing()) Spells._q.Cast(Game.CursorPos);
                if (Spells._w.IsReady() && objAiHeroes.Any()) foreach (var target in objAiHeroes) if (Logic.InWRange(target)) Spells._w.Cast();
                if (Spells._e.IsReady() && !Player.IsDashing()) Spells._e.Cast(x);
            }
        }
    }
}