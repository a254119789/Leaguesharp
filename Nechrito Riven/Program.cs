namespace NechritoRiven
{
    #region

    using NechritoRiven.Riven;

    using RethoughtLib;
    using RethoughtLib.Bootstraps.Implementations;

    #endregion

    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            var bootstrap = new LeagueSharpMultiBootstrap(new[] { new RivenLoader() });

            bootstrap.Run();
        }

        #endregion
    }
}