using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

[RequireComponent(typeof(ControllerConnectionHandler))]
public class ImageTrackerMultiple : MonoBehaviour
{
    [SerializeField]
    private Text imageTrackingStatus;

    [SerializeField]
    private Text debugLog;

    [SerializeField]
    private ImageTrackingObject[] imageTrackingObjects;

    private ControllerConnectionHandler connectionHandler;

    private PrivilegeRequester privilegeRequester;

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

        // Setup ML Image Tracking Behaviour 
        foreach(ImageTrackingObject imageTrackingObject in imageTrackingObjects)
        {
            imageTrackingObject.Setup();
        }
    }

    private void HandleOnTriggerDown(byte controllerId, float value)
    {
        MLInputController controller = connectionHandler.ConnectedController;
        if (controller != null && controller.Id == controllerId)
        {
            MLInputControllerFeedbackIntensity intensity = (MLInputControllerFeedbackIntensity)((int)(value * 2.0f));
            controller.StartFeedbackPatternVibe(MLInputControllerFeedbackPatternVibe.Buzz, intensity);
            debugLog.text = "Start Capturing has been enabled by the trigger button action";
            
            // Start tracking
            foreach(ImageTrackingObject imageTrackingObject in imageTrackingObjects)
            {
                imageTrackingObject.Start();
            }
        }
    }

    void OnDestroy()
    {
        MLInput.OnTriggerDown -= HandleOnTriggerDown;

        // Stop tracking
        foreach(ImageTrackingObject imageTrackingObject in imageTrackingObjects)
        {
            imageTrackingObject.Stop();
        }
    }
}
