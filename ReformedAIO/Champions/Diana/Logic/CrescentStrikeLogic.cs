﻿namespace ReformedAIO.Champions.Diana.Logic
{
    #region Using Directives

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    using SPrediction;

    #endregion

    internal class CrescentStrikeLogic
    {
        #region Fields

        protected Obj_AI_Hero Target;

        #endregion

        #region Public Methods and Operators

        //public float misayaDelay(Obj_AI_Hero target)
        //{
        //    var dist = Variables.Player.ServerPosition.Distance(target.ServerPosition);
        //    var delay = Variables.Spells[SpellSlot.Q].Delay;
        //    var speed = Variables.Spells[SpellSlot.Q].Speed;
        //    var movespeed = Variables.Player.MoveSpeed;

        //    var time = 0f;

        //    if (dist > delay)
        //    {

        //        time = (dist / (movespeed + speed));
        //    }

        //    return (time + delay);
        //}

        public float GetDmg(Obj_AI_Base x)
        {
            if (x == null)
            {
                return 0;
            }

            var dmg = 0f;

            if (Variables.Player.HasBuff("SummonerExhaust")) dmg = dmg * 0.6f;

            if (Variables.Spells[SpellSlot.Q].IsReady()) dmg = dmg + Variables.Spells[SpellSlot.Q].GetDamage(x);

            return dmg;
        }

        public float QDelay(Obj_AI_Hero target)
        {
            var time = target.Distance(Variables.Player) / Variables.Spells[SpellSlot.Q].Speed;

            return time + Variables.Spells[SpellSlot.Q].Delay;
        }

        public Vector3 QPred(Obj_AI_Hero target)
        {
            var pos = Variables.Spells[SpellSlot.Q].GetArcSPrediction(target);

            return pos.CastPosition.To3D() + this.QDelay(target);
        }

        #endregion
    }
}