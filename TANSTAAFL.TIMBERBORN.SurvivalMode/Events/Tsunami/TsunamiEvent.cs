using System;
using System.Collections.Generic;
using System.Text;
using TANSTAAFL.TIMBERBORN.SurvivalMode.Configs;
using Timberborn.Fields;
using Timberborn.MapIndexSystem;
using Timberborn.QuickNotificationSystem;
using Timberborn.WaterSystem;
using UnityEngine;

namespace TANSTAAFL.TIMBERBORN.SurvivalMode.Events.Tsunami
{
    public class TsunamiEvent : ITickable
    {
        private readonly MapIndexService _mapIndexService;
        private readonly QuickNotificationService _quickNotificationService;
        private readonly WaterChangeService _waterChangeService;
        private readonly IWaterService _waterService;
        private int _progressIncrement;
        private int _WaveSize;

        public TsunamiEvent(MapIndexService mapIndexService, QuickNotificationService quickNotificationService, WaterChangeService waterChangeService, IWaterService waterService)
        {
            _mapIndexService = mapIndexService;
            _quickNotificationService = quickNotificationService;
            _waterChangeService = waterChangeService;
            _waterService = waterService;

            var x = _mapIndexService.MapSize.x;
            var y = _mapIndexService.MapSize.y;

            if (x < 60 || y < 60)
            {
                _progressIncrement = 3;
                _WaveSize = 1;
            }
            else if (x < 160 || y < 160)
            {
                _progressIncrement = 2;
                _WaveSize = 2;
            }
            else
            {
                _progressIncrement = 1;
                _WaveSize = 3;
            }
        }

        public void Tick()
        {
            var eventGenerator = ConfigLoader._savedConfig.EventGenerator;

            if (eventGenerator.Progress == 0)
            {
                Plugin.Log.LogWarning($"tsunami started");

                var message = $"A tsunami is incoming!!!";
                _quickNotificationService.SendWarningNotification(message);
            }

            eventGenerator.Progress += _progressIncrement;

            if (eventGenerator.Progress > 135) // 45 seconds on default game speed
            {
                eventGenerator.Active = false;
                eventGenerator.Progress = 0;
                eventGenerator.DayStarted = 0;

                return;
            }

            var targetHeight = 20;

            var x = _mapIndexService.MapSize.x;
            var y = _mapIndexService.MapSize.y;

            for (int i = 0; i < y; i++)
            {
                for (var waveSize = 0; waveSize < _WaveSize; waveSize++)
                {
                    UpdateWaterHeight(targetHeight, waveSize, i);
                }
            }
        }

        private void UpdateWaterHeight(int targetHeight, int x, int y)
        {
            var coord = new Vector2Int(x, y);
            var waterHeight = _waterService.WaterHeight(coord);
            if (waterHeight < targetHeight)
            {
                if (waterHeight > 0)
                {
                    _waterChangeService.EnqueueWaterChange(coord, targetHeight - waterHeight, 0);
                }
                else
                {
                    _waterChangeService.EnqueueWaterChange(coord, 1, 0);
                }
            }
        }
    }
}
