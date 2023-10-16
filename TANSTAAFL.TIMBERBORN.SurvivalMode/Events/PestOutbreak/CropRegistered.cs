using System;
using System.Collections.Generic;
using System.Text;
using Timberborn.BaseComponentSystem;
using Timberborn.EntitySystem;
using Timberborn.Fields;
using Timberborn.Forestry;
using Timberborn.NaturalResourcesLifecycle;

namespace TANSTAAFL.TIMBERBORN.SurvivalMode.Events.PestOutbreak
{
    public class CropRegistered : BaseComponent, IRegisteredComponent
    {
        internal Crop _crop;
        internal LivingNaturalResource _livingNaturalResource;

        public void Awake()
        {
            _crop = GetComponentFast<Crop>();
            _livingNaturalResource = GetComponentFast<LivingNaturalResource>();
        }
    }
}
