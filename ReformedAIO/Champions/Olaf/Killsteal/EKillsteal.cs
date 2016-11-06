namespace ReformedAIO.Champions.Olaf.Killsteal
{
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Core.Spells;

    using RethoughtLib.FeatureSystem.Implementations;

    internal sealed class EKillsteal : OrbwalkingChild
    {
        public override string Name { get; set; } = "E";

        private readonly ESpell spell;

        public EKillsteal(ESpell spell)
        {
            this.spell = spell;
        }

        private Obj_AI_Hero Target => TargetSelector.GetTarget(spell.Spell.Range, TargetSelector.DamageType.Physical);

        private void OnUpdate(EventArgs args)
        {
            if (Target == null || Target.Health > spell.GetDamage(Target) || !CheckGuardians())
            {
                return;
            }

            spell.Spell.Cast(Target);
        }

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnDisable(sender, featureBaseEventArgs);

            Game.OnUpdate -= OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnEnable(sender, featureBaseEventArgs);

            Game.OnUpdate += OnUpdate;
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);
        }
    }
}
