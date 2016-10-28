﻿namespace ReformedAIO.Champions.Ezreal.OrbwalkingMode.Harass
{
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Ezreal.Core.Spells;

    using RethoughtLib.FeatureSystem.Implementations;

    internal sealed class QHarass : OrbwalkingChild
    {
        public override string Name { get; set; } = "Q";

        private readonly QSpell qSpell;

        public QHarass(QSpell qSpell)
        {
            this.qSpell = qSpell;
        }

        private Obj_AI_Hero Target => TargetSelector.GetTarget(qSpell.Spell.Range, TargetSelector.DamageType.Physical);

        private void OnUpdate(EventArgs args)
        {
            if (Target == null
                || !CheckGuardians()
                || (Menu.Item("Mana").GetValue<Slider>().Value > ObjectManager.Player.ManaPercent))
            {
                return;
            }

            var prediction = qSpell.Spell.GetPrediction(Target);

            switch (Menu.Item("Hitchance").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    if (prediction.Hitchance >= HitChance.Medium)
                    {
                        qSpell.Spell.Cast(prediction.CastPosition);
                    }
                    break;
                case 1:
                    if (prediction.Hitchance >= HitChance.High)
                    {
                        qSpell.Spell.Cast(prediction.CastPosition);
                    }
                    break;
                case 2:
                    if (prediction.Hitchance >= HitChance.VeryHigh)
                    {
                        qSpell.Spell.Cast(prediction.CastPosition);
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

            Menu.AddItem(new MenuItem("Hitchance", "Hitchance").SetValue(new StringList(new[] { "Medium", "High", "Very High" }, 1)));

            Menu.AddItem(new MenuItem("Mana", "Min Mana %").SetValue(new Slider(0, 0, 100)));
        }
    }
}
