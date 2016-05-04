﻿using System;
using LeagueSharp.Common;
using LeagueSharp;
using Nechrito_Nidalee.Handlers;
using Nechrito_Nidalee.Drawings;
using Nechrito_Nidalee.Extras;
using SharpDX;
using System.Collections.Generic;
using System.Linq;

namespace Nechrito_Nidalee
{
    class Program : Core
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
        }
        private static void OnLoad(EventArgs args)
        {
            if (Player.ChampionName != "Nidalee")
                return;

            Game.PrintChat("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Nechrito Nidalee</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\"> Good luck and have fun!</font></b>");
            Game.PrintChat("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Update</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\">Release!</font></b>");

            
           // Modes.Flee();
            Drawing.OnEndScene += Drawing_OnEndScene;
            Drawing.OnDraw += DRAWING.Drawing_OnDraw;
            Game.OnUpdate += OnUpdate;
            Champion.Load();
            MenuConfig.Load();
            Item.SmiteCombo();
            Item.SmiteJungle();
        }
        private static void OnUpdate(EventArgs args)
        {
         //   var target = TargetSelector.GetSelectedTarget();
         //   Game.PrintChat("Buffs: {0}", string.Join(" | ", target.Buffs.Where(b => b.Caster.NetworkId == Player.NetworkId).Select(b => b.DisplayName)));
            HealManager.Heal();
            Killsteal.KillSteal();
            Modes.Flee();
            switch (Orb.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Modes.Combo();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    Modes.Lane();
                    Modes.Jungle();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    Modes.Harass();
                    break;
               
            }

        }
        private static readonly HpBarIndicator Indicator = new HpBarIndicator();
        private static void Drawing_OnEndScene(EventArgs args)
        {
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(ene => ene.IsValidTarget() && !ene.IsZombie))
            {
                if (MenuConfig.dind)
                {
                    var EasyKill = Champion.Javelin.IsReady() && Dmg.IsLethal(enemy)
                       ? new ColorBGRA(0, 255, 0, 120)
                       : new ColorBGRA(255, 255, 0, 120);
                    Indicator.unit = enemy;
                    Indicator.drawDmg(Dmg.ComboDmg(enemy), EasyKill);
                }
            }
        }
    }
}
