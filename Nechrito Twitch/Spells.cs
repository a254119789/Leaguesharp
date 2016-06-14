﻿using LeagueSharp.Common;
using LeagueSharp;

namespace Nechrito_Twitch
{
    class Spells
    {
        private static Obj_AI_Hero Player = ObjectManager.Player;
        public static SpellSlot _ignite, _recall;
       
        public static Spell _q { get; set; }
        public static Spell _w { get; set; }
        public static Spell _e { get; set; }
        public static Spell _r { get; set; }

        public static void Initialise()
        {
            _q = new Spell(SpellSlot.Q);
            _w = new Spell(SpellSlot.W, 950);
            _w.SetSkillshot(0.25f, 120f, 1400f, false, SkillshotType.SkillshotCircle);
            _e = new Spell(SpellSlot.E, 1200);
            _r = new Spell(SpellSlot.R, 900);
        }
    }
}
