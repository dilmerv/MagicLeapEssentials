using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableRotation : MonoBehaviour
{
    [SerializeField, Range(10, 100)]
    private float rotationSpeed = 50.0f;

    private Grabbable _grabbable;

    void Awake() => _grabbable = GetComponent<Grabbable>();

    void Update()
    {
        if(_grabbable != null && !_grabbable.Grabbed)
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }
}
