﻿namespace ReformedAIO.Champions.Diana.OrbwalkingMode.Combo
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Diana.Logic;

    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    internal class LunarRush : ChildBase
    {
        #region Fields

        private LogicAll logic;

        #endregion


        #region Public Properties

        public override string Name { get; set; } = "[W] Lunar Rush";

        #endregion

        #region Methods

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Events.OnUpdate -= OnUpdate;

        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Events.OnUpdate += OnUpdate;
            
        }

        protected override void OnInitialize(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            logic = new LogicAll();
        }

        protected sealed override void OnLoad(object sender, Base.FeatureBaseEventArgs featureBaseEventArgs)
        {
            Menu = new Menu(Name, Name);

            Menu.AddItem(new MenuItem(Name + "WKillable", "Use Only If Killable By Combo").SetValue(false));

            //   Menu.AddItem(new MenuItem(Name + "WMana", "Mana %")
            //       .SetValue(new Slider(15, 100)));

            Menu.AddItem(new MenuItem(Name + "Enabled", "Enabled").SetValue(true));

            
        }

        private void Lunarrush()
        {
            var target = TargetSelector.GetTarget(
                Variables.Player.AttackRange + Variables.Player.BoundingRadius,
                TargetSelector.DamageType.Magical);

            if (target == null || !target.IsValid) return;

            if (Menu.Item(Menu.Name + "WKillable").GetValue<bool>() && logic.ComboDmg(target) < target.Health)
            {
                return;
            }

            Variables.Spells[SpellSlot.W].Cast();
        }

        private void OnUpdate(EventArgs args)
        {
            if (Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                || !Variables.Spells[SpellSlot.W].IsReady()) return;

            Lunarrush();
        }

        #endregion
    }
}