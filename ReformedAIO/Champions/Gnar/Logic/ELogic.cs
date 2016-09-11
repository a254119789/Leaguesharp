using LeagueSharp;
using LeagueSharp.Common;
using ReformedAIO.Champions.Gnar.Core;
using SharpDX;

namespace ReformedAIO.Champions.Gnar.Logic
{
    internal class ELogic
    {
        private readonly GnarState gnarState = new GnarState();

        public PredictionOutput EPrediction(Obj_AI_Base x)
        {
            PredictionOutput kek = null;

            if (gnarState.Mini)
            {
              kek = Spells.E.GetPrediction(x);
            }
            if (gnarState.Mega)
            {
              kek = Spells.E2.GetPrediction(x);
            }

            return kek;
        }
    }
}
