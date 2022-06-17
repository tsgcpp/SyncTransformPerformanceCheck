using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneGenerationSetting", menuName = "ScriptableObjects/SceneGenerationSetting", order = 1)]
public sealed class SceneGenerationSetting : ScriptableObject
{
    [SerializeField] private SceneAsset m_baseScene;
    public SceneAsset BaseScene => m_baseScene;

    [SerializeField] private string m_sceneFolderPath;
    public string SceneFolderPath => m_sceneFolderPath;

    [SerializeField] private GameObject m_playerPrefab;
    public GameObject PlayerPrefab => m_playerPrefab;

    [SerializeField] private GameObject m_physicsSynchronizerPrefab;
    public GameObject PhysicsSynchronizerPrefab => m_physicsSynchronizerPrefab;

    [SerializeField] private List<GameObject> m_targetPrefabs;
    public IReadOnlyList<GameObject> TargetPrefabs => m_targetPrefabs;
}
