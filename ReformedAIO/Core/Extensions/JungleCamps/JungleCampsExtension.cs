﻿namespace ReformedAIO.Core.Extensions.JungleCamps
{
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.SDK;

    internal class JungleCampsExtension
    {
        public List<Obj_AI_Minion> GetSmallJungle(float range)
        {
            return GameObjects.JungleSmall.Where(m => m.IsValidTarget(range)).ToList();
        }

        public List<Obj_AI_Minion> GetLargeJungle(float range)
        {
            return GameObjects.JungleLarge.Where(m => m.IsValidTarget(range)).ToList();
        }

        public List<Obj_AI_Minion> GetLegendaryJungle(float range)
        {
            return GameObjects.JungleSmall.Where(m => m.IsValidTarget(range)).ToList();
        }
    }
}
