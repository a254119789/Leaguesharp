﻿namespace ReformedAIO.Champions.Lucian.Core.Spells
{
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Implementations;
    using RethoughtLib.FeatureSystem.Switches;

    using SharpDX;

    class Q2Spell : SpellChild
    {
        public override string Name { get; set; } = "Piercing Light";

        public override Spell Spell { get; set; }

        public float GetDamage(Obj_AI_Base target)
        {
            return !Spell.IsReady() ? 0 : Spell.GetDamage(target);
        }

        public Vector3 QPred(Obj_AI_Base target)
        {
            var pos = Spell.GetPrediction(target);

            if (pos.Hitchance < HitChance.High)
            {
                pos.CastPosition = Vector3.Zero;
            }

            return pos.UnitPosition;
        }

        public bool QMinionExtend()
        {
            var m = MinionManager.GetMinions(500).FirstOrDefault();

            var target = HeroManager.Enemies.FirstOrDefault(x => x.IsValidTarget(Spell.Range));

            if (m == null)
            {
                return false;
            }

            var hit = new Geometry.Polygon.Rectangle(
                    ObjectManager.Player.Position,
                    ObjectManager.Player.Position.Extend(m.Position, Spell.Range), Spell.Width);


            return hit.IsInside(QPred(target).To2D());
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            Spell = new Spell(SpellSlot.Q, 1150);
            Spell.SetSkillshot(.5f, 50, float.MaxValue, false, SkillshotType.SkillshotLine);
        }

        protected override void SetSwitch()
        {
            Switch = new UnreversibleSwitch(Menu);
        }
    }
}
