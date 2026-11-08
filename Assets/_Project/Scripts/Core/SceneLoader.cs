using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ValVertII.Core
{
    public enum SceneLoadState
    {
        Unloaded,
        Loading,
        Loaded,
        Unloading,
        Failed
    }

    public sealed class SceneLoadStatus
    {
        public string ScenePath { get; internal set; }
        public SceneLoadState State { get; internal set; }
        public float Progress { get; internal set; }
        public string Error { get; internal set; }
    }

    public sealed class SceneLoader : IAppService
    {
        private readonly Dictionary<string, SceneLoadStatus> _statuses =
            new Dictionary<string, SceneLoadStatus>();

        private readonly Dictionary<string, Task<Scene>> _inflightLoads =
            new Dictionary<string, Task<Scene>>();

        private readonly AppLogger _logger;

        public bool IsInitialized { get; private set; }

        public SceneLoader(AppLogger logger)
        {
            _logger = logger;
        }

        public void Initialize()
        {
            IsInitialized = true;
        }

        public async Task<Scene> LoadSingleAsync(string scenePath)
        {
            EnsureInitialized();
            ValidateScenePath(scenePath);

            _logger.Info($"Chargement Single demandé : {scenePath}");

            Scene scene = await LoadInternalAsync(scenePath, LoadSceneMode.Single);

            if (!scene.IsValid() || !scene.isLoaded)
                throw new InvalidOperationException(
                    $"La scène n'est pas chargée : {scenePath}");

            return scene;
        }

        public async Task<Scene> LoadAdditiveAsync(
            string scenePath,
            bool setActive = false)
        {
            EnsureInitialized();
            ValidateScenePath(scenePath);

            Scene alreadyLoaded = SceneManager.GetSceneByPath(scenePath);

            if (alreadyLoaded.IsValid() && alreadyLoaded.isLoaded)
            {
                if (setActive)
                    SceneManager.SetActiveScene(alreadyLoaded);

                return alreadyLoaded;
            }

            Scene scene = await LoadInternalAsync(scenePath, LoadSceneMode.Additive);

            if (!scene.IsValid() || !scene.isLoaded)
                throw new InvalidOperationException(
                    $"La scène additive n'est pas chargée : {scenePath}");

            if (setActive)
                SceneManager.SetActiveScene(scene);

            return scene;
        }

        public async Task<bool> UnloadAsync(string scenePath)
        {
            EnsureInitialized();

            if (string.IsNullOrWhiteSpace(scenePath))
                throw new ArgumentException("Chemin de scène vide.");

            Scene scene = SceneManager.GetSceneByPath(scenePath);

            if (!scene.IsValid() || !scene.isLoaded)
            {
                SetStatus(scenePath, SceneLoadState.Unloaded, 0f, null);
                return false;
            }

            if (scene == SceneManager.GetActiveScene())
            {
                _logger.Warning(
                    $"Déchargement refusé : {scenePath} est la scène active.");
                return false;
            }

            SetStatus(scenePath, SceneLoadState.Unloading, 0f, null);

            AsyncOperation operation = SceneManager.UnloadSceneAsync(scene);

            if (operation == null)
            {
                SetStatus(
                    scenePath,
                    SceneLoadState.Failed,
                    0f,
                    "UnloadSceneAsync a retourné null.");

                return false;
            }

            while (!operation.isDone)
            {
                UpdateProgress(scenePath, operation.progress);
                await Task.Yield();
            }

            SetStatus(scenePath, SceneLoadState.Unloaded, 1f, null);
            _logger.Info($"Scène déchargée : {scenePath}");

            return true;
        }

        public SceneLoadStatus GetStatus(string scenePath)
        {
            if (_statuses.TryGetValue(scenePath, out SceneLoadStatus status))
                return status;

            return new SceneLoadStatus
            {
                ScenePath = scenePath,
                State = SceneLoadState.Unloaded,
                Progress = 0f
            };
        }

        private async Task<Scene> LoadInternalAsync(
            string scenePath,
            LoadSceneMode mode)
        {
            Scene existing = SceneManager.GetSceneByPath(scenePath);

            if (mode == LoadSceneMode.Additive &&
                existing.IsValid() && existing.isLoaded)
                return existing;

            if (_inflightLoads.TryGetValue(scenePath, out Task<Scene> pending))
                return await pending;

            Task<Scene> task = PerformLoadAsync(scenePath, mode);
            _inflightLoads[scenePath] = task;

            try
            {
                return await task;
            }
            finally
            {
                _inflightLoads.Remove(scenePath);
            }
        }

        private async Task<Scene> PerformLoadAsync(
            string scenePath,
            LoadSceneMode mode)
        {
            SetStatus(scenePath, SceneLoadState.Loading, 0f, null);

            try
            {
                AsyncOperation operation =
                    SceneManager.LoadSceneAsync(scenePath, mode);

                if (operation == null)
                    throw new InvalidOperationException(
                        "LoadSceneAsync a retourné null. Vérifier le Build Profile.");

                while (!operation.isDone)
                {
                    UpdateProgress(scenePath, operation.progress);
                    await Task.Yield();
                }

                Scene loaded = SceneManager.GetSceneByPath(scenePath);

                if (!loaded.IsValid() || !loaded.isLoaded)
                    throw new InvalidOperationException(
                        "Unity a terminé le chargement mais la scène est introuvable.");

                SetStatus(scenePath, SceneLoadState.Loaded, 1f, null);
                _logger.Info($"Scène chargée : {scenePath}");

                return loaded;
            }
            catch (Exception exception)
            {
                SetStatus(
                    scenePath,
                    SceneLoadState.Failed,
                    0f,
                    exception.Message);

                _logger.Error($"Échec de chargement : {scenePath}", exception);
                throw;
            }
        }

        private void ValidateScenePath(string scenePath)
        {
            if (string.IsNullOrWhiteSpace(scenePath))
                throw new ArgumentException("Chemin de scène vide.");

            int buildIndex =
                SceneUtility.GetBuildIndexByScenePath(scenePath);

            if (buildIndex < 0)
                throw new InvalidOperationException(
                    $"Scène absente des Build Profiles : {scenePath}");
        }

        private void SetStatus(
            string path,
            SceneLoadState state,
            float progress,
            string error)
        {
            _statuses[path] = new SceneLoadStatus
            {
                ScenePath = path,
                State = state,
                Progress = progress,
                Error = error
            };
        }

        private void UpdateProgress(string path, float progress)
        {
            if (!_statuses.TryGetValue(path, out SceneLoadStatus status))
                status = new SceneLoadStatus { ScenePath = path };

            status.State = SceneLoadState.Loading;
            status.Progress = Mathf.Clamp01(progress);
            _statuses[path] = status;
        }

        private void EnsureInitialized()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("SceneLoader non initialisé.");
        }

        public void Shutdown()
        {
            _inflightLoads.Clear();
            IsInitialized = false;
        }
    }
}