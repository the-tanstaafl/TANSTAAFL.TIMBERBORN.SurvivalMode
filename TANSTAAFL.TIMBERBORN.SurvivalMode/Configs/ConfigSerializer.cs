using System;
using System.Collections.Generic;
using System.Text;
using Timberborn.Persistence;

namespace TANSTAAFL.TIMBERBORN.SurvivalMode.Configs
{
    public class ConfigSerializer : IObjectSerializer<Config>
    {
        private static readonly PropertyKey<int> DayKey = new PropertyKey<int>("Day");
        private static readonly PropertyKey<float> HourKey = new PropertyKey<float>("Hour");
        private static readonly PropertyKey<bool> PunishmentActiveKey = new PropertyKey<bool>("PunishmentActive");
        private static readonly PropertyKey<float> WorkingHoursKey = new PropertyKey<float>("WorkingHours");

        public Obsoletable<Config> Deserialize(IObjectLoader objectLoader)
        {
            return new Config(objectLoader.Get(DayKey), objectLoader.Get(HourKey),
                objectLoader.Get(PunishmentActiveKey), objectLoader.Get(WorkingHoursKey));
        }

        public void Serialize(Config value, IObjectSaver objectSaver)
        {
            objectSaver.Set(DayKey, value.Day);
            objectSaver.Set(HourKey, value.Hour);
            objectSaver.Set(PunishmentActiveKey, value.PunishmentActive);
            objectSaver.Set(WorkingHoursKey, value.WorkingHours);
        }
    }
}
