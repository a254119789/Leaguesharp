namespace NechritoRiven.Riven
{
    using System.Collections.Generic;
    using System.Drawing;

    using Active.Active_Logic;
    using Active.Animation;
    using Active.OrbwalkingMode.Combo;

    using Logic.SpellParent;
    using Logic.Spells;

    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Guardians;
    using RethoughtLib.FeatureSystem.Implementations;

    using Color = SharpDX.Color;
    using Orbwalking = Orbwalking;
    using OrbwalkingParent = OrbwalkingParent;

    internal class RivenLoader : LoadableBase
    {
        public override string DisplayName { get; set; } = "Nechrito Riven";

        public override string InternalName { get; set; } = "NechritoRiven";

        public override IEnumerable<string> Tags { get; set; } = new[] { "Riven" };

        public override void Load()
        {
            var superParent = new SuperParent(DisplayName);

            var orbwalkingModule = new OrbwalkerModule();
            orbwalkingModule.Load();

            var spellParent = new SpellParent();

            var rivenQ = new RivenQ();
            var rivenW = new RivenW();
            var rivenE = new RivenE();
            var rivenR = new RivenR();

            spellParent.Add(new List<Base> { rivenQ, rivenW, rivenE, rivenR });

            var comboParent = new OrbwalkingParent("Combo", orbwalkingModule.OrbwalkerInstance, NechritoRiven.Orbwalking.OrbwalkingMode.Combo);

            var logicParent = new Parent("Logic");

            comboParent.Add(new List<Base>
                                {
                                    new Combo(rivenQ, rivenW, rivenE, rivenR).Guardian(new PlayerMustNotBeWindingUp()), 
                                    new ActiveLogic(), 
                                });

            logicParent.Add(new ChildBase[]
                                {
                                    new Animation(rivenQ, orbwalkingModule.OrbwalkerInstance), 
                                });

            superParent.Add(new List<Base> { orbwalkingModule, comboParent, logicParent });

            spellParent.Load();

            superParent.Load();

            superParent.Menu.Style = FontStyle.Bold;
            superParent.Menu.Color = Color.Cyan;
        }

        public Orbwalking.Orbwalker Orbwalking { get; set; }
    }
}
