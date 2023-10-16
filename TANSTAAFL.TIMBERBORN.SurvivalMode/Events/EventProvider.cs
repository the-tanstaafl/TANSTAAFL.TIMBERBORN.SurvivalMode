using System;
using System.Collections.Generic;
using System.Text;
using TANSTAAFL.TIMBERBORN.SurvivalMode.Events.PestOutbreak;
using TANSTAAFL.TIMBERBORN.SurvivalMode.Events.Plague;
using TANSTAAFL.TIMBERBORN.SurvivalMode.Events.Tsunami;
using TANSTAAFL.TIMBERBORN.SurvivalMode.Events.Wildfire;

namespace TANSTAAFL.TIMBERBORN.SurvivalMode.Events
{
    public class EventProvider
    {
        private readonly PestOutbreakEvent _pestOutbreak;
        private readonly PlagueEvent _plague;
        private readonly TsunamiEvent _tsunami;
        private readonly WildfireEvent _wildfireEvent;

        public EventProvider(PestOutbreakEvent pestOutbreak, PlagueEvent plague, TsunamiEvent tsunami, WildfireEvent wildfireEvent)
        {
            _pestOutbreak = pestOutbreak;
            _plague = plague;
            _tsunami = tsunami;
            _wildfireEvent = wildfireEvent;
        }

        public ITickable GetEvent(EventTypes eventType)
        {
            switch (eventType)
            {
                case EventTypes.Tsunami:
                    return _tsunami;
                case EventTypes.Wildfire:
                    return _wildfireEvent;
                case EventTypes.PestOutbreak:
                    return _pestOutbreak;
                case EventTypes.Plague:
                    return _plague;
                case EventTypes.Heatwave:
                    break;
                case EventTypes.Windless:
                    break;
                case EventTypes.Earthquake:
                    break;
            }

            return _tsunami;
        }
    }
}
