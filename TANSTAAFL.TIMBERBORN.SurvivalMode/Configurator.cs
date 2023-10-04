using Bindito.Core;
using System;
using System.Collections.Generic;
using System.Text;
using TANSTAAFL.TIMBERBORN.SurvivalMode.Configs;
using TimberApi.ConfiguratorSystem;
using TimberApi.EntityLinkerSystem;
using TimberApi.SceneSystem;
using Timberborn.Buildings;
using Timberborn.CameraSystem;
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
        }
    }
}
