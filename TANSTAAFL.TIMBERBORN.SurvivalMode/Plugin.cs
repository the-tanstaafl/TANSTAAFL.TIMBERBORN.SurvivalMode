using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimberApi.ConsoleSystem;
using TimberApi.ModSystem;
using Timberborn.BuildingsNavigation;
using Timberborn.OptionsGame;
using Timberborn.WeatherSystem;
using UnityEngine.UIElements;

namespace TANSTAAFL.TIMBERBORN.SurvivalMode
{
    [HarmonyPatch]
    public class Plugin : IModEntrypoint
    {
        internal static IConsoleWriter Log;

        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
            Log = consoleWriter;

            var harmony = new Harmony("tanstaafl.plugins.SurvivalMode");
            harmony.PatchAll();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(WeatherService), "StartNextDay")]
        static void StartNextDay(WeatherService __instance)
        {
            __instance._eventBus.Post(new DayStartedEvent());
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameOptionsBox), "GetPanel")]
        static void ShowConfigBox(ref VisualElement __result)
        {
            VisualElement root = __result.Query("OptionsBox");
            Button button = new() { classList = { "menu-button" } };

            button.text = "SurvivalMode config";
            //button.clicked += ConfigBox.OpenOptionsDelegate;
            root.Insert(4, button);
        }
    }
}
