using TMPro;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro selectableStateText = null;

    [SerializeField]
    private string selectableStateTextFormat = "Selectable State: <color={0}>({1})</color>\nHovering: {2}\nSelecting: {3}";

    [SerializeField]
    private Material noneStateMaterial;

    [SerializeField]
    private Material hoverStateMaterial;

    [SerializeField]
    private Material selectStateMaterial;

    private Grabber _grabber = null;

    private MeshRenderer _renderer = null;

    public bool Hovering { get; private set; }

    public bool Selecting { get; private set; }

    void OnTriggerEnter(Collider other) => Select(other);

    //void OnTriggerStay(Collider other) => Release(other);

    void OnTriggerExit(Collider other) => Release(other);

    void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _renderer.material = noneStateMaterial;
    }

    private void Select(Collider other)
    {
        _grabber = other.gameObject.GetComponent<Grabber>();
        if(_grabber != null && !_grabber.IsTriggerOn)
        {
            Hovering = true;
            Selecting = false;
            _renderer.material = hoverStateMaterial;
        }
        else if(_grabber != null && _grabber.IsTriggerOn)
        {
            Hovering = false;
            Selecting = true;
            _renderer.material = selectStateMaterial;
        }

        if(selectableStateText != null)
        {
            selectableStateText.text = string.Format(selectableStateTextFormat, "green", "ON", Hovering, Selecting);
        }
    }
    private void Release(Collider other)
    {
        // in case grab didn't get called
        if(_grabber == null)
            Select(other);

        if(_grabber != null)
        {
            Hovering = false;
            Selecting = false;

            _renderer.material = noneStateMaterial;

            if(selectableStateText != null)
            {
                selectableStateText.text = string.Format(selectableStateTextFormat, "red", "OFF", Hovering, Selecting);
            }
        }
    }
}
