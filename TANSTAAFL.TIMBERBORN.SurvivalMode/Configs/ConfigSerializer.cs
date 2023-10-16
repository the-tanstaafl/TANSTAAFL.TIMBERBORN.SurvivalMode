using System;
using System.Collections.Generic;
using System.Text;
using TANSTAAFL.TIMBERBORN.SurvivalMode.Events;
using Timberborn.Persistence;

namespace TANSTAAFL.TIMBERBORN.SurvivalMode.Configs
{
    public class ConfigSerializer : IObjectSerializer<Config>
    {
        private static readonly PropertyKey<int> GeneratorLastEventCycleKey = new PropertyKey<int>("Generator.LastEventCycle");
        private static readonly PropertyKey<int> GeneratorDayStartedKey = new PropertyKey<int>("Generator.DayStarted");
        private static readonly PropertyKey<bool> GeneratorActiveKey = new PropertyKey<bool>("Generator.Active");
        private static readonly PropertyKey<int> GeneratorProgressKey = new PropertyKey<int>("Generator.Progress");
        private static readonly PropertyKey<int> GeneratorEventTypeKey = new PropertyKey<int>("Generator.EventType");
        private static readonly PropertyKey<int> GeneratorTsunamiDirectionKey = new PropertyKey<int>("Generator.TsunamiDirection");

        private static readonly PropertyKey<int> PunisherDayKey = new PropertyKey<int>("Punisher.Day");
        private static readonly PropertyKey<float> PunisherHourKey = new PropertyKey<float>("Punisher.Hour");
        private static readonly PropertyKey<bool> PunisherActiveKey = new PropertyKey<bool>("Punisher.Active");
        private static readonly PropertyKey<float> PunisherWorkingHoursKey = new PropertyKey<float>("Punisher.WorkingHours");

        public Obsoletable<Config> Deserialize(IObjectLoader objectLoader)
        {
            Config.EventGeneratorConfig eventGenerator;
            Config.PunisherConfig punisher;

            if (objectLoader.Has(GeneratorLastEventCycleKey))
            {
                eventGenerator = new Config.EventGeneratorConfig(
                    objectLoader.Get(GeneratorLastEventCycleKey),
                    objectLoader.Get(GeneratorDayStartedKey),
                    objectLoader.Get(GeneratorActiveKey),
                    objectLoader.Get(GeneratorProgressKey),
                    (EventTypes)objectLoader.Get(GeneratorEventTypeKey),
                    objectLoader.Get(GeneratorTsunamiDirectionKey));
            }
            else
            {
                eventGenerator = new Config.EventGeneratorConfig();
            }

            if (objectLoader.Has(PunisherDayKey))
            {
                punisher = new Config.PunisherConfig(
                    objectLoader.Get(PunisherDayKey),
                    objectLoader.Get(PunisherHourKey),
                    objectLoader.Get(PunisherActiveKey),
                    objectLoader.Get(PunisherWorkingHoursKey));
            }
            else
            {
                punisher = new Config.PunisherConfig();
            }          

            return new Config(punisher, eventGenerator);
        }

        public void Serialize(Config value, IObjectSaver objectSaver)
        {
            objectSaver.Set(GeneratorLastEventCycleKey, value.EventGenerator.LastEventCycle);
            objectSaver.Set(GeneratorDayStartedKey, value.EventGenerator.DayStarted);
            objectSaver.Set(GeneratorActiveKey, value.EventGenerator.Active);
            objectSaver.Set(GeneratorProgressKey, value.EventGenerator.Progress);
            objectSaver.Set(GeneratorEventTypeKey, (int)value.EventGenerator.EventType);
            objectSaver.Set(GeneratorTsunamiDirectionKey, value.EventGenerator.TsunamiDirection);

            objectSaver.Set(PunisherDayKey, value.Punisher.Day);
            objectSaver.Set(PunisherHourKey, value.Punisher.Hour);
            objectSaver.Set(PunisherActiveKey, value.Punisher.Active);
            objectSaver.Set(PunisherWorkingHoursKey, value.Punisher.WorkingHours);
        }
    }
}
