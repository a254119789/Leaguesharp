using System;
using SharpDX;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using ReformedAIO.Champions.Gnar.Core;
using RethoughtLib.FeatureSystem.Abstract_Classes;

namespace ReformedAIO.Champions.Gnar.Drawings.Damage
{
    internal class GnarDamage : ChildBase
    {
        public override string Name { get; set; }

        public GnarDamage(string name)
        {
            Name = name;
        }

        private Dmg _dmg;

        private HpBarIndicator _hpBarIndicator;

        public void OnDraw(EventArgs args)
        {
            if (ObjectManager.Player.IsDead) return;

            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(ene => ene.IsValidTarget(1500)))
            {

                _hpBarIndicator.Unit = enemy;
                _hpBarIndicator.DrawDmg(_dmg.GetDamage(enemy), enemy.Health <= _dmg.GetDamage(enemy) * .75 ? Color.LawnGreen : Color.Yellow);
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

        protected sealed override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            _hpBarIndicator = new HpBarIndicator();
            _dmg = new Dmg();
        }
    }
}
