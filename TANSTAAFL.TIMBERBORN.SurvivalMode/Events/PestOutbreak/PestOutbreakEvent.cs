using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TANSTAAFL.TIMBERBORN.SurvivalMode.Configs;
using TANSTAAFL.TIMBERBORN.SurvivalMode.Events.Wildfire;
using Timberborn.EntitySystem;
using Timberborn.QuickNotificationSystem;
using Timberborn.WeatherSystem;

namespace TANSTAAFL.TIMBERBORN.SurvivalMode.Events.PestOutbreak
{
    public class PestOutbreakEvent : ITickable
    {
        private readonly EntityComponentRegistry _entityComponentRegistry;
        private readonly QuickNotificationService _quickNotificationService;
        private readonly WeatherService _weatherService;
        private readonly int _daysDuration = 5;

        public PestOutbreakEvent(EntityComponentRegistry entityComponentRegistry, QuickNotificationService quickNotificationService, WeatherService weatherService)
        {
            _entityComponentRegistry = entityComponentRegistry;
            _quickNotificationService = quickNotificationService;
            _weatherService = weatherService;
        }

        public void Tick()
        {
            var eventGenerator = ConfigLoader._savedConfig.EventGenerator;

            if (_weatherService.Cycle != eventGenerator.LastEventCycle)
            {
                EndEvent(eventGenerator);
                return;
            }

            if (_weatherService.CycleDay - eventGenerator.DayStarted < eventGenerator.Progress)
            {
                return;
            }

            eventGenerator.Progress++;

            if (eventGenerator.Progress > _daysDuration)
            {
                EndEvent(eventGenerator);
                return;
            }

            var crops = _entityComponentRegistry
                .GetEnabled<CropRegistered>()
                .Where(x => !x._livingNaturalResource.IsDead)
                .ToArray();

            var killed = 0;

            for (var i = 0; i < crops.Count(); i++)
            {
                if (i % 3 != 0)
                {
                    continue;
                }

                var crop = crops[i];
                crop._livingNaturalResource.Die();
                killed++;
            }

            Plugin.Log.LogWarning($"killed {killed} of {crops.Count()} crops");

            var message = $"A pest outbreak killed a third of the crops! Day {eventGenerator.Progress} of {_daysDuration}";
            _quickNotificationService.SendWarningNotification(message);
        }

        private static void EndEvent(Config.EventGeneratorConfig eventGenerator)
        {
            eventGenerator.Active = false;
            eventGenerator.Progress = 0;
            eventGenerator.DayStarted = 0;
        }
    }
}
