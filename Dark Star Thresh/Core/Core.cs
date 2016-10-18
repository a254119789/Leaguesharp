namespace Dark_Star_Thresh.Core
{
    using LeagueSharp;
    using LeagueSharp.Common;

    using HitChance = SebbyLib.Prediction.HitChance;
    using PredictionInput = SebbyLib.Prediction.PredictionInput;
    using SkillshotType = SebbyLib.Prediction.SkillshotType;

    internal class Core
    {
        public static Orbwalking.Orbwalker Orbwalker;

        public static Obj_AI_Hero Player => ObjectManager.Player;

        public static void CastQ(Obj_AI_Base target)
        {
            if (target == null)
            {
                return;
            }

            switch (MenuConfig.PredMode.SelectedIndex)
            {
                case 0:

                    var predictionInput = new PredictionInput
                                   {
                        Aoe = false,
                        Collision = true,
                        Speed = 1900f,
                        Delay = .5f,
                        Range = 1100f,
                        From = Player.ServerPosition, Radius = 60f, Unit = target,
                        Type = SkillshotType.SkillshotLine
                                   };

                    var prediction = SebbyLib.Prediction.Prediction.GetPrediction(predictionInput);

                    if (prediction.Hitchance < HitChance.High)
                    {
                        break;
                    }

                    Spells.Q.Cast(prediction.CastPosition);

                    break;

                case 1:

                    var commonPred = Spells.Q.GetPrediction(target);

                    if (commonPred.Hitchance < LeagueSharp.Common.HitChance.High)
                    {
                        break;
                    }

                    Spells.Q.Cast(commonPred.CastPosition);
                    break;
            }
        }
    }
}
