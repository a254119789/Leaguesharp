namespace ReformedAIO.Core.Extensions.PlayerInfo
{
    using System;

    using LeagueSharp;

    internal class PlayerInfo
    {
        public float GetBuffEndTime(Obj_AI_Base target, string buffname)
        {
            return Math.Max(0, target.GetBuff(buffname).EndTime) - Game.Time;
        }
    }
}
