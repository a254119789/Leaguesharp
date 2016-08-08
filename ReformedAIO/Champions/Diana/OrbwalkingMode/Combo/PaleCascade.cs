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

    internal class PaleCascade : ChildBase
    {
        #region Fields

        private LogicAll logic;

        private PaleCascadeLogic rLogic;

        #endregion


        #region Public Properties

        public override string Name { get; set; } = "[R] Pale Cascade";

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

        protected override void OnInitialize(object sender, Base.FeatureBaseEventArgs featureBaseEventArgs)
        {
            rLogic = new PaleCascadeLogic();
            logic = new LogicAll();
            base.OnInitialize(sender, featureBaseEventArgs);
        }

        protected sealed override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Menu = new Menu(Name, Name);

            Menu.AddItem(
                new MenuItem(Name + "REnemies", "Don't R Into >= x Enemies").SetValue(new Slider(2, 0, 5)));

            Menu.AddItem(new MenuItem(Name + "RTurret", "Don't R Into Turret").SetValue(true));

            Menu.AddItem(new MenuItem(Name + "RKillable", "Only If Killable").SetValue(true));

            Menu.AddItem(new MenuItem(Name + "Enabled", "Enabled").SetValue(true));
            
        }

        private void OnUpdate(EventArgs args)
        {
            if (Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                || !Variables.Spells[SpellSlot.R].IsReady()) return;

            paleCascade();
        }

        private void paleCascade()
        {
            var target = TargetSelector.GetTarget(825, TargetSelector.DamageType.Magical);

            if (target == null || !target.IsValid) return;

            if (target.CountEnemiesInRange(1400) >= Menu.Item(Menu.Name + "REnemies").GetValue<Slider>().Value) return;

            if (Menu.Item(Menu.Name + "RTurret").GetValue<bool>() && target.UnderTurret()) return;

            if (Menu.Item(Menu.Name + "RKillable").GetValue<bool>() && logic.ComboDmg(target) < target.Health) return;

            if (rLogic.Buff(target))
            {
                Variables.Spells[SpellSlot.R].Cast(target);
            }
        }

        #endregion
    }
}