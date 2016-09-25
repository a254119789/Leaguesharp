namespace NechritoRiven.Riven.Logic.Spells
{
    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Implementations;
    using RethoughtLib.FeatureSystem.Switches;

    internal class RivenR : SpellChild
    {
        #region Public Properties

        public override string Name { get; set; } = "Blade of the Exile";

        public override Spell Spell { get; set; }

        public string R1 = "RivenFengShuiEngine";

        public string R2 = "RivenIzunaBlade";

        #endregion

        #region Public Methods and Operators

        public float GetDamage(Obj_AI_Base target)
        {
            return !Spell.IsReady() ? 0 : Spell.GetDamage(target);
        }

        public bool IsR1()
        {
            return Spell.IsReady() && Spell.Instance.Name == R1;
        }

        public bool IsR2()
        {
            return Spell.IsReady() && Spell.Instance.Name == R2;
        }

        #endregion

        #region Methods

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            Spell = new Spell(SpellSlot.R, 900);
            Spell.SetSkillshot(0.25f, (float)(45 * 0.5), 1600, false, SkillshotType.SkillshotCone);
        }

        protected override void SetSwitch()
        {
            Switch = new UnreversibleSwitch(Menu);
        }

        #endregion
    }
}
