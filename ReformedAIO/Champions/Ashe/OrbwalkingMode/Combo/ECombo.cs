﻿namespace ReformedAIO.Champions.Ashe.OrbwalkingMode.Combo
{
    #region Using Directives

    using System;
    using System.Drawing;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Ashe.Logic;

    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    internal class ECombo : ChildBase
    {
        #region Fields

        private ELogic eLogic;

        #endregion

        #region Constructors and Destructors

        public ECombo(string name)
        {
            Name = name;
        }

        #endregion

        #region Public Properties

        public override string Name { get; set; }

        #endregion

        #region Methods

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Drawing.OnDraw -= OnDraw;
            Events.OnUpdate -= OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Drawing.OnDraw += OnDraw;
            Events.OnUpdate += OnUpdate;
        }

        protected override void OnInitialize(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            eLogic = new ELogic();
        }

        protected sealed override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Menu.AddItem(
                new MenuItem(Menu.Name + "EDistance", "Distance").SetValue(new Slider(1500, 0, 1500))
                    .SetTooltip("Only for enemeis & not objectives"));

            Menu.AddItem(new MenuItem(Menu.Name + "ECount", "Save 1 Charge").SetValue(true));

            Menu.AddItem(new MenuItem(Name + "EToVector", "E To Objectives").SetValue(true));

            Menu.AddItem(new MenuItem(Name + "VectorDraw", "Draw Objective Position").SetValue(true));
        }

        private void EToCamp()
        {
            var pos =
                eLogic.Camp.FirstOrDefault(
                    x =>
                    x.Value.Distance(Variable.Player.Position) > 1500
                    && x.Value.Distance(Variable.Player.Position) < 7000);

            if (!pos.Value.IsValid()) return;

            Utility.DelayAction.Add(290, () => Variable.Spells[SpellSlot.E].Cast(pos.Value)); // Humanized
        }

        private void Hawkshot()
        {
            var target = TargetSelector.GetTarget(Variable.Player.AttackRange, TargetSelector.DamageType.Physical);

            if (target == null || !target.IsValid
                || target.Distance(Variable.Player)
                > Menu.Item(Menu.Name + "EDistance").GetValue<Slider>().Value || target.IsVisible) return;

            if (!eLogic.ComboE(target)) return;

            foreach (var position in HeroManager.Enemies.Where(x => !x.IsDead && x.Distance(Variable.Player) < 1500))
            {
                var path = position.GetWaypoints().LastOrDefault().To3D();

                if (!NavMesh.IsWallOfGrass(path, 1)) return;

                if (position.Distance(path) > 1500) return;

                if (NavMesh.IsWallOfGrass(Variable.Player.Position, 1)) return; // Stil no proof wether or not this work yet.

                Variable.Spells[SpellSlot.E].Cast(path);
            }
        }

        private void OnDraw(EventArgs args)
        {
            if (Variable.Player.IsDead || !Menu.Item(Menu.Name + "VectorDraw").GetValue<bool>()) return;

            var pos =
                eLogic.Camp.FirstOrDefault(
                    x =>
                    x.Value.Distance(Variable.Player.Position) > 1500
                    && x.Value.Distance(Variable.Player.Position) < 7000);

            Render.Circle.DrawCircle(pos.Value, 100, !pos.Value.IsValid() ? Color.Red : Color.Green);
        }

        private void OnUpdate(EventArgs args)
        {
            if (!Variable.Spells[SpellSlot.E].IsReady() || Variable.Player.IsRecalling() || Variable.Player.InShop()) return;

            if (Menu.Item(Menu.Name + "ECount").GetValue<bool>() && eLogic.GetEAmmo() == 1) return;

            if (Variable.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.None)
                // I could make a new class and stuff but, doesn't really matter
            {
                EToCamp();
            }

            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo) return;

            Hawkshot();
        }

        #endregion
    }
}