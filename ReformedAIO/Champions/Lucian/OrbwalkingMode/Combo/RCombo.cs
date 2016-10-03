﻿namespace ReformedAIO.Champions.Lucian.OrbwalkingMode.Combo
{
    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Lucian.Core.Spells;
    using ReformedAIO.Champions.Lucian.Logic.Damage;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    class RCombo : ChildBase
    {
        public override string Name { get; set; } = "R";

        private readonly LucDamage damage;

        private readonly RSpell rSpell;

        private readonly Orbwalking.Orbwalker orbwalker;

        public RCombo(RSpell rSpell, Orbwalking.Orbwalker orbwalker, LucDamage damage)
        {
            this.rSpell = rSpell;
            this.orbwalker = orbwalker;
            this.damage = damage;
        }

        private void OnUpdate(EventArgs args)
        {
            var target = TargetSelector.GetTarget(Menu.Item("Range").GetValue<Slider>().Value, TargetSelector.DamageType.Physical);

            if (target == null
                || !target.IsValidTarget(rSpell.Spell.Range)
                || target.Distance(ObjectManager.Player) < ObjectManager.Player.AttackRange
                || (Menu.Item("RKillable").GetValue<bool>() && damage.GetComboDamage(target) < target.Health)
                || (Menu.Item("SafetyCheck").GetValue<bool>() && ObjectManager.Player.CountEnemiesInRange(1400) > ObjectManager.Player.CountAlliesInRange(1400)))
            {
                return;
            }

            var pred = rSpell.Spell.GetPrediction(target);

            if (pred.Hitchance >= HitChance.High)
            {
                rSpell.Spell.Cast(pred.CastPosition);
            }
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            Menu.AddItem(new MenuItem("RKillable", "Only If Killable").SetValue(true));
            Menu.AddItem(new MenuItem("SafetyCheck", "Safety Check").SetValue(true));
            Menu.AddItem(new MenuItem("Range", "R Range").SetValue(new Slider(1400, 150, 1400)));
            Menu.AddItem(new MenuItem("RMana", "Min Mana %").SetValue(new Slider(25, 0, 100)));

        }

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate -= OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate += OnUpdate;
        }
    }
}
