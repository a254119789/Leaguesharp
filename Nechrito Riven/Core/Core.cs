using LeagueSharp;

namespace NechritoRiven.Core
{
    internal partial class Core
    {
        public static AttackableUnit qTarget;

        public const string IsFirstR = "RivenFengShuiEngine";
        public const string IsSecondR = "RivenIzunaBlade";
       
        public static int Qstack = 1;

        public static Orbwalking.Orbwalker Orbwalker;
        public static Obj_AI_Hero Player => ObjectManager.Player;
    }
}
