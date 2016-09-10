﻿namespace ReformedAIO.Champions.Ashe.OrbwalkingMode.Combo
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    internal sealed class WCombo : ChildBase
    {
        #region Constructors and Destructors

        private readonly Orbwalking.Orbwalker Orbwalker;

        public WCombo(string name, Orbwalking.Orbwalker orbwalker)
        {
            Name = name;
            Orbwalker = orbwalker;
        }

        #endregion

        #region Public Properties

        public override string Name { get; set; }

        #endregion

        #region Methods

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate -= OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate += OnUpdate;
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            Menu.AddItem(new MenuItem(Menu.Name + "WDistance", "Max Distance").SetValue(new Slider(1100, 0, 1200)));

            Menu.AddItem(new MenuItem(Menu.Name + "WMana", "Mana %").SetValue(new Slider(10, 0, 100)));
        }

        private void OnUpdate(EventArgs args)
        {
            if (Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                || !Variable.Spells[SpellSlot.W].IsReady()
                || Variable.Player.IsWindingUp)
            {
                return;
            }

            Volley();
        }

        private void Volley()
        {
            var target = TargetSelector.GetTarget(
                Menu.Item(Menu.Name + "WDistance").GetValue<Slider>().Value,
                TargetSelector.DamageType.Physical);

            if (target == null || !target.IsValid) return;

            Variable.Spells[SpellSlot.W].CastIfHitchanceEquals(target, HitChance.VeryHigh);
        }

        #endregion
    }
}