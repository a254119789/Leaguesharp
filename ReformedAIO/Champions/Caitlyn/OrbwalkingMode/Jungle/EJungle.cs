namespace ReformedAIO.Champions.Caitlyn.OrbwalkingMode.Jungle
{
    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using Logic;

    internal class EJungle : ChildBase
    {
        public EJungle(string name)
        {
            Name = name;
        }

        public sealed override string Name { get; set; }

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
            base.OnLoad(sender, featureBaseEventArgs);
        }

        private void OnUpdate(EventArgs args)
        {
            if (Vars.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear
                || !Spells.Spell[SpellSlot.E].IsReady()
                || Vars.Player.IsWindingUp)
            {
                return;
            }

            var mobs = MinionManager.GetMinions(Spells.Spell[SpellSlot.E].Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth).FirstOrDefault();

            if (mobs == null || !mobs.IsValid) return;

            var qPrediction = (Spells.Spell[SpellSlot.E].GetPrediction(mobs));

            if (mobs.Health < Vars.Player.GetAutoAttackDamage(mobs) * 3) return;

            Spells.Spell[SpellSlot.E].Cast(qPrediction.CastPosition);
        }
    }
}
