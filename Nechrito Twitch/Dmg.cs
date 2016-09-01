using System;
using LeagueSharp;
using LeagueSharp.Common;

namespace Nechrito_Twitch
{
    internal class Dmg
    {
        private static readonly Obj_AI_Hero Player = ObjectManager.Player;

        public float GetDamage(Obj_AI_Base target)
        {
            if (!Spells.E.IsReady() || target == null || target.IsDead || target.IsInvulnerable) return 0;

            float eDmg = 0;

            eDmg += Spells.E.GetDamage(target);

            return eDmg;
        }


    }
}
