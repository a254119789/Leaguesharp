using System;
using System.Linq;

using LeagueSharp;
using LeagueSharp.Common;

using RethoughtLib.FeatureSystem.Abstract_Classes;

namespace ReformedAIO.Champions
{
    internal sealed class OrbwalkingParent : ParentBase
    {
        private Orbwalking.Orbwalker Orbwalker;

       // private readonly Orbwalking.OrbwalkingMode OrbwalkingMode;

        public OrbwalkingParent(Orbwalking.Orbwalker orbwalker)
        {
           
            orbwalker = orbwalker;
           // orbwalkingMode = orbwalkingMode;
        }

        public override string Name { get; set; }

        protected override void OnDisable(object sender, FeatureBaseEventArgs args)
        {
            base.OnDisable(sender, args);

            Orbwalker.SetAttack(false);
            Orbwalker.SetMovement(false);
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs args)
        {
            base.OnEnable(sender, args);

            Orbwalker.SetAttack(true);
            Orbwalker.SetMovement(true);
        }

        protected override void SetMenu()
        {
            base.SetMenu();
            Orbwalker = new Orbwalking.Orbwalker(Menu.Parent);
            Menu = Menu.Parent.SubMenu("Orbwalk");
        }
    }
}
