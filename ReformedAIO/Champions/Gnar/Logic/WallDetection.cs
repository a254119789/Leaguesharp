namespace ReformedAIO.Champions.Gnar.Logic
{
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Gnar.Core;

    using SharpDX;

    internal sealed class WallDetection
    {
        public Vector3 GetFirstWallPoint(Vector3 start, Vector3 end)
        {
            if (!start.IsValid() || !end.IsValid())
            {
                return Vector3.Zero;
            }

            var distance = start.Distance(end);

            if (distance > 590)
            {
                return Vector3.Zero;
            }

            var newPoint = start.Extend(end, 590);

            if (NavMesh.GetCollisionFlags(newPoint) == CollisionFlags.Wall || newPoint.IsWall())
            {
                return newPoint;
            }

            return Vector3.Zero;
        }

<<<<<<< HEAD
        public bool IsWallDash(Vector3 position, float Range)
=======
        public bool IsWall(Obj_AI_Hero t, Vector3 direction)
>>>>>>> parent of 686eda8... 6.19
        {
            var dashEndPos = ObjectManager.Player.Position.Extend(position, Range);

<<<<<<< HEAD
            var firstWallPoint = GetFirstWallPoint(ObjectManager.Player.Position, dashEndPos);

            if (firstWallPoint.Equals(Vector3.Zero))
            {
                // No Wall
                return false;
            }
=======
            var istrue = t.Position.Extend(direction, Spells.R2.Range);
            var firstwallpoint = this.GetFirstWallPoint(t.Position, istrue);
>>>>>>> parent of 686eda8... 6.19

            if (dashEndPos.IsWall())
            {
                Console.WriteLine("IsWall: TRUE");
                return true;
            }
          
            return false;
        }

        public Vector3 Wall(Vector3 position, float Range)
        {
            var dashEndPos = ObjectManager.Player.Position.Extend(position, Range);

           // IsWallDash(position, Range);

            if (dashEndPos.IsWall())
            {
                Console.WriteLine("IsWall: " + dashEndPos);
                return dashEndPos;
            }

            return Vector3.Zero;
        }
    }
}
