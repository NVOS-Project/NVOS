using MelonLoader;
using NVOS.Core;
using NVOS.Core.Logger;
using NVOS.Core.Logger.Enums;
using NVOS.Core.Modules;
using NVOS.Core.Modules.Extensions;
using NVOS.Core.Services;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NVOS.Il2CppLoader
{
    public class Entrypoint : MelonMod
    {
        public const string NVOS_ROOT_PATH = "NVOS_Data";
        public const string NVOS_MODULES_DIR = "Modules";

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("===== Loading NVOS =====");
            try
            {
                LoggerInstance.Msg($"Preparing NVOS root directory at {Path.GetFullPath(NVOS_ROOT_PATH)}");
                Directory.CreateDirectory(NVOS_ROOT_PATH);

                LoggerInstance.Msg("Preparing module directory");
                Directory.CreateDirectory(Path.Combine(NVOS_ROOT_PATH, NVOS_MODULES_DIR));

                LoggerInstance.Msg("Setting working directory");
                Directory.SetCurrentDirectory(NVOS_ROOT_PATH);

                LoggerInstance.Msg("NVOS root directory OK");
            }
            catch (Exception ex)
            {
                LoggerInstance.Error("Failed to prepare NVOS root directory", ex); ;
                return;
            }


            try
            {
                LoggerInstance.Msg("Calling Bootstrap.Init");
                Bootstrap.Init();
                LoggerInstance.Msg("Bootstrap OK");
            }
            catch (Exception ex)
            {
                LoggerInstance.Error("Failed to initialize NVOS", ex);
                return;
            }

            try
            {
                LoggerInstance.Msg("Hooking NVOS log stream");
                ILogger logger = ServiceLocator.Resolve<ILogger>();
                logger.OnLog += NVOS_OnLog;
                LoggerInstance.Msg("Log hook OK");
            }
            catch (Exception ex)
            {
                LoggerInstance.Warning($"Failed to hook NVOS log stream: {ex}");
            }

            if (!Bootstrap.IsInitialized)
            {
                LoggerInstance.Error("NVOS.Core did not set the IsInitialized flag. Perhaps it failed silently?");
                return;
            }

            LoggerInstance.Msg("NVOS modules will be loaded once the scene finishes initialization.");
            LoggerInstance.Msg("===== NVOS bootstrap finished =====");
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            LoggerInstance.Msg("===== Loading NVOS modules =====");

            ModuleManager mm;
            try
            {
                mm = ServiceLocator.Resolve<ModuleManager>();
            }
            catch (Exception ex)
            {
                LoggerInstance.Error("Failed to resolve the NVOS module manager", ex);
                return;
            }

            string[] modules = Directory.GetFiles(NVOS_MODULES_DIR, "*.dll");
            LoggerInstance.Msg($"Found {modules.Length} candidates");
            int loaded = 0;

            foreach (string module in modules)
            {
                try
                {
                    LoggerInstance.Msg($"Resolving module {module}");
                    Assembly assembly = Assembly.LoadFrom(module);
                    IModule manifest = assembly.GetModuleManifest();

                    LoggerInstance.WriteLine();
                    LoggerInstance.Msg($"{manifest.Name} {manifest.Version}");
                    LoggerInstance.Msg($"by {manifest.Author}");
                    LoggerInstance.Msg($"Description: {(manifest.Description != null ? manifest.Description : "None")}");
                    LoggerInstance.Msg($"Assembly: {Path.GetFileName(module)}");
                    LoggerInstance.WriteLine();

                    LoggerInstance.Msg($"Inserting module {manifest.Name} into NVOS");
                    mm.Load(assembly);
                    loaded++;
                }
                catch (Exception ex)
                {
                    LoggerInstance.Error($"Failed to load {module}", ex);
                }
            }

            LoggerInstance.Msg($"Loaded {loaded} modules directly, {mm.GetLoadedModules().Count()} modules running total");
            LoggerInstance.Msg("===== Module load complete =====");
        }

        private void NVOS_OnLog(object sender, Core.Logger.EventArgs.LogEventArgs e)
        {
            switch (e.Level)
            {
                case LogLevel.DEBUG:
                    LoggerInstance.Msg($"[DEBUG] {e.Message}");
                    break;
                case LogLevel.INFO:
                    LoggerInstance.Msg($"[INFO] {e.Message}");
                    break;
                case LogLevel.WARN:
                    LoggerInstance.Warning(e.Message);
                    break;
                case LogLevel.ERROR:
                    LoggerInstance.Error(e.Message);
                    break;
            }
        }
    }
}
