﻿using LeagueSharp.Common;

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
            var emoteMenu = new Menu("Emote", "Emote");
            animation.AddItem(new MenuItem("qReset", "Fast & Legit Q").SetValue(true)).SetTooltip("Uses dance in animation, off will not dance & look more legit");
            emoteMenu.AddItem(new MenuItem("Qstrange", "Enable").SetValue(false)).SetTooltip("Enables Emote");
            emoteMenu.AddItem(new MenuItem("animLaugh", "Laugh").SetValue(false));
            emoteMenu.AddItem(new MenuItem("animTaunt", "Taunt").SetValue(false));
            emoteMenu.AddItem(new MenuItem("animTalk", "Joke").SetValue(false));
            emoteMenu.AddItem(new MenuItem("animDance", "Dance").SetValue(false));
            animation.AddSubMenu(emoteMenu);
            Config.AddSubMenu(animation);

            /*
            var burst = new Menu("Burst", "burst");
            burst.AddItem(new MenuItem("OneShot", "OneShot Burst").SetValue(true)).SetTooltip("E R1 Tiamat AA W Q AA R2 Q");
            Config.AddSubMenu(burst);
            */

            var combo = new Menu("Combo", "Combo");
            combo.AddItem(new MenuItem("ignite", "Auto Ignite").SetValue(true)).SetTooltip("Auto Ignite When target is killable");
            combo.AddItem(new MenuItem("AlwaysR", "Force R").SetValue(new KeyBind('G', KeyBindType.Toggle))).SetTooltip("Off will only use R when target is killable");
            combo.AddItem(new MenuItem("AlwaysF", "Force Flash").SetValue(new KeyBind('L', KeyBindType.Toggle))).SetTooltip("Off Will only use R when target is killable");
            Config.AddSubMenu(combo);

            var lane = new Menu("Lane", "Lane");
            lane.AddItem(new MenuItem("LaneQ", "Use Q").SetValue(true));
            lane.AddItem(new MenuItem("LaneW", "Use W").SetValue(true));
            lane.AddItem(new MenuItem("LaneE", "Use E").SetValue(true));
            Config.AddSubMenu(lane);

            var misc = new Menu("Misc", "Misc");
            misc.AddItem(new MenuItem("KeepQ", "Keep Q Alive").SetValue(true));
            misc.AddItem(new MenuItem("QD", "Q1, Q2 Delay").SetValue(new Slider(29, 23, 43)));
            misc.AddItem(new MenuItem("QLD", "Q3 Delay").SetValue(new Slider(39, 36, 53)));
            Config.AddSubMenu(misc);

            var draw = new Menu("Draw", "Draw");
            draw.AddItem(new MenuItem("Dind", "Damage Indicator").SetValue(true));
            draw.AddItem(new MenuItem("DrawForceFlash", "Flash Status").SetValue(true));
            draw.AddItem(new MenuItem("DrawAlwaysR", "R Status").SetValue(true));
            draw.AddItem(new MenuItem("DrawTimer1", "Draw Q Expiry Time").SetValue(false));
            draw.AddItem(new MenuItem("DrawTimer2", "Draw R Expiry Time").SetValue(false));
            draw.AddItem(new MenuItem("DrawCB", "Combo Engage").SetValue(false));
            draw.AddItem(new MenuItem("DrawBT", "Burst Engage").SetValue(false));
            draw.AddItem(new MenuItem("DrawFH", "FastHarass Engage").SetValue(false));
            draw.AddItem(new MenuItem("DrawHS", "Harass Engage").SetValue(false));
            Config.AddSubMenu(draw);

            var skin = new Menu("SkinChanger", "SkinChanger");
            skin.AddItem(new MenuItem("UseSkin", "Use SkinChanger").SetValue(false)).SetTooltip("Toggles Skinchanger");
            skin.AddItem(new MenuItem("Skin", "Skin").SetValue(new StringList(new[] { "Default", "Redeemed", "Crimson Elite", "Battle Bunny", "Championship", "Dragonblade", "Arcade" })));
            Config.AddSubMenu(skin);

            var credit = new Menu("Credit", "Credit");
            credit.AddItem(new MenuItem("nechrito", "Originally made by Hoola, completely re-written By Nechrito"));
            Config.AddSubMenu(credit);

            Config.AddToMainMenu();
        }
        public static bool UseSkin => Config.Item("UseSkin").GetValue<bool>();
        public static bool AlwaysF => Config.Item("AlwaysF").GetValue<KeyBind>().Active;
        public static bool ignite => Config.Item("ignite").GetValue<bool>();
        public static bool ForceFlash => Config.Item("DrawForceFlash").GetValue<bool>();
        public static bool IreliaLogic=> Config.Item("IreliaLogic").GetValue<bool>();
        public static bool QReset => Config.Item("qReset").GetValue<bool>();
        public static bool Dind => Config.Item("Dind").GetValue<bool>();
        public static bool DrawCb => Config.Item("DrawCB").GetValue<bool>();
        public static bool AnimLaugh => Config.Item("animLaugh").GetValue<bool>();
        public static bool AnimTaunt => Config.Item("animTaunt").GetValue<bool>();
        public static bool AnimDance => Config.Item("animDance").GetValue<bool>();
        public static bool AnimTalk => Config.Item("animTalk").GetValue<bool>();
        public static bool DrawAlwaysR => Config.Item("DrawAlwaysR").GetValue<bool>();
        public static bool KeepQ => Config.Item("KeepQ").GetValue<bool>();
        public static bool DrawFh => Config.Item("DrawFH").GetValue<bool>();
        public static bool DrawTimer1 => Config.Item("DrawTimer1").GetValue<bool>();
        public static bool DrawTimer2 => Config.Item("DrawTimer2").GetValue<bool>();
        public static bool DrawHs => Config.Item("DrawHS").GetValue<bool>();
        public static bool DrawBt => Config.Item("DrawBT").GetValue<bool>();
        public static bool AlwaysR => Config.Item("AlwaysR").GetValue<KeyBind>().Active;
        public static int Qd => Config.Item("QD").GetValue<Slider>().Value;
        public static int Qld => Config.Item("QLD").GetValue<Slider>().Value;
        public static bool LaneW => Config.Item("LaneW").GetValue<bool>();
        public static bool LaneE => Config.Item("LaneE").GetValue<bool>();
        public static bool Qstrange => Config.Item("Qstrange").GetValue<bool>();
        public static bool LaneQ => Config.Item("LaneQ").GetValue<bool>();



    }
}