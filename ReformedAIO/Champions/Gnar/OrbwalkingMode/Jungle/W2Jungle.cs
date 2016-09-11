using System;
using LeagueSharp;
using LeagueSharp.Common;
using ReformedAIO.Champions.Gnar.Core;
using RethoughtLib.FeatureSystem.Abstract_Classes;

namespace ReformedAIO.Champions.Gnar.OrbwalkingMode.Jungle
{
    internal sealed class W2Jungle : ChildBase
    {
        private GnarState _gnarState;

        public override string Name { get; set; } = "W";

        private readonly Orbwalking.Orbwalker _orbwalker;

        public W2Jungle(Orbwalking.Orbwalker orbwalker)
        {
            _orbwalker = orbwalker;
        }

        private void GameOnUpdate(EventArgs args)
        {
            if (_orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear || !ObjectManager.Player.IsWindingUp)
            {
                return;
            }

            if (!Spells.W2.IsReady())
            {
                return;
            }

            foreach (var m in MinionManager.GetMinions(Menu.SubMenu("Menu").Item("W2Range").GetValue<Slider>().Value,
                MinionTypes.All,
                MinionTeam.Neutral,
                MinionOrderTypes.MaxHealth))
            {
                if (m == null)
                {
                    return;
                }

                Spells.W2.Cast(m);
            }
        }


        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            _gnarState = new GnarState();


            new MenuItem("W2Range", "Range").SetValue(new Slider(525, 0, 525));
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
