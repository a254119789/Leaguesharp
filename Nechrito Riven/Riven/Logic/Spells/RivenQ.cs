namespace NechritoRiven.Riven.Logic.Spells
{
    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Implementations;
    using RethoughtLib.FeatureSystem.Switches;

    internal class RivenQ : SpellChild
    {
        #region Public Properties

        public override string Name { get; set; } = "Broken Wings";

        public override Spell Spell { get; set; }

        public float LastQ;

        public int QStack = 1;

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

            Spell = new Spell(SpellSlot.Q, 260);
            Spell.SetSkillshot(0.25f, 100f, 2200f, false, SkillshotType.SkillshotCircle);
        }

        protected override void SetSwitch()
        {
            Switch = new UnreversibleSwitch(Menu);
        }

        #endregion
    }
}
