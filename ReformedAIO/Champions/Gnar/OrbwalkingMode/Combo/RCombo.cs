namespace ReformedAIO.Champions.Gnar.OrbwalkingMode.Combo
{
    using System.Collections.Generic;
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Core;
    using Logic;

    using RethoughtLib.Menu;
    using RethoughtLib.Menu.Presets;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    internal class RCombo : ChildBase
    {
        private GnarState gnarState;

        private WallDetection wallDetection;

        public override string Name { get; set; } = "R";

        private Obj_AI_Hero Target => TargetSelector.GetTarget(Spells.R2.Range, TargetSelector.DamageType.Physical);

        private void GameOnUpdate(EventArgs args)
        {
            if (Target == null || !Spells.R2.IsReady())
            {
                return;
            }

            if(Menu.SubMenu("Menu").Item("HitCount").GetValue<Slider>().Value < Vars.Player.CountEnemiesInRange(Spells.R2.Range))
            {
                
            }

            if (Target.IsInvulnerable 
                || (Vars.Player.CountEnemiesInRange(Spells.R2.Range) < 2 
                && Target.IsStunned)
                || !wallDetection.IsWall(Target))
            {
                return;
            }

          
            var prediction = Spells.R2.GetPrediction(Target);
            Spells.R2.Cast(prediction.CastPosition);
        }

       
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            Menu.AddItem(new MenuItem("RRange", "Range").SetValue(new Slider(590, 0, 590)));
            Menu.AddItem(new MenuItem("HitCount", "Auto If x Count").SetValue(new Slider(3, 0, 5)));

            gnarState = new GnarState();
            wallDetection = new WallDetection();
        }

        //protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        //{
        //    wallDetection = new WallDetection();
        //    gnarState = new GnarState();
        //}

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
