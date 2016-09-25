﻿namespace ReformedAIO.Champions.Gnar.OrbwalkingMode.Combo
{
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Gnar.Core;
    using ReformedAIO.Champions.Gnar.Logic;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using SharpDX;

    internal sealed class RCombo : ChildBase
    {
        private WallDetection wallDetection;

        private GnarState gnarState;

        public override string Name { get; set; } = "R";

        private readonly Orbwalking.Orbwalker orbwalker;

        public RCombo(Orbwalking.Orbwalker orbwalker)
        {
            this.orbwalker = orbwalker;
        }

        private Obj_AI_Hero Target => TargetSelector.GetTarget(Spells.R2.Range, TargetSelector.DamageType.Physical);

        private void GameOnUpdate(EventArgs args)
        {
            if (Menu.Item("ForceDisable").GetValue<bool>() || Target == null || !Spells.R2.IsReady() || gnarState.Mini
                || Target.IsInvulnerable)
            {
                return;
            }

<<<<<<< HEAD
            var prediction = Spells.R2.GetPrediction(Target, true);

            if (prediction.Hitchance < HitChance.High)
            {
                return;
            }

            var wallPoint = wallDetection.GetFirstWallPoint(Vars.Player.Position, Target.Position);
=======
            var wallPoint = this.wallDetection.GetFirstWallPoint(Target.Position, Vars.Player.Position);
>>>>>>> parent of 686eda8... 6.19
            Vars.Player.GetPath(wallPoint);

            if (wallDetection.IsWallDash(Target.Position, 590))
            {
                Console.WriteLine("Walldetection: TRUE");
                Spells.R2.Cast(wallDetection.Wall(Target.Position, 590));
            }
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            Menu.AddItem(new MenuItem("RRange", "Range").SetValue(new Slider(590, 0, 590)));

            Menu.AddItem(new MenuItem("ForceDisable", "Force DISABLE").SetValue(false));

            // Menu.AddItem(new MenuItem("HitCount", "Auto If x Count").SetValue(new Slider(2, 0, 5)));
            gnarState = new GnarState();
            this.wallDetection = new WallDetection();
        }

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate -= GameOnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate += GameOnUpdate;
        }
    }
}
