namespace ReformedAIO.Core.Extensions.TargetInfo
{
    using System.Linq;

    using LeagueSharp;

    internal class TargetInfo
    {
        public bool Unkillable(Obj_AI_Hero target)
        {
            if (target.Buffs.Any(b => b.IsValid 
            && (b.DisplayName == "UndyingRage" 
            || b.DisplayName == "ChronoShift"
            || b.DisplayName == "JudicatorIntervention" 
            || b.DisplayName == "kindredrnodeathbuff")))
            {
                return true;
            }

            return target.HasBuffOfType(BuffType.Invulnerability) || target.IsInvulnerable;
        }

        public bool HasSpellShield(Obj_AI_Hero target)
        {
            if (target.Buffs.Any(b => b.IsValid 
            && (b.DisplayName == "bansheesveil" 
            || b.DisplayName == "SivirE"
            || b.DisplayName == "NocturneW")))
            {
                return true;
            }
         
            return target.HasBuffOfType(BuffType.SpellShield) || target.HasBuffOfType(BuffType.SpellImmunity);
        }

        public bool Immobilized(Obj_AI_Hero target)
        {
            return target.HasBuffOfType(BuffType.Stun)
                   || target.HasBuffOfType(BuffType.Snare)
                   || target.HasBuffOfType(BuffType.Knockup)
                   || target.HasBuffOfType(BuffType.Charm)
                   || target.HasBuffOfType(BuffType.Fear)
                   || target.HasBuffOfType(BuffType.Knockback)
                   || target.HasBuffOfType(BuffType.Taunt)
                   || target.HasBuffOfType(BuffType.Suppression)
                   || target.IsStunned;
        }
    }
}
