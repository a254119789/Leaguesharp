namespace NechritoRiven.Riven.Logic.Spells
{
    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Implementations;
    using RethoughtLib.FeatureSystem.Switches;

    internal class RivenE : SpellChild
    {
        #region Public Properties

        public override string Name { get; set; } = "Valor";

        public override Spell Spell { get; set; }

        #endregion

        #region Methods

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            Spell = new Spell(SpellSlot.E, 270);
        }

        protected override void SetSwitch()
        {
            Switch = new UnreversibleSwitch(Menu);
        }

        #endregion
    }
}
