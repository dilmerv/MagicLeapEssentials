using UnityEngine;
using UnityEngine.XR.MagicLeap;

[RequireComponent(typeof(ControllerConnectionHandler))]
public class WeaponControllerManager : MonoBehaviour
{
    private WeaponController[] _weapons;

    private int currentWeapon = 0;

    private ControllerConnectionHandler _controllerConnectionHandler;
    
    void Awake()
    {
        _weapons = GetComponentsInChildren<WeaponController>(true);
        SelectNextWeapon();
    }

    void Start()
    {
        _controllerConnectionHandler = GetComponent<ControllerConnectionHandler>();
        MLInput.OnControllerButtonDown += HandleButtonDown;
    }

#if UNITY_EDITOR
    void Update()
    {   
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Selecting Next Gun
            SelectNextWeapon();
        }
    }
#endif

    void SelectNextWeapon()
    {
        _weapons[currentWeapon].gameObject.SetActive(true);
        // deactivate other guns
        DeactivateOthers();

        currentWeapon++;
        if(currentWeapon >= _weapons.Length)
            currentWeapon = 0;
    }

    void DeactivateOthers()
    {
        foreach(WeaponController controller in _weapons)
        {
            if(controller != _weapons[currentWeapon])
            {
                controller.gameObject.SetActive(false);
            }
        }
    }

    private void HandleButtonDown(byte controllerId, MLInputControllerButton button)
    {
        MLInputController controller = _controllerConnectionHandler.ConnectedController;
        if (controller != null && button == MLInputControllerButton.Bumper)
        {
            SelectNextWeapon();
        }
    }
}
