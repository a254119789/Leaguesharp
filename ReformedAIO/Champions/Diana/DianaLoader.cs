using System.Collections.Generic;
using RethoughtLib.Utility;

namespace ReformedAIO.Champions.Diana
{
    #region Using Directives

    using LeagueSharp.Common;

    using Logic.Killsteal;
    using Menus.Draw;
    using OrbwalkingMode.Combo;
    using OrbwalkingMode.Flee;
    using OrbwalkingMode.Jungleclear;
    using OrbwalkingMode.Laneclear;
    using OrbwalkingMode.Misaya;
    using OrbwalkingMode.Mixed;

    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Implementations;

    #endregion

    internal class DianaLoader : LoadableBase
    {
        #region Public Properties

        public override string DisplayName { get; set; } = String.ToTitleCase("Reformed Diana");

        public override string InternalName { get; set; } = "Diana";

        public override IEnumerable<string> Tags { get; set; } = new[] { "Diana" };

        #endregion

        #region Public Methods and Operators

        public override void Load()
        {
            var superParent = new SuperParent(DisplayName);

            // Parents
            var combo = new Parent("Combo");
            var misaya = new Parent("Misaya");
            var mixed = new Parent("Mixed");
            var lane = new Parent("LaneClear");
            var jungle = new Parent("JungleClear");
            var ks = new Parent("Killsteal");
            var draw = new Parent("Drawings");
            var flee = new Parent("Flee");

            superParent.Add(new[]
            {
                combo, misaya, mixed, lane, jungle, ks, draw, flee
            });

            combo.Add(new CrescentStrike());
            combo.Add(new Moonfall());
            combo.Add(new LunarRush());
            combo.Add(new PaleCascade());
            combo.Add(new MisayaCombo());

            mixed.Add(new MixedCrescentStrike());

            lane.Add(new LaneCrescentStrike());
            lane.Add(new LaneLunarRush());

            jungle.Add(new JungleCrescentStrike());
            jungle.Add(new JungleLunarRush());
            jungle.Add(new JungleMoonfall());
            jungle.Add(new JunglePaleCascade());

            ks.Add(new KsPaleCascade());
            ks.Add(new KsCrescentStrike());

            draw.Add(new DrawQ());
            draw.Add(new DrawE());
            draw.Add(new DrawDmg());
            draw.Add(new DrawPred());

            flee.Add(new FleeMode());

            superParent.OnLoadInvoker();

            var orbWalkingMenu = new Menu("Orbwalker", "Orbwalker");
            Variables.Orbwalker = new Orbwalking.Orbwalker(orbWalkingMenu);

            superParent.Menu.AddSubMenu(orbWalkingMenu);
        }

        #endregion
    }
}