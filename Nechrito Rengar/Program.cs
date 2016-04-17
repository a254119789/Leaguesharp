﻿using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace Nechrito_Rengar
{
    class Program : Logic
    {
       
        private static readonly HpBarIndicator Indicator = new HpBarIndicator();
        private static float lastQ;
        public static readonly int[] BlueSmite = { 3706, 1400, 1401, 1402, 1403 };

        public static readonly int[] RedSmite = { 3715, 1415, 1414, 1413, 1412 };
        private static void Main() => CustomEvents.Game.OnGameLoad += OnGameLoad;
        private static void OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != "Rengar") return;
            Game.PrintChat("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Nechrito Rengar</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\"> Version: 4 (Date: 4/17-16)</font></b>");
            Game.PrintChat("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Update</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\">Triple Q</font></b>");

            MenuConfig.LoadMenu();
            Spells.Initialise();
            Game.OnUpdate += OnTick;
            Obj_AI_Base.OnDoCast += OnDoCast;
            Obj_AI_Base.OnDoCast += OnDoCastLC;
            Drawing.OnDraw += Drawing_OnDraw;
            Drawing.OnEndScene += Drawing_OnEndScene;
       

        }
        private static void OnTick(EventArgs args)
        {
            SmiteJungle();
            SmiteCombo();
            Killsteal._Killsteal();
            switch (MenuConfig._orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Combo.ComboLogic();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    Jungle.JungleLogic();
                    break;
                case Orbwalking.OrbwalkingMode.FastHarass:
                    TripleQ.TripleQLogic();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    Burst.BurstLogic();
                    break;
            }
        }
        private static void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var spellName = args.SData.Name;
            if (!sender.IsMe || !Orbwalking.IsAutoAttack(spellName)) return;

            var hero = args.Target as Obj_AI_Hero;
            if (hero == null) return;
            var target = hero;
            {
                if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
                {
                    if (Spells._e.IsReady())
                    {
                        Spells._e.Cast(target);
                      
                    }
                      
                }
                if (MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Burst) return;
                {
                    var targetR = TargetSelector.GetSelectedTarget();
                    if (targetR != null && targetR.IsValidTarget() && !targetR.IsZombie)
                    {
                        if (Player.Mana == 5 && (Player.Distance(targetR.Position) <= 1000))
                        {
                            CastYoumoo();
                            if (Spells._q.IsReady())
                                Spells._q.Cast(targetR);
                            if (Smite != SpellSlot.Unknown
                          && Player.Spellbook.CanUseSpell(Smite) == SpellState.Ready && !target.IsZombie)
                                Player.Spellbook.CastSpell(Smite, targetR);
                            if (Spells._e.IsReady())
                                Spells._e.Cast(targetR);
                            if (Spells._w.IsReady())
                                Spells._w.Cast(targetR);
                            if (Spells._w.IsReady())
                                Spells._w.Cast(targetR);

                            if (Player.Mana <= 4)
                            {
                                CastYoumoo();
                                if (Spells._e.IsReady())
                                    Spells._e.Cast(targetR);
                                if (Spells._q.IsReady())
                                    Spells._q.Cast(targetR);
                                if (Smite != SpellSlot.Unknown
                               && Player.Spellbook.CanUseSpell(Smite) == SpellState.Ready)
                                    Player.Spellbook.CastSpell(Smite, target);
                                if (Spells._w.IsReady())
                                    Spells._w.Cast(targetR);
                            }
                           else if (Spells._q.IsReady())
                                Spells._q.Cast(target);
                           else if (Spells._e.IsReady())
                                Spells._e.Cast(target);
                           else if (Spells._w.IsReady())
                               Spells._w.Cast(target);
                        }
                    }

                }
            }
        }
        private static void OnDoCastLC(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (args.Target is Obj_AI_Minion)
            {
                if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
                {

                    var minions = MinionManager.GetMinions(Player.ServerPosition, 600f).FirstOrDefault();
                    {
                        if (minions == null || Player.Mana == 5 && MenuConfig.Passive)
                            return;

                        if(Player.Mana == 5)
                        {
                            if (Spells._w.IsReady() && minions.IsValidTarget(Spells._w.Range - 100))
                                Spells._w.Cast(minions);
                            
                            if (Spells._e.IsReady() && Player.Mana < 5 && minions.IsValidTarget(Spells._e.Range))
                                Spells._e.Cast(minions);
                            if (Spells._q.IsReady() && minions.IsValidTarget(Spells._q.Range - 70))
                            {
                                Spells._q.Cast(minions);
                                CastHydra();
                            }   
                        }  
                        if(Player.Mana <= 4)
                        {
                            if (Spells._w.IsReady() && minions.IsValidTarget(Spells._w.Range - 100))
                                Spells._w.Cast(minions);

                            if (Spells._e.IsReady() && Player.Mana < 5 && minions.IsValidTarget(Spells._e.Range))
                                Spells._e.Cast(minions);
                            if (Spells._q.IsReady() && minions.IsValidTarget(Spells._q.Range - 70))
                            {
                                Spells._q.Cast(minions);
                                CastHydra();
                            }
                        }
                    }
                }
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.IsDead)
                return;
            var heropos = Drawing.WorldToScreen(ObjectManager.Player.Position);

            if (MenuConfig.Passive)
            {
                Drawing.DrawText(heropos.X - 15, heropos.Y + 20, System.Drawing.Color.DodgerBlue, "Passive  (     )");
                Drawing.DrawText(heropos.X + 53, heropos.Y + 20,
                    MenuConfig.Passive ? System.Drawing.Color.LimeGreen : System.Drawing.Color.Red, MenuConfig.Passive ? "On" : "Off");
            }

        }
        private static void Drawing_OnEndScene(EventArgs args)
        {
            foreach (
               var enemy in
                   ObjectManager.Get<Obj_AI_Hero>()
                       .Where(ene => ene.IsValidTarget() && !ene.IsZombie))
            {
                if (MenuConfig.dind)
                {
                    var ezkill = Spells._r.IsReady() && Dmg.IsLethal(enemy)
                       ? new ColorBGRA(0, 255, 0, 120)
                       : new ColorBGRA(255, 255, 0, 120);
                    Indicator.unit = enemy;
                    Indicator.drawDmg(Dmg.ComboDmg(enemy), ezkill);
                }
            }
        }
       




    }
}
   

