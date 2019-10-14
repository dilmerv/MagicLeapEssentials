using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

[RequireComponent(typeof(MLImageTrackerBehavior))]
public class ImageTrackingObject : MonoBehaviour
{
    [SerializeField]
    private MLImageTrackerBehavior imageTrackerBehaviour;

     [SerializeField]
    private GameObject targetprefab;

    private GameObject targetObject;

    [SerializeField]
    private Text debugLog;

    [SerializeField]
    private Text imageTrackingStatus;

    private bool startTracking;

    public void Setup()
    {
        imageTrackerBehaviour.gameObject.SetActive(true);
        imageTrackerBehaviour.enabled = true;
        imageTrackerBehaviour.OnTargetFound += OnTargetFound;
        imageTrackerBehaviour.OnTargetLost += OnTargetLost;
        imageTrackerBehaviour.OnTargetUpdated += OnTargetUpdated;
    }

    public void Start()
    {
        startTracking = true;
    }

    public void Stop()
    {
        startTracking = false;
        imageTrackerBehaviour.gameObject.SetActive(false);
        imageTrackerBehaviour.enabled = false;
    }

    private void OnTargetFound(bool isReliable)
    {
        debugLog.text = isReliable ? "The target is reliable" : "The target is not reliable";
    }

    private void OnTargetLost()
    {
        debugLog.text = "The target was lost";
    }

    private void OnTargetUpdated(MLImageTargetResult imageTargetResult)
    {
        if(!startTracking){
            debugLog.text = "Start capturing is currently disabled";
            targetObject.SetActive(startTracking);
            return;
        }

        targetObject.SetActive(startTracking);
        imageTrackingStatus.text = imageTargetResult.Status.ToString();

        if(targetObject == null)
            targetObject = Instantiate(targetprefab, imageTargetResult.Position, imageTargetResult.Rotation);
        else
            targetObject.transform.SetPositionAndRotation(imageTargetResult.Position, imageTargetResult.Rotation);
    }

    void OnDestroy()
    {
        imageTrackerBehaviour.OnTargetFound -= OnTargetFound;
        imageTrackerBehaviour.OnTargetLost -= OnTargetLost;
        imageTrackerBehaviour.OnTargetUpdated -= OnTargetUpdated;   
    }
}