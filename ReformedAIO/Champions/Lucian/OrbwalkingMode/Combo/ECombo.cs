namespace ReformedAIO.Champions.Lucian.OrbwalkingMode.Combo
{
    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Lucian.Core.Spells;
    using ReformedAIO.Champions.Lucian.Logic.Damage;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using SharpDX;

    class ECombo : ChildBase
    {
        public override string Name { get; set; } = "E";

        private readonly LucDamage damage;

        private readonly ESpell eSpell;

        private readonly Orbwalking.Orbwalker orbwalker;

        public ECombo(ESpell eSpell, Orbwalking.Orbwalker orbwalker, LucDamage damage)
        {
            this.eSpell = eSpell;
            this.orbwalker = orbwalker;
            this.damage = damage;
        }

        private void OnUpdate(EventArgs args)
        {
            var target = TargetSelector.GetTarget(1150, TargetSelector.DamageType.Physical);

            if (target == null || !target.IsValidTarget(1150) || !Menu.Item("Execute").GetValue<bool>()
                || damage.GetComboDamage(target) < target.Health)
            {
                return;
            }


            eSpell.Spell.Cast(ObjectManager.Player.Position.Extend(Game.CursorPos, Menu.Item("EDistance").GetValue<Slider>().Value));
        }

        private void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe
                || ObjectManager.Player.HasBuff("LucianPassiveBuff")
                || !Orbwalking.IsAutoAttack(args.SData.Name)
                || !eSpell.Spell.IsReady()
                || Menu.Item("EMana").GetValue<Slider>().Value > ObjectManager.Player.ManaPercent)
            {
                return;
            }

            var heroes = HeroManager.Enemies.Where(x => x.IsValidTarget(Orbwalking.GetRealAutoAttackRange(ObjectManager.Player) + 400));

            foreach (var target in heroes as Obj_AI_Hero[] ?? heroes.ToArray())
            {
                if (target.Health < damage.GetComboDamage(target) && ObjectManager.Player.HealthPercent > target.HealthPercent)
                {
                    eSpell.Spell.Cast(target.Position);
                }
                else
                {
                    if (target.UnderTurret(true))
                    {
                        return;
                    }

                    switch (Menu.Item("EMode").GetValue<StringList>().SelectedIndex)
                    {
                        case 0:
                            eSpell.Spell.Cast(ObjectManager.Player.Position.Extend(Game.CursorPos, Menu.Item("EDistance").GetValue<Slider>().Value));
                            break;
                        case 1:
                            eSpell.Spell.Cast(ObjectManager.Player.Position.Extend(target.Position, Menu.Item("EDistance").GetValue<Slider>().Value));
                            break;
                        case 2: 
                            // CREDITS TO WHOEVER MADE THE DEVIATION 
                            eSpell.Spell.Cast(eSpell.Deviation(ObjectManager.Player.Position.To2D(), target.Position.To2D(), Menu.Item("EDistance").GetValue<Slider>().Value).To3D());
                            break;
                    }
                }
            }
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            Menu.AddItem(new MenuItem("EMode", "Mode").SetValue(new StringList(new [] {"Cursor", "Target Position", "Side"})));
            Menu.AddItem(new MenuItem("Execute", "Dive E If Killable").SetValue(true));
            Menu.AddItem(new MenuItem("EDistance", "E Distance").SetValue(new Slider(65, 1, 425)).SetTooltip("Less = Faster"));
            Menu.AddItem(new MenuItem("EMana", "Min Mana %").SetValue(new Slider(5, 0, 100)));

           // damage = new LucDamage();
        }

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate -= OnUpdate;
            Obj_AI_Base.OnDoCast -= OnDoCast;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate += OnUpdate;
            Obj_AI_Base.OnDoCast += OnDoCast;
        }
    }
}
