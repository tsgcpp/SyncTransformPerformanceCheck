using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(SceneGenerationSetting))]
public sealed class SceneGenerationSettingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Tools");
        if (GUILayout.Button("Generate Performance Check Scenes"))
        {
            GenerateScenes();
            Debug.Log($"Generated scenes from \"{target.name}\".");
        }

        EditorGUILayout.Space(8f);
        DrawDefaultInspector();
    }

    private void GenerateScenes()
    {
        var setting = target as SceneGenerationSetting;
        string baseScenePath = AssetDatabase.GetAssetPath(setting.BaseScene);

        foreach (var targetPrefab in setting.TargetPrefabs)
        {
            var scene = GeneratePerformanceCheckScene(
                baseScenePath: baseScenePath,
                playerPrefab: setting.PlayerPrefab,
                targetPrefab: targetPrefab);
            var sceneName = CreatePerformanceCheckSceneName(targetPrefab);
            SaveScene(scene, sceneName, setting.SceneFolderPath);
        }

        foreach (var targetPrefab in setting.TargetPrefabs)
        {
            var scene = GeneratePerformanceCheckScene(
                baseScenePath: baseScenePath,
                playerPrefab: setting.PlayerPrefab,
                targetPrefab: targetPrefab,
                physicsSynchronizerPrefab: setting.PhysicsSynchronizerPrefab);
            var sceneName = CreatePerformanceCheckSceneName(targetPrefab, setting.PhysicsSynchronizerPrefab);
            SaveScene(scene, sceneName, setting.SceneFolderPath);
        }
    }

    public static Scene GeneratePerformanceCheckScene(
        string baseScenePath,
        GameObject playerPrefab,
        GameObject targetPrefab,
        GameObject physicsSynchronizerPrefab = null)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        var scene = EditorSceneManager.OpenScene(baseScenePath);

        if (physicsSynchronizerPrefab)
        {
            PrefabUtility.InstantiatePrefab(physicsSynchronizerPrefab);
        }

        PrefabUtility.InstantiatePrefab(playerPrefab);
        PrefabUtility.InstantiatePrefab(targetPrefab);

        return scene;
    }

    public static void SaveScene(Scene scene, string sceneName, string sceneFolderPath)
    {
        EditorSceneManager.SaveScene(scene, $"{sceneFolderPath}/{sceneName}.unity");
    }

    public static string CreatePerformanceCheckSceneName(
        GameObject targetPrefab,
        GameObject physicsSynchronizerPrefab = null)
    {
        string name = $"PerformanceCheck_{targetPrefab.name}";

        if (physicsSynchronizerPrefab)
        {
            name += "_" + physicsSynchronizerPrefab.name;
        }

        return name;
    }
}
