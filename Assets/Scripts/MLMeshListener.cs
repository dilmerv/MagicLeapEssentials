using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.MagicLeap;

[RequireComponent(typeof(MLSpatialMapper))]
public class MLMeshListener : MonoBehaviour
{
    private MLSpatialMapper _mlSpatialMapper;

    [SerializeField]
    private ParticleSystem collidingParticleSystem;

    void Start()
    {
        _mlSpatialMapper = GetComponent<MLSpatialMapper>();
        _mlSpatialMapper.meshAdded += MeshAdded;
    }

    void MeshAdded(TrackableId trackableId)
    {
        GameObject gameObject = _mlSpatialMapper.meshIdToGameObjectMap[trackableId];
        Collider collider = gameObject.GetComponent<Collider>();
    }

    void OnDestroy()
    {
        _mlSpatialMapper.meshAdded -= MeshAdded;
    }
}
