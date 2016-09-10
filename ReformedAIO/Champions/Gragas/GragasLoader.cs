﻿using System.Collections.Generic;

namespace ReformedAIO.Champions.Gragas
{
    #region Using Directives

    using LeagueSharp.Common;

    using Logic;
    using Menus.Draw;
    using OrbwalkingMode.Combo;
    using OrbwalkingMode.Jungle;
    using OrbwalkingMode.Lane;
    using OrbwalkingMode.Mixed;

    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Implementations;

    using Prediction = SPrediction.Prediction;

    #endregion

    internal class GragasLoader : LoadableBase
    {
        #region Public Properties

        public override string DisplayName { get; set; } = "Reformed Gragas";

        public override string InternalName { get; set; } = "Gragas";

        public override IEnumerable<string> Tags { get; set; } = new List<string>() { "Gragas" };

        #endregion

        #region Public Methods and Operators

        public override void Load()
        {
            var superParent = new SuperParent(DisplayName);

            var combo = new Parent("Combo");
            var mixed = new Parent("Mixed");
            var lane = new Parent("LaneClear");
            var jungle = new Parent("JungleClear");
            var draw = new Parent("Drawings");

            var qLogic = new QLogic();
            qLogic.Load();

            superParent.Add(new[]
            {
                combo, mixed, lane, jungle, draw
            });

            combo.Add(new QCombo());
            combo.Add(new WCombo());
            combo.Add(new ECombo());
            combo.Add(new RCombo());

            lane.Add(new LaneQ());
            lane.Add(new LaneW());
            lane.Add(new LaneE());

            mixed.Add(new QMixed());

            jungle.Add(new QJungle());
            jungle.Add(new WJungle());
            jungle.Add(new EJungle());

            draw.Add(new DrawIndicator());
            draw.Add(new DrawQ());
            draw.Add(new DrawE());
            draw.Add(new DrawR());

            superParent.OnLoadInvoker();

            Prediction.Initialize(superParent.Menu);

            var orbWalkingMenu = new Menu("Orbwalker", "Orbwalking");
            Variable.Orbwalker = new Orbwalking.Orbwalker(orbWalkingMenu);
            superParent.Menu.AddSubMenu(orbWalkingMenu);
        }

        #endregion
    }
}