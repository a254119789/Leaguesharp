namespace ReformedAIO.Champions.Yasuo.Stack
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Core.Spells;

    using ReformedAIO.Champions.Yasuo.Core;

    using RethoughtLib.FeatureSystem.Implementations;

    internal sealed class YasuoQStack: OrbwalkingChild
    {
        public override string Name { get; set; } = "Stack";

        private readonly Q1Spell spell;

        public YasuoQStack(Q1Spell spell)
        {
            this.spell = spell;
        }

        private Obj_AI_Hero Target => TargetSelector.GetTarget(spell.Spell.Range, TargetSelector.DamageType.Physical);

        private IOrderedEnumerable<Obj_AI_Base> Mob =>
             MinionManager.GetMinions(ObjectManager.Player.Position,
                 spell.Spell.Range,
                 MinionTypes.All,
                 MinionTeam.Neutral).OrderBy(m => m.MaxHealth);

        private List<Obj_AI_Base> Minion => MinionManager.GetMinions(ObjectManager.Player.Position, spell.Spell.Range);

        private void OnUpdate(EventArgs args)
        {
            if (ObjectManager.Player.HasBuff("Recall") || ObjectManager.Player.HasBuff("YasuoQ3W") || ObjectManager.Player.CountEnemiesInRange(500) >= 1)
            {
                return;
            }

            if (Target != null)
            {
                spell.Spell.Cast(Target);
            }
            else if (Minion != null)
            {
                foreach (var m in Minion)
                {
                    spell.Spell.Cast(m);
                }
            }
            else if (Mob != null)
            {
                foreach (var m in Mob)
                {
                    spell.Spell.Cast(m);
                }
            }
        }

        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            Game.OnUpdate -= OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            Game.OnUpdate += OnUpdate;
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnLoad(sender, eventArgs);
        }
    }
}
