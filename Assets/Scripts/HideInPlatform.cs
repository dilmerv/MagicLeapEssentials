using UnityEngine;

public class HideInPlatform : MonoBehaviour
{
    #if !UNITY_EDITOR
    void Awake() => gameObject.SetActive(false);
    #endif
}