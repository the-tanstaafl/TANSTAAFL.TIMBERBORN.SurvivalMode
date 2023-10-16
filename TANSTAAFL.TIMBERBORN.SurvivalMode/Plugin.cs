using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimberApi.ConsoleSystem;
using TimberApi.DependencyContainerSystem;
using TimberApi.ModSystem;
using Timberborn.BuildingsNavigation;
using Timberborn.GameScene;
using Timberborn.Goods;
using Timberborn.OptionsGame;
using Timberborn.SceneLoading;
using Timberborn.TimeSpeedButtonSystem;
using Timberborn.TimeSystem;
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

        [HarmonyPrefix]
        [HarmonyPatch(typeof(TimeSpeedButtonFactory), "Create")]
        static void Create(Button button, int index, Action<int> clickCallback)
        {
            if (button.name == "Speed0")
            {
                button.name = "Speed2";
            }
            else if (button.name == "Speed1")
            {
                button.name = "Speed4";
            }
            else if (button.name == "Speed3")
            {
                button.name = "Speed8";
            }
            else if (button.name == "Speed7")
            {
                button.name = "Speed16";
            }
            else
            {
                Log.LogWarning($"{button.name} not changed!");
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(LoadingScreen), "Disable")]
        static void Disable()
        {
            var speedManager = DependencyContainer.GetInstance<SpeedManager>();
            if (speedManager != null)
            { 
                speedManager.ChangeSpeed(2);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(NewGameInitializer), "InitialGoods")]
        static IEnumerable<GoodAmount> InitialGoods(IEnumerable<GoodAmount> items)
        {
            foreach (var item in items)
            {
                yield return item;
            }

            if (!items.Any(x => x.GoodId == "Plank"))
            {
                yield return new GoodAmount("Plank", 11);
            }

            if (!items.Any(x => x.GoodId == "Log"))
            {
                yield return new GoodAmount("Log", 15);
            }
        }
    }
}