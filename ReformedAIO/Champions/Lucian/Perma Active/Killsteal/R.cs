namespace ReformedAIO.Champions.Lucian.Perma_Active.Killsteal
{
    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Lucian.Core.Spells;

    using RethoughtLib.FeatureSystem.Implementations;

    internal sealed class R : OrbwalkingChild
    {
        public override string Name { get; set; } = nameof(R);

        private readonly RSpell rSpell;

        public R(RSpell rSpell)
        {
            this.rSpell = this.rSpell;
        }

        private Obj_AI_Hero Target => TargetSelector.GetTarget(rSpell.Spell.Range, TargetSelector.DamageType.Physical);

        private void OnUpdate(EventArgs args)
        {                                                                               
            if (this.Target == null 
                || this.Target.Health > rSpell.GetDamage(Target)
                || !CheckGuardians()
                || this.Target.Distance(ObjectManager.Player) > rSpell.Spell.Range
                || (Menu.Item("Safety").GetValue<bool>() && ObjectManager.Player.CountEnemiesInRange(rSpell.Spell.Range) > 1))
            {               // Soz for lazy 'safety' check xd cba
                return;
            }

            var wPred = rSpell.Spell.GetPrediction(this.Target);

            if (wPred.Hitchance > HitChance.Medium)
            {
                rSpell.Spell.Cast(Target);
            }
        }

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate -= this.OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate += this.OnUpdate;
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Menu.AddItem(new MenuItem("Safety", "Safety Check").SetValue(true));
        }
    }
}
