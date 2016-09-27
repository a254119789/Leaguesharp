namespace NechritoRiven.Event.OrbwalkingModes
{
    #region

    using Core;

    using LeagueSharp.Common;

    using Menus;

    #endregion

    internal class LaneclearMode : Core
    {
        #region Public Methods and Operators

        public static void Laneclear()
        {
            var minions = MinionManager.GetMinions(Player.AttackRange + 380);

            if (minions == null || Player.IsWindingUp)
            {
                return;
            }

            if (minions.Count <= 1)
            {
                return;
            }

            foreach (var m in minions)
            {
                if (m.UnderTurret(true)) return;

                if (Spells.E.IsReady() && MenuConfig.LaneE)
                {
                    Spells.E.Cast(m);
                }

                if (!MenuConfig.laneQFast)
                {
                    return;
                }

                if (m.Health < Spells.Q.GetDamage(m) && Spells.Q.IsReady())
                {
                    CastQ(m);
                }
                else if (!Spells.W.IsReady()
                         || !MenuConfig.LaneW
                         || !InRange(m)
                         || Player.IsWindingUp
                         || m.Health > Spells.W.GetDamage(m))
                {
                    return;
                }

                Spells.W.Cast(m);
            }
        }

        #endregion
    }
}
