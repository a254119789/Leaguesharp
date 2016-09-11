using System;
using LeagueSharp;
using LeagueSharp.Common;
using ReformedAIO.Champions.Gnar.Core;
using RethoughtLib.FeatureSystem.Abstract_Classes;


namespace ReformedAIO.Champions.Gnar.OrbwalkingMode.Jungle
{
    internal sealed class W2Lane : ChildBase
    {
        private GnarState _gnarState;

        public override string Name { get; set; } = "W";

        private readonly Orbwalking.Orbwalker _orbwalker;

        public W2Lane(Orbwalking.Orbwalker orbwalker)
        {
            _orbwalker = orbwalker;
        }

        private void GameOnUpdate(EventArgs args)
        {
            if (_orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear
                || !ObjectManager.Player.IsWindingUp
                || _gnarState.Mini)
            {
                return;
            }

            if (!Spells.W2.IsReady())
            {
                return;
            }

            foreach (var m in MinionManager.GetMinions(Menu.SubMenu("Menu").Item("W2Range").GetValue<Slider>().Value,
                MinionTypes.All,
                MinionTeam.Enemy,
                MinionOrderTypes.MaxHealth))
            {
                if (m == null)
                {
                    return;
                }

                var pred = Spells.W2.GetPrediction(m, true);

                if (pred.AoeTargetsHitCount >= Menu.SubMenu("Menu").Item("W2HitCount").GetValue<Slider>().Value)
                {
                    Spells.W2.Cast(m);
                }
            }
        }


        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            _gnarState = new GnarState();

            new MenuItem("W2Range", "Range").SetValue(new Slider(525, 0, 525));
            new MenuItem("W2HitCount", "Min Hit Count").SetValue(new Slider(3, 0, 6));
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
