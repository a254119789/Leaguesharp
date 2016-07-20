﻿using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using ReformedAIO.Champions.Gragas.Logic;
using RethoughtLib.Classes.Feature;
using RethoughtLib.Events;
using RethoughtLib.TargetValidator;
using RethoughtLib.TargetValidator.Implementations;
using SharpDX;
using SPrediction;

namespace ReformedAIO.Champions.Gragas.OrbwalkingMode.Combo
{
    class RCombo : FeatureChild<Combo>
    {
        public override string Name => "[R] Explosive Cask";

        public RCombo(Combo parent) : base(parent)
        {
            this.OnLoad();
        }

        private RLogic rLogic;

        private QLogic qLogic;
       
        // Need to fix this to make better QRQ Combo
        //private void OnProcessSpellCast(GameObject sender, GameObjectProcessSpellCastEventArgs args)
        //{
        //    if (!Menu.Item(Menu.Name + "QRQ").GetValue<bool>() || !Variable.Spells[SpellSlot.Q].IsReady() ||
        //        Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo) return;


        //    var target = args.Target as Obj_AI_Hero;
        //    // args.SData.Name == "Gragas_Base_Q_Ally.troy"

        //    if (target == null || !target.IsValidTarget(1150) || !sender.IsMe) return;

        //    var pred = LeagueSharp.Common.Prediction.GetPrediction(target, Variable.Spells[SpellSlot.R].Delay
        //        + Variable.Player.Position.Distance(args.End) / Variable.Spells[SpellSlot.R].Speed).CastPosition;

        //    Variable.Spells[SpellSlot.Q].Cast(args.End.Extend(pred, Variable.Spells[SpellSlot.R].Width));
        //}


        private void ExplosiveCask()
        {
            var target = TargetSelector.GetSelectedTarget();

            var targetValidator = new TargetValidator();
            
            var check = new IsValidTargetCheck();

            targetValidator.AddCheck(check);
            targetValidator.RemoveCheck(check);
            var valid = targetValidator.Check(target);

            if (target.IsDashing()) return;

            Variable.Player.IssueOrder(GameObjectOrder.MoveTo, target);

            if (Menu.Item(Menu.Name + "QRQ").GetValue<bool>() && Variable.Spells[SpellSlot.Q].IsReady()
                && Menu.Item(Menu.Name + "QRQDistance").GetValue<Slider>().Value >= target.Distance(Variable.Player))
            {
                Variable.Spells[SpellSlot.Q].Cast(InsecQ(target));
            }

            Variable.Spells[SpellSlot.R].Cast(InsecTo(target));
        }

        private void OnUpdate(EventArgs args)
        {
            if (Variable.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo || !Variable.Spells[SpellSlot.R].IsReady()
                || Menu.Item(Menu.Name + "RMana").GetValue<Slider>().Value > Variable.Player.ManaPercent) return;

            
            this.ExplosiveCask();
        }

        private void OnDraw(EventArgs args)
        {
            if(Variable.Player.IsDead || !Menu.Item(Menu.Name + "RDraw").GetValue<bool>()) return;

            var target = TargetSelector.GetSelectedTarget();

            if(target == null || !target.IsValid) return;

            Render.Circle.DrawCircle(InsecTo(target), 100, System.Drawing.Color.Cyan);

            if (Menu.Item(Menu.Name + "QRQ").GetValue<bool>())
            {
                Render.Circle.DrawCircle(InsecQ(target), 60, System.Drawing.Color.Cyan);
            }  
        }

        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "InsecTo", "Insec To").SetValue(new StringList(new[] { "Ally / Turret", " Player", " Cursor" })));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "AllyRange", "Range To Find Allies").SetValue(new Slider(1500, 0, 2400)));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "TurretRange", "Range To Find Turret").SetValue(new Slider(1300, 0, 1600)));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "RRange", "R Range ").SetValue(new Slider(950, 0, 1050)));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "RRangePred", "Range Behind Target").SetValue(new Slider(150, 0, 185)));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "RMana", "Mana %").SetValue(new Slider(45, 0, 100)));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "QRQ", "Use Q?").SetValue(true).SetTooltip("Will do QRQ insec (BETA)"));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "QRQDistance", "Max Distance For QRQ Combo").SetValue(new Slider(725, 0, 800)));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "RDraw", "Draw R Prediction").SetValue(false));

            this.Menu.AddItem(new MenuItem(this.Menu.Name + "Enabled", "Enabled").SetValue(false));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        protected override void OnInitialize()
        {
            this.qLogic = new QLogic();
            this.rLogic = new Logic.RLogic();
            base.OnInitialize();
        }

        protected override void OnDisable()
        {
            Drawing.OnDraw -= OnDraw;
          //  Obj_AI_Base.OnProcessSpellCast -= OnProcessSpellCast;
            Events.OnUpdate -= this.OnUpdate;
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            Drawing.OnDraw += OnDraw;
         //   Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Events.OnUpdate += this.OnUpdate;
            base.OnEnable();
        }

        private Vector3 InsecTo(Obj_AI_Hero target)
        {
            var mePos = Variable.Player.Position; // Doing this we can extend to our own position if we can't access anything else (Tower, Ally)

            switch (this.Menu.Item(Menu.Name + "InsecTo").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    var ally = HeroManager.Allies.Where(x => x.IsValidTarget(Menu.Item(Menu.Name + "AllyRange").GetValue<Slider>().Value, false, target.ServerPosition) && x.Distance(target) > 325 && !x.IsMe && x.IsAlly).MaxOrDefault(x => x.CountAlliesInRange(Menu.Item(Menu.Name + "AllyRange").GetValue<Slider>().Value));

                    if (ally != null)
                    {
                        mePos = ally.ServerPosition;
                    }

                    var turret = ObjectManager.Get<Obj_AI_Turret>().Where(x => x.IsAlly && x.Distance(target) > 325 && x.Distance(target) < Menu.Item(Menu.Name + "TurretRange").GetValue<Slider>().Value && !x.IsEnemy).OrderBy(x => x.Distance(Variable.Player.Position)).FirstOrDefault();

                    if (turret != null)
                    {
                        mePos = turret.ServerPosition;
                    }

                    break;
                case 1:
                    mePos = mePos; // Kappa just because i can
                    break;
                case 2:
                    mePos = Game.CursorPos;
                    break;
            }

            var pos = Variable.Spells[SpellSlot.R].GetVectorSPrediction(target, 980).CastTargetPosition.Extend(mePos.To2D(), -Menu.Item(Menu.Name + "RRangePred").GetValue<Slider>().Value);

            return pos.To3D();
        }

        // Hotfix..!
        private Vector3 InsecQ(Obj_AI_Hero target)
        {
            var rPred = rLogic.RPred(target).Extend(Variable.Player.Position, Variable.Spells[SpellSlot.R].Width - Menu.Item(Menu.Name + "RRangePred").GetValue<Slider>().Value);

            return rPred;
        }
    }
}
