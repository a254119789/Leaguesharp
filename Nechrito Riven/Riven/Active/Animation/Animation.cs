namespace NechritoRiven.Riven.Active.Animation
{
    using LeagueSharp;
    using LeagueSharp.Common;

    using Logic.Spells;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using Orbwalking = Orbwalking;

    internal class Animation : ChildBase
    {
        #region Fields

        private readonly Orbwalking.Orbwalker orbwalker;

        #endregion

        #region Constructors and Destructors

        public Animation(RivenQ q, Orbwalking.Orbwalker orbwalker)
        {
            Q = q;
            this.orbwalker = orbwalker;
        }

        #endregion

        #region Public Properties

        public override string Name { get; set; } = "Animation";

        public RivenQ Q { get; }

        #endregion

        #region Properties

        private static int Ping => Game.Ping / 2;

        #endregion

        #region Public Methods and Operators

        protected void OnPlay(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            switch (args.Animation)
            {
                case "Spell1a":
                    Q.LastQ = Utils.GameTimeTickCount;
                    Q.QStack = 2;

                    if (SafeReset())
                    {
                        Utility.DelayAction.Add(Menu.Item("Q1Delay").GetValue<Slider>().Value + Ping, Reset);
                    }

                    break;
                case "Spell1b":
                    Q.LastQ = Utils.GameTimeTickCount;
                    Q.QStack = 3;

                    if (SafeReset())
                    {
                        Utility.DelayAction.Add(Menu.Item("Q2Delay").GetValue<Slider>().Value + Ping, Reset);
                    }

                    break;
                case "Spell1c":
                    Q.LastQ = Utils.GameTimeTickCount;
                    Q.QStack = 1;

                    if (SafeReset())
                    {
                        Utility.DelayAction.Add(Menu.Item("Q3Delay").GetValue<Slider>().Value + Ping, Reset);
                    }

                    break;
            }

        }

        #endregion

        #region Methods

        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            Obj_AI_Base.OnPlayAnimation -= OnPlay;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            Obj_AI_Base.OnPlayAnimation += OnPlay;
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            Menu.AddItem(new MenuItem("Q1Delay", "Q1 Delay")).SetValue(new Slider(230, 230, 350));

            Menu.AddItem(new MenuItem("Q2Delay", "Q2 Delay")).SetValue(new Slider(230, 230, 350));

            Menu.AddItem(new MenuItem("Q3Delay", "Q3 Delay")).SetValue(new Slider(360, 345, 400));

            Menu.AddItem(new MenuItem("SemiQ", "Semi Q")).SetValue(false);

            Menu.AddItem(new MenuItem("EmoteList", "Emotes").SetValue(new StringList(new[] { "Dance", "Laugh", "Taunt", "Joke" })));
        }

        protected void Emotes()
        {
            switch (Menu.Item("EmoteList").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    Game.SendEmote(Emote.Dance);
                    break;
                case 1:
                    Game.SendEmote(Emote.Laugh);
                    break;
                case 2:
                    Game.SendEmote(Emote.Taunt);
                    break;
                case 3:
                    Game.SendEmote(Emote.Joke);
                    break;
            }
        }

        protected void Reset()
        {
            Emotes();
            ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos, false);
            Orbwalking.LastAaTick = 0;
        }

        protected bool SafeReset() => orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None || Menu.Item("SemiQ").GetValue<bool>();

        #endregion
    }
}
