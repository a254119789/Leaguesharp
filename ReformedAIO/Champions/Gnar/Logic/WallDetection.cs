﻿using LeagueSharp;
using LeagueSharp.Common;

using SharpDX;
using System.Linq;
using ReformedAIO.Champions.Gnar.Core;

namespace ReformedAIO.Champions.Gnar.Logic
{
    class WallDetection
    {
        public Vector3 GetFirstWallPoint(Vector3 start, Vector3 end, int step = 1)
        {
            if (start.IsValid() && end.IsValid())
            {
                var distance = start.Distance(end);
                for (var i = 0; i < distance; i = i + step)
                {
                    var newPoint = start.Extend(end, i);

                    if (NavMesh.GetCollisionFlags(newPoint) == CollisionFlags.Wall || newPoint.IsWall())
                    {
                        return newPoint;
                    }
                }
            }
            return Vector3.Zero;
        }



        public bool IsWall(Obj_AI_Hero t)
        {
            var x = false;

            var istrue = Vars.Player.Position.Extend(t.Position, Spells.R2.Range);
            var firstwallpoint = GetFirstWallPoint(Vars.Player.Position, istrue);

            if (firstwallpoint == Vector3.Zero)
            {
                x = false;
            }

            if (istrue.IsWall())
            {
                x = true;
            }

            return x;
        }
    }
}
