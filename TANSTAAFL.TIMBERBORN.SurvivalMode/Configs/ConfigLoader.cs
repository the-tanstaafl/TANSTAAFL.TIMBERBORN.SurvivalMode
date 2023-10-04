using System;
using System.Collections.Generic;
using System.Text;
using Timberborn.Navigation;
using Timberborn.Persistence;
using Timberborn.SingletonSystem;

namespace TANSTAAFL.TIMBERBORN.SurvivalMode.Configs
{
    public class ConfigLoader : ISaveableSingleton, ILoadableSingleton, IPostLoadableSingleton
    {
        private static readonly SingletonKey SurvivalModeStateRestorerKey = new SingletonKey("SurvivalModeStateRestorer");
        private static readonly PropertyKey<Config> SavedSurvivalModeStateKey = new PropertyKey<Config>("SurvivalModeState");

        internal static Config _savedConfig;
        private ConfigSerializer _configSerializer;

        private readonly ISingletonLoader _singletonLoader;

        public ConfigLoader(ISingletonLoader singletonLoader, ConfigSerializer configSerializer)
        {
            _singletonLoader = singletonLoader;
            _configSerializer = configSerializer;
        }

        public void Load()
        {
            _savedConfig = null;
            if (_singletonLoader.HasSingleton(SurvivalModeStateRestorerKey))
            {
                IObjectLoader singleton = _singletonLoader.GetSingleton(SurvivalModeStateRestorerKey);
                _savedConfig = singleton.Get(SavedSurvivalModeStateKey, _configSerializer);
            }
        }

        public void PostLoad()
        {
            if (_savedConfig == null)
            {
                _savedConfig = new Config();
            }
        }

        public void Save(ISingletonSaver singletonSaver)
        {
            singletonSaver.GetSingleton(SurvivalModeStateRestorerKey).Set(SavedSurvivalModeStateKey, _savedConfig, _configSerializer);
        }
    }
}
