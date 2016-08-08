﻿namespace ReformedAIO.Champions.Ryze.OrbwalkingMode.Lane
{
    #region Using Directives

    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Ryze.Logic;

    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    internal class ELane : ChildBase
    {
        #region Fields

        private ELogic eLogic;

        #endregion

        #region Public Properties

        public override string Name { get; set; } = "[E] Spell Flux";

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
            eLogic = new ELogic();
            base.OnInitialize(sender, featureBaseEventArgs);
        }

        protected sealed override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Menu.AddItem(new MenuItem(Name + "LaneEEnemy", "Only If No Enemies Visible").SetValue(true));

            Menu.AddItem(new MenuItem(Name + "LaneEMana", "Mana %").SetValue(new Slider(65, 0, 100)));
        }

        private void GetMinions()
        {
            if (Variable.Player.Mana < Variable.Spells[SpellSlot.Q].ManaCost) return;

            var minions = MinionManager.GetMinions(Variable.Spells[SpellSlot.E].Range);

            if (minions == null) return;

            if (Menu.Item(Menu.Name + "LaneEEnemy").GetValue<bool>()
                && minions.Any(m => m.CountEnemiesInRange(1750) > 0))
            {
                return;
            }

            foreach (var m in minions)
            {
                if (eLogic.RyzeE(m)
                    || m.Health > Variable.Player.GetAutoAttackDamage(m)
                    && m.Health > Variable.Spells[SpellSlot.E].GetDamage(m))
                {
                    Variable.Spells[SpellSlot.E].Cast(m);
                }
            }
        }

        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear
                || !Variable.Spells[SpellSlot.E].IsReady()) return;

            if (Menu.Item(Menu.Name + "LaneEMana").GetValue<Slider>().Value > Variable.Player.ManaPercent) return;

            GetMinions();
        }

        #endregion
    }
}