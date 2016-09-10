﻿using LeagueSharp;
using ReformedAIO.Champions.Ryze.OrbwalkingMode.Combo;
using RethoughtLib.FeatureSystem.Abstract_Classes;

namespace ReformedAIO.Champions.Ryze
{
    #region Using Directives

    using LeagueSharp.Common;
    using System.Collections.Generic;
    using RethoughtLib.Utility;

    using Drawings;
    using Logic;
    using OrbwalkingMode.Jungle;
    using OrbwalkingMode.Lane;
    using OrbwalkingMode.Mixed;
    using OrbwalkingMode.None.Killsteal;

    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Implementations;

    #endregion

    internal class RyzeLoader : LoadableBase
    {
        #region Public Properties

        public override string DisplayName { get; set; } = String.ToTitleCase("Reformed Ryze");

        public override string InternalName { get; set; } = "Ryze";

        public override IEnumerable<string> Tags { get; set; } = new List<string>() { "Ryze" };

        #endregion

        #region Public Methods and Operators

        public override void Load()
        { 
            var superParent = new SuperParent(DisplayName);
            superParent.Initialize();

            var orbwalker = new Orbwalking.Orbwalker(superParent.Menu.SubMenu("Orbwalker"));

            var setSpells = new SetSpells(); // lazy af
            setSpells.Load();

            var comboParent = new OrbwalkingParent("Combo", orbwalker, Orbwalking.OrbwalkingMode.Combo);
            var laneParent = new OrbwalkingParent("Lane", orbwalker, Orbwalking.OrbwalkingMode.LaneClear);
            var jungleParent = new OrbwalkingParent("Jungle", orbwalker, Orbwalking.OrbwalkingMode.LaneClear);
            var mixedParent = new OrbwalkingParent("Mixed", orbwalker, Orbwalking.OrbwalkingMode.Mixed);
            var killstealParent = new Parent("Killsteal");
            var drawParent = new Parent("Drawings");

            comboParent.Add(new ChildBase[]
            {
             new RyzeCombo(orbwalker)
            });

            mixedParent.Add(new ChildBase[]
            {
                new QMixed(orbwalker),
                new WMixed(orbwalker),
                new EMixed(orbwalker)
            });

            laneParent.Add(new ChildBase[]
            {
                new QLane(orbwalker),
                new WLane(orbwalker),
                new ELane(orbwalker)
            });
          
            jungleParent.Add(new ChildBase[]
            {
                new QJungle(orbwalker),
                new WJungle(orbwalker),
                new EJungle(orbwalker), 
            });
           
            killstealParent.Add(new ChildBase[]
            {
                new KillstealMenu(), 
            });
          
            drawParent.Add(new ChildBase[]
            {
                new QDraw(), new EDraw(), new RDraw(), new DmgDraw(), 
            });

            superParent.Add(new Base[] { comboParent, laneParent, jungleParent, mixedParent, killstealParent, drawParent, });

            superParent.Load();

            if (superParent.Loaded)
            {
                Game.PrintChat("Reformed Ryze - Loaded");
            }
        }

        #endregion
    }
}