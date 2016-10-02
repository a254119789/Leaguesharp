﻿namespace ReformedAIO.Champions.Lucian.Logic.Damage
{
    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Lucian.Core.Spells;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    class LucDamage : ChildBase
    {
        private readonly QSpell qSpell;

        private readonly WSpell wSpell;

        private readonly ESpell eSpell;

        private readonly RSpell rSpell;

        public LucDamage(ESpell eSpell, WSpell wSpell, QSpell qSpell, RSpell rSpell)
        {
            this.eSpell = eSpell;
            this.wSpell = wSpell;
            this.qSpell = qSpell;
            this.rSpell = rSpell;
        }

        public float GetComboDamage(Obj_AI_Hero target)
        {
            if (target == null) return 0;

            float comboDmg = 0;

            var aaDmg = (float)ObjectManager.Player.GetAutoAttackDamage(target, true);

            comboDmg += qSpell.GetDamage(target) + aaDmg;

            comboDmg += wSpell.Spell.GetDamage(target) + aaDmg;

            comboDmg += aaDmg;

            if (target.Distance(ObjectManager.Player) >= 900 && rSpell.Spell.IsReady())
            {
                comboDmg += rSpell.GetDamage(target);
            }

            return comboDmg;
        }

        public override string Name { get; set; } = "Damage";
    }
}
