namespace ReformedAIO.Champions.Olaf.OrbwalkingMode.Combo
{
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Core.Spells;

    using ReformedAIO.Champions.Olaf.Core.Damage;
    using ReformedAIO.Library.Get_Information.HeroInfo;

    using RethoughtLib.FeatureSystem.Implementations;

    internal sealed class RCombo : OrbwalkingChild
    {
        public override string Name { get; set; } = "R";

        private HeroInfo heroInfo;

        private readonly OlafDamage damage;

        private readonly RSpell spell;

        public RCombo(RSpell spell, OlafDamage damage)
        {
            this.spell = spell;
            this.damage = damage;
        }

        private static Obj_AI_Hero Target => TargetSelector.GetTarget(800, TargetSelector.DamageType.Physical);

        private void OnUpdate(EventArgs args)
        {
            if (Target == null
                || !CheckGuardians()
                || (Menu.Item("Mana").GetValue<Slider>().Value > ObjectManager.Player.ManaPercent))
            {
                return;
            }

            if ((heroInfo.Immobilized(ObjectManager.Player) && Menu.Item("Immobilized").GetValue<bool>())
                || damage.GetComboDamage(Target) * 1.3 >= Target.Health && Menu.Item("Killable").GetValue<bool>())
            {
                spell.Spell.Cast();
            }
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

            Menu.AddItem(new MenuItem("Killable", "Use When Killable").SetValue(true));

            Menu.AddItem(new MenuItem("Immobilized", "Use When Immobilized").SetValue(true));

            Menu.AddItem(new MenuItem("Mana", "Min Mana %").SetValue(new Slider(0, 0, 100)));

            heroInfo = new HeroInfo();
        }
    }
}
