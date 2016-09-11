using System;
using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;
using ReformedAIO.Champions.Gnar.Core;
using RethoughtLib.FeatureSystem.Abstract_Classes;
using RethoughtLib.Menu;
using RethoughtLib.Menu.Presets;

namespace ReformedAIO.Champions.Gnar.OrbwalkingMode.Jungle
{
    internal sealed class QJungle : ChildBase
    {
        private GnarState _gnarState;

        public override string Name { get; set; } = "Q";

        private readonly Orbwalking.Orbwalker _orbwalker;

        public QJungle(Orbwalking.Orbwalker orbwalker)
        {
            _orbwalker = orbwalker;
        }

        private void GameOnUpdate(EventArgs args)
        {
            if (_orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear
                || (Menu.SubMenu("Menu").Item("BlockIfTransforming").GetValue<bool>()
                && _gnarState.TransForming))
            {
                return;
            }

            if (_gnarState.Mini)
            {
                Mini();
            }

            if (_gnarState.Mega)
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
           
            foreach (var m in MinionManager.GetMinions(Menu.SubMenu("Menu").Item("Q1Range").GetValue<Slider>().Value, 
                MinionTypes.All,
                MinionTeam.Neutral,
                MinionOrderTypes.MaxHealth))
            {
                if (m == null)
                {
                    return;
                }

                var prediction = Spells.Q.GetPrediction(m, true);

                if (prediction.Hitchance >= HitChance.High)
                {
                    Spells.Q.Cast(prediction.CastPosition);
                }
            }
        }

        private void Mega()
        {
            if (!Spells.Q2.IsReady())
            {
                return;
            }
         
            foreach (var m in MinionManager.GetMinions(Menu.SubMenu("Menu").Item("Q2Range").GetValue<Slider>().Value, 
                MinionTypes.All,
                MinionTeam.Neutral,
                MinionOrderTypes.MaxHealth))
            {
                if (m == null)
                {
                    return;
                }

                var prediction = Spells.Q2.GetPrediction(m);

                if (prediction.Hitchance >= HitChance.High)
                {
                    Spells.Q2.Cast(prediction.CastPosition);
                }
            }
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            _gnarState = new GnarState();

            var selecter = new MenuItem("GnarForm", "Form").SetValue(new StringList(new[] { "Mini", "Mega" }));

            var mini = new List<MenuItem>()
             {
                 new MenuItem("Q1Range", "Range").SetValue(new Slider(600, 0, 600)),
                 new MenuItem("BlockIfTransforming", "Block If Transforming").SetValue(true),
             };

            var mega = new List<MenuItem>()
             {
                 new MenuItem("Q2Range", "Range").SetValue(new Slider(600, 0, 700)),
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
