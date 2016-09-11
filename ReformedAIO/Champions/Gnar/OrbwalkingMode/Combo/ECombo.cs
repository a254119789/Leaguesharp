using RethoughtLib.FeatureSystem.Abstract_Classes;

namespace ReformedAIO.Champions.Gnar.OrbwalkingMode.Combo
{
    using Logic;
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Core;
   

    internal sealed class ECombo : ChildBase
    {
        private GnarState gnarState;

        private Dmg dmg;

        private ELogic eLogic;

        public override string Name { get; set; } = "E";

        private readonly Orbwalking.Orbwalker Orbwalker;

        public ECombo(Orbwalking.Orbwalker orbwalker)
        {
            Orbwalker = orbwalker;
        }

        private void GameOnUpdate(EventArgs args)
        {
            if (Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo)
            {
                return;
            }

            var target = TargetSelector.GetTarget(Menu.SubMenu("Menu").Item("E1Range").GetValue<Slider>().Value * 2, TargetSelector.DamageType.Physical);

            if (!Spells.E.IsReady() || target == null)
            {
                return;
            }

            var config = Menu.SubMenu("Menu");

            if ((gnarState.Mini && Vars.Player.Mana >= 95)
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

        private void Gapcloser(ActiveGapcloser gapcloser)
        {
            var target = gapcloser.Sender;

            if (target == null 
                || !Menu.SubMenu("Menu").Item("EAwayMelee").GetValue<bool>())
            {
                return;
            }

            Spells.E.Cast(eLogic.EPrediction(target).CollisionObjects[0].Position); // TODO: Might need to change this..
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            Menu.AddItem(new MenuItem("E1Range", "Range").SetValue(new Slider(475, 0, 475)));
            Menu.AddItem(new MenuItem("EAwayMelee", "E Away From Melee's").SetValue(false));
            Menu.AddItem(new MenuItem("EonTransform", "E On Transformation").SetValue(true));

            dmg = new Dmg();
            eLogic = new ELogic();
            gnarState = new GnarState();
        }

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            AntiGapcloser.OnEnemyGapcloser -= Gapcloser;
            Game.OnUpdate -= GameOnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            AntiGapcloser.OnEnemyGapcloser += Gapcloser;
            Game.OnUpdate += GameOnUpdate;
        }
    }
}
