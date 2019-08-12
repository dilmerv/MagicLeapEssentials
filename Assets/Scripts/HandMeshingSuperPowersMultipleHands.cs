using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

public class HandMeshingSuperPowersMultipleHands : MonoBehaviour
{
    [SerializeField, Tooltip("The Hand Meshing Behavior to control")]
    private MLHandMeshingBehavior _behavior = null;

    [SerializeField, Tooltip("Material used for hand meshing")]
    private Material _handMeshMaterial = null;

    [SerializeField, Tooltip("Key Pose to generate super powers")]
    private MLHandKeyPose _keyposeForSuperPowers = MLHandKeyPose.OpenHand;
    private const float _minimumConfidence = 0.5f;

    [SerializeField, Tooltip("Game Object holding the particle system for super powers on the left hand")]
    private GameObject _particleSystemToUseForLeftHand;

    [SerializeField, Tooltip("Game Object holding the particle system for super powers on the right hand")]
    private GameObject _particleSystemToUseForRightHand;

    [SerializeField]
    private float _particleOffset = 0.005f;
    
    [SerializeField]
    private Text _statusLeftHand;

    [SerializeField]
    private Text _statusRightHand;

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
            // left hand tracking
            if(MLHands.Left.KeyPose == _keyposeForSuperPowers && MLHands.Left.KeyPoseConfidence > _minimumConfidence)
            {
                _statusLeftHand.text = "Left Hand Open: <color=green>Yes</color>";
                _particleSystemToUseForLeftHand.SetActive(true);
                _particleSystemToUseForLeftHand.GetComponent<ParticleSystem>().Play();
            }
            else 
            {
                _statusLeftHand.text = "Left Hand Open: <color=red>No</color>";
                _particleSystemToUseForLeftHand.SetActive(false);
                _particleSystemToUseForLeftHand.GetComponent<ParticleSystem>().Stop();
            }

            _particleSystemToUseForLeftHand.transform.position = new Vector3(MLHands.Left.Center.x, MLHands.Left.Center.y, MLHands.Left.Center.z + _particleOffset);

            // right hand tracking
            if(MLHands.Right.KeyPose == _keyposeForSuperPowers && MLHands.Right.KeyPoseConfidence > _minimumConfidence)
            {
                _statusRightHand.text = "Right Hand Open: <color=green>Yes</color>";
                _particleSystemToUseForRightHand.SetActive(true);
                _particleSystemToUseForRightHand.GetComponent<ParticleSystem>().Play();
            }
            else 
            {
                _statusRightHand.text = "Right Hand Open: <color=red>No</color>";
                _particleSystemToUseForRightHand.SetActive(false);
                _particleSystemToUseForRightHand.GetComponent<ParticleSystem>().Stop();
            }

            _particleSystemToUseForRightHand.transform.position = new Vector3(MLHands.Right.Center.x, MLHands.Right.Center.y, MLHands.Right.Center.z + _particleOffset);
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