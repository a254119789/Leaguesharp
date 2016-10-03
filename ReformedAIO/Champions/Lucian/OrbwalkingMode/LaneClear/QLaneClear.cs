namespace ReformedAIO.Champions.Lucian.OrbwalkingMode.LaneClear
{
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Lucian.Core.Spells;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    internal sealed class QLaneClear: ChildBase
    {
        public override string Name { get; set; } = "Q";

        private readonly QSpell qSpell;

        private readonly Orbwalking.Orbwalker orbwalker;

        public QLaneClear(QSpell qSpell, Orbwalking.Orbwalker orbwalker)
        {
            this.qSpell = qSpell;
            this.orbwalker = orbwalker;
        }

        private void OnUpdate(EventArgs args)
        {
            if (Menu.Item("EnemiesCheck").GetValue<bool>() 
                && ObjectManager.Player.CountEnemiesInRange(1350) >= 1 
                || (ObjectManager.Player.ManaPercent <= Menu.Item("QMana").GetValue<Slider>().Value)
                || ObjectManager.Player.HasBuff("LucianPassiveBuff"))
            {
                return;
            }

            var minion = MinionManager.GetMinions(ObjectManager.Player.Position, Orbwalking.GetAttackRange(ObjectManager.Player));

            var qPred = qSpell.Spell.GetLineFarmLocation(minion);

            if (qPred.MinionsHit >= Menu.Item("MinHit").GetValue<Slider>().Value)
            {
                qSpell.Spell.Cast(qPred.Position);
            }
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            Menu.AddItem(new MenuItem("EnemiesCheck", "Check Enemies First").SetValue(true).SetTooltip("Wont Q If Nearby Enemies"));
            Menu.AddItem(new MenuItem("MinHit", "Min Hit By Q").SetValue(new Slider(3, 0, 6)));
            Menu.AddItem(new MenuItem("QMana", "Min Mana %").SetValue(new Slider(5, 0, 100)));
        }

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate -= OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate += OnUpdate;
        }
    }
}
