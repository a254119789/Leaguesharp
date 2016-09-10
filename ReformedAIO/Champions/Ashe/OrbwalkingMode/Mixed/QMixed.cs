namespace ReformedAIO.Champions.Ashe.OrbwalkingMode.Mixed
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Logic;

    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    internal sealed class QMixed : ChildBase
    {
        #region Fields

        private QLogic qLogic;

        #endregion

        #region Constructors and Destructors

        private readonly Orbwalking.Orbwalker Orbwalker;

        public QMixed(string name, Orbwalking.Orbwalker orbwalker)
        {
            Name = name;
            Orbwalker = orbwalker;
        }

        #endregion

        #region Public Properties

        public override string Name { get; set; }

        #endregion

        #region Methods

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate -= OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate += OnUpdate;
        }

        //protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        //{
        //    qLogic = new QLogic();
        //}

        protected sealed override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            Menu.AddItem(new MenuItem(Menu.Name + "QMana", "Mana %").SetValue(new Slider(80, 0, 100)));

            Menu.AddItem(new MenuItem(Name + "AAQ", "AA Before Q").SetValue(true).SetTooltip("AA Q Reset"));

            qLogic = new QLogic();
        }

        private void OnUpdate(EventArgs args)
        {
            if (Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Mixed
                || !Variable.Spells[SpellSlot.Q].IsReady()) return;

            if (Menu.Item(Menu.Name + "QMana").GetValue<Slider>().Value > Variable.Player.ManaPercent) return;

            RangersFocus();
        }

        private void RangersFocus()
        {
            var target = TargetSelector.GetTarget(
                Orbwalking.GetRealAutoAttackRange(Variable.Player) + 65,
                TargetSelector.DamageType.Physical);

            if (target == null || !target.IsValid) return;

            if (Menu.Item(Menu.Name + "AAQ").GetValue<bool>() && Variable.Player.IsWindingUp) return;

            Variable.Spells[SpellSlot.Q].Cast();

            qLogic.Kite(target);
        }

        #endregion
    }
}