using UnityEditor;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] GunStats gun;
    private void OnTriggerEnter(Collider other)
    {
        IPickup pickup = other.GetComponent<IPickup>();
        if(pickup != null )
        {
            pickup.GetGunStats(gun);
            Destroy(gameObject);
        }
    }
}
