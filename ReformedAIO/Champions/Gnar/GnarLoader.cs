using ReformedAIO.Champions.Gnar.Core;
using ReformedAIO.Champions.Gnar.OrbwalkingMode.Combo;
using RethoughtLib.FeatureSystem.Abstract_Classes;

namespace ReformedAIO.Champions.Gnar
{
    using LeagueSharp.Common;
    using System.Collections.Generic;

    using RethoughtLib.Utility;
    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Implementations;

    internal class GnarLoader : LoadableBase
    {
        public override string DisplayName { get; set; } = "Reformed Gnar";

        public override string InternalName { get; set; } = "Gnar";

        public override IEnumerable<string> Tags { get; set; } = new[] { "Gnar" };

        public override void Load()
        {
            var superParent = new SuperParent(DisplayName);

            var comboParent = new Parent("Combo");

            superParent.Add(new Base[]
            {
                comboParent
            });

            comboParent.Add(new Base[]
            {
                new QCombo(), new WCombo(),
                new ECombo(), new RCombo(),
            });

            var spells = new Spells(); // - Loads the spells (Can't use dictionary, since transform afaik.
            spells.Initialise();

            var orbWalkingMenu = new Menu("Orbwalker", "Orbwalking");
            Vars.Orbwalker = new Orbwalking.Orbwalker(orbWalkingMenu);

            superParent.Menu.AddSubMenu(orbWalkingMenu);

            superParent.OnLoadInvoker();
        }
    }
}
