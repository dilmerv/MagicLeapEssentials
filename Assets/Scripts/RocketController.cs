using UnityEngine;
using UnityEngine.XR.MagicLeap;

[RequireComponent(typeof(ControllerConnectionHandler))]
public class RocketController : MonoBehaviour
{
    private ControllerConnectionHandler _controllerConnectionHandler;

    [SerializeField]
    private float upwardForce = 5.0f;

    private Rigidbody _rigidbody;

    private bool _isTriggerDown = false;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _controllerConnectionHandler = GetComponent<ControllerConnectionHandler>();
        MLInput.OnTriggerDown += HandleOnTriggerDown;
        MLInput.OnTriggerUp += HandleOnTriggerUp;
    }
    void FixedUpdate()
    {
#if UNITY_EDITOR
        if(Input.GetKey(KeyCode.UpArrow))
        {
            ApplyForce();
        }

#else
        if(_isTriggerDown)
        {
            ApplyForce();
        }
#endif
    }

    private void HandleOnTriggerDown(byte controllerId, float value)
    {
        MLInputController controller = _controllerConnectionHandler.ConnectedController;
        if (controller != null && controller.Id == controllerId)
        {
            MLInputControllerFeedbackIntensity intensity = (MLInputControllerFeedbackIntensity)((int)(value * 2.0f));
            controller.StartFeedbackPatternVibe(MLInputControllerFeedbackPatternVibe.Buzz, intensity);
            _isTriggerDown = true;
        }
    }

    private void HandleOnTriggerUp(byte controllerId, float value)
    {
        MLInputController controller = _controllerConnectionHandler.ConnectedController;
        if (controller != null && controller.Id == controllerId)
        {
            _isTriggerDown = false;
        }
    }

    void ApplyForce() => _rigidbody.AddForceAtPosition(Vector3.up * upwardForce, transform.position, ForceMode.Acceleration);
    
    void OnDestroy()
    {
        MLInput.OnTriggerDown -= HandleOnTriggerDown;
        MLInput.OnTriggerUp -= HandleOnTriggerUp;
    }
}
