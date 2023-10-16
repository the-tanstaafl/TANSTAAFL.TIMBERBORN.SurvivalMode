using System;
using System.Collections.Generic;
using System.Text;
using TANSTAAFL.TIMBERBORN.SurvivalMode.Configs;
using TANSTAAFL.TIMBERBORN.SurvivalMode.Events.Wildfire;
using Timberborn.Characters;
using Timberborn.EntitySystem;
using Timberborn.Navigation;
using Timberborn.NeedSystem;
using Timberborn.QuickNotificationSystem;
using Timberborn.WeatherSystem;
using UnityEngine;

namespace TANSTAAFL.TIMBERBORN.SurvivalMode.Events.Plague
{
    public class PlagueEvent : ITickable
    {
        private readonly CharacterPopulation _characterPopulation;
        private readonly QuickNotificationService _quickNotificationService;
        private readonly WeatherService _weatherService;
        private readonly int _daysDuration = 5;

        public PlagueEvent(CharacterPopulation characterPopulation, QuickNotificationService quickNotificationService, WeatherService weatherService)
        {
            _characterPopulation = characterPopulation;
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

            var healthy = 0;
            var injured = 0;

            for (int i = 0; i < _characterPopulation.NumberOfCharacters; i++)
            {
                Character character = _characterPopulation.Characters[i];
                var needManager = character.GetComponentFast<NeedManager>();

                if (needManager.HasNeed("Injure")) 
                {
                    continue;
                }

                healthy++;

                if (healthy % 5 == 0)
                {
                    var effect = new Timberborn.Effects.InstantEffect("Injury", -1f, 1);
                    needManager.ApplyEffect(in effect);
                    injured++;
                }
            }

            Plugin.Log.LogWarning($"Injured {injured} of {healthy} healthy beavers");

            var message = $"A plague injured a fifth of the healthy beavers! Day {eventGenerator.Progress} of {_daysDuration}";
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
