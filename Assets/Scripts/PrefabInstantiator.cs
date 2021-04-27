using UnityEngine;
using System.Diagnostics;

public class PrefabInstantiator : MonoBehaviour
{
    [SerializeField] Transform _colliderRoot;

    [SerializeField] GameObject _prefab;

    public Vector3Int _prefabCount = new Vector3Int(2, 2, 2);

    public Vector3 _localMargin = new Vector3(1f, 1f, 1f);

    void Start()
    {
        for (int z = 0; z < _prefabCount.z; ++z)
        {
            for (int y = 0; y < _prefabCount.y; ++y)
            {
                for (int x = 0; x < _prefabCount.x; ++x)
                {
                    var go = GameObject.Instantiate(_prefab, parent: _colliderRoot);
                    go.transform.localPosition = new Vector3(
                        x: CalcPoint(x, _prefabCount.x, _localMargin.x),
                        y: CalcPoint(y, _prefabCount.y, _localMargin.y),
                        z: CalcPoint(z, _prefabCount.z, _localMargin.z));
                }
            }
        }
    }

    private static float CalcPoint(int index, int count, float margin)
    {
        float start = -margin * (float)(count - 1) / 2.0f;
        return start + margin * (float)index;
    }
}
