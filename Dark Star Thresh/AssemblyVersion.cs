namespace Dark_Star_Thresh
{
    using System;
    using System.Net;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using LeagueSharp;

    internal class AssemblyVersion
    {
        private static string DownloadServerVersion
        {
            get
            {
                using (var wC = new WebClient()) return wC.DownloadString("https://raw.githubusercontent.com/Nechrito/Leaguesharp/master/Dark%20Star%20Thresh/Properties/AssemblyInfo.cs");
            }
        }

        public static void CheckVersion()
        {
            try
            {
                var match = new Regex(@"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]").Match(DownloadServerVersion);

                if (!match.Success) return;
                var gitVersion = new Version($"{match.Groups[1]}.{match.Groups[2]}.{match.Groups[3]}.{match.Groups[4]}");

                if (gitVersion <= Assembly.GetExecutingAssembly().GetName().Version) return;
                Game.PrintChat("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\"> Dark Star Thresh</font></b><b><font color=\"#FFFFFF\">]</font></b> <font color=\"#FFFFFF\">Version:</font>{0} Available!", gitVersion);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Game.PrintChat("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\"> Dark Star Thresh</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\"> Unable to fetch latest version</font></b>");
            }
        }
    }
}