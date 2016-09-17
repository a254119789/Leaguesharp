namespace ReformedAIO.Champions
{
    using LeagueSharp;
    using LeagueSharp.Common;

    internal class Vars
    {
        public static Orbwalking.Orbwalker Orbwalker { get; internal set; }

        public static Obj_AI_Hero Player => ObjectManager.Player;
    }
}
