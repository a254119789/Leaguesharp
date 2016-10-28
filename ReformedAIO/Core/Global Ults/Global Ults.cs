namespace ReformedAIO.Core.Global_Ults
{
    using LeagueSharp;
    using LeagueSharp.Common;

    internal sealed class GlobalUlts
    {
        public float TravelTime(Obj_AI_Base target, float delay, float speed)
        {
            return ObjectManager.Player.Distance(target.Position) / (speed + delay);
        }


    }
}
