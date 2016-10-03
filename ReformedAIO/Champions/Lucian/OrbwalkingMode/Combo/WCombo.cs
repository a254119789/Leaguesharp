﻿namespace ReformedAIO.Champions.Lucian.OrbwalkingMode.Combo
{
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Lucian.Core.Damage;
    using ReformedAIO.Champions.Lucian.Core.Spells;

    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Implementations;

    internal sealed class WCombo : OrbwalkingChild
    {
        public override string Name { get; set; } = "W";

        private readonly WSpell wSpell;

        public WCombo(WSpell wSpell)
        {
            this.wSpell = wSpell;
        }

        private void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe
                || !CheckGuardians()
                || ObjectManager.Player.HasBuff("LucianPassiveBuff")
                || !Orbwalking.IsAutoAttack(args.SData.Name)
                || !wSpell.Spell.IsReady()
                || Menu.Item("WMana").GetValue<Slider>().Value > ObjectManager.Player.ManaPercent)
            {
                return;
            }

            var heroes = HeroManager.Enemies.Where(x => x.IsValidTarget(Orbwalking.GetRealAutoAttackRange(ObjectManager.Player)));

            foreach (var target in heroes as Obj_AI_Hero[] ?? heroes.ToArray())
            {
                //if (target.Health > damage.GetComboDamage(target) && Menu.Item("WKillable").GetValue<bool>())
                //{
                //    return;
                //}

                if (Menu.Item("WPred").GetValue<bool>())
                {
                   wSpell.Spell.Cast(target.Position);
                }
                else
                {
                    var wPred = wSpell.Spell.GetPrediction(target, true);

                    if (wPred.Hitchance > HitChance.Medium)
                    {
                       wSpell.Spell.Cast(wPred.CastPosition);
                    }
                }
            }
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            Menu.AddItem(new MenuItem("WPred", "Disable Prediction").SetValue(true));
          //  Menu.AddItem(new MenuItem("WKillable", "Only If Killable").SetValue(false));
            Menu.AddItem(new MenuItem("WMana", "Min Mana %").SetValue(new Slider(20, 0, 100)));

        }

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Obj_AI_Base.OnDoCast -= OnDoCast;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Obj_AI_Base.OnDoCast += OnDoCast;
        }
    }
}
