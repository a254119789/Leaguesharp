namespace NechritoRiven.Event.OrbwalkingModes
{
    #region

    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using NechritoRiven.Core;
    using NechritoRiven.Menus;

    using Orbwalking = Orbwalking;

    #endregion

    internal class QAA : Core
    {
        #region Public Methods and Operators

        // Jungle, Combo etc.
        public static void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe || !Orbwalking.IsAutoAttack(args.SData.Name)) return;

            var a = HeroManager.Enemies.Where(x => x.IsValidTarget(Player.AttackRange + 360));

            var targets = a as Obj_AI_Hero[] ?? a.ToArray();

            foreach (var target in targets)
            {
                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
                {
                    if (Spells.Q.IsReady())
                    {
                        ForceItem();
                        Utility.DelayAction.Add(1, () => ForceCastQ(target)); // Else Q AA wont go off at all times, cause of tiamat.
                    }
                    else
                    {
                        ForceItem();
                    }

                    if (MenuConfig.NechLogic && InWRange(target) && (Qstack != 1 || !Spells.Q.IsReady()))
                    {
                        ForceW();
                        return;
                    }
                }

                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
                {
                    if (Qstack == 2)
                    {
                        ForceItem();
                        Utility.DelayAction.Add(1, () => ForceCastQ(target));
                    }
                }

                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.FastHarass)
                {
                    if (Spells.Q.IsReady() && InQRange(target))
                    {
                        Utility.DelayAction.Add(1, () => ForceCastQ(target));
                    }

                    if (Spells.W.IsReady() && !Spells.Q.IsReady() && InWRange(target))
                    {
                        Spells.W.Cast(target);
                    }
                }

                if (Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Burst) return;

                if (!Spells.Q.IsReady()) return;

                ForceItem();
                Utility.DelayAction.Add(1, () => ForceCastQ(target));
            }

            if (Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear) return;

            if (args.Target is Obj_AI_Minion)
            {
                var minions = MinionManager.GetMinions(Player.AttackRange + 450);

                if (minions == null)
                {
                    return;
                }

                foreach (var m in minions)
                {

                    if (!Spells.Q.IsReady() || !MenuConfig.LaneQ || m.UnderTurret(true))
                    {
                        return;
                    }

                    ForceItem();
                    Utility.DelayAction.Add(1, () => ForceCastQ(m));
                }
            }

            var mobs = MinionManager.GetMinions(Player.Position, 400f, MinionTypes.All, MinionTeam.Neutral);

            if (mobs == null) return;

            foreach (var m in mobs)
            {
                if (Spells.Q.IsReady() && MenuConfig.JnglQ)
                {
                    ForceItem();
                    Utility.DelayAction.Add(1, () => ForceCastQ(m));
                }
                else
                {
                    ForceItem();
                }

                if (!Spells.W.IsReady() || !MenuConfig.JnglW || Player.HasBuff("RivenFeint") || Qstack > 2) return;

                ForceItem();
                Spells.W.Cast(m);
            }

            var inhib = args.Target as Obj_BarracksDampener;
            if (inhib != null)
            {
                if (inhib.IsValid && Spells.Q.IsReady() && MenuConfig.LaneQ)
                {
                    Spells.Q.Cast(inhib.Position - 350);
                }
            }

            var turret = args.Target as Obj_AI_Turret;

            if (turret == null) return;

            if (turret.IsValid && Spells.Q.IsReady() && MenuConfig.LaneQ)
            {
                Spells.Q.Cast(turret.Position - 250);
            }
        }

        #endregion
    }
}