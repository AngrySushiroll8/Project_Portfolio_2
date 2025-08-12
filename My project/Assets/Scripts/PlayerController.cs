using UnityEditor.Search;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDDamage {
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] CharacterController controller;

    [SerializeField] int health;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;

    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDistance;

    Vector3 moveDir;
    Vector3 playerVel;

    int jumpCount;
    bool isSprint;

    float shootTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Debug.DrawRay(Camera.main.transform.position, transform.forward * shootDistance, Color.green);
        Movement();
        Sprint();
    }
    void Movement() {
        shootTimer += Time.deltaTime;
        if (controller.isGrounded) jumpCount = 0;
        else playerVel.y -= gravity * Time.deltaTime;
        moveDir = (Input.GetAxis("Horizontal") * transform.right) + (Input.GetAxis("Vertical") * transform.forward);
        controller.Move(moveDir * speed * Time.deltaTime);
        Jump();
        controller.Move(playerVel * Time.deltaTime);
        playerVel.y -= gravity * Time.deltaTime;
        if (Input.GetButton("Fire1") && shootTimer >= shootRate) Shoot();
    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax) {
            jumpCount++;
            playerVel.y = jumpSpeed;
        }
    }
    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
            isSprint = true;
        }
        else if(Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
            isSprint = false;
        }
    }
    void Shoot()
    {
        shootTimer = 0;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDistance, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);
            IDDamage damage = hit.collider.GetComponent<IDDamage>();
            if (damage != null)
            {
                damage.TakeDamage(shootDamage);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            GameManager.instance.YouLose();
        }
    }
}
