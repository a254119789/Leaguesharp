namespace ReformedAIO.Champions.Caitlyn.OrbwalkingMode.Combo
{
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Caitlyn.Logic;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    internal sealed class ECombo : ChildBase
    {
        private readonly Orbwalking.Orbwalker orbwalker;

        public ECombo(Orbwalking.Orbwalker orbwalker)
        {
            this.orbwalker = orbwalker;
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

            Menu.AddItem(new MenuItem("EMana", "Mana %").SetValue(new Slider(30, 0, 100)));

            Menu.AddItem(new MenuItem("AntiGapcloser", "Anti Gapcloser").SetValue(true));

            Menu.AddItem(new MenuItem("AntiMelee", "E Anti-Melee").SetValue(true));
        }

        private void Gapcloser(ActiveGapcloser gapcloser)
        {
            if (!Menu.Item("AntiGapcloser").GetValue<bool>()) return;

            var target = gapcloser.Sender;

            if (target == null) return;

            if (!target.IsEnemy || !Spells.Spell[SpellSlot.E].IsReady()) return;

            Spells.Spell[SpellSlot.E].Cast(gapcloser.End);
        }

        private void OnUpdate(EventArgs args)
        {
            if (this.orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                || Vars.Player.IsWindingUp
                || !Spells.Spell[SpellSlot.E].IsReady()
                || Target == null
                || Menu.Item("EMana").GetValue<Slider>().Value > Vars.Player.ManaPercent)
            {
                return;
            }

            var ePrediction = Spells.Spell[SpellSlot.E].GetPrediction(this.Target);

            if (Target.Distance(Vars.Player) > 270 && Menu.Item("AntiMelee").GetValue<bool>())
            {
                Spells.Spell[SpellSlot.E].Cast(ePrediction.CastPosition);
            }

            if (ePrediction.Hitchance < HitChance.High) return;

            Spells.Spell[SpellSlot.E].Cast(ePrediction.CastPosition);
        }
    }
}
