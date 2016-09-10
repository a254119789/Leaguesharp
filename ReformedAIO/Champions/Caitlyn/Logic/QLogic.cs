using LeagueSharp;
using LeagueSharp.SDK;

namespace ReformedAIO.Champions.Caitlyn.Logic
{
    internal class QLogic
    {
        public float QDelay(Obj_AI_Hero target)
        {
            var time = target.Distance(Vars.Player)/Spells.Spell[SpellSlot.Q].Speed;

            return time + Spells.Spell[SpellSlot.Q].Delay;
        }

        public bool CanKillSteal(Obj_AI_Hero target)
        {
            return QDelay(target) > target.MoveSpeed;
        }
    }
}
