using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TANSTAAFL.TIMBERBORN.SurvivalMode.Configs;
using Timberborn.Characters;
using Timberborn.EntitySystem;
using Timberborn.NotificationSystem;
using Timberborn.QuickNotificationSystem;
using Timberborn.WeatherSystem;
using UnityEngine;

namespace TANSTAAFL.TIMBERBORN.SurvivalMode.Events.Wildfire
{
    public class WildfireEvent : ITickable
    {
        private readonly EntityComponentRegistry _entityComponentRegistry;
        private readonly QuickNotificationService _quickNotificationService;
        private readonly WeatherService _weatherService;
        private readonly int _daysDuration = 5;

        public WildfireEvent(EntityComponentRegistry entityComponentRegistry, QuickNotificationService quickNotificationService, WeatherService weatherService) 
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

            var trees = _entityComponentRegistry
                .GetEnabled<TreeRegistered>()
                .Where(x => !x._livingNaturalResource.IsDead)
                .ToArray();

            var killed = 0;

            for (var i = 0; i < trees.Count(); i++)
            {
                if (i % 5 != 0)
                {
                    continue;
                }

                var tree = trees[i];
                tree._livingNaturalResource.Die();
                killed++;
            }

            Plugin.Log.LogWarning($"killed {killed} of {trees.Count()} trees");

            var message = $"A wildfire killed a fifth of the trees! Day {eventGenerator.Progress} of {_daysDuration}";
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
