using LeagueSharp.SDK.Utils;

namespace ReformedAIO.Champions.Caitlyn.Killsteal
{
    using Logic;
    using System;
    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    internal sealed class QKillsteal : ChildBase
    {
        public QKillsteal(string name)
        {
            Name = name;
        }

        public override string Name { get; set; }

        private Obj_AI_Hero Target => TargetSelector.GetTarget(Spells.Spell[SpellSlot.Q].Range, TargetSelector.DamageType.Physical);

        private QLogic _qLogic;

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate -= OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate += OnUpdate;
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            _qLogic = new QLogic();
        }

        private void OnUpdate(EventArgs args)
        {
            if (!Spells.Spell[SpellSlot.Q].IsReady()
                || Target == null
                || Target.IsDead
                || Target.Health > Spells.Spell[SpellSlot.Q].GetDamage(Target)
                || Target.Distance(Vars.Player) < Vars.Player.GetRealAutoAttackRange()
                || !_qLogic.CanKillSteal(Target))
            {
                return;
            }

            var pos = Spells.Spell[SpellSlot.Q].GetPrediction(Target);

            Spells.Spell[SpellSlot.Q].Cast(pos.CastPosition);
        }
    }
}
