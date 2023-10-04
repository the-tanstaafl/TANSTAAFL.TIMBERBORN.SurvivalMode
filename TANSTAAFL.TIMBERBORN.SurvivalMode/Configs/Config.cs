using System;
using System.Collections.Generic;
using System.Text;

namespace TANSTAAFL.TIMBERBORN.SurvivalMode.Configs
{
    public class Config
    {
        public Config() { }
        public Config(int day, float hour, bool punishmentActive, float workingHours)
        {
            Day = day;
            Hour = hour;
            PunishmentActive = punishmentActive;
            WorkingHours = workingHours;
        }

        public int Day { get; set; }
        public float Hour { get; set; }
        public bool PunishmentActive { get; set; }
        public float WorkingHours { get; set; }
    }
}
