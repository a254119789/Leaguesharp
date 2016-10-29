﻿namespace ReformedAIO.Champions.Lucian.OrbwalkingMode.JungleClear
{
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Lucian.Spells;

    using RethoughtLib.FeatureSystem.Implementations;

    internal sealed class QJungleClear : OrbwalkingChild
    {
        public override string Name { get; set; } = "Q";

        private readonly QSpell qSpell;

        public QJungleClear(QSpell qSpell)
        {
            this.qSpell = qSpell;
        }

        private void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe
                ||Menu.Item("QMana").GetValue<Slider>().Value > ObjectManager.Player.ManaPercent
                || !CheckGuardians())
            {
                return;
            }

            var mob =
                MinionManager.GetMinions(
                    ObjectManager.Player.Position,
                    ObjectManager.Player.AttackRange,
                    MinionTypes.All,
                    MinionTeam.Neutral).OrderBy(x => x.MaxHealth).FirstOrDefault();

            if (mob == null)
            {
                return;
            }

            qSpell.Spell.CastOnUnit(mob);

        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            Menu.AddItem(new MenuItem("QMana", "Min Mana %").SetValue(new Slider(5, 0, 100)));
        }

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnDisable(sender, featureBaseEventArgs);

            Obj_AI_Base.OnDoCast -= OnDoCast;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnEnable(sender, featureBaseEventArgs);

            Obj_AI_Base.OnDoCast += OnDoCast;
        }
    }
}
