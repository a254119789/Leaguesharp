namespace ReformedAIO.Champions.Gnar.OrbwalkingMode.Combo
{
    using Logic;
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Core;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    class ECombo : ChildBase
    {
        private GnarState gnarState;

        private Dmg dmg;

        private ELogic eLogic;

        public override string Name { get; set; } = "E";

        private void GameOnUpdate(EventArgs args)
        {
            var target = TargetSelector.GetTarget(Menu.SubMenu("Menu").Item("E1Range").GetValue<Slider>().Value * 2, TargetSelector.DamageType.Physical);

            if (!Spells.E.IsReady() || target == null)
            {
                return;
            }

            var config = Menu.SubMenu("Menu");

            if ((gnarState.Mini && Vars.Player.Mana > 99)
                || gnarState.TransForming
                || target.Health < dmg.GetDamage(target))
            {
                if (eLogic.EPrediction(target).CollisionObjects[0].CountEnemiesInRange(config.Item("E1Range").GetValue<Slider>().Value * 2) > 0)
                {
                    Spells.E.Cast(eLogic.EPrediction(target).CollisionObjects[0].Position);
                }
            }

            if (gnarState.Mega && target.Health < dmg.GetDamage(target))
            {
                if (Spells.E2.Cast(eLogic.EPrediction(target).CastPosition)) ;
            }
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);


            Menu.AddItem(new MenuItem("E1Range", "Range").SetValue(new Slider(475, 0, 475)));
            Menu.AddItem(new MenuItem("EonTransform", "E On Transformation").SetValue(true));

            dmg = new Dmg();
            eLogic = new ELogic();
            gnarState = new GnarState();
        }

        //protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        //{
        //    eLogic = new ELogic();
        //    dmg = new Dmg();
        //    gnarState = new GnarState();
        //}

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate -= GameOnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate += GameOnUpdate;
        }
    }
}
