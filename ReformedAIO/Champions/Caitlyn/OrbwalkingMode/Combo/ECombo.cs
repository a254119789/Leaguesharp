﻿namespace ReformedAIO.Champions.Caitlyn.OrbwalkingMode.Combo
{
    using System;
    using LeagueSharp;
    using LeagueSharp.Common;
    using Logic;
    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    internal sealed class ECombo : ChildBase
    {
        private readonly Orbwalking.Orbwalker Orbwalker;

        public ECombo(Orbwalking.Orbwalker orbwalker)
        {
            Orbwalker = orbwalker;
        }


        public override string Name { get; set; }

        private Obj_AI_Hero Target => TargetSelector.GetTarget(Spells.Spell[SpellSlot.E].Range, TargetSelector.DamageType.Physical);

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            AntiGapcloser.OnEnemyGapcloser -= Gapcloser;
            Game.OnUpdate -= OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            AntiGapcloser.OnEnemyGapcloser += Gapcloser;
            Game.OnUpdate += OnUpdate;
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            Menu.AddItem(new MenuItem(Menu.Name + "EMana", "Mana %").SetValue(new Slider(30, 0, 100)));

            Menu.AddItem(new MenuItem(Name + "AntiGapcloser", "Anti Gapcloser").SetValue(true));

            Menu.AddItem(new MenuItem(Name + "AntiMelee", "E Anti-Melee").SetValue(true));
        }

        private void Gapcloser(ActiveGapcloser gapcloser)
        {
            if (!Menu.Item(Menu.Name + "AntiGapcloser").GetValue<bool>()) return;

            var target = gapcloser.Sender;

            if (target == null) return;

            if (!target.IsEnemy || !Spells.Spell[SpellSlot.E].IsReady()) return;

            Spells.Spell[SpellSlot.E].Cast(gapcloser.End);
        }

        private void OnUpdate(EventArgs args)
        {
            if (Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                || Vars.Player.IsWindingUp
                || !Spells.Spell[SpellSlot.E].IsReady()
                || Target == null
                || Menu.Item(Menu.Name + "EMana").GetValue<Slider>().Value > Vars.Player.ManaPercent)
            {
                return;
            }

            var ePrediction = (Spells.Spell[SpellSlot.E].GetPrediction(Target));

            if (Target.Distance(Vars.Player) > 270 && Menu.Item(Menu.Name + "AntiMelee").GetValue<bool>())
            {
                Spells.Spell[SpellSlot.E].Cast(ePrediction.CastPosition);
            }

            if (ePrediction.Hitchance < HitChance.High) return;

            Spells.Spell[SpellSlot.E].Cast(ePrediction.CastPosition);
        }
    }
}
