﻿namespace ReformedAIO.Champions.Lucian.Killsteal
{
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Lucian.Spells;

    using RethoughtLib.FeatureSystem.Implementations;

    internal sealed class W : OrbwalkingChild
    {
        public override string Name { get; set; } = nameof(W); 

        private readonly WSpell wSpell;

        public W(WSpell wSpell)
        {
            this.wSpell = wSpell;
        }

        private Obj_AI_Hero Target => TargetSelector.GetTarget(wSpell.Spell.Range, TargetSelector.DamageType.Physical);

        private void OnUpdate(EventArgs args)
        {
            if (this.Target == null || this.Target.Health > wSpell.GetDamage(Target) || !CheckGuardians())
            {
                return;
            }

            if (this.Target.Distance(ObjectManager.Player) <= wSpell.Spell.Range)
            {
                var wPred = wSpell.Spell.GetPrediction(this.Target, true);

                if (wPred.Hitchance > HitChance.Medium)
                {
                    wSpell.Spell.Cast(Target);
                }
            }
        }

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate -= this.OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate += this.OnUpdate;
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
        }
    }
}