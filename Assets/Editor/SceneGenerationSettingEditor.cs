using System.Collections.Generic;
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
            GenerateScene(
                baseScenePath: baseScenePath,
                sceneFolderPath: setting.SceneFolderPath,
                playerPrefab: setting.PlayerPrefab,
                targetPrefab: targetPrefab,
                colliderPrefabs: setting.ColliderPrefabs);
        }

        foreach (var targetPrefab in setting.TargetPrefabs)
        {
            GenerateScene(
                baseScenePath: baseScenePath,
                sceneFolderPath: setting.SceneFolderPath,
                playerPrefab: setting.PlayerPrefab,
                targetPrefab: targetPrefab,
                colliderPrefabs: setting.ColliderPrefabs,
                physicsSynchronizerPrefab: setting.PhysicsSynchronizerPrefab);
        }

        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
    }

    private void GenerateScene(
        string baseScenePath,
        string sceneFolderPath,
        GameObject playerPrefab,
        GameObject targetPrefab,
        IReadOnlyList<GameObject> colliderPrefabs,
        GameObject physicsSynchronizerPrefab = null)
    {
        foreach (var colliderPrefab in colliderPrefabs)
        {
            var scene = GeneratePerformanceCheckScene(
                baseScenePath: baseScenePath,
                playerPrefab: playerPrefab,
                targetPrefab: targetPrefab,
                colliderPrefab: colliderPrefab,
                physicsSynchronizerPrefab: physicsSynchronizerPrefab);
            var sceneName = CreatePerformanceCheckSceneName(
                targetPrefab: targetPrefab,
                colliderPrefab: colliderPrefab,
                physicsSynchronizerPrefab: physicsSynchronizerPrefab);
            SaveScene(scene, sceneName, sceneFolderPath);
        }
    }

    public static Scene GeneratePerformanceCheckScene(
        string baseScenePath,
        GameObject playerPrefab,
        GameObject targetPrefab,
        GameObject colliderPrefab,
        GameObject physicsSynchronizerPrefab = null)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        var scene = EditorSceneManager.OpenScene(baseScenePath);

        if (physicsSynchronizerPrefab)
        {
            PrefabUtility.InstantiatePrefab(physicsSynchronizerPrefab);
        }

        PrefabUtility.InstantiatePrefab(playerPrefab);
        var instantiatedTargetPrefab = PrefabUtility.InstantiatePrefab(targetPrefab) as GameObject;

        var prefabInstantiatorSO = new SerializedObject(instantiatedTargetPrefab.GetComponent<PrefabInstantiator>());
        prefabInstantiatorSO.FindProperty("_prefab").objectReferenceValue = colliderPrefab;
        prefabInstantiatorSO.ApplyModifiedPropertiesWithoutUndo();

        return scene;
    }

    public static void SaveScene(Scene scene, string sceneName, string sceneFolderPath)
    {
        EditorSceneManager.SaveScene(scene, $"{sceneFolderPath}/{sceneName}.unity");
    }

    public static string CreatePerformanceCheckSceneName(
        GameObject targetPrefab,
        GameObject colliderPrefab,
        GameObject physicsSynchronizerPrefab = null)
    {
        string name = $"PerformanceCheck_{targetPrefab.name}_{colliderPrefab.name}";

        if (physicsSynchronizerPrefab)
        {
            name += $"_{physicsSynchronizerPrefab.name}";
        }

        return name;
    }
}
