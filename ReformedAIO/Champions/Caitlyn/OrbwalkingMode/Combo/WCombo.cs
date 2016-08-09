﻿namespace ReformedAIO.Champions.Caitlyn.OrbwalkingMode.Combo
{
    using System.Linq;
    using Logic;
    using System;
    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Events;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    internal sealed class WCombo : ChildBase
    {
        public WCombo(string name)
        {
            Name = name;
        }

        public override string Name { get; set; }

        private Obj_AI_Hero Target => TargetSelector.GetTarget(Spells.Spell[SpellSlot.W].Range, TargetSelector.DamageType.Physical);

        private bool EWQ;

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Obj_AI_Base.OnProcessSpellCast -= OnProcessSpellCast;
            AntiGapcloser.OnEnemyGapcloser -= Gapcloser;
            Events.OnUpdate -= OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            AntiGapcloser.OnEnemyGapcloser += Gapcloser;
            Events.OnUpdate += OnUpdate;
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            Menu.AddItem(new MenuItem(Menu.Name + "WMana", "Mana %").SetValue(new Slider(30, 0, 100)));

            Menu.AddItem(new MenuItem(Name + "AntiGapcloser", "Anti-Gapcloser").SetValue(true));

            Menu.AddItem(new MenuItem(Name + "WTarget", "W Behind Target").SetValue(true));

            Menu.AddItem(new MenuItem(Name + "WImmobile", "W On Immobile").SetValue(true));

            Menu.AddItem(new MenuItem(Name + "WBush", "Auto W On Bush").SetValue(false));
        }

        private void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            EWQ = false;

            if (!Spells.Spell[SpellSlot.W].IsReady()
                || !sender.IsMe
                || (args.SData.Name != "CaitlynEntrapment") && args.SData.Name == "CaitlynPiltoverPeacemaker") return;

            EWQ = true;
        }

        private void Gapcloser(ActiveGapcloser gapcloser)
        {
            if (!Menu.Item(Menu.Name + "AntiGapcloser").GetValue<bool>()) return;

            var target = gapcloser.Sender;

            if (target == null) return;

            if (!target.IsEnemy || !Spells.Spell[SpellSlot.W].IsReady()) return;

            Spells.Spell[SpellSlot.W].Cast(gapcloser.End);
        }

      
        private void OnUpdate(EventArgs args)
        {
            if (Vars.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.None
                && Menu.Item(Menu.Name + "WBush").GetValue<bool>()
                && Utils.TickCount - Spells.Spell[SpellSlot.W].LastCastAttemptT > 10000
                && !Vars.Player.IsRecalling()
                && !Vars.Player.IsWindingUp)
            {
                // Beta
                if (Vars.Player.Spellbook.GetSpell(SpellSlot.W).Ammo < 3) return;

                var path = Vars.Player.GetWaypoints().LastOrDefault().To3D();

                if (!NavMesh.IsWallOfGrass(path, 0)) return;

                Utility.DelayAction.Add(400, ()=> Spells.Spell[SpellSlot.W].Cast(path));
            }

            if (Vars.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                || Vars.Player.IsWindingUp
                || !Spells.Spell[SpellSlot.W].IsReady()
                || Target == null
                || Menu.Item(Menu.Name + "WMana").GetValue<Slider>().Value > Vars.Player.ManaPercent
                || Utils.TickCount - Spells.Spell[SpellSlot.W].LastCastAttemptT < 5000) return;

            var wPrediction = Spells.Spell[SpellSlot.W].GetPrediction(Target);

            if (Menu.Item(Menu.Name + "WTarget").GetValue<bool>()) 
            {
                if (EWQ)
                {
                    Utility.DelayAction.Add(170, ()=> Spells.Spell[SpellSlot.W].Cast(Target.Position));
                }

                if (Target.IsInvulnerable || Target.CountEnemiesInRange(1000) < Target.CountAlliesInRange(1000))
                {
                    Spells.Spell[SpellSlot.W].Cast(wPrediction.CastPosition);
                }
            }

            if (wPrediction.Hitchance < HitChance.Immobile || !Menu.Item(Menu.Name + "WImmobile").GetValue<bool>()) return;

            Spells.Spell[SpellSlot.W].Cast(wPrediction.CastPosition);
        }
    }
}
