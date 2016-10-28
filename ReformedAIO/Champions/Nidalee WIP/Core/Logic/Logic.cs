namespace ReformedAIO.Champions.Nidalee_WIP.Core.Logic
{
    using System;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    internal sealed class Logic
    {
        public bool IsReady(float time, float extra)
        {
            return time <= extra;
        }

        public bool IsCatForm()
        {
            return ObjectManager.Player.GetSpell(SpellSlot.Q).Name != "JavelinToss";
        }

        public bool Humanform()
        {
            return ObjectManager.Player.AttackRange > 300;
        }

        public bool IsHunted(Obj_AI_Base target)
        {
            return target.HasBuff("nidaleepassivehunted");
        }

        public Dictionary<string, float> SpellTimer = new Dictionary<string, float>
        {
            { "Takedown", 0f },
            { "Pounce", 0f },
            { "ExPounce", 0f },
            { "Swipe", 0f },
            { "Javelin", 0f },
            { "Bushwhack", 0f },
            { "Primalsurge", 0f },
            { "Aspect", 0f  }
        };

        public Dictionary<string, float> TimeStamp = new Dictionary<string, float>
        {
            { "Takedown", 0f },
            { "Pounce", 0f },
            { "Swipe", 0f },
            { "Javelin", 0f },
            { "Bushwhack", 0f },
            { "Primalsurge", 0f },
        };

        public void SpellsOnUpdate(EventArgs args)
        {
            SpellTimer["Javelintoss"] = TimeStamp["Javelintoss"] - Game.Time > 0
                ? TimeStamp["Javelintoss"] - Game.Time
                : 0;

            SpellTimer["Bushwhack"] = TimeStamp["Bushwhack"] - Game.Time > 0
                ? TimeStamp["Bushwhack"] - Game.Time
                : 0;

            SpellTimer["Primalsurge"] = TimeStamp["Primalsurge"] - Game.Time > 0
                ? TimeStamp["Primalsurge"] - Game.Time
                : 0;

            SpellTimer["Takedown"] = TimeStamp["Takedown"] - Game.Time > 0
               ? TimeStamp["Takedown"] - Game.Time
               : 0;

            SpellTimer["Pounce"] = TimeStamp["Pounce"] - Game.Time > 0
                ? TimeStamp["Pounce"] - Game.Time
                : 0;

            SpellTimer["Swipe"] = TimeStamp["Swipe"] - Game.Time > 0
                ? TimeStamp["Swipe"] - Game.Time
                : 0;

            SpellTimer["Aspect"] = TimeStamp["Aspect"] - Game.Time > 0
                ? TimeStamp["Aspect"] - Game.Time
                : 0;
        }

        public void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.Name.ToLower() == "pounce")
            {
                var unit = args.Target as Obj_AI_Base;

                if (unit.IsValidTarget() && IsHunted(unit))
                {
                    TimeStamp["Pounce"] = Game.Time + args.SData.Cooldown * 0.3f;
                }
                else
                {
                    TimeStamp["Pounce"] = Game.Time + args.SData.Cooldown;
                }
            }

            if (sender.IsMe && args.SData.Name.ToLower() == "swipe")
            {
                TimeStamp["Swipe"] = Game.Time + args.SData.Cooldown;
            }

            if (sender.IsMe && args.SData.Name.ToLower() == "primalsurge")
            {
                TimeStamp["Primalsurge"] = Game.Time + args.SData.Cooldown;
            }

            if (sender.IsMe && args.SData.Name.ToLower() == "bushwhack")
            {
                TimeStamp["Bushwhack"] = Game.Time + args.SData.Cooldown;
            }

            if (sender.IsMe && args.SData.Name.ToLower() == "javelintoss")
            {
                TimeStamp["Javelintoss"] = Game.Time + args.SData.Cooldown;
            }

            if (sender.IsMe && args.SData.Name.ToLower() == "takedown")
            {
                if (!Humanform() && !ObjectManager.Player.HasBuff("Takedown"))
                {
                    TimeStamp["Takedown"] = Game.Time + args.SData.Cooldown;
                }
            }

            if (sender.IsMe && args.SData.Name.ToLower() == "aspectofthecougar")
            {
                TimeStamp["Aspect"] = Game.Time + args.SData.Cooldown;
            }
        }
    }
}
