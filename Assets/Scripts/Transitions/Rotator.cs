using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private Vector3 rotationSpeed = Vector3.zero;

    void Update() => transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);
}
