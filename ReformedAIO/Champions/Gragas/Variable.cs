﻿using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;

namespace ReformedAIO.Champions.Gragas
{
    internal class Variable
    {
        public static Orbwalking.Orbwalker Orbwalker { get; internal set; }

        public static Obj_AI_Hero Player => ObjectManager.Player;

        public static Dictionary<SpellSlot, Spell> Spells = new Dictionary<SpellSlot, Spell>()
        {
            {
                SpellSlot.Q, new Spell(SpellSlot.Q, 775f)
            },
            {
                SpellSlot.W, new Spell(SpellSlot.W)
            },
            {
                SpellSlot.E, new Spell(SpellSlot.E, 600f)
            },
            {
                 SpellSlot.R, new Spell(SpellSlot.R, 1050f)
            }
        };
    }
}
