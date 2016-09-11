using RethoughtLib.FeatureSystem.Abstract_Classes;

namespace ReformedAIO.Champions.Gnar.OrbwalkingMode.Combo
{
    using System.Linq;
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Core;
   

    internal sealed class WCombo : ChildBase
    {
        private GnarState gnarState;

        public override string Name { get; set; } = "W";

        private readonly Orbwalking.Orbwalker Orbwalker;

        public WCombo(Orbwalking.Orbwalker orbwalker)
        {
            Orbwalker = orbwalker;
        }

        private void GameOnUpdate(EventArgs args)
        {
            if (Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                || gnarState.Mini || 
                !Spells.W2.IsReady())
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

                if(prediction.Hitchance >= HitChance.High
                    || prediction.AoeTargetsHitCount > 1
                    || Vars.Player.IsCastingInterruptableSpell())
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
