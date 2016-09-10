﻿namespace ReformedAIO.Champions.Ryze.Drawings
{
    #region Using Directives

    using System;
    using System.Drawing;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Diana;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    internal class RDraw : ChildBase
    {
        #region Public Properties

        public override string Name { get; set; } = "[R] Draw";

        #endregion

        #region Public Methods and Operators

        public void OnDraw(EventArgs args)
        {
            if (Variables.Player.IsDead) return;

            Render.Circle.DrawCircle(
                Variable.Player.Position,
                Variable.Spells[SpellSlot.R].Level <= 1 ? 1500 : 3000,
                Color.Aqua);
        }

        #endregion

        #region Methods

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Drawing.OnDraw -= OnDraw;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Drawing.OnDraw += OnDraw;
        }

        protected sealed override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Menu = new Menu(Name, Name);

            Menu.AddItem(new MenuItem(Name + "Enabled", "Enabled").SetValue(true));
        }

        #endregion
    }
}