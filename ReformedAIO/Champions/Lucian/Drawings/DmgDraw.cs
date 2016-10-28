﻿namespace ReformedAIO.Champions.Lucian.Drawings
{
    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Lucian.Damage;
    using ReformedAIO.Core.Drawings;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using SharpDX;

    internal sealed class DmgDraw : ChildBase
    {
        public override string Name { get; set; } = "Damage";

        public DmgDraw(LucDamage damage)
        {
            this.damage = damage;
        }

        private HeroHealthBarIndicator heroHealthBarIndicator;

        public readonly LucDamage damage;

        public void OnDraw(EventArgs args)
        {
            if (ObjectManager.Player.IsDead) return;

            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(ene => ene.IsValidTarget(1500)))
            {
                this.heroHealthBarIndicator.Unit = enemy;
                this.heroHealthBarIndicator.DrawDmg(this.damage.GetComboDamage(enemy), enemy.Health <= this.damage.GetComboDamage(enemy) * 1.25 ? Color.LawnGreen : Color.Yellow);
            }
        }

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Drawing.OnDraw -= OnDraw;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Drawing.OnDraw += OnDraw;
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            this.heroHealthBarIndicator = new HeroHealthBarIndicator();
        }
    }
}
