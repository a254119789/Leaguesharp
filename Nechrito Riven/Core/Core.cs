namespace NechritoRiven.Core
{
    #region

    using LeagueSharp;

    #endregion

    internal partial class Core
    {
        #region Constants

        public const string IsFirstR = "RivenFengShuiEngine";

        public const string IsSecondR = "RivenIzunaBlade";

        #endregion

        #region Static Fields

        public static Orbwalking.Orbwalker Orbwalker;

        public static int Qstack = 1;

        public static AttackableUnit qTarget;

        #endregion

        #region Public Properties

        public static Obj_AI_Hero Player => ObjectManager.Player;

        #endregion
    }
}