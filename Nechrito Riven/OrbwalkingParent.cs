namespace NechritoRiven
{
    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    class OrbwalkingParent : ParentBase
    {

        private readonly Orbwalking.Orbwalker orbwalker;

        private readonly Orbwalking.OrbwalkingMode[] orbwalkingMode;

        public OrbwalkingParent(string name, Orbwalking.Orbwalker orbwalker, params Orbwalking.OrbwalkingMode[] orbwalkingMode)
        {
            this.Name = name;
            this.orbwalker = orbwalker;

            this.orbwalkingMode = orbwalkingMode;
        }

        public override sealed string Name { get; set; }

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnDisable(sender, featureBaseEventArgs);

            Game.OnUpdate -= this.GameOnOnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnEnable(sender, featureBaseEventArgs);

            Game.OnUpdate += this.GameOnOnUpdate;
        }

        private void GameOnOnUpdate(EventArgs args)
        {
            if (!this.orbwalkingMode.Any(x => x == this.orbwalker.ActiveMode))
            {
                foreach (var keyValuePair in this.Children.Where(x => x.Value.Item1))
                {
                    keyValuePair.Key.Switch.InternalDisable(new FeatureBaseEventArgs(this));
                }
            }
            else
            {
                foreach (var child in this.Children.Where(x => x.Value.Item1))
                {
                    child.Key.Switch.InternalEnable(new FeatureBaseEventArgs(this));
                }
            }
        }
    }
}
