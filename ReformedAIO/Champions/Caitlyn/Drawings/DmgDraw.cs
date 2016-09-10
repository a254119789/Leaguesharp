namespace ReformedAIO.Champions.Caitlyn.Drawings
{

    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Logic;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using SharpDX;

    internal class DmgDraw : ChildBase
    {
        private HpBarIndicator _drawDamage;

        private EwqrLogic _ewqrLogic;

        public sealed override string Name { get; set; }

        public DmgDraw(string name)
        {
            Name = name;
        }

        public void OnDraw(EventArgs args)
        {
            if (Vars.Player.IsDead) return;

            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(ene => ene.IsValidTarget(1500)))
            {
                _drawDamage.Unit = enemy;

                _drawDamage.DrawDmg(_ewqrLogic.EwqrDmg(enemy), _ewqrLogic.CanExecute(enemy) ? Color.Green : Color.LimeGreen);
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
            _ewqrLogic = new EwqrLogic();
            _drawDamage = new HpBarIndicator();
            base.OnLoad(sender, featureBaseEventArgs);
        }

       
    }
}
