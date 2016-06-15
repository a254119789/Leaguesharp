﻿#region

using LeagueSharp.Common;

#endregion

namespace Nechrito_Twitch
{
    internal class MenuConfig
    {
        public static Menu Config;
        public static Menu TargetSelectorMenu;
        public static Orbwalking.Orbwalker Orbwalker;
        public static string MenuName = "Nechrito Twitch";

        public static void LoadMenu()
        {
            Config = new Menu(MenuName, MenuName, true);
            TargetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(TargetSelectorMenu);
            Config.AddSubMenu(TargetSelectorMenu);
            var orbwalker = new Menu("Orbwalker", "rorb");
            Orbwalker = new Orbwalking.Orbwalker(orbwalker);
            Config.AddSubMenu(orbwalker);

            var combo = new Menu("Combo", "Combo");

            combo.AddItem(new MenuItem("UseW", "Use W").SetValue(true));
            combo.AddItem(new MenuItem("KsE", "Killsecure E").SetValue(true).SetTooltip("Might interfere with Exploit"));
            Config.AddSubMenu(combo);

            var harass = new Menu("Harass", "Harass");
            harass.AddItem(new MenuItem("harassW", "Use W").SetValue(false));
            harass.AddItem(new MenuItem("ESlider", "E Stack When Out Of AA Range").SetValue(new Slider(4, 0, 6)).SetTooltip("Will E if out of AA range"));
            Config.AddSubMenu(harass);

            var lane = new Menu("Lane", "Lane");
            lane.AddItem(new MenuItem("laneW", "Use W").SetValue(true).SetTooltip("Will only W if 4 minions can be hit"));
            Config.AddSubMenu(lane);

            var steal = new Menu("Steal", "Steal");
            steal.AddItem(new MenuItem("StealEpic", "Dragons & Baron").SetValue(true));
            steal.AddItem(new MenuItem("StealBuff", "Steal Redbuff").SetValue(true));
            Config.AddSubMenu(steal);

            var draw = new Menu("Draw", "Draw");
            draw.AddItem(new MenuItem("dind", "Dmg Indicator").SetValue(true));
            Config.AddSubMenu(draw);

            
            var misc = new Menu("Misc", "Misc");
            misc.AddItem(new MenuItem("QRecall", "QRecall").SetValue(new KeyBind('T', KeyBindType.Press)));
            Config.AddSubMenu(misc);

            var ExploitMenu = new Menu("ExploitMenu", "Exploit");
            ExploitMenu.AddItem(new MenuItem("Exploit", "Exploits").SetValue(false).SetTooltip("Will Instant Q After Kill"));
            ExploitMenu.AddItem(new MenuItem("EAA", "E AA Q").SetValue(false).SetTooltip("Will cast E if killable by E + AA then Q"));
           // ExploitMenu.AddItem(new MenuItem("ExploitMultiplier", "Attack * % Multiplier").SetValue(new Slider(1, 0, 1.5)).SetTooltip("Debugging"));
            Config.AddSubMenu(ExploitMenu);

            Config.AddToMainMenu();
        }

        // Menu Items

        public static bool StealEpic => Config.Item("StealEpic").GetValue<bool>();
        public static bool StealBuff => Config.Item("StealBuff").GetValue<bool>();
        public static bool UseW => Config.Item("UseW").GetValue<bool>();
        public static bool KsE => Config.Item("KsE").GetValue<bool>();
        public static bool LaneW => Config.Item("laneW").GetValue<bool>();
        public static bool HarassW => Config.Item("harassW").GetValue<bool>();
        public static bool Dind => Config.Item("dind").GetValue<bool>();
        public static bool Exploit => Config.Item("Exploit").GetValue<bool>();
        public static bool EAA => Config.Item("EAA").GetValue<bool>();

        public static bool QRecall => Config.Item("QRecall").GetValue<KeyBind>().Active;

        public static int ESlider => Config.Item("ESlider").GetValue<Slider>().Value;
      //  public static int ExploitMultiplier => Config.Item("ExploitMultiplier").GetValue<Slider>().Value;
    }
}
