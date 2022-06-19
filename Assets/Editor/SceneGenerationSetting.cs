using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneGenerationSetting", menuName = "ScriptableObjects/SceneGenerationSetting", order = 1)]
public sealed class SceneGenerationSetting : ScriptableObject
{
    [SerializeField] private SceneAsset _baseScene;
    public SceneAsset BaseScene => _baseScene;

    [SerializeField] private string _sceneFolderPath;
    public string SceneFolderPath => _sceneFolderPath;

    [SerializeField] private GameObject _playerPrefab;
    public GameObject PlayerPrefab => _playerPrefab;

    [SerializeField] private GameObject _physicsSynchronizerPrefab;
    public GameObject PhysicsSynchronizerPrefab => _physicsSynchronizerPrefab;

    [SerializeField] private List<GameObject> _targetPrefabs;
    public IReadOnlyList<GameObject> TargetPrefabs => _targetPrefabs;

    [SerializeField] private List<GameObject> _colliderPrefabs;
    public IReadOnlyList<GameObject> ColliderPrefabs => _colliderPrefabs;
}
