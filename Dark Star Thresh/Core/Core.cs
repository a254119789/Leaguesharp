namespace Dark_Star_Thresh.Core
{
    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using HitChance = SebbyLib.Prediction.HitChance;
    using PredictionInput = SebbyLib.Prediction.PredictionInput;
    using SkillshotType = SebbyLib.Prediction.SkillshotType;

    internal class Core
    {
        public static Orbwalking.Orbwalker Orbwalker;

        public static Obj_AI_Hero Player => ObjectManager.Player;

        public static float GetStunDuration(Obj_AI_Base target)
        {
            return target.Buffs.Where(b => b.IsActive && Game.Time < b.EndTime
                && (b.Type == BuffType.Charm
                || b.Type == BuffType.Stun
                || b.Type == BuffType.Suppression 
                || b.Type == BuffType.Snare)).Aggregate(0f, (current, buff) => Math.Max(current, buff.EndTime)) - Game.Time;
        }

        public static void CastQ(Obj_AI_Base target)
        {
            if (target == null || MenuConfig.Config.Item("blacklist" + target.CharData.BaseSkinName).GetValue<bool>())
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

                    if (MenuConfig.Hitchance.SelectedIndex == 0 && prediction.Hitchance < HitChance.High)
                    {
                        if (MenuConfig.Debug)
                        {
                            Console.WriteLine("OKTW: High");
                        }
                        return;
                    }

                    if (MenuConfig.Hitchance.SelectedIndex == 1 && prediction.Hitchance < HitChance.VeryHigh)
                    {
                        if (MenuConfig.Debug)
                        {
                            Console.WriteLine("OKTW: Very High");
                        }
                        return;
                    }

                    Spells.Q.Cast(prediction.CastPosition);

                    break;

                case 1:

                    var commonPred = Spells.Q.GetPrediction(target);


                    if (MenuConfig.Hitchance.SelectedIndex == 0 && commonPred.Hitchance < LeagueSharp.Common.HitChance.High)
                    {
                        if (MenuConfig.Debug)
                        {
                            Console.WriteLine("L# Common: High");
                        }
                        return;
                    }

                    if (MenuConfig.Hitchance.SelectedIndex == 1 && commonPred.Hitchance < LeagueSharp.Common.HitChance.VeryHigh)
                    {
                        if (MenuConfig.Debug)
                        {
                            Console.WriteLine("L# Common: Very High");
                        }
                        return;
                    }

                    Spells.Q.Cast(commonPred.CastPosition);
                    break;
            }
        }
    }
}
