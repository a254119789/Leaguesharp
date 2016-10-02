﻿namespace ReformedAIO.Champions.Caitlyn
{
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Drawings;
    using Killsteal;
    using Logic;
    using OrbwalkingMode.Combo;
    using OrbwalkingMode.Jungle;
    using OrbwalkingMode.Lane;

    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Implementations;
    using RethoughtLib.Utility;

    internal class CaitlynLoader : LoadableBase
    {
        public override string DisplayName { get; set; } = String.ToTitleCase("Reformed Caitlyn");

        public override string InternalName { get; set; } = "Caitlyn";

        public override IEnumerable<string> Tags { get; set; } = new[] { "Caitlyn" };

        public override void Load()
        {
            var superParent = new SuperParent(DisplayName);
            superParent.Initialize();

            var orbwalker = new Orbwalking.Orbwalker(superParent.Menu.SubMenu("Orbwalker"));

            var comboParent = new OrbwalkingParent("Combo", orbwalker, Orbwalking.OrbwalkingMode.Combo);
            var laneParent = new OrbwalkingParent("Lane", orbwalker, Orbwalking.OrbwalkingMode.LaneClear);
            var jungleParent = new OrbwalkingParent("Jungle", orbwalker, Orbwalking.OrbwalkingMode.LaneClear);
            var killstealParent = new Parent("Killsteal");
            var drawParent = new Parent("Drawings");

            var setSpells = new Spells();
            setSpells.OnLoad();

            superParent.Add(new Base[] {
                comboParent,
                laneParent,
                jungleParent,
                killstealParent,
                drawParent
           });

            comboParent.Add(new ChildBase[]
            {
                new QCombo(orbwalker),
                new WCombo(orbwalker),
                new ECombo(orbwalker)   
            });

            laneParent.Add(new QLane(orbwalker));

            jungleParent.Add(new ChildBase[]
            {
                new QJungle(orbwalker),
                new EJungle(orbwalker)
            });

            killstealParent.Add(new ChildBase[]
            {
                new QKillsteal("[Q]"),
                new RKillsteal("[R]")  
            });
          
            drawParent.Add(new ChildBase[]
            {
                new DmgDraw("Damage"), 
                new QDraw("[Q]"),
                new WDraw("[W]"),
                new EDraw("[E]"),
                new RDraw("[R]")    
            });

            superParent.Load();

            if (superParent.Loaded)
            {
                Game.PrintChat(DisplayName + " - Loaded");
            }
        }
    }
}
