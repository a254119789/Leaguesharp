﻿using ReformedAIO.Champions.Gnar.Drawings.Damage;
using ReformedAIO.Champions.Gnar.PermaActive;
using RethoughtLib.Bootstraps.Abstract_Classes;
using RethoughtLib.FeatureSystem.Abstract_Classes;

namespace ReformedAIO.Champions.Gnar
{
    using LeagueSharp;
    using LeagueSharp.Common;

    using System.Collections.Generic;


    using RethoughtLib.FeatureSystem.Implementations;


    using Core;
    using Drawings.SpellRange;
    using OrbwalkingMode.Combo;
    using OrbwalkingMode.Harass;
    using OrbwalkingMode.Jungle;
    using OrbwalkingMode.Lane;

    internal sealed class GnarLoader : LoadableBase
    {
        public override string DisplayName { get; set; } = "Reformed Gnar";

        public override string InternalName { get; set; } = "Gnar";

        public override IEnumerable<string> Tags { get; set; } = new[] { "Gnar" };

        public override void Load()
        {
            var superParent = new SuperParent(DisplayName);
            superParent.Initialize();

            var spells = new Spells(); // - RIP CLEAN CODE
            spells.Initialize();

            var orbwalker = new Orbwalking.Orbwalker(superParent.Menu.SubMenu("Orbwalker"));

            var comboParent = new OrbwalkingParent("Combo", orbwalker, Orbwalking.OrbwalkingMode.Combo);
            var harassParent = new OrbwalkingParent("Harass", orbwalker, Orbwalking.OrbwalkingMode.Mixed);
            var jungleParent = new OrbwalkingParent("Jungle", orbwalker, Orbwalking.OrbwalkingMode.LaneClear);
            var laneParent = new OrbwalkingParent("Lane", orbwalker, Orbwalking.OrbwalkingMode.LaneClear);
            var killstealParent = new Parent("Killsteal");
            var fleeParent = new Parent("Flee");
            var drawingParent = new Parent("Drawing");

            superParent.Add(new Base[]
            {
                comboParent,
                harassParent,
                jungleParent,
                laneParent,
                jungleParent,
                drawingParent
            });

            comboParent.Add(new Base[]
            {
                new QCombo(orbwalker),
                new WCombo(orbwalker),
                new ECombo(orbwalker),
                new RCombo(orbwalker)
            });

            harassParent.Add(new Base[]
            {
                new QHarass(orbwalker), 
            });

            laneParent.Add(new Base[]
            {
                new QLane(orbwalker),
                new W2Lane(orbwalker)  
            });

            jungleParent.Add(new Base[]
            {
                new QJungle(orbwalker),
                new W2Jungle(orbwalker)  
            });

            fleeParent.Add(new Base[]
            {
                new Flee("Flee")
            });

            drawingParent.Add(new Base[]
            {
                new GnarDamage("Damage Indicator"), 
                new QRange("Q"),
                new WRange("W"),
                new ERange("E"),
                new RRange("R"),    
            });

            superParent.Load();

            if (superParent.Loaded)
            {
                Game.PrintChat("Reformed Gnar - Loaded!");
            }
        }
    }
}
