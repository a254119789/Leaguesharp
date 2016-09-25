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

            if (minions == null)
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

                if (!Spells.W.IsReady()
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
