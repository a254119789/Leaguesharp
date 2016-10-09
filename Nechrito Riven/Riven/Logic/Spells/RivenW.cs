namespace NechritoRiven.Riven.Logic.Spells
{
    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Implementations;
    using RethoughtLib.FeatureSystem.Switches;

    internal class RivenW : SpellChild
    {
        #region Public Properties

        public override string Name { get; set; } = "Ki Burst";

        public override Spell Spell { get; set; }

        #endregion

        #region Public Methods and Operators

        public float GetDamage(Obj_AI_Base target)
        {
            return !Spell.IsReady() ? 0 : Spell.GetDamage(target);
        }

        #endregion

        #region Methods

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            Spell = new Spell(SpellSlot.W, 250);
        }

        protected override void SetSwitch()
        {
            Switch = new UnreversibleSwitch(Menu);
        }

        #endregion
    }
}
