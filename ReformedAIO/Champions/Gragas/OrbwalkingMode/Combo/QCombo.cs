namespace ReformedAIO.Champions.Gragas.OrbwalkingMode.Combo
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Gragas.Logic;

    using RethoughtLib.FeatureSystem.Implementations;

    #endregion

    internal class QCombo : OrbwalkingChild
    {
        #region Fields

        private QLogic qLogic;

        #endregion

        #region Public Properties

        public override string Name { get; set; } = "[Q] Barrel Roll";

        #endregion
        private readonly Orbwalking.Orbwalker orbwalker;

        public QCombo(Orbwalking.Orbwalker orbwalker)
        {
            this.orbwalker = orbwalker;
        }
        #region Methods

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnDisable(sender, featureBaseEventArgs);

            Game.OnUpdate -= OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnEnable(sender, featureBaseEventArgs);

            Game.OnUpdate += OnUpdate;
        }

        protected sealed override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Menu.AddItem(new MenuItem(Menu.Name + "QRange", "Q Range ").SetValue(new Slider(835, 0, 850)));

            Menu.AddItem(new MenuItem(Menu.Name + "QMana", "Mana %").SetValue(new Slider(45, 0, 100)));

            qLogic = new QLogic();
        }

        private void BarrelRoll()
        {
            if (!CheckGuardians())
            {
                return;
            }

            var target = TargetSelector.GetTarget(
                Menu.Item(Menu.Name + "QRange").GetValue<Slider>().Value,
                TargetSelector.DamageType.Magical);

            if (target == null || !target.IsValid) return;

            if (qLogic.CanThrowQ())
            {
                Variable.Spells[SpellSlot.Q].Cast(qLogic.QPred(target));
            }

            if (qLogic.CanExplodeQ(target))
            {
                Variable.Spells[SpellSlot.Q].Cast();
            }
        }

        private void OnUpdate(EventArgs args)
        {
            if (this.orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                || !Variable.Spells[SpellSlot.Q].IsReady()) return;

            if (Menu.Item(Menu.Name + "QMana").GetValue<Slider>().Value > Variable.Player.ManaPercent) return;

            BarrelRoll();
        }

        #endregion
    }
}