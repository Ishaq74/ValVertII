using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ValVertII.Core
{
    public enum ValidationSeverity
    {
        Info,
        Warning,
        Error
    }

    public sealed class ValidationIssue
    {
        public ValidationSeverity Severity;
        public string Code;
        public string Message;
    }

    public sealed class ValidationReport
    {
        public readonly List<ValidationIssue> Issues =
            new List<ValidationIssue>();

        public int ErrorCount
        {
            get
            {
                int count = 0;

                foreach (var issue in Issues)
                    if (issue.Severity == ValidationSeverity.Error)
                        count++;

                return count;
            }
        }

        public int WarningCount
        {
            get
            {
                int count = 0;

                foreach (var issue in Issues)
                    if (issue.Severity == ValidationSeverity.Warning)
                        count++;

                return count;
            }
        }

        public bool IsValid => ErrorCount == 0;

        public void Add(
            ValidationSeverity severity,
            string code,
            string message)
        {
            Issues.Add(new ValidationIssue
            {
                Severity = severity,
                Code = code,
                Message = message
            });
        }
    }

    public sealed class ProjectValidator : IAppService
    {
        private readonly IdRegistry _registry;
        private readonly AppLogger _logger;

        public bool IsInitialized { get; private set; }

        public ProjectValidator(IdRegistry registry, AppLogger logger)
        {
            _registry = registry;
            _logger = logger;
        }

        public void Initialize()
        {
            IsInitialized = true;
        }

        public ValidationReport ValidateLoadedContent()
        {
            if (!IsInitialized)
                throw new System.InvalidOperationException(
                    "ProjectValidator non initialisé.");

            var report = new ValidationReport();
            var seen = new Dictionary<string, StableId>();

            StableId[] all =
                UnityEngine.Object.FindObjectsByType<StableId>();

            foreach (StableId item in all)
            {
                if (item == null) continue;

                if (string.IsNullOrWhiteSpace(item.Id))
                {
                    report.Add(
                        ValidationSeverity.Error,
                        "ID_EMPTY",
                        $"StableId vide sur '{item.name}' " +
                        $"(scène {item.gameObject.scene.name}).");

                    continue;
                }

                if (seen.TryGetValue(item.Id, out StableId previous) &&
                    previous != item)
                {
                    report.Add(
                        ValidationSeverity.Error,
                        "ID_DUPLICATE",
                        $"ID '{item.Id}' utilisé par '{previous.name}' " +
                        $"et '{item.name}'.");
                }
                else
                {
                    seen[item.Id] = item;
                }
            }

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);

                if (scene.IsValid() && scene.isLoaded)
                {
                    report.Add(
                        ValidationSeverity.Info,
                        "SCENE_LOADED",
                        $"Scène chargée : {scene.path}");
                }
            }

            foreach (ValidationIssue issue in report.Issues)
            {
                string line = $"[{issue.Code}] {issue.Message}";

                if (issue.Severity == ValidationSeverity.Error)
                    _logger.Error(line);
                else if (issue.Severity == ValidationSeverity.Warning)
                    _logger.Warning(line);
                else
                    _logger.Info(line);
            }

            _registry.RebuildFromLoadedObjects();

            _logger.Info(
                $"Validation terminée : {report.ErrorCount} erreur(s), " +
                $"{report.WarningCount} avertissement(s).");

            return report;
        }

        public bool IsSceneInBuild(string scenePath)
        {
            return !string.IsNullOrWhiteSpace(scenePath) &&
                SceneUtility.GetBuildIndexByScenePath(scenePath) >= 0;
        }

        public void Shutdown()
        {
            IsInitialized = false;
        }
    }
}