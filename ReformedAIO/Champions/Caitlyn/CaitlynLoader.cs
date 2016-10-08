namespace ReformedAIO.Champions.Caitlyn
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
    using RethoughtLib.FeatureSystem.Guardians;
    using RethoughtLib.FeatureSystem.Implementations;
    using RethoughtLib.Utility;

    internal class CaitlynLoader : LoadableBase
    {
        public override string DisplayName { get; set; } = "Reformed Caitlyn";

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

            comboParent.Add(new List<Base>()
                                {
                new QCombo().Guardian(new PlayerMustNotBeWindingUp()).Guardian(new SpellMustBeReady(SpellSlot.Q)).Guardian(new SpellMustBeReady(SpellSlot.E) {Negated = true}),
                new WCombo().Guardian(new PlayerMustNotBeWindingUp()).Guardian(new SpellMustBeReady(SpellSlot.W)),
                new ECombo().Guardian(new PlayerMustNotBeWindingUp()).Guardian(new SpellMustBeReady(SpellSlot.E))
                                });

            laneParent.Add(new List<Base>
                               {
                new QLane().Guardian(new PlayerMustNotBeWindingUp()).Guardian(new SpellMustBeReady(SpellSlot.Q))
                               });
           
            jungleParent.Add(new List<Base>
            {
                new QJungle().Guardian(new PlayerMustNotBeWindingUp()).Guardian(new SpellMustBeReady(SpellSlot.Q)).Guardian(new SpellMustBeReady(SpellSlot.E) {Negated = true}),
                new EJungle().Guardian(new PlayerMustNotBeWindingUp()).Guardian(new SpellMustBeReady(SpellSlot.E))
            });

            killstealParent.Add(new List<Base>
            {
                new QKillsteal().Guardian(new PlayerMustNotBeWindingUp()).Guardian(new SpellMustBeReady(SpellSlot.Q)),
                new RKillsteal().Guardian(new SpellMustBeReady(SpellSlot.R))  
            });
          
            drawParent.Add(new List<Base>
            {
                new DmgDraw("Damage"), 
                new QDraw("[Q]"),
                new WDraw("[W]"),
                new EDraw("[E]"),
                new RDraw("[R]")    
            });

            superParent.Add(new List<Base> {
                comboParent,
                laneParent,
                jungleParent,
                killstealParent,
                drawParent
           });

            superParent.Load();

            if (superParent.Loaded)
            {
                Game.PrintChat(DisplayName + " - Loaded");
            }
        }
    }
}
