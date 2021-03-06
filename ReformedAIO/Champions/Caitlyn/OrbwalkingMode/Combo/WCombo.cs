﻿namespace ReformedAIO.Champions.Caitlyn.OrbwalkingMode.Combo
{
    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Caitlyn.Spells;

    using RethoughtLib.FeatureSystem.Implementations;

    internal sealed class WCombo : OrbwalkingChild
    {
        public override string Name { get; set; } = "W";

        private readonly WSpell wSpell;

        public WCombo(WSpell wSpell)
        {
            this.wSpell = wSpell;
        }

        private Obj_AI_Hero Target => TargetSelector.GetTarget(wSpell.Spell.Range, TargetSelector.DamageType.Physical);

        private bool ewq;

        private Obj_AI_Turret Obj_AI_Turret;

        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            Obj_AI_Base.OnProcessSpellCast -= OnProcessSpellCast;
            AntiGapcloser.OnEnemyGapcloser -= Gapcloser;
            Game.OnUpdate -= OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            AntiGapcloser.OnEnemyGapcloser += Gapcloser;
            Game.OnUpdate += OnUpdate;
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnLoad(sender, eventArgs);

            Menu.AddItem(new MenuItem("Combo.W.Mana", "Mana %").SetValue(new Slider(5, 0, 100)));

            Menu.AddItem(new MenuItem("Combo.W.AntiGapcloser", "Anti-Gapcloser").SetValue(true));

            Menu.AddItem(new MenuItem("Combo.W.Target", "W Behind Target").SetValue(true));

            Menu.AddItem(new MenuItem("Combo.W.Immobile", "W On Immobile").SetValue(true));

            Menu.AddItem(new MenuItem("Combo.W.Bush", "Auto W On Bush").SetValue(false));
        }

        private void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe || !wSpell.Spell.IsReady())
            {
                return;
            }

            this.ewq = args.SData.Name == "CaitlynPiltoverPeacemaker";
        }

        private void Gapcloser(ActiveGapcloser gapcloser)
        {
            if (!Menu.Item("Combo.W.AntiGapcloser").GetValue<bool>() || !CheckGuardians() || !gapcloser.Sender.IsEnemy) return;

            wSpell.Spell.Cast(gapcloser.End);
        }

      
        private void OnUpdate(EventArgs args)
        {
            if (!CheckGuardians())
            {
                return;
            }

            if (Menu.Item("Combo.W.Bush").GetValue<bool>()
                && Utils.TickCount - wSpell.Spell.LastCastAttemptT < 2500
                && !ObjectManager.Player.IsRecalling())
            {
                // Beta
                if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Ammo < 2) return;

                var path = ObjectManager.Player.GetWaypoints().LastOrDefault().To3D();

                if (!NavMesh.IsWallOfGrass(path, 0)) return;

                Utility.DelayAction.Add(100, ()=> wSpell.Spell.Cast(path));
            }

            if (Target == null 
                || Menu.Item("Combo.W.Mana").GetValue<Slider>().Value > ObjectManager.Player.ManaPercent
                || Utils.TickCount - wSpell.Spell.LastCastAttemptT < 1500)
            {
                return;
            }

            var wPrediction = wSpell.Spell.GetPrediction(Target);

            if (Menu.Item("Combo.W.Target").GetValue<bool>()) 
            {
                if (wPrediction.Hitchance < HitChance.VeryHigh)
                {
                    return;
                }

                wSpell.Spell.Cast(wPrediction.CastPosition);
            }

            if (wPrediction.Hitchance < HitChance.Immobile || !Menu.Item("Combo.W.Immobile").GetValue<bool>()) return;

            wSpell.Spell.Cast(wPrediction.CastPosition);
        }
    }
}
