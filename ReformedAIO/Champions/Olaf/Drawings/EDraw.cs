namespace ReformedAIO.Champions.Olaf.Drawings
{
    using System;
    using System.Drawing;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Olaf.Core.Spells;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    internal sealed class EDraw : ChildBase
    {
        public override string Name { get; set; } = "E";

        private readonly ESpell spell;

        public EDraw(ESpell spell)
        {
            this.spell = spell;
        }

        public void OnDraw(EventArgs args)
        {
            if (ObjectManager.Player.IsDead)
            {
                return;
            }

            Render.Circle.DrawCircle(ObjectManager.Player.Position, spell.Spell.Range, spell.Spell.IsReady()
                ? Color.Cyan
                : Color.DarkSlateGray,
                4, true);
        }

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnDisable(sender, featureBaseEventArgs);

            Drawing.OnDraw -= OnDraw;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnEnable(sender, featureBaseEventArgs);

            Drawing.OnDraw += OnDraw;
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);
        }
    }
}
