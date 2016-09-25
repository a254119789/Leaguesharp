﻿namespace NechritoRiven.Riven.Logic.SpellParent
{
    using System.Collections.Generic;

    using LeagueSharp;

    using RethoughtLib.FeatureSystem.Implementations;

    internal interface ISpellIndex
    {
        Dictionary<SpellSlot, SpellChild> Spells { get; set; }

        SpellChild this[SpellSlot spellSlot] { get; }
    }
}
