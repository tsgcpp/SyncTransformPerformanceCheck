using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class PrefabGeneratorEditorWindow : EditorWindow
{
    public Transform _root;
    [SerializeField] GameObject _prefab;
    public Vector3Int _prefabCount = new Vector3Int(2, 2, 2);
    public Vector3 _localMargin = new Vector3(1f, 1f, 1f);

    [MenuItem("PrefabGenerator/Prefab Generator Editor Window")]
    static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(PrefabGeneratorEditorWindow));
    }

    void OnGUI()
    {
        if (GUILayout.Button("Create Prefabs"))
        {
            CreatePrefabs();
        }

        _root = EditorGUILayout.ObjectField("Root", _root, typeof(Transform), allowSceneObjects: true) as Transform;
        _prefab = EditorGUILayout.ObjectField("Prefab", _prefab, typeof(GameObject), allowSceneObjects: false) as GameObject;
        _prefabCount = EditorGUILayout.Vector3IntField("Prefab Count", _prefabCount);
        _localMargin = EditorGUILayout.Vector3Field("Local Margine", _localMargin);
    }

    private void CreatePrefabs()
    {
        for (int z = 0; z < _prefabCount.z; ++z)
        {
            for (int y = 0; y < _prefabCount.y; ++y)
            {
                for (int x = 0; x < _prefabCount.x; ++x)
                {
                    var go = PrefabUtility.InstantiatePrefab(_prefab, parent: _root) as GameObject;
                    go.transform.localPosition = new Vector3(
                        x: CalcPoint(x, _prefabCount.x, _localMargin.x),
                        y: CalcPoint(y, _prefabCount.y, _localMargin.y),
                        z: CalcPoint(z, _prefabCount.z, _localMargin.z));
                }
            }
        }

        EditorSceneManager.MarkSceneDirty(_root.gameObject.scene);
    }

    private static float CalcPoint(int index, int count, float margin)
    {
        float start = -margin * (float)(count - 1) / 2.0f;
        return start + margin * (float)index;
    }
}