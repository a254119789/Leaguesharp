namespace ReformedAIO.Champions.Yasuo.OrbwalkingMode.Lane
{
    using System;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Yasuo.Core.Spells;
    using ReformedAIO.Library.Dash_Handler;
    using ReformedAIO.Library.Get_Information.HeroInfo;

    using RethoughtLib.FeatureSystem.Implementations;

    internal sealed class QLane : OrbwalkingChild
    {
        public override string Name { get; set; } = "Q";

        private readonly Q1Spell qSpell;

        private readonly Q3Spell q3Spell;

        private DashPosition dashPos;

        public QLane(Q1Spell qSpell, Q3Spell q3Spell)
        {
            this.qSpell = qSpell;
            this.q3Spell = q3Spell;
        }

        private float Range => ObjectManager.Player.HasBuff("YasuoQ3W") ? q3Spell.Spell.Range : qSpell.Spell.Range;

        private List<Obj_AI_Base> Minion => MinionManager.GetMinions(ObjectManager.Player.Position, Range);

        private void OnUpdate(EventArgs args)
        {
            if (Minion == null || !CheckGuardians())
            {
                return;
            }

            foreach (var m in Minion)
            {
                if (q3Spell.Active)
                {
                    var pred = q3Spell.Spell.GetPrediction(m, true);

                    if (ObjectManager.Player.IsDashing() && (m.Health < qSpell.GetDamage(m) || dashPos.DashEndPosition(m, 475).Distance(pred.UnitPosition) > qSpell.Spell.Range))
                    {
                        return;
                    }

                    if (Menu.Item("LQ3").GetValue<Slider>().Value >= pred.AoeTargetsHitCount)
                    {
                        switch (Menu.Item("LHitchance").GetValue<StringList>().SelectedIndex)
                        {
                            case 0:
                                if (pred.Hitchance >= HitChance.Medium)
                                {
                                    q3Spell.Spell.Cast(pred.CastPosition);
                                }
                                break;
                            case 1:
                                if (pred.Hitchance >= HitChance.High)
                                {
                                    q3Spell.Spell.Cast(pred.CastPosition);
                                }
                                break;
                            case 2:
                                if (pred.Hitchance >= HitChance.VeryHigh)
                                {
                                    q3Spell.Spell.Cast(pred.CastPosition);
                                }
                                break;
                        }
                    }
                }
                else
                {
                    qSpell.Spell.Cast(m);
                }
            }
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

            Menu.AddItem(new MenuItem("LQ3", "Use Q3 If X Hit Count").SetValue(new Slider(4, 0, 7)));

            Menu.AddItem(new MenuItem("LHitchance", "Hitchance").SetValue(new StringList(new[] { "Medium", "High", "Very High" }, 1)));
        }
    }
}