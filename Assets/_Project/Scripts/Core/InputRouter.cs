using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ValVertII.Core
{
    public sealed class InputRouter : IAppService
    {
        private readonly InputActionAsset _actions;

        private readonly string _gameplayMapName;
        private readonly string _uiMapName;
        private readonly string _developerMapName;
        private readonly string _cutsceneMapName;

        public bool IsInitialized { get; private set; }
        public AppInputMode CurrentMode { get; private set; } =
            AppInputMode.None;

        public InputRouter(
            InputActionAsset actions,
            string gameplayMapName = "Gameplay",
            string uiMapName = "UI",
            string developerMapName = "Developer",
            string cutsceneMapName = "Cutscene")
        {
            _actions = actions;
            _gameplayMapName = gameplayMapName;
            _uiMapName = uiMapName;
            _developerMapName = developerMapName;
            _cutsceneMapName = cutsceneMapName;
        }

        public void Initialize()
        {
            if (_actions == null)
                throw new InvalidOperationException(
                    "Input Actions asset non assigné au PersistentRoot.");

            DisableAllMaps();
            IsInitialized = true;
        }

        public void SetMode(AppInputMode mode)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("InputRouter non initialisé.");

            DisableAllMaps();

            string mapName = mode switch
            {
                AppInputMode.Gameplay => _gameplayMapName,
                AppInputMode.UI => _uiMapName,
                AppInputMode.Developer => _developerMapName,
                AppInputMode.Cutscene => _cutsceneMapName,
                _ => null
            };

            if (!string.IsNullOrEmpty(mapName))
            {
                InputActionMap map = _actions.FindActionMap(mapName, false);

                if (map == null)
                    throw new InvalidOperationException(
                        $"Action Map introuvable : {mapName}");

                map.Enable();
            }

            CurrentMode = mode;
            ApplyCursorPolicy(mode);
        }

        private void DisableAllMaps()
        {
            foreach (InputActionMap map in _actions.actionMaps)
                map.Disable();
        }

        private static void ApplyCursorPolicy(AppInputMode mode)
        {
            bool ui = mode == AppInputMode.UI || mode == AppInputMode.None;

            Cursor.visible = ui;
            Cursor.lockState =
                ui ? CursorLockMode.None : CursorLockMode.Locked;
        }

        public void Shutdown()
        {
            if (_actions != null)
                DisableAllMaps();

            CurrentMode = AppInputMode.None;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            IsInitialized = false;
        }
    }
}