namespace ReformedAIO.Champions.Lucian
{
    using System.Collections.Generic;
    using System.Drawing;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Lucian.Core.Spells;
    using ReformedAIO.Champions.Lucian.Core.Spells.SpellParent;
    using ReformedAIO.Champions.Lucian.Logic.Damage;
    
    using OrbwalkingMode.Combo;

    using ReformedAIO.Champions.Lucian.Drawings;

    using RethoughtLib.FeatureSystem.Guardians;
    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Implementations;

    using Color = SharpDX.Color;

    class LucianLoader : LoadableBase
    {
        public override string DisplayName { get; set; } = "Reformed Lucian";

        public override string InternalName { get; set; } = "Lucian";

        public override IEnumerable<string> Tags { get; set; } = new[] {"Lucian"};

        public override void Load()
        {
            var superParent = new SuperParent(DisplayName);
            superParent.Initialize();

            var qSpell = new QSpell();
            var q2Spell = new Q2Spell();
            var wSpell = new WSpell();
            var eSpell = new ESpell();
            var rSpell = new RSpell();

            var spellParent = new SpellParent();
            spellParent.Add(new List<Base> { qSpell, q2Spell, wSpell, eSpell, rSpell });
            spellParent.Load();

            var dmg = new LucDamage(eSpell, wSpell, qSpell, rSpell);
           
         
            var orbwalker = new Orbwalking.Orbwalker(superParent.Menu.SubMenu("Orbwalker"));

            var comboParent = new OrbwalkingParent("Combo", orbwalker, Orbwalking.OrbwalkingMode.Combo);
            var laneParent = new OrbwalkingParent("Lane", orbwalker, Orbwalking.OrbwalkingMode.LaneClear);
            var jungleParent = new OrbwalkingParent("Jungle", orbwalker, Orbwalking.OrbwalkingMode.LaneClear);
            var drawingParent = new Parent("Drawings");

            comboParent.Add(new List<Base>()
                                {
                                    new QCombo(qSpell, q2Spell, orbwalker),
                                    new WCombo(wSpell, orbwalker, dmg),
                                    new ECombo(eSpell, orbwalker, dmg),
                                    new RCombo(rSpell, orbwalker, dmg)
                                });

            drawingParent.Add(new List<Base>()
                                  {
                                      new QDraw("Q", qSpell)
                                  });

            superParent.Add(new List<Base>()
                                {
                                    comboParent, drawingParent
                                });

            superParent.Load();

            superParent.Menu.Style = FontStyle.Bold;
            superParent.Menu.Color = Color.Cyan;

            if (superParent.Loaded)
            {
                Game.PrintChat(DisplayName + " - Loaded");
            }
        }
    }
}
