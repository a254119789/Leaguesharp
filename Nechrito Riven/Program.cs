﻿using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using ItemData = LeagueSharp.Common.Data.ItemData;

namespace NechritoRiven
{
    public class Program
    {
        private const string IsFirstR = "RivenFengShuiEngine";
        private const string IsSecondR = "RivenIzunaBlade";
        public static Menu Menu;
        private static Orbwalking.Orbwalker _orbwalker;
        private static readonly Obj_AI_Hero Player = ObjectManager.Player;
        private static readonly HpBarIndicator Indicator = new HpBarIndicator();
        public static SpellSlot Ignite, Flash;
        private static int _qstack = 1;
        public static Render.Text Timer, Timer2;
        private static bool _forceQ;
        private static bool _forceW;
        private static bool _forceR;
        private static bool _forceR2;
        private static bool _forceItem;
        private static float _lastQ;
        private static float _lastR;
        private static AttackableUnit _qtarget;
      
        public static bool IsLethal(Obj_AI_Base unit)
        {
            return Totaldame(unit) / 1.65 >= unit.Health;
        }

        private static Obj_AI_Base GetCenterMinion()
        {
            var minionposition = MinionManager.GetMinions(300 + Spells._q.Range).Select(x => x.Position.To2D()).ToList();
            var center = MinionManager.GetBestCircularFarmLocation(minionposition, 250, 300 + Spells._q.Range);

            return center.MinionsHit >= 3
                ? MinionManager.GetMinions(1000).OrderBy(x => x.Distance(center.Position)).FirstOrDefault()
                : null;
        }

        private static int Item
            =>
                Items.CanUseItem(3077) && Items.HasItem(3077)
                    ? 3077
                    : Items.CanUseItem(3074) && Items.HasItem(3074) ? 3074 : 0;

        private static int GetWRange => Player.HasBuff("RivenFengShuiEngine") ? 330 : 265;


        private static void Main() => CustomEvents.Game.OnGameLoad += OnGameLoad;

        private static void OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != "Riven") return;
            Game.PrintChat("<b><font color=\"#00e5e5\">Nechrito Riven</font></b>");

            

            MenuConfig.LoadMenu();
            Spells.Initialise();

            Timer =
                new Render.Text(
                    "Q Expiry =>  " + ((double)(_lastQ - Utils.GameTimeTickCount + 3800) / 1000).ToString("0.0"),
                    (int)Drawing.WorldToScreen(Player.Position).X - 140,
                    (int)Drawing.WorldToScreen(Player.Position).Y + 10, 30, Color.DodgerBlue, "calibri");
            Timer2 =
                new Render.Text(
                    "R Expiry =>  " + (((double)_lastR - Utils.GameTimeTickCount + 15000) / 1000).ToString("0.0"),
                    (int)Drawing.WorldToScreen(Player.Position).X - 60,
                    (int)Drawing.WorldToScreen(Player.Position).Y + 10, 30, Color.DodgerBlue, "calibri");

            Game.OnUpdate += OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            Drawing.OnEndScene += Drawing_OnEndScene;
            Obj_AI_Base.OnProcessSpellCast += OnCast;
            Obj_AI_Base.OnDoCast += OnDoCast;
            Obj_AI_Base.OnDoCast += OnDoCastLc;
            Obj_AI_Base.OnPlayAnimation += OnPlay;
            Obj_AI_Base.OnProcessSpellCast += OnCasting;
            Interrupter2.OnInterruptableTarget += Interrupt;
        }

        private static bool HasTitan() => Items.HasItem(3748) && Items.CanUseItem(3748);

        private static void CastTitan()
        {
            if (Items.HasItem(3748) && Items.CanUseItem(3748))
            {
                Items.UseItem(3748);
                Orbwalking.LastAATick = 0;
            }
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            foreach (
                var enemy in
                    ObjectManager.Get<Obj_AI_Hero>()
                        .Where(ene => ene.IsValidTarget() && !ene.IsZombie))
            {
                if (MenuConfig.Dind)
                {
                    Indicator.unit = enemy;
                    Indicator.drawDmg(GetComboDamage(enemy), new ColorBGRA(255, 204, 0, 170));
                }
            }
        }

        private static void OnDoCastLc(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe || !Orbwalking.IsAutoAttack(args.SData.Name)) return;
            _qtarget = (Obj_AI_Base)args.Target;
            if (args.Target is Obj_AI_Minion)
            {
                if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
                {
                    var minions = MinionManager.GetMinions(190 + Player.BoundingRadius);
                    if (minions.Count >= 0)
                    {
                        if (Spells._e.IsReady() && !Orbwalking.InAutoAttackRange(minions[0]) && MenuConfig.LaneE)
                        {
                            Spells._e.Cast(minions[0].Position);
                        }

                        if (Spells._e.IsReady() && Spells._e.IsReady() &&
                            !Player.ServerPosition.Extend(minions[0].ServerPosition, Spells._e.Range).UnderTurret(true) &&
                            MenuConfig.LaneE)
                            Spells._e.Cast(GetCenterMinion().IsValidTarget() ? GetCenterMinion() : minions[0]);


                        if (Spells._q.IsReady() && (Player.Distance(Player.ServerPosition) <= 70 + 120 + Player.BoundingRadius) &&
                            MenuConfig.LaneQ)
                        {
                            ForceItem();
                            Spells._q.Cast(GetCenterMinion().IsValidTarget() ? GetCenterMinion() : minions[1]);
                        }

                        if (Spells._w.IsReady() && minions.Count >= 2 && MenuConfig.LaneW)
                        {
                            ForceItem();
                            Spells._w.Cast(GetCenterMinion());
                        }
                    }
                }
            }
        }

        private static void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var spellName = args.SData.Name;
            if (!sender.IsMe || !Orbwalking.IsAutoAttack(spellName)) return;
            _qtarget = (Obj_AI_Base)args.Target;

            if (args.Target is Obj_AI_Minion)
            {
                if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
                {
                    var mobs = MinionManager.GetMinions(70 + 120 + Player.BoundingRadius, MinionTypes.All,
                        MinionTeam.Neutral, MinionOrderTypes.MaxHealth);
                    if (mobs.Count != 0)
                    {
                        if (Spells._e.IsReady() && !Orbwalking.InAutoAttackRange(mobs[0]))
                        {
                            Spells._e.Cast(mobs[0].Position);
                        }

                        if (HasTitan())
                        {
                            CastTitan();
                            return;
                        }
                        if (Spells._q.IsReady())
                        {
                            ForceItem();
                            Utility.DelayAction.Add(1, () => ForceCastQ(mobs[0]));
                        }
                        if (Spells._w.IsReady() )
                        {
                            ForceItem();
                            Utility.DelayAction.Add(1, ForceW);
                        }

                        else if (Spells._e.IsReady())
                        {
                            Spells._e.Cast(mobs[0].Position);
                        }
                    }
                }
            }
            var @base = args.Target as Obj_AI_Turret;
            if (@base != null)
                if (@base.IsValid && args.Target != null && Spells._q.IsReady() && MenuConfig.LaneQ &&
                    MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear) ForceCastQ(@base);

            var hero = args.Target as Obj_AI_Hero;
            if (hero == null) return;
            var target = hero;

            if (MenuConfig.KillstealR && Spells._r.IsReady() && Spells._r.Instance.Name == IsSecondR)
                if (target.Health < Rdame(target, target.Health) + Player.GetAutoAttackDamage(target) &&
                    target.Health > Player.GetAutoAttackDamage(target)) Spells._r.Cast(target.Position);
            if (MenuConfig.KillstealW && Spells._w.IsReady())
                if (target.Health < Spells._w.GetDamage(target) + Player.GetAutoAttackDamage(target) &&
                    target.Health > Player.GetAutoAttackDamage(target)) Spells._w.Cast();
            if (MenuConfig.KillstealQ && Spells._q.IsReady())
                if (target.Health < Spells._q.GetDamage(target) + Player.GetAutoAttackDamage(target) &&
                    target.Health > Player.GetAutoAttackDamage(target)) Spells._q.Cast(target);

            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                if (HasTitan())
                {
                    CastTitan();
                    return;
                }
                if (Spells._e.IsReady())
                    Spells._e.Cast(target.Position);


                if (Spells._w.IsReady() && InWRange(target))
                    Spells._w.Cast();

                if (Spells._q.IsReady())
                {
                    ForceItem();
                    Utility.DelayAction.Add(1, () => ForceCastQ(target));
                }

                if (Spells._r.IsReady() && _qstack == 2 && Spells._r.Instance.Name == IsSecondR)
                    Spells._r.Cast(target.Position);
            }

            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.FastHarass)
            {
                if (HasTitan())
                {
                    CastTitan();
                    return;
                }
                if (Spells._w.IsReady() && InWRange(target))
                {
                    ForceItem();
                    Utility.DelayAction.Add(1, ForceW);
                    Utility.DelayAction.Add(2, () => ForceCastQ(target));
                }
                else if (Spells._q.IsReady())
                {
                    ForceItem();
                    Utility.DelayAction.Add(1, () => ForceCastQ(target));
                }
                else if (Spells._e.IsReady() && !Orbwalking.InAutoAttackRange(target) && !InWRange(target))
                {
                    Spells._e.Cast(target.Position);
                }
            }

            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
            {
                if (HasTitan())
                {
                    CastTitan();
                    return;
                }
                if (_qstack == 2 && Spells._q.IsReady())
                {
                    ForceItem();
                    Utility.DelayAction.Add(1, () => ForceCastQ(target));
                }
            }

            if (MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Burst) return;

            if (Spells._r.IsReady() && Spells._r.Instance.Name == IsFirstR &&
                  target != null) ForceR();

            if (Spells._e.IsReady() && Player.Distance(target.Position) <= Spells._e.Range + 50)
                Spells._e.Cast(target.Position);

            

            if (Spells._q.IsReady())
            {
                ForceItem();
                Utility.DelayAction.Add(1, () => ForceCastQ(target));
            }

            if (Spells._w.IsReady() && InWRange(target))
                Spells._w.Cast();

            if (Spells._r.IsReady() && Spells._r.Instance.Name == IsSecondR)
                Spells._r.Cast(target.Position);

        }

    

        private static void Interrupt(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (sender.IsEnemy && Spells._w.IsReady() && sender.IsValidTarget() && !sender.IsZombie && MenuConfig.WInterrupt)
            {
                if (sender.IsValidTarget(125 + Player.BoundingRadius + sender.BoundingRadius)) Spells._w.Cast();
            }
        }

        private static void AutoUseW()
        {
            if (MenuConfig.AutoW > 0)
            {
                if (Player.CountEnemiesInRange(GetWRange) >= MenuConfig.AutoW)
                {
                    ForceW();
                }
            }
        }



        private static void OnTick(EventArgs args)
        {
            Timer.X = (int)Drawing.WorldToScreen(Player.Position).X - 60;
            Timer.Y = (int)Drawing.WorldToScreen(Player.Position).Y + 43;
            Timer2.X = (int)Drawing.WorldToScreen(Player.Position).X - 60;
            Timer2.Y = (int)Drawing.WorldToScreen(Player.Position).Y + 65;
            ForceSkill();
            AutoUseW();
            Killsteal();
            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo) Combo();
            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear) Jungleclear();
            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed) Harass();
            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.FastHarass) FastHarass();
            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Burst) Burst();
            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Flee) Flee();
            if (Utils.GameTimeTickCount - _lastQ >= 3650 && _qstack != 1 && !Player.IsRecalling() &&
                !Player.InFountain() && MenuConfig.KeepQ &&
                !Player.Spellbook.IsChanneling &&
                Spells._q.IsReady()) Spells._q.Cast(Game.CursorPos);
        }

        private static void Killsteal()
        {
            if (MenuConfig.KillstealQ && Spells._q.IsReady())
            {
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(Spells._r.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < Spells._q.GetDamage(target) && InQRange(target))
                        Spells._q.Cast(target);
                }
            }
            if (MenuConfig.KillstealW && Spells._w.IsReady())
            {
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(Spells._r.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < Spells._w.GetDamage(target) && InWRange(target))
                        Spells._w.Cast();
                }
            }
            if (MenuConfig.KillstealR && Spells._r.IsReady() && Spells._r.Instance.Name == IsSecondR)
            {
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(Spells._r.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < Rdame(target, target.Health) && !target.HasBuff("kindrednodeathbuff") &&
                        !target.HasBuff("Undying Rage") && !target.HasBuff("JudicatorIntervention"))
                        Spells._r.Cast(target.Position);
                }
            }
        }

    

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.IsDead)
                return;
            var heropos = Drawing.WorldToScreen(ObjectManager.Player.Position);


            if (_qstack != 1 && MenuConfig.DrawTimer1)
            {
                Timer.text = "Q Expiry =>  " + ((double)(_lastQ - Utils.GameTimeTickCount + 3800) / 1000).ToString("0.0") +
                             "s";
                Timer.OnEndScene();
            }

            if (Player.HasBuff("RivenFengShuiEngine") && MenuConfig.DrawTimer2)
            {
                Timer2.text = "R Expiry =>  " +
                              (((double)_lastR - Utils.GameTimeTickCount + 15000) / 1000).ToString("0.0") + "s";
                Timer2.OnEndScene();
            }

            if (MenuConfig.DrawCb)
                Render.Circle.DrawCircle(Player.Position, 250 + Player.AttackRange + 70,
                    Spells._e.IsReady() ? System.Drawing.Color.FromArgb(120, 0, 170, 255) : System.Drawing.Color.IndianRed);
            if (MenuConfig.DrawBt && Flash != SpellSlot.Unknown)
                Render.Circle.DrawCircle(Player.Position, 750,
                    Spells._r.IsReady() && Flash.IsReady()
                        ? System.Drawing.Color.FromArgb(120, 0, 170, 255)
                        : System.Drawing.Color.IndianRed);

            if (MenuConfig.DrawFh)
                Render.Circle.DrawCircle(Player.Position, 450 + Player.AttackRange + 70,
                    Spells._e.IsReady() && Spells._q.IsReady()
                        ? System.Drawing.Color.FromArgb(120, 0, 170, 255)
                        : System.Drawing.Color.IndianRed);
            if (MenuConfig.DrawHs)
                Render.Circle.DrawCircle(Player.Position, 400,
                    Spells._q.IsReady() && Spells._w.IsReady()
                        ? System.Drawing.Color.FromArgb(120, 0, 170, 255)
                        : System.Drawing.Color.IndianRed);
            if (MenuConfig.DrawAlwaysR)
            {
                Drawing.DrawText(heropos.X - 15, heropos.Y + 20, System.Drawing.Color.DodgerBlue, "Use R  (     )");
                Drawing.DrawText(heropos.X + 40, heropos.Y + 20,
                    MenuConfig.AlwaysR ? System.Drawing.Color.LimeGreen : System.Drawing.Color.Red, MenuConfig.AlwaysR ? "On" : "Off");
            }
           
        }

        private static void Jungleclear()
        {
            var mobs = MinionManager.GetMinions(190 + Player.AttackRange + 70, MinionTypes.All, MinionTeam.Neutral,
                MinionOrderTypes.MaxHealth);

            if (mobs.Count <= 0)
                return;

            if (Spells._e.IsReady() && !Orbwalking.InAutoAttackRange(mobs[0]))
            {
                Spells._e.Cast(mobs[0].Position);
            }
        }

        private static void Combo()
        {
            var targetR = TargetSelector.GetTarget(250 + Player.AttackRange + 70, TargetSelector.DamageType.Physical);
            if (MenuConfig.DoIgnite > 0)
            {
                if (targetR.HealthPercent < MenuConfig.DoIgnite && Player.Spellbook.GetSpell(Spells.Ignite).State == SpellState.Ready)
                    Player.Spellbook.CastSpell(Spells.Ignite, targetR);
            }
            if (Spells._r.IsReady() && Spells._r.Instance.Name == IsFirstR  && MenuConfig.AlwaysR &&
                targetR != null) ForceR();
            if (Spells._r.IsReady() && Spells._r.Instance.Name == IsFirstR && Spells._w.IsReady() && InWRange(targetR)  && MenuConfig.AlwaysR &&
                targetR != null)
            {
                ForceR();
                Utility.DelayAction.Add(1, ForceW);
            }

            if (Spells._w.IsReady() && InWRange(targetR)  && targetR != null) Spells._w.Cast();
            if ( Spells._r.IsReady() && Spells._r.Instance.Name == IsFirstR && Spells._w.IsReady() && targetR != null &&
                Spells._e.IsReady() &&
                targetR.IsValidTarget() && !targetR.IsZombie && (IsKillableR(targetR) || MenuConfig.AlwaysR))
            {
                if (!InWRange(targetR))
                {
                    Spells._e.Cast(targetR.Position);
                    ForceR();
                    Utility.DelayAction.Add(20, ForceW);
                    Utility.DelayAction.Add(30, () => ForceCastQ(targetR));
                }
            }
            
            else if ( Spells._w.IsReady() && Spells._e.IsReady())
            {
                if (targetR.IsValidTarget() && targetR != null && !targetR.IsZombie && !InWRange(targetR))
                {
                    Spells._e.Cast(targetR.Position);
                    Utility.DelayAction.Add(100, ForceItem);
                    Utility.DelayAction.Add(200, ForceW);
                    Utility.DelayAction.Add(30, () => ForceCastQ(targetR));
                }
            }
            
            else if (Spells._e.IsReady())
            {
                if (targetR != null && (targetR.IsValidTarget() && !targetR.IsZombie && !InWRange(targetR)))
                {
                    Spells._e.Cast(targetR.Position);
                }
            }
        }

        private static void Burst()
        {
            var target = TargetSelector.GetSelectedTarget();
            if (MenuConfig.DoIgnite > 0)
            {
                if (target.HealthPercent < MenuConfig.DoIgnite && Player.Spellbook.GetSpell(Spells.Ignite).State == SpellState.Ready)
                    Player.Spellbook.CastSpell(Spells.Ignite, target);
            }
            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {
                // Else R wont cast, Bug
                if (Spells._r.IsReady() && Spells._r.Instance.Name == IsFirstR && MenuConfig.AlwaysR && Player.Distance(target.Position) <= Spells._e.Range + (Player.AttackRange) &&
               target != null)
                    ForceR();

                //Flash
                if (MenuConfig.Config.Item("FlashB").GetValue<KeyBind>().Active)
                {
                    if ((Player.Distance(target.Position) <= 750) && (Player.Distance(target.Position) >= 600) &&  Player.Spellbook.GetSpell(Spells.Flash).State == SpellState.Ready && Spells._r.IsReady() && Spells._e.IsReady() && Spells._w.IsReady())
                    {
                        Spells._e.Cast(target.Position);
                        ForceR();
                        CastYoumoo();
                        Utility.DelayAction.Add(100, () => Player.Spellbook.CastSpell(Spells.Flash, target.Position));
                    }
                }
                //SHY | Nechrito
                if (Spells._r.IsReady() && Spells._e.IsReady() && Spells._w.IsReady() && Spells._r.Instance.Name == IsFirstR &&
                        (Player.Distance(target.Position) <= Spells._e.Range + (Player.AttackRange)))
                {
                    Spells._e.Cast(target.ServerPosition);
                    ForceR();
                    CastYoumoo();
                    Utility.DelayAction.Add(10, ForceItem);
                    Utility.DelayAction.Add(10, ForceW);
                    Spells._r.Cast(target.ServerPosition);
                    Utility.DelayAction.Add(30, () => ForceCastQ(target));
                }
            }
        }

        private static void FastHarass()
        {
            if (Spells._q.IsReady() && Spells._e.IsReady())
            {
                var target = TargetSelector.GetTarget(450 + Player.AttackRange + 70, TargetSelector.DamageType.Physical);
                if (target.IsValidTarget() && !target.IsZombie)
                {
                    if (!Orbwalking.InAutoAttackRange(target) && !InWRange(target)) Spells._e.Cast(target.Position);
                    Utility.DelayAction.Add(10, ForceItem);
                    Utility.DelayAction.Add(170, () => ForceCastQ(target));
                }
            }
        }

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(400, TargetSelector.DamageType.Physical);
            if (Spells._q.IsReady() && Spells._w.IsReady() && Spells._e.IsReady() && _qstack == 1)
            {
                if (target.IsValidTarget() && !target.IsZombie)
                {
                    ForceCastQ(target);
                    Utility.DelayAction.Add(1, ForceW);
                }
            }
            if (Spells._q.IsReady() && Spells._e.IsReady() && _qstack == 3 && !Orbwalking.CanAttack() && Orbwalking.CanMove(5))
            {
                var epos = Player.ServerPosition +
                           (Player.ServerPosition - target.ServerPosition).Normalized() * 300;
                Spells._e.Cast(epos);
                Utility.DelayAction.Add(190, () => Spells._q.Cast(epos));
            }
        }

        private static void Flee()
        {
            var enemy =
                HeroManager.Enemies.Where(
                    hero =>
                        hero.IsValidTarget(Player.HasBuff("RivenFengShuiEngine")
                            ? 70 + 195 + Player.BoundingRadius
                            : 70 + 120 + Player.BoundingRadius) && Spells._w.IsReady());
            var x = Player.Position.Extend(Game.CursorPos, 300);
            var objAiHeroes = enemy as Obj_AI_Hero[] ?? enemy.ToArray();
            if (Spells._w.IsReady() && objAiHeroes.Any()) foreach (var target in objAiHeroes) if (InWRange(target)) Spells._w.Cast();
            if (Spells._q.IsReady() && !Player.IsDashing()) Spells._q.Cast(Game.CursorPos);
            if (Spells._e.IsReady() && !Player.IsDashing()) Spells._e.Cast(x);
        }

        private static void OnPlay(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
        {
            if (!sender.IsMe) return;

            switch (args.Animation)
            {
                case "Spell1a":
                    _lastQ = Utils.GameTimeTickCount;
                    if (MenuConfig.Qstrange && MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None)
                    {
                        if (MenuConfig.AnimDance) Game.Say("/d");
                        if (MenuConfig.AnimLaugh) Game.Say("/l");
                        if (MenuConfig.AnimTaunt) Game.Say("/t");
                        if (MenuConfig.AnimTalk)  Game.Say("/j");
                    }
                    _qstack = 2;
                    if (MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None &&
                        MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LastHit &&
                        MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Flee)
                        Utility.DelayAction.Add(MenuConfig.Qd * 10 + 1, Reset);
                    break;
                case "Spell1b":
                    _lastQ = Utils.GameTimeTickCount;
                    if (MenuConfig.Qstrange && MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None)
                    {
                        if (MenuConfig.AnimDance) Game.Say("/d");
                        if (MenuConfig.AnimLaugh) Game.Say("/l");
                        if (MenuConfig.AnimTaunt) Game.Say("/t");
                        if (MenuConfig.AnimTalk)  Game.Say("/j");
                    }
                    _qstack = 3;
                    if (MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None &&
                        MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LastHit &&
                        MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Flee)
                        Utility.DelayAction.Add(MenuConfig.Qd * 10 + 1, Reset);
                    break;
                case "Spell1c":
                    _lastQ = Utils.GameTimeTickCount;
                    if (MenuConfig.Qstrange && MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None)
                    {
                        if (MenuConfig.AnimDance) Game.Say("/d");
                        if (MenuConfig.AnimLaugh) Game.Say("/l");
                        if (MenuConfig.AnimTaunt) Game.Say("/t");
                        if (MenuConfig.AnimTalk)  Game.Say("/j");
                    }
                    _qstack = 1;
                    if (MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None &&
                        MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LastHit &&
                        MenuConfig._orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Flee)
                        Utility.DelayAction.Add(MenuConfig.Qld * 10 + 3, Reset);
                    break;
                case "Spell3":
                    if ((MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Burst ||
                         MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo ||
                         MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.FastHarass ||
                         MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Flee) && MenuConfig.Youmu) CastYoumoo();
                    break;
                case "Spell4a":
                    _lastR = Utils.GameTimeTickCount;
                    break;
                case "Spell4b":
                    var target = TargetSelector.GetSelectedTarget();
                    if (Spells._q.IsReady() && target.IsValidTarget()) ForceCastQ(target);
                    break;
            }
        }

        private static void OnCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;

            if (args.SData.Name.Contains("ItemTiamatCleave")) _forceItem = false;
            if (args.SData.Name.Contains("RivenTriCleave")) _forceQ = false;
            if (args.SData.Name.Contains("RivenMartyr")) _forceW = false;
            if (args.SData.Name == IsFirstR) _forceR = false;
            if (args.SData.Name == IsSecondR) _forceR2 = false;
        }

        private static void Reset()
        {
            if (MenuConfig.QReset) Game.Say("/d");
            Orbwalking.LastAATick = 0;
            Player.IssueOrder(GameObjectOrder.MoveTo,
                 Player.Position.Extend(Game.CursorPos, Player.Distance(Game.CursorPos) + 10));
            
        }
        

        private static int WRange => Player.HasBuff("RivenFengShuiEngine")
            ? 330
            : 265;

        private static bool InWRange(AttackableUnit t) => t != null && t.IsValidTarget(WRange);


        private static bool InQRange(GameObject target)
        {
            return target != null && (Player.HasBuff("RivenFengShuiEngine")
                ? 330 >= Player.Distance(target.Position)
                : 265 >= Player.Distance(target.Position));
        }


        private static void ForceSkill()
        {
            if (_forceQ && _qtarget != null && _qtarget.IsValidTarget(Spells._e.Range + Player.BoundingRadius + 70) &&
                Spells._q.IsReady())
                Spells._q.Cast(_qtarget.Position);
            if (_forceW) Spells._w.Cast();
            if (_forceR && Spells._r.Instance.Name == IsFirstR) Spells._r.Cast();
            if (_forceItem && Items.CanUseItem(Item) && Items.HasItem(Item) && Item != 0) Items.UseItem(Item);
            if (_forceR2 && Spells._r.Instance.Name == IsSecondR)
            {
                var target = TargetSelector.GetSelectedTarget();
                if (target != null) Spells._r.Cast(target.Position);
            }
        }

        private static void ForceItem()
        {
            if (Items.CanUseItem(Item) && Items.HasItem(Item) && Item != 0) _forceItem = true;
            Utility.DelayAction.Add(500, () => _forceItem = false);
        }

        private static void ForceR()
        {
            _forceR = Spells._r.IsReady() && Spells._r.Instance.Name == IsFirstR;
            Utility.DelayAction.Add(500, () => _forceR = false);
        }

        public static void ForceR2()
        {
            _forceR2 = Spells._r.IsReady() && Spells._r.Instance.Name == IsSecondR;
            Utility.DelayAction.Add(500, () => _forceR2 = false);
        }

        private static void ForceW()
        {
            _forceW = Spells._w.IsReady();
            Utility.DelayAction.Add(500, () => _forceW = false);
        }

        private static void ForceCastQ(AttackableUnit target)
        {
            _forceQ = true;
            _qtarget = target;
        }



        private static bool HasItem()
            => ItemData.Tiamat_Melee_Only.GetItem().IsReady() || ItemData.Ravenous_Hydra_Melee_Only.GetItem().IsReady();

        private static void CastYoumoo()
        {
            if (ItemData.Youmuus_Ghostblade.GetItem().IsReady()) ItemData.Youmuus_Ghostblade.GetItem().Cast();
        }

        private static void OnCasting(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsEnemy && sender.Type == Player.Type && MenuConfig.AutoShield)
            {
                var epos = Player.ServerPosition +
                           (Player.ServerPosition - sender.ServerPosition).Normalized() * 300;

                if (Player.Distance(sender.ServerPosition) <= args.SData.CastRange)
                {
                    switch (args.SData.TargettingType)
                    {
                        case SpellDataTargetType.Unit:

                            if (args.Target.NetworkId == Player.NetworkId)
                            {
                                if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LastHit &&
                                    !args.SData.Name.Contains("NasusW"))
                                {
                                    if (Spells._e.IsReady()) Spells._e.Cast(epos);
                                }
                            }

                            break;
                        case SpellDataTargetType.SelfAoe:

                            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LastHit)
                            {
                                if (Spells._e.IsReady()) Spells._e.Cast(epos);
                            }

                            break;
                    }
                    if (args.SData.Name.Contains("IreliaEquilibriumStrike"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._w.IsReady() && InWRange(sender)) Spells._w.Cast();
                            else if (Spells._e.IsReady()) Spells._e.Cast(epos);
                        }
                    }
                    if (args.SData.Name.Contains("TalonCutthroat"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._w.IsReady()) Spells._w.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("RenektonPreExecute"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._w.IsReady()) Spells._w.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("GarenRPreCast"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._e.IsReady()) Spells._e.Cast(epos);
                        }
                    }

                    if (args.SData.Name.Contains("GarenQAttack"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._e.IsReady()) Spells._e.Cast();
                        }
                    }

                    if (args.SData.Name.Contains("XenZhaoThrust3"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._w.IsReady()) Spells._w.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("RengarQ"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._e.IsReady()) Spells._e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("RengarPassiveBuffDash"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._e.IsReady()) Spells._e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("RengarPassiveBuffDashAADummy"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._e.IsReady()) Spells._e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("TwitchEParticle"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._e.IsReady()) Spells._e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("FizzPiercingStrike"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._e.IsReady()) Spells._e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("HungeringStrike"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._e.IsReady()) Spells._e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("YasuoDash"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._e.IsReady()) Spells._e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("KatarinaRTrigger"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._w.IsReady() && InWRange(sender)) Spells._w.Cast();
                            else if (Spells._e.IsReady()) Spells._e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("YasuoDash"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._e.IsReady()) Spells._e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("KatarinaE"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._w.IsReady()) Spells._w.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("MonkeyKingQAttack"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._e.IsReady()) Spells._e.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("MonkeyKingSpinToWin"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                        {
                            if (Spells._e.IsReady()) Spells._e.Cast();
                            else if (Spells._w.IsReady()) Spells._w.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("MonkeyKingQAttack"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                            if (Spells._e.IsReady()) Spells._e.Cast();
                    }
                    if (args.SData.Name.Contains("MonkeyKingQAttack"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                            if (Spells._e.IsReady()) Spells._e.Cast();
                    }
                    if (args.SData.Name.Contains("MonkeyKingQAttack"))
                    {
                        if (args.Target.NetworkId == Player.NetworkId)
                            if (Spells._e.IsReady()) Spells._e.Cast();
                    }
                }
            }
        }


        private static double Basicdmg(Obj_AI_Base target)
        {
            if (target != null)
            {
                double dmg = 0;
                double passivenhan;
                if (Player.Level >= 18)
                    passivenhan = 0.5;
                else if (Player.Level >= 15)
                    passivenhan = 0.45;
                else if (Player.Level >= 12)
                    passivenhan = 0.4;
                else if (Player.Level >= 9)
                    passivenhan = 0.35;
                else if (Player.Level >= 6)
                    passivenhan = 0.3;
                else if (Player.Level >= 3)
                    passivenhan = 0.25;
                else
                    passivenhan = 0.2;
                if (HasItem()) dmg = dmg + Player.GetAutoAttackDamage(target) * 0.7;
                if (Spells._w.IsReady()) dmg = dmg + Spells._w.GetDamage(target);
                if (Spells._q.IsReady())
                {
                    var qnhan = 4 - _qstack;
                    dmg = dmg + Spells._q.GetDamage(target) * qnhan + Player.GetAutoAttackDamage(target) * qnhan * (1 + passivenhan);
                }
                dmg = dmg + Player.GetAutoAttackDamage(target) * (1 + passivenhan);
                return dmg;
            }
            return 0;
        }


        private static float GetComboDamage(Obj_AI_Base enemy)
        {
            if (enemy != null)
            {
                float damage = 0;
                float passivenhan;
                if (Player.Level >= 18)
                    passivenhan = 0.5f;
                else if (Player.Level >= 15)
                    passivenhan = 0.45f;
                else if (Player.Level >= 12)
                    passivenhan = 0.4f;
                else if (Player.Level >= 9)
                    passivenhan = 0.35f;
                else if (Player.Level >= 6)
                    passivenhan = 0.3f;
                else if (Player.Level >= 3)
                    passivenhan = 0.25f;
                else
                    passivenhan = 0.2f;
                if (HasItem()) damage = damage + (float)Player.GetAutoAttackDamage(enemy) * 0.7f;
                if (Spells._w.IsReady()) damage = damage + Spells._w.GetDamage(enemy);
                if (Spells._q.IsReady())
                {
                    var qnhan = 4 - _qstack;
                    damage = damage + Spells._q.GetDamage(enemy) * qnhan +
                             (float)Player.GetAutoAttackDamage(enemy) * qnhan * (1 + passivenhan);
                }
                damage = damage + (float)Player.GetAutoAttackDamage(enemy) * (1 + passivenhan);
                if (Spells._r.IsReady())
                {
                    return damage * 1.2f + Spells._r.GetDamage(enemy);
                }

                return damage;
            }
            return 0;
        }

        public static bool IsKillableR(Obj_AI_Hero target)
        {
            return MenuConfig.RKillable && target.IsValidTarget() && Totaldame(target) >= target.Health &&
                   Basicdmg(target) <= target.Health ||
                   Player.CountEnemiesInRange(900) >= 2 && !target.HasBuff("kindrednodeathbuff") &&
                   !target.HasBuff("Undying Rage") && !target.HasBuff("JudicatorIntervention");
        }


        private static double Totaldame(Obj_AI_Base target)
        {
            if (target == null) return 0;
            double dmg = 0;
            double passivenhan;
            if (Player.Level >= 18)
                passivenhan = 0.5;
            else if (Player.Level >= 15)
                passivenhan = 0.45;
            else if (Player.Level >= 12)
                passivenhan = 0.4;
            else if (Player.Level >= 9)
                passivenhan = 0.35;
            else if (Player.Level >= 6)
                passivenhan = 0.3;
            else if (Player.Level >= 3)
                passivenhan = 0.25;
            else
                passivenhan = 0.2;
            if (HasItem()) dmg = dmg + Player.GetAutoAttackDamage(target) * 0.7;
            if (Spells._w.IsReady()) dmg = dmg + Spells._w.GetDamage(target);
            if (Spells._q.IsReady())
            {
                var qnhan = 4 - _qstack;
                dmg = dmg + Spells._q.GetDamage(target) * qnhan + Player.GetAutoAttackDamage(target) * qnhan * (1 + passivenhan);
            }
            dmg = dmg + Player.GetAutoAttackDamage(target) * (1 + passivenhan);
            if (!Spells._r.IsReady()) return dmg;
            var rdmg = Rdame(target, target.Health - dmg * 1.2);
            return dmg * 1.2 + rdmg;
        }

        private static double Rdame(Obj_AI_Base target, double health)
        {
            if (target != null)
            {
                var missinghealth = (target.MaxHealth - health) / target.MaxHealth > 0.75 ? 0.75 : (target.MaxHealth - health) / target.MaxHealth;
                var pluspercent = missinghealth * (8 / 3);
                var rawdmg = new double[] { 80, 120, 160 }[Spells._r.Level - 1] + 0.6 * Player.FlatPhysicalDamageMod;
                return Player.CalcDamage(target, Damage.DamageType.Physical, rawdmg * (1 + pluspercent));
            }
            return 0;
        }
    }
}