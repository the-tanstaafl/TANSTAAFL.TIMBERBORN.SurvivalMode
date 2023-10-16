using Bindito.Core;
using System;
using System.Collections.Generic;
using System.Text;
using TANSTAAFL.TIMBERBORN.SurvivalMode.Configs;
using TANSTAAFL.TIMBERBORN.SurvivalMode.Events;
using TANSTAAFL.TIMBERBORN.SurvivalMode.Events.PestOutbreak;
using TANSTAAFL.TIMBERBORN.SurvivalMode.Events.Plague;
using TANSTAAFL.TIMBERBORN.SurvivalMode.Events.Tsunami;
using TANSTAAFL.TIMBERBORN.SurvivalMode.Events.Wildfire;
using TimberApi.ConfiguratorSystem;
using TimberApi.EntityLinkerSystem;
using TimberApi.SceneSystem;
using Timberborn.Buildings;
using Timberborn.CameraSystem;
using Timberborn.Fields;
using Timberborn.Forestry;
using Timberborn.Growing;
using Timberborn.IrrigationSystem;
using Timberborn.TemplateSystem;

namespace TANSTAAFL.TIMBERBORN.SurvivalMode
{
    [Configurator(SceneEntrypoint.InGame)]
    public class Configurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<ConfigLoader>().AsSingleton();
            containerDefinition.Bind<ConfigSerializer>().AsSingleton();

            containerDefinition.Bind<BeaverSpawner>().AsSingleton();
            containerDefinition.Bind<DeathPunisher>().AsSingleton();

            containerDefinition.Bind<EventGenerator>().AsSingleton();
            containerDefinition.Bind<EventProvider>().AsSingleton();
            containerDefinition.Bind<TsunamiEvent>().AsSingleton();
            containerDefinition.Bind<WildfireEvent>().AsSingleton();
            containerDefinition.Bind<PestOutbreakEvent>().AsSingleton();
            containerDefinition.Bind<PlagueEvent>().AsSingleton();

            containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
        }

        private static TemplateModule ProvideTemplateModule()
        {
            var builder = new TemplateModule.Builder();
            builder.AddDecorator<TreeComponent, TreeRegistered>();
            builder.AddDecorator<Crop, CropRegistered>();
            return builder.Build();
        }
    }
}
