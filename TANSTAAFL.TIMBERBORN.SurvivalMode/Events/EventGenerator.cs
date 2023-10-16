using System;
using System.Collections.Generic;
using System.Text;
using TANSTAAFL.TIMBERBORN.SurvivalMode.Configs;
using Timberborn.GameDistricts;
using Timberborn.Reproduction;
using Timberborn.SingletonSystem;
using Timberborn.TickSystem;
using Timberborn.WalkingSystem;
using Timberborn.WeatherSystem;

namespace TANSTAAFL.TIMBERBORN.SurvivalMode.Events
{
    public class EventGenerator : ITickableSingleton, ILoadableSingleton
    {
        private readonly EventBus _eventBus;
        private readonly WeatherService _weatherService;
        private readonly EventProvider _eventProvider;

        public EventGenerator(EventBus eventBus, WeatherService weatherService, EventProvider eventProvider)
        {
            _eventBus = eventBus;
            _weatherService = weatherService;
            _eventProvider = eventProvider;
        }

        public void Load()
        {
            _eventBus.Register(this);
        }

        [OnEvent]
        public void OnDayStartedEvent(DayStartedEvent dayStartedEvent)
        {
            var eventGenerator = ConfigLoader._savedConfig.EventGenerator;

            //eventGenerator.Active = true;
            //eventGenerator.EventType = EventTypes.Plague;
            //eventGenerator.LastEventCycle = _weatherService.Cycle;
            //eventGenerator.DayStarted = _weatherService.CycleDay;
            //eventGenerator.Progress = 0;

            //return;

            if (!ShouldStartNewEvent())
            {
                return;
            }

            var randomEvent = (EventTypes)new Random().Next(4);

            eventGenerator.Active = true;
            eventGenerator.EventType = randomEvent;
            eventGenerator.LastEventCycle = _weatherService.Cycle;
            eventGenerator.DayStarted = _weatherService.CycleDay;
            eventGenerator.Progress = 0;
        }

        private bool ShouldStartNewEvent()
        {
            if (_weatherService.Cycle < 5)
            {
                return false;
            }

            if (_weatherService.CycleDay > 4) 
            {
                return false;
            }

            var eventGenerator = ConfigLoader._savedConfig.EventGenerator;

            if (eventGenerator.Active)
            {
                return false;
            }

            if (eventGenerator.LastEventCycle + 1 >= _weatherService.Cycle) 
            {
                return false;
            }

            var odds = 0.075 + _weatherService.CycleDay * 0.025;

            if (new Random().NextDouble() > odds)
            {
                return false;
            }

            return true;
        }

        public void Tick()
        {
            var eventGenerator = ConfigLoader._savedConfig.EventGenerator;

            if (!eventGenerator.Active)
            { 
                return;
            }

            _eventProvider.GetEvent(eventGenerator.EventType).Tick();
        }
    }
}
