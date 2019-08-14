using UnityEngine;

public class GrabberDebug : MonoBehaviour
{
    #if !UNITY_EDITOR
    void Awake() => gameObject.SetActive(false);
    #endif
}