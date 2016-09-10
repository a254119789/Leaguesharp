namespace ReformedAIO.Champions.Gnar.OrbwalkingMode.Combo
{
    using System.Linq;
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Core;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    class WCombo : ChildBase
    {
        private GnarState gnarState;

        public override string Name { get; set; } = "W";

        private void GameOnUpdate(EventArgs args)
        {
            if (gnarState.Mini || !Spells.W.IsReady())
            {
                return;
            }

            foreach (var target in HeroManager.Enemies.Where(x => x.IsValidTarget(Spells.W2.Range)))
            {
                if (target == null)
                {
                    return;
                }

                var prediction = Spells.W2.GetPrediction(target);

                if(prediction.Hitchance >= HitChance.High || prediction.AoeTargetsHitCount > 0)
                {
                    Spells.W2.Cast(prediction.CastPosition);
                }
            }
        }
        
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            gnarState = new GnarState();
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
