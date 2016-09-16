namespace NechritoRiven.Event.OrbwalkingModes
{
    #region

    using LeagueSharp.Common;

    using Core;
    using Menus;

    #endregion

    internal class JungleClearMode : Core
    {
        #region Public Methods and Operators

        public static void Jungleclear()
        {
            var mobs = MinionManager.GetMinions(
                Player.Position,
                Player.AttackRange + Spells.E.Range + 50,
                MinionTypes.All,
                MinionTeam.Neutral);

            if (mobs == null) return;

            foreach (var m in mobs)
            {
                if (!m.IsValid || !Spells.E.IsReady() || !MenuConfig.JnglE || Player.IsWindingUp) return;

                Spells.E.Cast(m.Position);
                Utility.DelayAction.Add(10, ForceItem);
            }
        }

        #endregion
    }
}
