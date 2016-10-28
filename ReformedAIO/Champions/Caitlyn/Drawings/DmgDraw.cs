﻿namespace ReformedAIO.Champions.Caitlyn.Drawings
{
    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Caitlyn.Logic;
    using ReformedAIO.Library.Drawings;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using SharpDX;

    internal sealed class DmgDraw : ChildBase
    {
        private HeroHealthBarIndicator drawDamage;

        private EwqrLogic ewqrLogic;

        public override string Name { get; set; } = "Damage";

        public void OnDraw(EventArgs args)
        {
            if (Vars.Player.IsDead) return;

            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(ene => ene.IsValidTarget(1500)))
            {
                this.drawDamage.Unit = enemy;

                this.drawDamage.DrawDmg(this.ewqrLogic.EwqrDmg(enemy), this.ewqrLogic.CanExecute(enemy) ? Color.LimeGreen: Color.Green);
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
            this.ewqrLogic = new EwqrLogic();
            this.drawDamage = new HeroHealthBarIndicator();
            base.OnLoad(sender, featureBaseEventArgs);
        }
    }
}
