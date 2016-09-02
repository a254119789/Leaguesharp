﻿using LeagueSharp.SDK.Utils;

namespace ReformedAIO.Champions.Caitlyn.Killsteal
{
    using Logic;
    using System;
    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    internal sealed class RKillsteal : ChildBase
    {
        public RKillsteal(string name)
        {
            Name = name;
        }

        public override string Name { get; set; }

        private Obj_AI_Hero Target => TargetSelector.GetTarget(Spells.Spell[SpellSlot.R].Range, TargetSelector.DamageType.Physical);

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Events.OnUpdate -= OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Events.OnUpdate += OnUpdate;
        }

        private void OnUpdate(EventArgs args)
        {
             // Brian if you see this i'm sorry XD
            if (Spells.Spell[SpellSlot.R].Level < 2)
            {
                Spells.Spell[SpellSlot.R].Range = 2000;
            }
            else
            {
                Spells.Spell[SpellSlot.R].Range = 1500 + (500 * Spells.Spell[SpellSlot.R].Level);
            }

            if (!Spells.Spell[SpellSlot.R].IsReady()
                || Target == null
                || Vars.Player.Distance(Target) < Vars.Player.GetRealAutoAttackRange() + 350
                || Target.Health > Spells.Spell[SpellSlot.R].GetDamage(Target))
            {
                return;
            }

            Spells.Spell[SpellSlot.R].Cast(Target);
        }
    }
}
