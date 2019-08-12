using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

public class HandMeshingSuperPowers : MonoBehaviour
{
    [SerializeField, Tooltip("The Hand Meshing Behavior to control")]
    private MLHandMeshingBehavior _behavior = null;

    [SerializeField, Tooltip("Material used for hand meshing")]
    private Material _handMeshMaterial = null;

    [SerializeField, Tooltip("Key Pose to generate super powers")]
    private MLHandKeyPose _keyposeForSuperPowers = MLHandKeyPose.OpenHand;
    private const float _minimumConfidence = 0.5f;

    [SerializeField, Tooltip("Game Object holding the particle system for super powers")]
    private GameObject _particleSystemToUse;

    [SerializeField]
    private float _particleOffset = 0.005f;
    
    [SerializeField]
    private Text _status;

    /// <summary>
    /// Validate and initialize properties
    /// </summary>
    void Start()
    {
        if (_behavior == null)
        {
            Debug.LogError("Error: HandMeshingExample._behavior is not set, disabling script.");
            enabled = false;
            return;
        }

        if (_handMeshMaterial == null)
        {
            Debug.LogError("Error: HandMeshingExample._wireframeMaterial is not set, disabling script.");
            enabled = false;
            return;
        }

        MLResult result = MLHands.Start();
        if (!result.IsOk)
        {
            Debug.LogError("Error: HandMeshingExample failed to start MLHands, disabling script.");
            enabled = false;
            return;
        }
        MLHands.KeyPoseManager.EnableKeyPoses(new [] { _keyposeForSuperPowers }, true, true);
        MLHands.KeyPoseManager.SetPoseFilterLevel(MLPoseFilterLevel.ExtraRobust);
        MLHands.KeyPoseManager.SetKeyPointsFilterLevel(MLKeyPointFilterLevel.ExtraSmoothed);

        _behavior.MeshMaterial = _handMeshMaterial;
    }

    void Update()
    {
        if(MLHands.IsStarted)
        {
            if(MLHands.Right.KeyPose == _keyposeForSuperPowers && MLHands.Right.KeyPoseConfidence > _minimumConfidence)
            {
                _status.text = "Is Hand Open: <color=green>Yes</color>";
                _particleSystemToUse.SetActive(true);
                _particleSystemToUse.GetComponent<ParticleSystem>().Play();
                _particleSystemToUse.transform.position = new Vector3(MLHands.Right.Center.x, MLHands.Right.Center.y, MLHands.Right.Center.z + _particleOffset);
            }
            else 
            {
                _status.text = "Is Hand Open: <color=red>No</color>";
                _particleSystemToUse.SetActive(false);
                _particleSystemToUse.GetComponent<ParticleSystem>().Stop();
            }
        }
    }

    void OnDestroy()
    {
        if (MLHands.IsStarted)
        {
            MLHands.KeyPoseManager.DisableAllKeyPoses();
            MLHands.Stop();
        }
    }
}