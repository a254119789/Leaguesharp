using ReformedAIO.Champions.Caitlyn;
using ReformedAIO.Champions.Gnar;

namespace ReformedAIO
{
    #region Using Directives

    using System.Collections.Generic;

    using Champions;
    using Champions.Ashe;
    using Champions.Diana;
    using Champions.Gragas;
    using Champions.Ryze;
    using RethoughtLib.Bootstraps.Abstract_Classes;

    #endregion

    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            var bootstrap = new Bootstrap(new List<LoadableBase>
            {
                new DianaLoader(), new GragasLoader(), new AsheLoader(), new RyzeLoader(), new CaitlynLoader(), new GnarLoader()
            });

            bootstrap.Run();
        }

        #endregion
    }
}