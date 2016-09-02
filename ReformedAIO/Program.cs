using ReformedAIO.Champions.Caitlyn;

namespace ReformedAIO
{
    #region Using Directives

    using System.Collections.Generic;

    using ReformedAIO.Champions;
    using ReformedAIO.Champions.Ashe;
    using ReformedAIO.Champions.Diana;
    using ReformedAIO.Champions.Gragas;
    using ReformedAIO.Champions.Ryze;
    using RethoughtLib.Bootstraps.Abstract_Classes;

    #endregion

    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            var bootstrap = new Bootstrap(new List<LoadableBase>
            {
                new DianaLoader(), new GragasLoader(), new AsheLoader(), new RyzeLoader(), new CaitlynLoader()
            });

            bootstrap.Run();
        }

        #endregion
    }
}