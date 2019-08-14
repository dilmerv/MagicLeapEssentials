using TMPro;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro grabbedStateText = null;

    [SerializeField]
    private string grabbedStateTextFormat = "Grab State: <color={0}>({1})</color>";

    [SerializeField]
    private bool useGravity = false;

    private Grabber _grabber = null;

    private Transform _originalParent = null;

    private Rigidbody _rigidBody = null;

    public bool Grabbed { get; private set;}

    void Awake()
    {
        _originalParent = transform.parent;
        _rigidBody = GetComponent<Rigidbody>();
        if(_rigidBody != null)
        {
            _rigidBody.isKinematic = !useGravity;
        }
    }

    void OnTriggerEnter(Collider other) => Grab(other);

    void OnTriggerStay(Collider other) => Release(other);

    void OnTriggerExit(Collider other) => Release(other);

    private void Grab(Collider other)
    {
        _grabber = other.gameObject.GetComponent<Grabber>();
        if(_grabber != null && _grabber.IsGrabbing)
        {
            Grabbed = true;

            if(_rigidBody != null && useGravity)
            {
                _rigidBody.isKinematic = true;
            }
            transform.parent = _grabber.transform;
            if(grabbedStateText != null)
            {
                grabbedStateText.text = string.Format(grabbedStateTextFormat, "green", "ON");
            }
        }
    }
    private void Release(Collider other)
    {
        // in case grab didn't get called
        if(_grabber == null)
            Grab(other);

        if(_grabber != null && !_grabber.IsGrabbing)
        {
            Grabbed = false;
            if(_rigidBody != null && useGravity)
            {
                _rigidBody.isKinematic = false;
            }
            _grabber = null;
            transform.parent = _originalParent;
            if(grabbedStateText != null)
            {
                grabbedStateText.text = string.Format(grabbedStateTextFormat, "red", "OFF");
            }
        }
    }
}
