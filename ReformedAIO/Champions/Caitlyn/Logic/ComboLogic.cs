namespace ReformedAIO.Champions.Caitlyn.Logic
{
    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Caitlyn.Spells;

    internal sealed class ComboLogic
    {
        private readonly QSpell qSpell;

        private readonly WSpell wSpell;

        private readonly ESpell eSpell;

        private readonly RSpell rSpell;

        public ComboLogic(ESpell eSpell, WSpell wSpell, QSpell qSpell, RSpell rSpell)
        {
            this.eSpell = eSpell;
            this.wSpell = wSpell;
            this.qSpell = qSpell;
            this.rSpell = rSpell;
        }

        public float EwqrDmg(Obj_AI_Hero target)
        {
            if (target == null) return 0;

            float dmg = 0;

            dmg += qSpell.Spell.GetDamage(target);

            dmg += qSpell.Spell.GetDamage(target);

            dmg += qSpell.Spell.GetDamage(target);

            dmg += qSpell.Spell.GetDamage(target);

            if (!ObjectManager.Player.IsWindingUp)
            {
                dmg += (float) ObjectManager.Player.GetAutoAttackDamage(target);
            }

            return dmg;
        }

        public bool CanExecute(Obj_AI_Hero target)
        {
            return EwqrDmg(target) > target.Health
                && qSpell.Spell.IsReady()
                && wSpell.Spell.IsReady()
                && eSpell.Spell.IsReady();
        }
    }
}
