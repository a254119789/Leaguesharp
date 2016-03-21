﻿
using LeagueSharp;
using LeagueSharp.Common;

namespace NechritoRiven
{
    class MenuConfig
    {
        public static Menu Config;
        public static Menu TargetSelectorMenu;
        public static Orbwalking.Orbwalker _orbwalker;

        public static string menuName = "Nechrito Riven";
        public static void LoadMenu()
        {
            Config = new Menu(menuName, menuName, true);

            TargetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(TargetSelectorMenu);
            Config.AddSubMenu(TargetSelectorMenu);


            var orbwalker = new Menu("Orbwalker", "rorb");
            _orbwalker = new Orbwalking.Orbwalker(orbwalker);
            Config.AddSubMenu(orbwalker);


            var animation = new Menu("Animation", "Animation");
            animation.AddItem(new MenuItem("LegitQ", "All off = Legit"));
            animation.AddItem(new MenuItem("qReset", "Fast & Legit Q").SetValue(true));
            animation.AddItem(new MenuItem("Qstrange", "Animation | Enables Below").SetValue(false));
            animation.AddItem(new MenuItem("animLaugh", "Laugh | Not Legit").SetValue(false));
            animation.AddItem(new MenuItem("animTaunt", "Taunt | Not Legit").SetValue(false));
            animation.AddItem(new MenuItem("animTalk", "Joke | Not Legit").SetValue(false));
            animation.AddItem(new MenuItem("animDance", "Dance | Not Legit").SetValue(false));
            Config.AddSubMenu(animation);

            var combo = new Menu("Combo", "Combo");

            combo.AddItem(new MenuItem("AlwaysR", "Use R").SetValue(new KeyBind('G', KeyBindType.Toggle)));
            combo.AddItem(new MenuItem("DoIgnite", "Ignite %").SetValue(new Slider(0, 0, 50))); ;
            combo.AddItem(new MenuItem("RKillable", "Smart R").SetValue(true));
            Config.AddSubMenu(combo);

            var burst = new Menu("Burst", "Burst");
            burst.AddItem(new MenuItem("FlashB", "Flash").SetValue(new KeyBind('L', KeyBindType.Toggle)));
            Config.AddSubMenu(burst);

            var lane = new Menu("Lane", "Lane");
            lane.AddItem(new MenuItem("LaneQ", "Use Q").SetValue(true));
            lane.AddItem(new MenuItem("LaneW", "Use W").SetValue(true));
            lane.AddItem(new MenuItem("LaneE", "Use E").SetValue(true));
            Config.AddSubMenu(lane);

            var killsteal = new Menu("Killsteal", "Killsteal");
            killsteal.AddItem(new MenuItem("killstealq", "Killsteal Q").SetValue(true));
            killsteal.AddItem(new MenuItem("killstealw", "Killsteal W").SetValue(true));
            killsteal.AddItem(new MenuItem("killstealr", "Killsteal Second R").SetValue(true));
            Config.AddSubMenu(killsteal);

            var misc = new Menu("Misc", "Misc");
            misc.AddItem(new MenuItem("youmu", "Auto Yomuu's").SetValue(true));
            misc.AddItem(new MenuItem("AutoShield", "Auto Cast E").SetValue(true));
            misc.AddItem(new MenuItem("AutoW", "Auto W When x Enemy").SetValue(new Slider(5, 0, 5)));
            misc.AddItem(new MenuItem("Winterrupt", "W interrupt").SetValue(true));
            misc.AddItem(new MenuItem("KeepQ", "Keep Q Alive").SetValue(true));
            misc.AddItem(new MenuItem("QD", "Q1, Q2 Delay").SetValue(new Slider(29, 23, 43)));
            misc.AddItem(new MenuItem("QLD", "Q3 Delay").SetValue(new Slider(39, 36, 53)));
            Config.AddSubMenu(misc);

            var draw = new Menu("Draw", "Draw");
            draw.AddItem(new MenuItem("Dind", "Damage Indicator").SetValue(true));
            draw.AddItem(new MenuItem("DrawAlwaysR", "R Status").SetValue(true));
            draw.AddItem(new MenuItem("DrawTimer1", "Draw Q Expiry Time").SetValue(false));
            draw.AddItem(new MenuItem("DrawTimer2", "Draw R Expiry Time").SetValue(false));
            draw.AddItem(new MenuItem("DrawCB", "Combo Engage").SetValue(false));
            draw.AddItem(new MenuItem("DrawBT", "Burst Engage").SetValue(false));
            draw.AddItem(new MenuItem("DrawFH", "FastHarass Engage").SetValue(false));
            draw.AddItem(new MenuItem("DrawHS", "Harass Engage").SetValue(false));
            Config.AddSubMenu(draw);

            var credit = new Menu("Credit", "Credit");
            credit.AddItem(new MenuItem("hoola", "Made by Hoola, Re-written By Nechrito"));
            Config.AddSubMenu(credit);

            Config.AddToMainMenu();
        }

        public static int DoIgnite => Config.Item("DoIgnite").GetValue<Slider>().Value;
        public static bool QReset => Config.Item("qReset").GetValue<bool>();
        public static bool Dind => Config.Item("Dind").GetValue<bool>();
        public static bool DrawCb => Config.Item("DrawCB").GetValue<bool>();
        public static bool KillstealW => Config.Item("killstealw").GetValue<bool>();

        public static bool KillstealQ => Config.Item("killstealq").GetValue<bool>();
        public static bool AnimLaugh => Config.Item("animLaugh").GetValue<bool>();
        public static bool AnimTaunt => Config.Item("animTaunt").GetValue<bool>();
        public static bool AnimDance => Config.Item("animDance").GetValue<bool>();
        public static bool AnimTalk => Config.Item("animTalk").GetValue<bool>();
        public static bool KillstealR => Config.Item("killstealr").GetValue<bool>();

        public static bool DrawAlwaysR => Config.Item("DrawAlwaysR").GetValue<bool>();
        
        public static bool KeepQ => Config.Item("KeepQ").GetValue<bool>();


        public static bool DrawFh => Config.Item("DrawFH").GetValue<bool>();
        public static bool DrawTimer1 => Config.Item("DrawTimer1").GetValue<bool>();
        public static bool DrawTimer2 => Config.Item("DrawTimer2").GetValue<bool>();
        public static bool DrawHs => Config.Item("DrawHS").GetValue<bool>();
        public static bool DrawBt => Config.Item("DrawBT").GetValue<bool>();

        public static bool AlwaysR => Config.Item("AlwaysR").GetValue<KeyBind>().Active;
        public static bool AutoShield => Config.Item("AutoShield").GetValue<bool>();
        public static int Qd => Config.Item("QD").GetValue<Slider>().Value;
        public static int Qld => Config.Item("QLD").GetValue<Slider>().Value;
        public static int AutoW => Config.Item("AutoW").GetValue<Slider>().Value;


        public static bool RKillable => Config.Item("RKillable").GetValue<bool>();
        public static bool LaneW => Config.Item("LaneW").GetValue<bool>();
        public static bool LaneE => Config.Item("LaneE").GetValue<bool>();
        public static bool WInterrupt => Config.Item("WInterrupt").GetValue<bool>();
        public static bool Qstrange => Config.Item("Qstrange").GetValue<bool>();

        public static bool LaneQ => Config.Item("LaneQ").GetValue<bool>();
        public static bool Youmu => Config.Item("youmu").GetValue<bool>();



    }
}