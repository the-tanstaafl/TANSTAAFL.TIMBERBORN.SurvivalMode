using System;
using System.Collections.Generic;
using System.Text;
using TANSTAAFL.TIMBERBORN.SurvivalMode.Configs;
using Timberborn.Beavers;
using Timberborn.Characters;
using Timberborn.LifeSystem;
using Timberborn.Persistence;
using Timberborn.SingletonSystem;
using Timberborn.TickSystem;
using Timberborn.TimeSystem;
using Timberborn.WorkSystem;

namespace TANSTAAFL.TIMBERBORN.SurvivalMode
{
    public class DeathPunisher : ITickableSingleton, ILoadableSingleton
    {
        private EventBus _eventBus;
        private IDayNightCycle _dayNightCycle;
        private WorkingHoursManager _workingHoursManager;

        public DeathPunisher(EventBus eventBus, IDayNightCycle dayNightCycle, WorkingHoursManager workingHoursManager)
        {
            _eventBus = eventBus;
            _dayNightCycle = dayNightCycle;
            _workingHoursManager = workingHoursManager;
        }
        
        public void Load()
        {
            _eventBus.Register(this);
        }
        
        [OnEvent]
        public void OnCharacterKilled(CharacterKilledEvent characterKilledEvent)
        {
            var child = characterKilledEvent.Character.GetComponentFast<Child>();
            if (child != null)
            {
                if (child.GrowthProgress < 1)
                {
                    Punish();
                }

                return;
            }

            var lifeProgess = characterKilledEvent.Character.GetComponentFast<LifeProgressor>();
            if ((object)lifeProgess != null && !lifeProgess.ShouldDie && lifeProgess.LifeProgress > 0.05)
            {
                Punish();
            }
        }

        private void Punish()
        {
            Plugin.Log.LogWarning("A beaver died! Beavers are not working for a day");

            if (ConfigLoader._savedConfig.WorkingHours == 0)
            {
                ConfigLoader._savedConfig.WorkingHours = _workingHoursManager.EndHours;
            }

            ConfigLoader._savedConfig.Day = _dayNightCycle.DayNumber;
            ConfigLoader._savedConfig.Hour = _dayNightCycle.DayProgress;
            ConfigLoader._savedConfig.PunishmentActive = true;
        }

        private void Unpunish()
        {
            Plugin.Log.LogWarning("Beavers are returning to work");

            _workingHoursManager.EndHours = ConfigLoader._savedConfig.WorkingHours;

            ConfigLoader._savedConfig.Day = 0;
            ConfigLoader._savedConfig.Hour = 0;
            ConfigLoader._savedConfig.PunishmentActive = false;
            ConfigLoader._savedConfig.WorkingHours = 0;
        }

        public void Tick()
        {
            if (ConfigLoader._savedConfig?.PunishmentActive??false)
            {
                if (ConfigLoader._savedConfig.Day == _dayNightCycle.DayNumber || ConfigLoader._savedConfig.Hour > _dayNightCycle.DayProgress)
                {
                    _workingHoursManager.EndHours = 0;
                }
                else
                {
                    Unpunish();
                }
            }
        }
    }
}
