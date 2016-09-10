namespace ReformedAIO.Champions.Caitlyn.OrbwalkingMode.Combo
{
    using Logic;
    using System;
    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    internal sealed class Ewqr : ChildBase
    {
        public Ewqr(string name)
        {
            Name = name;
        }

        public override string Name { get; set; }

        private EwqrLogic _ewqrLogic;

        private Obj_AI_Hero Target => TargetSelector.GetTarget(Spells.Spell[SpellSlot.E].Range, TargetSelector.DamageType.Physical);

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
            _ewqrLogic = new EwqrLogic();
        }

        private void OnUpdate(EventArgs args) // Note: This is only a test and will likely be removed.
        {
            if (Vars.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo || Target == null)
            {
                return;
            }

            if (!_ewqrLogic.CanExecute(Target)) return;

            var qPred = Spells.Spell[SpellSlot.Q].GetPrediction(Target);
            var ePred = Spells.Spell[SpellSlot.E].GetPrediction(Target);
            var wPrediction = (Spells.Spell[SpellSlot.W].GetPrediction(Target));

            Console.WriteLine("DOING EWQR COMBO..");

            Spells.Spell[SpellSlot.E].Cast(ePred.CastPosition);

            Spells.Spell[SpellSlot.W].Cast(wPrediction.CastPosition);

            Spells.Spell[SpellSlot.Q].Cast(qPred.CastPosition);

            if (Spells.Spell[SpellSlot.R].IsReady())
            {
                Spells.Spell[SpellSlot.R].Cast(Target);
            }
        }
    }
}
