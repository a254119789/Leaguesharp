using RethoughtLib.FeatureSystem.Abstract_Classes;

namespace ReformedAIO.Champions.Gnar.OrbwalkingMode.Combo
{
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Core;
    using Logic;
   

    internal sealed class RCombo : ChildBase
    {
        private WallDetection _wallDetection;

        public override string Name { get; set; } = "R";

        private readonly Orbwalking.Orbwalker _orbwalker;

        public RCombo(Orbwalking.Orbwalker orbwalker)
        {
            _orbwalker = orbwalker;
        }

        private Obj_AI_Hero Target => TargetSelector.GetTarget(Spells.R2.Range, TargetSelector.DamageType.Physical);

        private void GameOnUpdate(EventArgs args)
        {
            if (Target == null || !Spells.R2.IsReady())
            {
                return;
            }

            //var prediction = Spells.R2.GetPrediction(Target);

            if (Menu.SubMenu("Menu").Item("HitCount").GetValue<Slider>().Value
                >= Vars.Player.CountEnemiesInRange(Spells.R2.Range)
                && _wallDetection.IsWall(Target))
            {
                Spells.R2.Cast(_wallDetection.GetFirstWallPoint(Vars.Player.Position, Target.Position));
            }

            if (_orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo)
            {
                return;
            }

            if (Target.IsInvulnerable 
                || (Vars.Player.CountEnemiesInRange(Spells.R2.Range) < 2 
                && Target.IsStunned)
                || !_wallDetection.IsWall(Target))
            {
                return;
            }

            Spells.R2.Cast(_wallDetection.GetFirstWallPoint(Vars.Player.Position, Target.Position));
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            Menu.AddItem(new MenuItem("RRange", "Range").SetValue(new Slider(590, 0, 590)));
            Menu.AddItem(new MenuItem("HitCount", "Auto If x Count").SetValue(new Slider(3, 0, 5)));

            _wallDetection = new WallDetection();
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
