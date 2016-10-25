﻿namespace ReformedAIO
{
    #region Using Directives

    using System.Collections.Generic;

  //  using ReformedAIO.Champions;
    using ReformedAIO.Champions.Ashe;
    using ReformedAIO.Champions.Caitlyn;
    using ReformedAIO.Champions.Diana;
    using ReformedAIO.Champions.Gnar;
    using ReformedAIO.Champions.Gragas;
    using ReformedAIO.Champions.Lucian;
    using ReformedAIO.Utilities;

    using RethoughtLib.Bootstraps.Abstract_Classes;

    #endregion

    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            var bootstrap = new Bootstrap(new List<LoadableBase>
            {
                new AsheLoader(),
                new CaitlynLoader(),
                new DianaLoader(),
                new LucianLoader(),
                new GragasLoader(),
                new GnarLoader(),


                new ReformedUtlity()
            },
            new List<string>() {"Reformed Utility"});

          //  bootstrap.AddString("Reformed Utility");

            bootstrap.Run();
        }

        #endregion
    }
}