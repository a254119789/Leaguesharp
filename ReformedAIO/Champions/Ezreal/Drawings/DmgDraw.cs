namespace ReformedAIO.Champions.Ezreal.Drawings
{
    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Ezreal.Core.Damage;
    using ReformedAIO.Core.Drawings;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using SharpDX;

    internal sealed class DmgDraw : ChildBase
    {
        public override string Name { get; set; } = "Ezreal Damage";

        public DmgDraw(EzrealDamage damage)
        {
            this.damage = damage;
        }

        private HeroHealthBarIndicator heroHealthBarIndicator;

        public readonly EzrealDamage damage;

        public void OnDraw(EventArgs args)
        {
            if (ObjectManager.Player.IsDead) return;

            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(ene => ene.IsValidTarget(2000)))
            {
                heroHealthBarIndicator.Unit = enemy;

                heroHealthBarIndicator.DrawDmg(damage.GetComboDamage(enemy),
                    enemy.Health <= damage.GetComboDamage(enemy) * 1.25
                    ? Color.LawnGreen 
                    : Color.Yellow);
            }
        }

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnDisable(sender, featureBaseEventArgs);

            Drawing.OnDraw -= OnDraw;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnEnable(sender, featureBaseEventArgs);


            Drawing.OnDraw += OnDraw;
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            heroHealthBarIndicator = new HeroHealthBarIndicator();
        }
    }
}
