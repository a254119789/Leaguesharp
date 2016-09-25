namespace NechritoRiven.Riven.Active.Active_Logic
{
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Logic.Spells;

    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Switches;

    public class ActiveLogic : ChildBase
    {
        #region Fields

        private readonly RivenQ RivenQ;

        private readonly RivenR RivenR;

        private readonly RivenW RivenW;

        private bool canItem;

        private bool canQ;

        private bool canR1;

        private bool canW;

        #endregion

        #region Constructors and Destructors

        //public ActiveLogic(RivenQ q, RivenW w, RivenR r)
        //{
        //    RivenQ = q;
        //    RivenW = w;
        //    RivenR = r;
        //}

        #endregion

        #region Properties

        private static int Item =>
              Items.CanUseItem(3077) && Items.HasItem(3077)
            ? 3077 
            : Items.CanUseItem(3074) && Items.HasItem(3074)
            ? 3074 
            : Items.CanUseItem(3748) && Items.HasItem(3748) 
            ? 3748 : 0;

        private AttackableUnit Unit { get; set; }

        #endregion

        #region Public Methods and Operators

        public bool CanQ(AttackableUnit unit)
        {
            return canQ && InRange(unit);
        }

        public void CastQ(AttackableUnit target)
        {
            Unit = target;
            canQ = RivenQ.Spell.IsReady();
        }

        public void CastW(Obj_AI_Base x)
        {
            canW = RivenW.Spell.IsReady();
        }

        public bool CanW(AttackableUnit unit)
        {
            return canW && InRange(unit);
        }

        public bool InRange(AttackableUnit x)
        {
            return ObjectManager.Player.HasBuff("RivenFengShuiEngine")
            ? ObjectManager.Player.Distance(x.Position) <= 330
            : ObjectManager.Player.Distance(x.Position) <= 265;
        }

        public void OnCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            var name = args.SData.Name;

            if (name.Contains("ItemTiamatCleave"))
            {
                canItem = false;
            }

            if (name.Contains("RivenTriCleave"))
            {
                canQ = false;
            }

            if (name.Contains("RivenMartyr"))
            {
             // canQ = false;
                canW = false;
            }

            //if (name == RivenR.R1)
            //{
            //    canR1 = false;
            //}
        }

        public void UseItem()
        {
            if (Items.CanUseItem(Item) && Item != 0)
            {
                canItem = true;
            }
        }

        #endregion

        #region Methods

        public void CastSkill(EventArgs args)
        {
            if (CanQ(Unit))
            {
                Console.WriteLine("HEEEEELLLOOOOOOO");
                if (canItem && Items.CanUseItem(Item) && Item != 0)
                {
                    Items.UseItem(Item);
                    Utility.DelayAction.Add(2, () => RivenQ.Spell.Cast(Unit.Position));
                }
                else
                {
                    RivenQ.Spell.Cast(Unit.Position);
                }
            }

            if (CanW(Unit))
            {
                if (canItem && Items.CanUseItem(Item) && Item != 0)
                {
                    Items.UseItem(Item);
                    Utility.DelayAction.Add(3, () => RivenW.Spell.Cast());
                }
                else
                {
                    RivenW.Spell.Cast();
                }
            }

            if (canR1 && RivenR.IsR1())
            {
                RivenR.Spell.Cast();
            }
        }

        #endregion

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);
        }

        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            Obj_AI_Base.OnDoCast -= OnCast;
            Game.OnUpdate -= CastSkill;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            Obj_AI_Base.OnDoCast += OnCast;
            Game.OnUpdate += CastSkill;
        }

        //protected override void SetSwitch()
        //{
        //    Switch = new UnreversibleSwitch(Menu);
        //}

        public override string Name { get; set; } = "Temp Logic";
    }
}
