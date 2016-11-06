namespace ReformedAIO.Champions.Olaf.OrbwalkingMode.Mixed
{
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Core.Spells;

    using RethoughtLib.FeatureSystem.Implementations;

    internal sealed class QMixed : OrbwalkingChild
    {
        public override string Name { get; set; } = "Q";

        private readonly QSpell spell;

        public QMixed(QSpell spell)
        {
            this.spell = spell;
        }

        private Obj_AI_Hero Target => TargetSelector.GetTarget(spell.Spell.Range, TargetSelector.DamageType.Physical);

        private void OnUpdate(EventArgs args)
        {
            if (Target == null
                || !CheckGuardians()
                || (Menu.Item("Mana").GetValue<Slider>().Value > ObjectManager.Player.ManaPercent))
            {
                return;
            }

            var prediction = spell.Spell.GetPrediction(Target, true);

            switch (Menu.Item("Hitchance").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    if (prediction.Hitchance >= HitChance.Medium)
                    {
                        spell.Spell.Cast(prediction.CastPosition.Extend(ObjectManager.Player.Position, -Menu.Item("Distance").GetValue<Slider>().Value));
                    }
                    break;
                case 1:
                    if (prediction.Hitchance >= HitChance.High)
                    {
                        spell.Spell.Cast(prediction.CastPosition.Extend(ObjectManager.Player.Position, -Menu.Item("Distance").GetValue<Slider>().Value));
                    }
                    break;
                case 2:
                    if (prediction.Hitchance >= HitChance.VeryHigh)
                    {
                        spell.Spell.Cast(prediction.CastPosition.Extend(ObjectManager.Player.Position, -Menu.Item("Distance").GetValue<Slider>().Value));
                    }
                    break;
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

            Menu.AddItem(new MenuItem("Hitchance", "Hitchance").SetValue(new StringList(new[] { "Medium", "High", "Very High" }, 2)));

            Menu.AddItem(new MenuItem("Distance", "Shortened Throw Distance").SetValue(new Slider(20, 0, 100)));

            Menu.AddItem(new MenuItem("Mana", "Min Mana %").SetValue(new Slider(40, 0, 100)));
        }
    }
}
