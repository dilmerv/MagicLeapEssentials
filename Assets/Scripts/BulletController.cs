using UnityEngine;

public class BulletController : MonoBehaviour
{   
    [SerializeField, Range(1, 30)]
    private float raycastCheckUnits = 5.0f;

    [SerializeField]
    private Vector3 enemyImpulseOnHit = Vector3.forward * 5.0f;

    [SerializeField]
    private LayerMask whatToHit = ~0;

    [SerializeField]
    private ForceMode enemyImpulseForceMode = ForceMode.Force;

    void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, raycastCheckUnits, whatToHit))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Rigidbody enemyRigidBody = hit.transform.GetComponent<Rigidbody>();

            StatsManager.Instance.IncrementItemsHit();

            if(enemyRigidBody != null)
            {
                enemyRigidBody.AddForceAtPosition(enemyImpulseOnHit, hit.transform.position, enemyImpulseForceMode);
            }
        }
    }
}
