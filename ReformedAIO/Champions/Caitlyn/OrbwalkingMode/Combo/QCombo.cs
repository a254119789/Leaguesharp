﻿namespace ReformedAIO.Champions.Caitlyn.OrbwalkingMode.Combo
{
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Caitlyn.Logic;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    internal sealed class QCombo : ChildBase
    {
        private readonly Orbwalking.Orbwalker orbwalker;

        public QCombo(Orbwalking.Orbwalker orbwalker)
        {
            this.orbwalker = orbwalker;
        }

        public override string Name { get; set; } 

        private Obj_AI_Hero Target => TargetSelector.GetTarget(Spells.Spell[SpellSlot.Q].Range, TargetSelector.DamageType.Physical);

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
            base.OnLoad(sender, featureBaseEventArgs);

            Menu.AddItem(new MenuItem("QMana", "Mana %").SetValue(new Slider(30, 0, 100)));

            Menu.AddItem(new MenuItem("QImmobile", "Q On Immobile").SetValue(true));

            Menu.AddItem(new MenuItem("QHit", "Cast if 2 can be hit").SetValue(true));

            Menu.AddItem(new MenuItem("QHigh", "E + Q Cancel").SetValue(false));
        }

        private void OnUpdate(EventArgs args)
        {
            if (Vars.Player.IsWindingUp
                || !Spells.Spell[SpellSlot.Q].IsReady()
                || Target == null
                || Menu.Item("QMana").GetValue<Slider>().Value > Vars.Player.ManaPercent) return;

            if (Menu.Item("QHit").GetValue<bool>())
            {
                Spells.Spell[SpellSlot.Q].CastIfWillHit(Target, 2);
            }

            if (this.orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo)
            {
                return;
            }

            var qPrediction = Spells.Spell[SpellSlot.Q].GetPrediction(this.Target);

            if (qPrediction.Hitchance >= HitChance.VeryHigh && Menu.Item("QHigh").GetValue<bool>()) // Needs work, too tired
            {
                Utility.DelayAction.Add(30, ()=> Spells.Spell[SpellSlot.Q].Cast(qPrediction.CastPosition));
            }

            if (qPrediction.Hitchance >= HitChance.Immobile && Menu.Item("QImmobile").GetValue<bool>())
            {
                Spells.Spell[SpellSlot.Q].Cast(qPrediction.CastPosition);
            }
        }
    }
}
