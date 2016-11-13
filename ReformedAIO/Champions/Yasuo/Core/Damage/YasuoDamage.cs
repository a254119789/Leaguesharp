namespace ReformedAIO.Champions.Yasuo.Core.Damage
{
    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Yasuo.Core.Spells;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    internal sealed class YasuoDamage : ChildBase
    {
        public override string Name { get; set; } = "Damage";

        private readonly Q1Spell qSpell;

        private readonly ESpell eSpell;

        private readonly RSpell rSpell;

        public YasuoDamage(Q1Spell qSpell, ESpell eSpell, RSpell rSpell)
        {
            this.qSpell = qSpell;
            this.eSpell = eSpell;
            this.rSpell = rSpell;
        }

        public float GetComboDamage(Obj_AI_Hero target)
        {
            if (target == null) return 0;

            float comboDmg = 0;

            if (qSpell.Spell.IsReady())
            {
                comboDmg += qSpell.GetDamage(target);
            }

            if (eSpell.Spell.IsReady())
            {
                comboDmg += eSpell.Spell.GetDamage(target);
            }

            if (rSpell.Spell.IsReady())
            {
                comboDmg += rSpell.Spell.GetDamage(target);
            }

            if (!ObjectManager.Player.IsWindingUp)
            {
                comboDmg += (float)ObjectManager.Player.GetAutoAttackDamage(target);
            }

            return comboDmg;
        }
    }
}
