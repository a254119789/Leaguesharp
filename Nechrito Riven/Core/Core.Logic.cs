namespace NechritoRiven.Core
{
    #region

    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    #endregion

    internal partial class Core
    {
        #region Static Fields

        /// <summary>
        ///     The e anti spell.
        /// </summary>
        public static List<string> EAntiSpell = new List<string>
                                                    {
                                                        "MonkeyKingSpinToWin", "KatarinaRTrigger", "HungeringStrike",
                                                        "TwitchEParticle", "RengarPassiveBuffDashAADummy",
                                                        "RengarPassiveBuffDash", "IreliaEquilibriumStrike",
                                                        "BraumBasicAttackPassiveOverride", "gnarwproc",
                                                        "hecarimrampattack", "illaoiwattack", "JaxEmpowerTwo",
                                                        "JayceThunderingBlow", "RenektonSuperExecute",
                                                        "vaynesilvereddebuff"
                                                    };

        public static float LastQ;

        /// <summary>
        ///     The targeted anti spell.
        /// </summary>
        public static List<string> TargetedAntiSpell = new List<string>
                                                           {
                                                               "MonkeyKingQAttack", "YasuoDash", "FizzPiercingStrike",
                                                               "RengarQ", "GarenQAttack", "GarenRPreCast",
                                                               "PoppyPassiveAttack", "viktorqbuff", "FioraEAttack"
                                                           };

        /// <summary>
        ///     The w anti spell.
        /// </summary>
        public static List<string> WAntiSpell = new List<string>
                                                    {
                                                        "RenektonPreExecute", "TalonCutthroat", "IreliaEquilibriumStrike",
                                                        "XenZhaoThrust3", "KatarinaRTrigger", "KatarinaE",
                                                        "MonkeyKingSpinToWin"
                                                    };

        private static bool forceItem;

        private static bool forceQ;

        private static bool forceR;

        private static bool forceW;

        #endregion

        #region Public Properties

        public static AttackableUnit QTarget { get; private set; }

        public static int WRange => Player.HasBuff("RivenFengShuiEngine") ? 330 : 265;

        #endregion

        #region Properties

        private static int Item
            =>
                Items.CanUseItem(3077) && Items.HasItem(3077)
                    ? 3077
                    : Items.CanUseItem(3074) && Items.HasItem(3074) ? 3074 : 0;

        #endregion

        #region Public Methods and Operators

        public static void FlashW()
        {
            var target = TargetSelector.GetSelectedTarget();

            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            Spells.W.Cast();
            Utility.DelayAction.Add(10, () => Player.Spellbook.CastSpell(Spells.Flash, target.Position));
        }

        public static void ForceCastQ(AttackableUnit target)
        {
            forceQ = true;
            qTarget = target;
        }

        public static void ForceItem()
        {
            if (Items.CanUseItem(Item) && Items.HasItem(Item) && Item != 0) forceItem = true;
            Utility.DelayAction.Add(500, () => forceItem = false);
        }

        public static void ForceQ(AttackableUnit target)
        {
            forceQ = true;
            QTarget = target;
        }

        public static void ForceR()
        {
            forceR = Spells.R.IsReady() && Spells.R.Instance.Name == IsFirstR;
            {
                Utility.DelayAction.Add(500, () => forceR = false);
            }
        }

        public static void ForceSkill()
        {
            if (forceQ && qTarget != null && qTarget.IsValidTarget(Spells.E.Range + Player.BoundingRadius + 70)
                && Spells.Q.IsReady())
            {
                Spells.Q.Cast(qTarget.Position);
            }

            if (forceW)
            {
                Spells.W.Cast();
            }

            if (forceR && Spells.R.Instance.Name == IsFirstR)
            {
                Spells.R.Cast();
            }

            if (forceItem && Items.CanUseItem(Item) && Items.HasItem(Item) && Item != 0)
            {
                Items.UseItem(Item);
            }
        }

        public static void ForceW()
        {
            forceW = Spells.W.IsReady();
            Utility.DelayAction.Add(500, () => forceW = false);
        }

        public static bool InQRange(GameObject target)
        {
            return target != null
                   && (Player.HasBuff("RivenFengShuiEngine")
                           ? Player.Distance(target.Position) <= 330
                           : Player.Distance(target.Position) <= 265);
        }

        public static bool InWRange(Obj_AI_Base t) => t != null && t.IsValidTarget(WRange);

        public static void OnCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;

            if (args.SData.Name.Contains("ItemTiamatCleave")) forceItem = false;
            if (args.SData.Name.Contains("RivenTriCleave")) forceQ = false;
            if (args.SData.Name.Contains("RivenMartyr")) forceW = false;
            if (args.SData.Name == IsFirstR) forceR = false;
        }

        #endregion
    }
}