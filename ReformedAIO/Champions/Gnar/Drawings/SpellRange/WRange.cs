﻿namespace ReformedAIO.Champions.Gnar.Drawings.SpellRange
{
    using System;
    using System.Drawing;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Gnar.Core;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    internal class WRange : ChildBase
    {
        public override string Name { get; set; }

        public WRange(string name)
        {
            Name = name;
        }


        public void OnDraw(EventArgs args)
        {
            if (ObjectManager.Player.IsDead) return;

            if (!Spells.W2.IsReady())
            {
                return;
            }

            Render.Circle.DrawCircle(
                ObjectManager.Player.Position,
                Spells.W2.Range,
                Color.Red);

        }

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
           
        }
    }
}
