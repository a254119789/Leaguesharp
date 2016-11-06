namespace ReformedAIO.Champions.Olaf.OrbwalkingMode.Combo
{
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Core.Spells;

    using RethoughtLib.FeatureSystem.Implementations;

    internal sealed class WCombo : OrbwalkingChild
    {
        public override string Name { get; set; } = "W";

        private readonly WSpell spell;

        public WCombo(WSpell spell)
        {
            this.spell = spell;
        }

        private static Obj_AI_Hero Target => TargetSelector.GetTarget(ObjectManager.Player.AttackRange + 100, TargetSelector.DamageType.Physical);

        private void OnUpdate(EventArgs args)
        {
            if (Target == null
                || !CheckGuardians()
                || Menu.Item("Mana").GetValue<Slider>().Value > ObjectManager.Player.ManaPercent)
            {
                return;
            }

            spell.Spell.Cast();
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

            Menu.AddItem(new MenuItem("Mana", "Min Mana %").SetValue(new Slider(0, 0, 100)));
        }
    }
}
