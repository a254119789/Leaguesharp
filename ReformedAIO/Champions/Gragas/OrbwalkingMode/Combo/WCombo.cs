namespace ReformedAIO.Champions.Gragas.OrbwalkingMode.Combo
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    internal class WCombo : ChildBase
    {
        #region Public Properties

        public override string Name { get; set; } = "[W] Drunken Rage";

        #endregion
        private readonly Orbwalking.Orbwalker Orbwalker;

        public WCombo(Orbwalking.Orbwalker orbwalker)
        {
            Orbwalker = orbwalker;
        }
        #region Methods

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate -= OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate += OnUpdate;
        }

        protected sealed override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Menu.AddItem(new MenuItem(Menu.Name + "WRange", "W Range ").SetValue(new Slider(500, 0, 800)));

            Menu.AddItem(new MenuItem(Menu.Name + "WMana", "Mana %").SetValue(new Slider(45, 0, 100)));
        }

        private void DrunkenRage()
        {
            var target = TargetSelector.GetTarget(
                Menu.Item(Menu.Name + "WRange").GetValue<Slider>().Value,
                TargetSelector.DamageType.Magical);

            if (target == null || !target.IsValid) return;

            Variable.Spells[SpellSlot.W].Cast();
        }

        private void OnUpdate(EventArgs args)
        {
            if (Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                || !Variable.Spells[SpellSlot.W].IsReady()) return;

            if (Menu.Item(Menu.Name + "WMana").GetValue<Slider>().Value > Variable.Player.ManaPercent) return;

            DrunkenRage();
        }

        #endregion
    }
}