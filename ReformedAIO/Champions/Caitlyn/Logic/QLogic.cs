namespace ReformedAIO.Champions.Caitlyn.Logic
{
    using LeagueSharp;
    using LeagueSharp.SDK;

    internal class QLogic
    {
        public float QDelay(Obj_AI_Hero target)
        {
            var time = target.Distance(Vars.Player)/Spells.Spell[SpellSlot.Q].Speed;

            return time + Spells.Spell[SpellSlot.Q].Delay;
        }
    }
}
