using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Text;
using TANSTAAFL.TIMBERBORN.SurvivalMode.Events;
using UnityEngine;

namespace TANSTAAFL.TIMBERBORN.SurvivalMode.Configs
{
    public class Config
    {
        public Config() 
        {
            Punisher = new PunisherConfig();
            EventGenerator = new EventGeneratorConfig();
        }

        public Config(PunisherConfig punisher, EventGeneratorConfig eventGenerator)
        {
            Punisher = punisher;
            EventGenerator = eventGenerator;
        }

        public PunisherConfig Punisher { get; }
        public EventGeneratorConfig EventGenerator { get; }

        public class PunisherConfig
        {
            public PunisherConfig() { }
            public PunisherConfig(int day, float hour, bool active, float workingHours)
            {
                Day = day;
                Hour = hour;
                Active = active;
                WorkingHours = workingHours;
            }

            public int Day { get; set; }
            public float Hour { get; set; }
            public bool Active { get; set; }
            public float WorkingHours { get; set; }
        }

        public class EventGeneratorConfig
        {
            public EventGeneratorConfig() { }
            public EventGeneratorConfig(int lastEventCycle, int dayStarted, bool active, int progress, EventTypes eventType, int tsunamiDirection)
            {
                LastEventCycle = lastEventCycle;
                DayStarted = dayStarted;
                Active = active;
                Progress = progress;
                EventType = eventType;
                TsunamiDirection = tsunamiDirection;
            }

            public int LastEventCycle { get; set; }
            public int DayStarted { get; set; }
            public bool Active { get; set; }
            public int Progress { get; set; }
            public EventTypes EventType { get; set; }
            public int TsunamiDirection { get; set; }
        }
    }
}
