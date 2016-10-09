namespace NechritoRiven.Riven.Active.OrbwalkingMode.Combo
{
    using System;

    using Active_Logic;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Logic.Spells;

    using RethoughtLib.FeatureSystem.Implementations;

    using Orbwalking = Orbwalking;

    internal class Combo : OrbwalkingChild
    {
        public override string Name { get; set; } = "Combo";

        private ActiveLogic activeLogic;

        private Obj_AI_Hero target;

        private readonly RivenQ Q;

        private readonly RivenW W;

        private readonly RivenE E;

        private readonly RivenR R;

        public Combo(RivenQ Q, RivenW W, RivenE E, RivenR R)
        {
            this.Q = Q;
            this.W = W;
            this.E = E;
            this.R = R;
        }

        private void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe || !Orbwalking.IsAutoAttack(args.SData.Name) || !Q.Spell.IsReady())
            {
                return;
            }

            var target = TargetSelector.GetTarget(350, TargetSelector.DamageType.Physical);

            if (target == null)
            {
                return;
            }

            //if (activeLogic.CanQ(target))
            {
                Q.Spell.Cast(target);
            }
        }

        private void R2Logic(Obj_AI_Base target)
        {
            if (target.Distance(ObjectManager.Player) < 300)
            {
                return;
            }

            var prediction = R.Spell.GetPrediction(target, true, collisionable: new[] { CollisionableObjects.YasuoWall });

            if (prediction.Hitchance > HitChance.Medium || prediction.AoeTargetsHitCount > 1)
            {
                R.Spell.Cast(prediction.CastPosition);
            }
        }

        private void AllIn(Obj_AI_Base target)
        {
            if (E.Spell.IsReady())
            {
                E.Spell.Cast(target.Position);
            }

            if (R.IsR1() && R.Spell.IsReady() && Menu.Item("ToggleR1").GetValue<bool>())
            {
                R.Spell.Cast();
            }

            if (!W.Spell.IsReady() || !activeLogic.CanW(target))
            {
                return;
            }

            W.Spell.Cast();
        }

        private void Efficent(Obj_AI_Base target)
        {
            if (E.Spell.IsReady() && !activeLogic.InRange(target))
            {
                E.Spell.Cast(target.Position);
            }

            if (R.IsR1() && R.Spell.IsReady() && Menu.Item("ToggleR1").GetValue<bool>()
                && R.GetDamage(target) * 2.35 > target.Health)
            {
                R.Spell.Cast();
            }

            if (!W.Spell.IsReady() || !activeLogic.InRange(target))
            {
                return;
            }

            if (Menu.Item("SmartW").GetValue<bool>() && Q.QStack > 1)
            {
                W.Spell.Cast();
            }
            else
            {
                W.Spell.Cast();
            }
        }

        private void OnGameUpdate(EventArgs args)
        {
            if (R.IsR2())
            {
                R2Logic(target);
            }

            target = TargetSelector.GetTarget(480, TargetSelector.DamageType.Physical);

            if (target == null)
            {
                return;
            }

            switch (Menu.Item("Mode").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    AllIn(target);
                    break;
                case 1:
                    Efficent(target);
                    break;
                case 2:
                    if (ObjectManager.Player.HealthPercent > target.HealthPercent
                        || ObjectManager.Player.CountAlliesInRange(1500)
                        > ObjectManager.Player.CountEnemiesInRange(1500))
                    {
                        goto case 0;
                    }

                    goto case 1;
            }
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            Menu.AddItem(new MenuItem("R2MaxDmg", "R2 Max Damage")).SetValue(true);

            Menu.AddItem(new MenuItem("ToggleR1", "Use R1")).SetValue(true);

            Menu.AddItem(new MenuItem("SmartW", "Smart W")).SetValue(true);

            Menu.AddItem(new MenuItem("Mode", "Combo Mode")).SetValue(new StringList(new []{ "All In", "Efficent", "Automatic" }));

            activeLogic = new ActiveLogic();
        }

        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            Obj_AI_Base.OnDoCast -= OnDoCast;
            Game.OnUpdate -= OnGameUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            Obj_AI_Base.OnDoCast += OnDoCast;
            Game.OnUpdate += OnGameUpdate;
        }
    }
}
