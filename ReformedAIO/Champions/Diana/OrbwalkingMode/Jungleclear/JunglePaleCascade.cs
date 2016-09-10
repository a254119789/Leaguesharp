﻿namespace ReformedAIO.Champions.Diana.OrbwalkingMode.Jungleclear
{
    #region Using Directives

    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    internal class JunglePaleCascade : ChildBase
    {
        #region Public Properties

        public override string Name { get; set; } = "[R] Pale Cascade";

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
            Menu = new Menu(Name, Name);

            Menu.AddItem(new MenuItem(Name + "JungleRMana", "Mana %").SetValue(new Slider(35, 0, 100)));

            Menu.AddItem(
                new MenuItem(Name + "Enabled", "Enabled").SetValue(true)
                    .SetTooltip("Wont cast unless Reset avaible"));
        }

        private void GetMob()
        {
            var mobs =
                MinionManager.GetMinions(825, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth)
                    .FirstOrDefault();

            if (mobs == null) return;

            if (!mobs.HasBuff("dianamoonlight")) return;

            Variables.Spells[SpellSlot.R].Cast(mobs);
        }

        private void OnUpdate(EventArgs args)
        {
            if (Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear
                || !Variables.Spells[SpellSlot.R].IsReady()) return;

            if (Menu.Item(Menu.Name + "JungleRMana").GetValue<Slider>().Value > Variables.Player.ManaPercent) return;

            GetMob();
        }

        #endregion
    }
}