namespace ReformedAIO.Champions.Caitlyn.Logic
{
    using LeagueSharp;
    using LeagueSharp.Common;

    internal sealed class EwqrLogic
    {
        public float EwqrDmg(Obj_AI_Hero target)
        {
            if (target == null) return 0;

            float dmg = 0;

            if (Spells.Spell[SpellSlot.Q].IsReady())
            {
                dmg += Spells.Spell[SpellSlot.Q].GetDamage(target);
            }

            if (Spells.Spell[SpellSlot.W].IsReady())
            {
                dmg += Spells.Spell[SpellSlot.Q].GetDamage(target);
            }

            if (Spells.Spell[SpellSlot.E].IsReady())
            {
                dmg += Spells.Spell[SpellSlot.Q].GetDamage(target);
            }

            if (Spells.Spell[SpellSlot.R].IsReady())
            {
                dmg += Spells.Spell[SpellSlot.Q].GetDamage(target);
            }

            if (!Vars.Player.IsWindingUp)
            {
                dmg += (float) Vars.Player.GetAutoAttackDamage(target, true);
            }

            return dmg;
        }

        public bool EwqrMana()
        {
            return Spells.Spell[SpellSlot.Q].ManaCost
                + Spells.Spell[SpellSlot.W].ManaCost
                + Spells.Spell[SpellSlot.E].ManaCost
                < Vars.Player.Mana;
        }

        public bool CanExecute(Obj_AI_Hero target)
        {
            return EwqrMana() && EwqrDmg(target) > target.Health && Spells.Spell[SpellSlot.Q].IsReady() && Spells.Spell[SpellSlot.W].IsReady() && Spells.Spell[SpellSlot.E].IsReady();
        }
    }
}
