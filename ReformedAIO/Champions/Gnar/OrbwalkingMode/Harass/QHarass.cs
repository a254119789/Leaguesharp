using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using ReformedAIO.Champions.Gnar.Core;
using RethoughtLib.FeatureSystem.Abstract_Classes;
using RethoughtLib.Menu;
using RethoughtLib.Menu.Presets;

namespace ReformedAIO.Champions.Gnar.OrbwalkingMode.Harass
{
    internal sealed class QHarass: ChildBase
    {
        private GnarState gnarState;


        public override string Name { get; set; } = "Q";

        private readonly Orbwalking.Orbwalker Orbwalker;

        public QHarass(Orbwalking.Orbwalker orbwalker)
        {
            Orbwalker = orbwalker;
        }

        private void GameOnUpdate(EventArgs args)
        {
            if (Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Mixed
                || (Menu.SubMenu("Menu").Item("BlockIfTransforming").GetValue<bool>()
                && gnarState.TransForming))
            {
                return;
            }

            if (gnarState.Mini)
            {
                Mini();
            }

            if (gnarState.Mega)
            {
                Mega();
            }
        }

        private void Mini()
        {
            if (!Spells.Q.IsReady() || !Vars.Player.IsWindingUp)
            {
                return;
            }

            var config = Menu.SubMenu("Menu");

            foreach (var target in HeroManager.Enemies.Where(x => x.IsValidTarget(config.Item("Q1Range").GetValue<Slider>().Value)))
            {
                if (target == null)
                {
                    return;
                }

                var prediction = Spells.Q.GetPrediction(target);

                if (prediction.Hitchance >= HitChance.VeryHigh
                    || (config.Item("BetaQ").GetValue<bool>()
                    && prediction.CollisionObjects.Count > 0
                    && prediction.CollisionObjects[0].CountEnemiesInRange(100) > 0))
                {
                    Spells.Q.Cast(prediction.CastPosition);
                }
            }
        }

        private void Mega()
        {
            var config = Menu.SubMenu("Menu");

            foreach (var target in HeroManager.Enemies.Where(x => x.IsValidTarget(config.Item("Q2Range").GetValue<Slider>().Value)))
            {
                if (target == null)
                {
                    return;
                }

                var prediction = Spells.Q2.GetPrediction(target);

                if ((config.Item("BetaQ").GetValue<bool>()
                    && prediction.CollisionObjects.Count > 0
                    && prediction.CollisionObjects[0].CountEnemiesInRange(100) > 0)
                    || prediction.Hitchance >= HitChance.High)
                {
                    Spells.Q2.Cast(prediction.CastPosition);
                }
            }
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            gnarState = new GnarState();

            var selecter = new MenuItem("GnarForm", "Form").SetValue(new StringList(new[] { "Mini", "Mega" }));

            var mini = new List<MenuItem>()
             {
                 new MenuItem("Q1Range", "Range").SetValue(new Slider(1100, 0, 1100)),
                 new MenuItem("BetaQ", "Allow Collision").SetValue(true).SetTooltip("Will Q On Minions Near Target"),
                 new MenuItem("BlockIfTransforming", "Block If Transforming").SetValue(true),
             };

            var mega = new List<MenuItem>()
             {
                 new MenuItem("Q2Range", "Range").SetValue(new Slider(1100, 0, 1100)),
                 new MenuItem("BetaQ2", "Allow Collision Q").SetValue(true).SetTooltip("Will Q On Minions Near Target"),
             };

            var menuGenerator = new MenuGenerator(Menu, new DynamicMenu("Menu", selecter, new[] { mini }));

            menuGenerator.Generate();
        }


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
