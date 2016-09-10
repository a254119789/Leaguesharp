using ReformedAIO.Champions.Caitlyn.Logic;

namespace ReformedAIO.Champions.Caitlyn.OrbwalkingMode.Jungle
{
    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    internal class QJungle : ChildBase
    {
        public QJungle(string name)
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

            Menu.AddItem(new MenuItem(Menu.Name + "QOverkill", "Overkill Check").SetValue(true));
        }

        private void OnUpdate(EventArgs args)
        {
            if (Vars.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear
                || !Spells.Spell[SpellSlot.Q].IsReady()
                || Vars.Player.IsWindingUp)
            {
                return;
            }

            var mobs = MinionManager.GetMinions(Spells.Spell[SpellSlot.E].Range, MinionTypes.All, MinionTeam.Neutral,MinionOrderTypes.MaxHealth).FirstOrDefault();

            if (mobs == null || !mobs.IsValid) return;

            var qPrediction = (Spells.Spell[SpellSlot.Q].GetPrediction(mobs));

            if (Menu.Item(Menu.Name + "QOverkill").GetValue<bool>() &&
                mobs.Health < Vars.Player.GetAutoAttackDamage(mobs) * 4) return;


            Utility.DelayAction.Add(50, ()=> Spells.Spell[SpellSlot.Q].Cast(qPrediction.CastPosition));

        }
    }
}
