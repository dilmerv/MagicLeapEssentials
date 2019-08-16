using UnityEngine;
using UnityEngine.XR.MagicLeap;

[RequireComponent(typeof(ControllerConnectionHandler))]
public class Grabber : MonoBehaviour
{
    private ControllerConnectionHandler _controllerConnectionHandler;

    [SerializeField]
    private bool isTriggerOn = false;

    public bool IsTriggerOn => isTriggerOn;

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
            isTriggerOn = true;
        if(Input.GetKeyUp(KeyCode.LeftShift))
            isTriggerOn = false;
    }

    #endif
    private void HandleOnTriggerDown(byte controllerId, float value)
    {
        MLInputController controller = _controllerConnectionHandler.ConnectedController;
        if (controller != null && controller.Id == controllerId)
        {
            MLInputControllerFeedbackIntensity intensity = (MLInputControllerFeedbackIntensity)((int)(value * 2.0f));
            controller.StartFeedbackPatternVibe(MLInputControllerFeedbackPatternVibe.Buzz, intensity);
            isTriggerOn = true;
        }
    }

    private void HandleOnTriggerUp(byte controllerId, float value)
    {
        MLInputController controller = _controllerConnectionHandler.ConnectedController;
        if (controller != null && controller.Id == controllerId)
        {
            isTriggerOn = false;
        }
    }
}