namespace ReformedAIO.Library.Dash_Handler
{
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    internal class DashSmart
    {
        public Vector3 ToSafePosition(Obj_AI_Hero target, Vector3 targetPosition, double distance)
        {
            return IsDangerous(target, targetPosition) 
                ? Kite(target.Position.To2D(), distance).To3D()
                : Game.CursorPos;
        }

        public Vector2 Kite(Vector2 targetPos, double angle)
        {
            angle *= Math.PI / 200.0;
            var temp = Vector2.Subtract(targetPos, ObjectManager.Player.Position.To2D());
            var result = new Vector2(0)
            {
                X = (float)(temp.X * Math.Cos(angle) - temp.Y * Math.Sin(angle)) / 4,
                Y = (float)(temp.X * Math.Sin(angle) + temp.Y * Math.Cos(angle)) / 4
            };

            result = Vector2.Add(result, ObjectManager.Player.Position.To2D());
            return result;
        }

        private static bool IsDangerous(Obj_AI_Base target, Vector3 position)
        {
            return (ObjectManager.Player.CountEnemiesInRange(1000) > ObjectManager.Player.CountAlliesInRange(1000))
                   || (target.Position.UnderTurret(true) && target.HealthPercent >= 70)
                   || (target.IsMelee && target.Distance(ObjectManager.Player) <= (target.AttackRange + target.BoundingRadius) / 2);
        }
    }
}
