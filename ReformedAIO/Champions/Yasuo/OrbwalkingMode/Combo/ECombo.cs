namespace ReformedAIO.Champions.Yasuo.OrbwalkingMode.Combo
{
    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Yasuo.Core.Spells;
    using ReformedAIO.Library.Dash_Handler;

    using RethoughtLib.FeatureSystem.Implementations;

    internal sealed class ECombo : OrbwalkingChild
    {
        public override string Name { get; set; } = "E";

        private readonly ESpell spell;

        public ECombo(ESpell spell)
        {
            this.spell = spell;
        }

        private DashPosition dashPos;

        private static Obj_AI_Hero Target => TargetSelector.GetTarget(1500, TargetSelector.DamageType.Physical);

        private Obj_AI_Base Minion => MinionManager.GetMinions(ObjectManager.Player.Position, 1000).LastOrDefault(
            m => dashPos.DashEndPosition(m, spell.Spell.Range).Distance(Target.Position) <= ObjectManager.Player.Distance(Target)
                 && dashPos.DashEndPosition(m, spell.Spell.Range).Distance(Target.Position) > ObjectManager.Player.AttackRange);

        private void OnUpdate(EventArgs args)
        {
            if (Target == null || !CheckGuardians()
                || (Menu.Item("Combo.E.Turret").GetValue<bool>() && dashPos.DashEndPosition(Target, spell.Spell.Range).UnderTurret(true))
                || (Menu.Item("Combo.E.Enemies").GetValue<Slider>().Value < ObjectManager.Player.CountEnemiesInRange(1000)))
            {
                return;
            }

            spell.Spell.CastOnUnit(Minion ?? Target);
        }

        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            Game.OnUpdate -= OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            Game.OnUpdate += OnUpdate;
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnLoad(sender, eventArgs);

            dashPos = new DashPosition();

         //   Menu.AddItem(new MenuItem("DistanceRange", "Target Radius").SetValue(new Slider(125, 0, 600)));

            Menu.AddItem(new MenuItem("Combo.E.Enemies", "Don't E Into X Enemies").SetValue(new Slider(3, 0, 5)));

            Menu.AddItem(new MenuItem("Combo.E.Turret", "Turret Check").SetValue(true));
        }
    }
}