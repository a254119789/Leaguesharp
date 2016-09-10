using LeagueSharp;
using LeagueSharp.Common;

namespace ReformedAIO.Champions.Gnar.Core
{
    internal class Dmg
    {
        public float GetDamage(Obj_AI_Base x)
        {
            if (x == null
                || x.IsInvulnerable
                || x.HasBuffOfType(BuffType.SpellShield)
                || x.HasBuffOfType(BuffType.SpellImmunity))
            {
                return 0;
            }

            float dmg = 0;

            if (!Vars.Player.IsWindingUp)
            {
                dmg += (float)Vars.Player.GetAutoAttackDamage(x);
            }

            if (Spells.Q.IsReady())
            {
                dmg += Spells.Q.GetDamage(x);
            }
            if (Spells.W.IsReady())
            {
                dmg += Spells.W.GetDamage(x);
            }
            if (Spells.E.IsReady())
            {
                dmg += Spells.E.GetDamage(x);
            }
            if (Spells.R.IsReady())
            {
                dmg += Spells.R.GetDamage(x);
            }

            return dmg;
        }
    }
}
