using UnityEngine;
using UnityEngine.XR.MagicLeap;

[RequireComponent(typeof(ControllerConnectionHandler))]
public class Grabber : MonoBehaviour
{
    private ControllerConnectionHandler _controllerConnectionHandler;

    [SerializeField]
    private bool grabbing = false;

    public bool IsGrabbing => grabbing;

    void Start()
    {
        _controllerConnectionHandler = GetComponent<ControllerConnectionHandler>();

        MLInput.OnTriggerDown += HandleOnTriggerDown;
        MLInput.OnTriggerUp += HandleOnTriggerUp;
    }

    #if UNITY_EDITOR

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
            grabbing = true;
        if(Input.GetKeyUp(KeyCode.LeftShift))
            grabbing = false;
    }

    #endif
    private void HandleOnTriggerDown(byte controllerId, float value)
    {
        MLInputController controller = _controllerConnectionHandler.ConnectedController;
        if (controller != null && controller.Id == controllerId)
        {
            MLInputControllerFeedbackIntensity intensity = (MLInputControllerFeedbackIntensity)((int)(value * 2.0f));
            controller.StartFeedbackPatternVibe(MLInputControllerFeedbackPatternVibe.Buzz, intensity);
            grabbing = true;
        }
    }

    private void HandleOnTriggerUp(byte controllerId, float value)
    {
        MLInputController controller = _controllerConnectionHandler.ConnectedController;
        if (controller != null && controller.Id == controllerId)
        {
            grabbing = false;
        }
    }
}