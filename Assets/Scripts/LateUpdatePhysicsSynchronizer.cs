using UnityEngine;

[DefaultExecutionOrder(-1)]
public class LateUpdatePhysicsSynchronizer : MonoBehaviour
{
    void LateUpdate()
    {
        Physics.SyncTransforms();
    }
}
