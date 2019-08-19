using TMPro;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

[RequireComponent(typeof(ControllerConnectionHandler))]
public class WeaponController : MonoBehaviour
{
    [SerializeField]
    private string nameYourWeapon = "NoName";

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private GameObject weapon;

    [SerializeField]
    private float force = 5.0f;

    [SerializeField]
    private ForceMode bulletForceMode = ForceMode.Force;

    [SerializeField, Range(-10.0f, 10.0f)]
    private float bulletInitialPositionX = 0;

    [SerializeField, Range(-10.0f, 10.0f)]
    private float bulletInitialPositionY = 0;

    [SerializeField, Range(-10.0f, 10.0f)]
    private float bulletInitialPositionZ = 0;

    [SerializeField]
    private float bulletAliveFor = 1.0f;

    [SerializeField]
    private float bulletFrequency = 0.5f;

    private float bulletTimer = 0;

    [SerializeField]
    private AudioSource bulletSound;

    [SerializeField]
    private TextMeshPro weaponStatsParamsText;

    private ControllerConnectionHandler _controllerConnectionHandler;

    void Start()
    {
        _controllerConnectionHandler = GetComponent<ControllerConnectionHandler>();
        MLInput.OnTriggerDown += HandleOnTriggerDown;
        
        if(weaponStatsParamsText != null)
        {
            weaponStatsParamsText.text = 
                string.Format("NAME: {0}\nRATE: {1}\nFORCE: {2}\nFREQUENCY: {3}",
                nameYourWeapon, force, bulletForceMode.ToString(), bulletFrequency);
        }
    }

#if UNITY_EDITOR

    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space))
           MultipleShoot();
    }

#endif

    void MultipleShoot()
    {
        bulletSound.Play();
        while(true)
        {
            Shoot();
            bulletTimer += Time.deltaTime * 1.0f;
            if(bulletTimer >= bulletFrequency)
            {
                bulletTimer = 0;
                break;
            }
        }   
    }

    void Shoot()
    {
        Vector3 bulletInitialPosition = new Vector3(bulletInitialPositionX, bulletInitialPositionY, bulletInitialPositionZ);

        GameObject bullet = Instantiate(bulletPrefab, bulletInitialPosition, Quaternion.identity);
        bullet.transform.parent = weapon.transform;
        bullet.transform.localPosition = bulletInitialPosition;
        bullet.transform.parent = transform.parent.parent;
        Rigidbody bulletRigidBody = bullet.GetComponent<Rigidbody>();
        bulletRigidBody.AddForceAtPosition(weapon.transform.forward * force, bullet.transform.position, bulletForceMode);
        Destroy(bullet, bulletAliveFor);

        // update stats
        StatsManager.Instance.IncrementBulletsUsed();
    }

    private void HandleOnTriggerDown(byte controllerId, float value)
    {
        MLInputController controller = _controllerConnectionHandler.ConnectedController;
        if (controller != null && controller.Id == controllerId && value == 1)
        {
            MLInputControllerFeedbackIntensity intensity = (MLInputControllerFeedbackIntensity)((int)(value * 2.0f));
            controller.StartFeedbackPatternVibe(MLInputControllerFeedbackPatternVibe.Buzz, intensity);
            MultipleShoot();
        }
    }
}
