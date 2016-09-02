using ReformedAIO.Champions.Caitlyn.Drawings;
using ReformedAIO.Champions.Caitlyn.Killsteal;
using ReformedAIO.Champions.Caitlyn.Logic;
using ReformedAIO.Champions.Caitlyn.OrbwalkingMode.Combo;
using ReformedAIO.Champions.Caitlyn.OrbwalkingMode.Jungle;
using ReformedAIO.Champions.Caitlyn.OrbwalkingMode.Lane;

namespace ReformedAIO.Champions.Caitlyn
{
    using LeagueSharp.Common;
    using System.Collections.Generic;

    using RethoughtLib.Utility;
    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Implementations;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    internal class CaitlynLoader : LoadableBase
    {
        public override string DisplayName { get; set; } = String.ToTitleCase("Reformed Caitlyn");

        public override string InternalName { get; set; } = "Caitlyn";

        public override IEnumerable<string> Tags { get; set; } = new[] { "Caitlyn" };

        public override void Load()
        {
            var superParent = new SuperParent(DisplayName);

            var comboParent = new Parent("Combo");
            var laneParent = new Parent("Lane");
            var jungleParent = new Parent("Jungle");
            var killstealParent = new Parent("Killsteal");
            var drawParent = new Parent("Drawings");

            var setSpells = new Spells();
            setSpells.OnLoad();

            superParent.Add(new[]
          {
                comboParent,
                laneParent,
                jungleParent,
                killstealParent,
                drawParent,
            });

            comboParent.Add(new ChildBase[]
            {
             //   new Ewqr("EWQR Execute"), 
                new QCombo("[Q]"),
                new WCombo("[W]"),
                new ECombo("[E]"),   
            });

            laneParent.Add(new QLane("[Q]"));

            jungleParent.Add(new ChildBase[]
            {
                new QJungle("[Q]"),
                new EJungle("[E]"),
            });

            killstealParent.Add(new ChildBase[]
            {
                new QKillsteal("[Q]"),
                new RKillsteal("[R]"),  
            });
          
            drawParent.Add(new ChildBase[]
            {
                new DmgDraw("Damage"), 
                new QDraw("[Q]"),
                new WDraw("[W]"),
                new EDraw("[E]"),
                new RDraw("[R]"),    
            });

            var orbWalkingMenu = new Menu("Orbwalker", "Orbwalking");
            Vars.Orbwalker = new Orbwalking.Orbwalker(orbWalkingMenu);
            superParent.Menu.AddSubMenu(orbWalkingMenu);

            superParent.OnLoadInvoker();
        }
    }
}
