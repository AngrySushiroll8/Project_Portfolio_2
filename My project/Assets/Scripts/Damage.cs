using Unity.Jobs;
using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour
{
    enum DamageType { moving, stationary, DOT, homing }
    [SerializeField] DamageType type;
    [SerializeField] Rigidbody body;
    [SerializeField] int damageAmount, speed, destroyTime;
    [SerializeField] float damageRate;
    bool isDamaging;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (type == DamageType.moving || type == DamageType.homing)
        {
            Destroy(gameObject, destroyTime);
            if(type == DamageType.moving) body.linearVelocity = transform.forward * speed;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(type == DamageType.homing)
        {
            body.linearVelocity = (GameManager.instance.player.transform.position - transform.position).normalized * speed * Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        IDDamage damage = other.GetComponent<IDDamage>();
        if(damage != null && type != DamageType.DOT)
        {
            damage.TakeDamage(damageAmount);
        }
        if (type == DamageType.moving || type == DamageType.homing)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        IDDamage damage = other.GetComponent <IDDamage>();
        if(damage != null && type == DamageType.DOT)
        {
            if(!isDamaging)
            {
                StartCoroutine(DamageOther(damage));
            }
        }
    }
    IEnumerator DamageOther(IDDamage damage) {
        isDamaging = true;
        damage.TakeDamage(damageAmount);
        yield return new WaitForSeconds(damageRate);
        isDamaging = false;
    }
}
