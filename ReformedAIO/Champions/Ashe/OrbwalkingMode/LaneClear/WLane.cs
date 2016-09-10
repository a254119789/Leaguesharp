﻿namespace ReformedAIO.Champions.Ashe.OrbwalkingMode.LaneClear
{
    #region Using Directives

    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    internal class WLane : ChildBase
    {
        #region Constructors and Destructors

        private readonly Orbwalking.Orbwalker Orbwalker;

        public WLane(string name, Orbwalking.Orbwalker orbwalker)
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

        protected sealed override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            Menu.AddItem(new MenuItem(Name + "LaneWEnemy", "Only If No Enemies Visible").SetValue(true));

            Menu.AddItem(
                new MenuItem(Name + "LaneWMDistance", "Distance").SetValue(new Slider(600, 0, 900))
                    .SetTooltip("Put it too high and you'll miss minions"));

            Menu.AddItem(new MenuItem(Name + "LaneWMana", "Mana %").SetValue(new Slider(70, 0, 100)));
        }

        private void GetMinions()
        {
            var minions =
                MinionManager.GetMinions(Menu.Item(Menu.Name + "LaneWMDistance").GetValue<Slider>().Value);

            if (minions == null) return;

            if (Menu.Item(Menu.Name + "LaneWEnemy").GetValue<bool>()
                && minions.Any(m => m.CountEnemiesInRange(2000) > 0))
            {
                return;
            }

            if (minions.Count < 2) return;

            foreach (var m in minions)
            {
                if (m.Health < Variable.Spells[SpellSlot.W].GetDamage(m) * 1.2) return;

                Variable.Spells[SpellSlot.W].CastIfHitchanceEquals(m, HitChance.High);
            }
        }

        private void OnUpdate(EventArgs args)
        {
            if (Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear
                || !Variable.Spells[SpellSlot.W].IsReady()) return;

            if (Menu.Item(Menu.Name + "LaneWMana").GetValue<Slider>().Value > Variable.Player.ManaPercent) return;

            GetMinions();
        }

        #endregion
    }
}