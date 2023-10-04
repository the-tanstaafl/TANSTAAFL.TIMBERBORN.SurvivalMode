using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Timberborn.Beavers;
using Timberborn.BlockSystem;
using Timberborn.Buildings;
using Timberborn.GameDistricts;
using Timberborn.Navigation;
using Timberborn.Reproduction;
using Timberborn.SingletonSystem;
using Timberborn.TimeSystem;
using Timberborn.WalkingSystem;
using Timberborn.WaterSystem;
using Timberborn.WeatherSystem;
using UnityEngine;

namespace TANSTAAFL.TIMBERBORN.SurvivalMode
{
    public class BeaverSpawner : ILoadableSingleton
    {
        private readonly EventBus _eventBus;
        private readonly DistrictCenterRegistry _districtCenterRegistry;
        private readonly WalkerService _walkerService;
        private readonly NewbornSpawner _newbornSpawner;
        private readonly WeatherService _weatherService;

        public BeaverSpawner(EventBus eventBus, DistrictCenterRegistry districtCenterRegistry, WalkerService walkerService, NewbornSpawner newbornSpawner, WeatherService weatherService) 
        {
            _eventBus = eventBus;
            _districtCenterRegistry = districtCenterRegistry;
            _newbornSpawner = newbornSpawner;
            _weatherService = weatherService;
            _walkerService = walkerService;
        }

        public void Load()
        {
            _eventBus.Register(this);
        }

        [OnEvent]
        public void OnDayStartedEvent(DayStartedEvent dayStartedEvent)
        {
            var cycle = _weatherService.Cycle;
            var day = _weatherService.CycleDay;
            var create = 0;

            if (cycle < 4)
            {
                switch (cycle)
                {
                    case 1:
                        create = day % 4 == 0 ? 1 : 0;
                        break;
                    case 2:
                        create = day % 3 == 0 ? 1 : 0;
                        break;
                    case 3:
                        create = day % 2 == 0 ? 1 : 0;
                        break;
                }
            }
            else
            {
                create = cycle - 3;
            }

            Plugin.Log.LogInfo($"cycle {cycle} day {day} add {create} beavers");

            if (create == 0)
            {
                return;
            }

            if (!_districtCenterRegistry.AllDistrictCenters.Any())
            {
                return;
            }

            var district = _districtCenterRegistry.AllDistrictCenters
                .OrderByDescending(x => x.DistrictPopulation)
                .First();

            for (int i = 0; i < create; i++)
            {
                _newbornSpawner.SpawnChild(district.GetComponentFast<Building>());
            }
        }
    }
}
