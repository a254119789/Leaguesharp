using ReformedAIO.Champions.Ryze.OrbwalkingMode.Combo;

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

            var setSpells = new SetSpells();
            setSpells.Load();

            var comboParent = new Parent("Combo");
            var laneParent = new Parent("Lane");
            var jungleParent = new Parent("Jungle");
            var mixedParent = new Parent("Mixed");
            var killstealParent = new Parent("Killsteal");
            var drawParent = new Parent("Drawings");

            superParent.Add(new RyzeCombo("Combo"));

            superParent.Add(new[] { laneParent, jungleParent, mixedParent, killstealParent, drawParent });

            mixedParent.Add(new QMixed());
            mixedParent.Add(new WMixed());
            mixedParent.Add(new EMixed());

            laneParent.Add(new QLane());
            laneParent.Add(new WLane());
            laneParent.Add(new ELane());

            jungleParent.Add(new QJungle());
            jungleParent.Add(new WJungle());
            jungleParent.Add(new EJungle());

            killstealParent.Add(new KillstealMenu());

            drawParent.Add(new QDraw());
            drawParent.Add(new EDraw());
            drawParent.Add(new RDraw());
            drawParent.Add(new DmgDraw());

            var orbWalkingMenu = new Menu("Orbwalker", "Orbwalker");
            Variable.Orbwalker = new Orbwalking.Orbwalker(orbWalkingMenu);
            superParent.Menu.AddSubMenu(orbWalkingMenu);

            superParent.OnLoadInvoker();
        }

        #endregion
    }
}