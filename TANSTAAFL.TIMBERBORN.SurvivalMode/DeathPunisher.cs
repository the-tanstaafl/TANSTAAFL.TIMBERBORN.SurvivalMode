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

            var punisherConfig = ConfigLoader._savedConfig.Punisher;

            if (punisherConfig.WorkingHours == 0)
            {
                punisherConfig.WorkingHours = _workingHoursManager.EndHours;
            }

            punisherConfig.Day = _dayNightCycle.DayNumber;
            punisherConfig.Hour = _dayNightCycle.DayProgress;
            punisherConfig.Active = true;
        }

        private void Unpunish()
        {
            Plugin.Log.LogWarning("Beavers are returning to work");

            var punisherConfig = ConfigLoader._savedConfig.Punisher;

            _workingHoursManager.EndHours = punisherConfig.WorkingHours;

            punisherConfig.Day = 0;
            punisherConfig.Hour = 0;
            punisherConfig.Active = false;
            punisherConfig.WorkingHours = 0;
        }

        public void Tick()
        {
            var punisherConfig = ConfigLoader._savedConfig.Punisher;

            if (ConfigLoader._savedConfig?.Punisher.Active??false)
            {
                if (punisherConfig.Day == _dayNightCycle.DayNumber 
                    || punisherConfig.Hour > _dayNightCycle.DayProgress)
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
