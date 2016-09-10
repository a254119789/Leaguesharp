﻿namespace ReformedAIO.Champions.Gragas.OrbwalkingMode.Jungle
{
    #region Using Directives

    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    internal class WJungle : ChildBase
    {
        #region Public Properties

        public override string Name { get; set; } = "[W] Drunken Rage";

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

        protected sealed override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Menu.AddItem(new MenuItem(Menu.Name + "WMana", "Mana %").SetValue(new Slider(10, 0, 100)));
        }

        private void DrunkenRage()
        {
            var mobs =
                MinionManager.GetMinions(350, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth)
                    .FirstOrDefault();

            if (mobs == null || !mobs.IsValid || Variable.Player.IsWindingUp) return;

            if (mobs.HealthPercent < 15) return;

            Variable.Spells[SpellSlot.W].Cast();
        }

        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear
                || !Variable.Spells[SpellSlot.W].IsReady()) return;
            if (Menu.Item(Menu.Name + "WMana").GetValue<Slider>().Value > Variable.Player.ManaPercent) return;

            DrunkenRage();
        }

        #endregion
    }
}