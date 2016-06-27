﻿using LeagueSharp.Common;
using LeagueSharp;
using SPrediction;
using System;
using SharpDX;

namespace Nechrito_Gragas
{
    class Mode
    {
        private static Obj_AI_Hero Player => ObjectManager.Player;

        public static Vector3 rpred(Obj_AI_Hero Target)
        {
            var pos = Spells.R.GetVectorSPrediction(Target, -50).CastTargetPosition;

            if (Target != null && !pos.IsWall())
            {
                if (Target.IsFacing(Player))
                {
                    if (Target.IsMoving)
                    {
                        pos = pos.Extend(Player.Position.To2D(), -90);
                    }
                    pos = pos.Extend(Player.Position.To2D(), -100);
                }

                if (!Target.IsFacing(Player))
                {
                    if (Target.IsMoving)
                    {
                        pos = pos.Extend(Player.Position.To2D(), -145);
                    }
                    pos = pos.Extend(Player.Position.To2D(), -130);
                }
            }
            return pos.To3D2();
        }

        public static Vector3 qpred(Obj_AI_Hero Target)
        {
            var pos = Spells.Q.GetVectorSPrediction(Target, 50).CastTargetPosition;

            pos = pos.Extend(Player.Position.To2D(), + Spells.R.Range);

            if (Target != null && !pos.IsWall())
            {
                if (Target.IsFacing(Player))
                {
                    if (Target.IsMoving)
                    {
                        pos = pos.Extend(Player.Position.To2D(), 90);
                    }
                    pos = pos.Extend(Player.Position.To2D(), 100);
                }

                if (!Target.IsFacing(Player))
                {
                    if (Target.IsMoving)
                    {
                        pos = pos.Extend(Player.Position.To2D(), 150);
                    }
                    pos = pos.Extend(Player.Position.To2D(), 140);
                }
            }

            return pos.To3D2();
        }

        public static void ComboLogic()
        {
            var Target = TargetSelector.GetSelectedTarget();
           
            if (Target != null && !Target.IsZombie && MenuConfig.ComboR && Target.Distance(Player) <= 1050f)
            {
                if (Target.IsDashing()) return;

                if (Spells.Q.IsReady()  && Spells.R.IsReady())
                {
                    if(Program.GragasQ == null)
                    {
                        Spells.Q.Cast(qpred(Target), true);
                    }

                    if(Spells.R.IsReady())
                    {
                        Spells.R.Cast(rpred(Target), true);
                    }

                    if (Program.GragasQ != null && Target.Distance(Program.GragasQ.Position) <= 250)
                    {
                        Spells.Q.Cast();
                            
                        var pos = Spells.E.GetVectorSPrediction(Target, Spells.E.Range).CastTargetPosition;
                        Spells.E.Cast(pos);
                    }
                }
            }

             var target = TargetSelector.GetTarget(700f, TargetSelector.DamageType.Magical);
            
             if (target != null && target.IsValidTarget() && !target.IsZombie)
             {

                if (Spells.Q.IsReady())
                {
                    if (!Spells.R.IsReady())
                    {

                        if (Program.GragasQ == null)
                        {
                            Spells.Q.Cast(target, true);
                        }
                        if (Program.GragasQ != null && target.Distance(Program.GragasQ.Position) <= 250)
                        {
                            Spells.Q.Cast();
                        }
                    }
                }

                // Smite
                if (Spells.Smite != SpellSlot.Unknown && Spells.R.IsReady() && Player.Spellbook.CanUseSpell(Spells.Smite) == SpellState.Ready && !target.IsZombie)
                {
                    Player.Spellbook.CastSpell(Spells.Smite, Target);
                }

                else if (Spells.W.IsReady() && !Spells.R.IsReady())
                {
                    Spells.W.Cast();
                }

                // E
                else if (Spells.E.IsReady() && !Spells.W.IsReady())
                {
                    var pos = Spells.E.GetVectorSPrediction(Target, Spells.E.Range).CastTargetPosition;

                    if (!Spells.E.CheckMinionCollision(pos))
                    {
                        Spells.E.Cast(pos);
                    }
                }
            }
        }
        
        public static void JungleLogic()
        {
            var mobs = MinionManager.GetMinions(Player.Position, Spells.W.Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);
            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
            {   
                if (mobs.Count == 0 || mobs == null || Player.IsWindingUp)
                    return;
               
                foreach(var m in mobs)
                {
                    if(m.Distance(Player) <= 400f)
                    {
                        if (Spells.W.IsReady())
                        {
                            Spells.W.Cast();
                        }

                        if (Spells.E.IsReady())
                        {
                            Spells.E.Cast(m);
                        }

                        if (Spells.Q.IsReady())
                        {
                            Spells.Q.Cast(m);
                        }
                    }
                }
            }
        }
        public static void HarassLogic()
        {
            var target = TargetSelector.GetTarget(Spells.R.Range - 50, TargetSelector.DamageType.Magical);
            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {
                if (Spells.E.IsReady() && MenuConfig.harassE)
                {
                    Spells.E.Cast(target);
                }
                    
                if (Spells.Q.IsReady() && MenuConfig.harassQ)
                {
                    var pos = Spells.Q.GetSPrediction(target).CastPosition;
                    Spells.Q.Cast(pos);
                }

                if(Spells.W.IsReady())
                {
                    if(target.Distance(Player) <= Player.AttackRange)
                    {
                        Spells.W.Cast();
                    }
                }
            }
        }
        public static void Game_OnUpdate(EventArgs args)
        {
            if (MenuConfig.UseSkin)
            {
                Program.Player.SetSkin(Program.Player.CharData.BaseSkinName, MenuConfig.Config.Item("Skin").GetValue<StringList>().SelectedIndex);
            }
            else Program.Player.SetSkin(Program.Player.CharData.BaseSkinName, Program.Player.BaseSkinId);
        }
    }
}
