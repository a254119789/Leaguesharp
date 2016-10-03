namespace ReformedAIO.Champions.Lucian
{
    using System.Collections.Generic;
    using System.Drawing;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Core.Spells;
    using Core.Spells.SpellParent;

    using OrbwalkingMode.Combo;

    using Lucian.Drawings;
    using Lucian.OrbwalkingMode.LaneClear;

    using ReformedAIO.Champions.Lucian.Core.Damage;
    using ReformedAIO.Champions.Lucian.OrbwalkingMode.JungleClear;

    using RethoughtLib.FeatureSystem.Guardians;
    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Implementations;

    using Color = SharpDX.Color;

    internal sealed class LucianLoader : LoadableBase
    {
        public override string DisplayName { get; set; } = "Reformed Lucian";

        public override string InternalName { get; set; } = "Lucian";

        public override IEnumerable<string> Tags { get; set; } = new[] { "Lucian" };

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
            spellParent.Add(new List<Base>
                                  {
                                     qSpell,
                                     q2Spell,
                                     wSpell,
                                     eSpell,
                                     rSpell
                                  });
            spellParent.Load();

            var dmg = new LucDamage(eSpell, wSpell, qSpell, rSpell);
            
            var orbwalker = new Orbwalking.Orbwalker(superParent.Menu.SubMenu("Orbwalker"));

            var comboParent = new OrbwalkingParent("Combo", orbwalker, Orbwalking.OrbwalkingMode.Combo);
            var harassParent = new OrbwalkingParent("Harass", orbwalker, Orbwalking.OrbwalkingMode.Mixed);
            var laneParent = new OrbwalkingParent("Lane", orbwalker, Orbwalking.OrbwalkingMode.LaneClear);
            var jungleParent = new OrbwalkingParent("Jungle", orbwalker, Orbwalking.OrbwalkingMode.LaneClear);
            var drawingParent = new Parent("Drawings");

            comboParent.Add(new List<Base>
                                {
                                    new QCombo(qSpell, q2Spell).Guardian(new PlayerMustNotBeWindingUp()).Guardian(new SpellMustBeReady(SpellSlot.E) {Negated = true}),
                                    new WCombo(wSpell).Guardian(new PlayerMustNotBeWindingUp()).Guardian(new SpellMustBeReady(SpellSlot.Q) {Negated = true}),
                                    new ECombo(eSpell, orbwalker, dmg).Guardian(new PlayerMustNotBeWindingUp()),
                                    new RCombo(rSpell, dmg).Guardian(new PlayerMustNotBeWindingUp()),
                                 });

            harassParent.Add(new List<Base>
                                 {
                                     // Because why the fuck not
                                     new QCombo(qSpell, q2Spell).Guardian(new PlayerMustNotBeWindingUp()), 
                                     new WCombo(wSpell).Guardian(new PlayerMustNotBeWindingUp()),
                                     new ECombo(eSpell, orbwalker, dmg).Guardian(new PlayerMustNotBeWindingUp())
                                 });

            laneParent.Add(new List<Base>
                               {
                                   new QLaneClear(qSpell, orbwalker),
                                   new WLaneClear(wSpell, orbwalker),
                                   new ELaneClear(eSpell, orbwalker)
                               });

            jungleParent.Add(new List<Base>
                                 {
                                     new QJungleClear(qSpell, orbwalker).Guardian(new PlayerMustNotBeWindingUp()).Guardian(new SpellMustBeReady(SpellSlot.E) {Negated = true}),
                                     new WJungleClear(wSpell, orbwalker).Guardian(new PlayerMustNotBeWindingUp()),
                                     new EJungleClear(eSpell, orbwalker).Guardian(new PlayerMustNotBeWindingUp())
                                 });

            drawingParent.Add(new List<Base>
                                  {
                                   new DmgDraw(dmg),
                                   new RDraw(rSpell),
                                   new WDraw(wSpell),
                                   new QDraw(qSpell),

                                  });

            superParent.Add(new List<Base>
                                  {
                                   comboParent,
                                   harassParent,
                                   laneParent,
                                   jungleParent,
                                   drawingParent
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
