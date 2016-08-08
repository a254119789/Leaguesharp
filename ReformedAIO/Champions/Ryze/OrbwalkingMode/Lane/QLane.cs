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

    internal class QLane : ChildBase
    {
        #region Fields

        private ELogic eLogic;

        private QLogic qLoigc;

        #endregion

        #region Public Properties

        public override string Name { get; set; } = "[Q] Overload";

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
            qLoigc = new QLogic();
            base.OnInitialize(sender, featureBaseEventArgs);
        }

        protected sealed override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Menu.AddItem(new MenuItem(Name + "LaneQEnemy", "Only If No Enemies Visible").SetValue(true));

            Menu.AddItem(new MenuItem(Name + "LaneQMana", "Mana %").SetValue(new Slider(65, 0, 100)));
        }

        private void GetMinions()
        {
            var minions = MinionManager.GetMinions(Variable.Spells[SpellSlot.Q].Range);

            if (minions == null) return;

            if (Menu.Item(Menu.Name + "LaneQEnemy").GetValue<bool>()
                && minions.Any(m => m.CountEnemiesInRange(1750) > 0))
            {
                return;
            }

            foreach (var m in minions)
            {
                if (eLogic.RyzeE(m))
                {
                    Variable.Spells[SpellSlot.Q].Cast(qLoigc.QPred(m));
                }
            }
        }

        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear
                || !Variable.Spells[SpellSlot.Q].IsReady()) return;

            if (Menu.Item(Menu.Name + "LaneQMana").GetValue<Slider>().Value > Variable.Player.ManaPercent) return;

            GetMinions();
        }

        #endregion
    }
}