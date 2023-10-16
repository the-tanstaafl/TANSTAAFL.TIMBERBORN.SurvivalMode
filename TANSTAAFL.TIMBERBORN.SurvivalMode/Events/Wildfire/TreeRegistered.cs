using System;
using System.Collections.Generic;
using System.Text;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.EntitySystem;
using Timberborn.Forestry;
using Timberborn.Growing;
using Timberborn.NaturalResourcesLifecycle;
using Timberborn.SoilMoistureSystem;

namespace TANSTAAFL.TIMBERBORN.SurvivalMode.Events.Wildfire
{
    public class TreeRegistered : BaseComponent, IRegisteredComponent
    {
        internal TreeComponent _tree;
        internal LivingNaturalResource _livingNaturalResource;

        public void Awake()
        {
            _tree = GetComponentFast<TreeComponent>();
            _livingNaturalResource = GetComponentFast<LivingNaturalResource>();
        }
    }
}
