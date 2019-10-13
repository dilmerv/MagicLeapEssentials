using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;


[RequireComponent(typeof(ControllerConnectionHandler))]
public class ImageTracker : MonoBehaviour
{
    [SerializeField]
    private Text imageTrackingStatus;

    [SerializeField]
    private Text debugLog;

    [SerializeField]
    private MLImageTrackerBehavior imageTrackerBehaviour;

    [SerializeField]
    private GameObject targetprefab;

    private GameObject targetObject;

    private ControllerConnectionHandler connectionHandler;

    private PrivilegeRequester privilegeRequester;

    private bool startTracking = false;

    void Awake()
    {
        connectionHandler = GetComponent<ControllerConnectionHandler>();

        if (connectionHandler == null)
        {
            debugLog.text = "Error: ImageTrackingExample._controllerConnectionHandler is not set, disabling script.";
            enabled = false;
            return;
        }
        // If not listed here, the PrivilegeRequester assumes the request for
        // the privileges needed, CameraCapture in this case, are in the editor.
        privilegeRequester = GetComponent<PrivilegeRequester>();

        debugLog.text = "Setting up OnPrivilegesDone binding";
        // Before enabling the MLImageTrackerBehavior GameObjects, the scene must wait until the privilege has been granted.
        privilegeRequester.OnPrivilegesDone += HandlePrivilegesDone;
    }

    void Start()
    {
        MLInput.OnTriggerDown += HandleOnTriggerDown;
    }

    void HandlePrivilegesDone(MLResult result)
    {
        if (!result.IsOk)
        {
            if (result.Code == MLResultCode.PrivilegeDenied)
            {
                Instantiate(Resources.Load("PrivilegeDeniedError"));
            }

            debugLog.text = $"Error: ImageTrackingExample failed to get requested privileges, disabling script. Reason: {result}";
            enabled = false;
            return;
        }

        debugLog.text = $"Succeeded in requesting all privileges";
        StartCapture();
    }

    private void StartCapture()
    {
        imageTrackerBehaviour.gameObject.SetActive(true);
        imageTrackerBehaviour.enabled = true;
        imageTrackerBehaviour.OnTargetFound += OnTargetFound;
        imageTrackerBehaviour.OnTargetLost += OnTargetLost;
        imageTrackerBehaviour.OnTargetUpdated += OnTargetUpdated;
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

    private void HandleOnTriggerDown(byte controllerId, float value)
    {
        MLInputController controller = connectionHandler.ConnectedController;
        if (controller != null && controller.Id == controllerId)
        {
            MLInputControllerFeedbackIntensity intensity = (MLInputControllerFeedbackIntensity)((int)(value * 2.0f));
            controller.StartFeedbackPatternVibe(MLInputControllerFeedbackPatternVibe.Buzz, intensity);
            debugLog.text = "Start Capturing has been enabled by the trigger button action";
            startTracking = true;
        }
    }

    void OnDestroy()
    {
        MLInput.OnTriggerDown -= HandleOnTriggerDown;
        imageTrackerBehaviour.OnTargetFound -= OnTargetFound;
        imageTrackerBehaviour.OnTargetLost -= OnTargetLost;
        imageTrackerBehaviour.OnTargetUpdated -= OnTargetUpdated;
    }
}
