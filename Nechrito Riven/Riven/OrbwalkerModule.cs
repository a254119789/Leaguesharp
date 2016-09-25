namespace NechritoRiven.Riven
{
    using System;

    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using Orbwalking = NechritoRiven.Orbwalking;

    internal class OrbwalkerModule : Base
    {
        public override string Name { get; set; } = "Orbwalker";

        public Orbwalking.Orbwalker OrbwalkerInstance { get; set; }

        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            this.OrbwalkerInstance.SetAttack(false);
            this.OrbwalkerInstance.SetMovement(false);
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            this.OrbwalkerInstance.SetAttack(true);
            this.OrbwalkerInstance.SetMovement(true);
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);
        }

        protected override void SetMenu()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.OrbwalkerInstance = new Orbwalking.Orbwalker(this.Menu);

            this.Menu.DisplayName = this.Name;
        }
    }
}
