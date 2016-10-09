namespace NechritoRiven.Riven.Logic.SpellParent
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;

    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Implementations;

    internal class SpellParent : ParentBase
    {
        public SpellParent()
        {
            Spells = new Dictionary<SpellSlot, SpellChild>();
        }

        public override string Name { get; set; } = "Spells";

        public Dictionary<SpellSlot, SpellChild> Spells { get; set; }

        public SpellChild this[SpellSlot spellSlot]
        {
            get
            {
                var spellChild = Spells[spellSlot];

                if (spellChild == null)
                {
                    throw new InvalidOperationException("Can't return a SpellChild for a SpellSlot that is non-existing");
                }

                return spellChild;
            }
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            foreach (var spell in this.Children.OfType<SpellChild>())
            {
                if (spell.Spell == null) return;

                this.Spells[spell.Spell.Slot] = spell;
            }
        }
    }
}
