using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour, IDDamage, IPickup {
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] CharacterController controller;

    [SerializeField] int health, speed, sprintMod, jumpSpeed, jumpMax, gravity, shootDistance, shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] GameObject gunModel;
    [SerializeField] List<GunStats> guns = new List<GunStats>();

    Vector3 moveDir;
    Vector3 playerVel;

    int jumpCount, gunList;
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
        if (Input.GetButton("Fire1") && guns.Count > 0 && guns[gunList].ammoCurrent > 0 && shootTimer >= shootRate) Shoot();
        SelectGun();
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
        guns[gunList].ammoCurrent--;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDistance, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);
            IDDamage damage = hit.collider.GetComponent<IDDamage>();
            Instantiate(guns[gunList].hitEffect, hit.point, Quaternion.identity);
            if (damage != null)
            {
                damage.TakeDamage(shootDamage);
            }
        }
    }
    void Reload()
    {
        if (Input.GetButtonDown("Reload"))
        {
            guns[gunList].ammoCurrent = guns[gunList].ammoMax;
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

    public void GetGunStats(GunStats gun)
    {
        guns.Add(gun);
        gunList = guns.Count - 1;
        ChangeGun();
    }
    void ChangeGun()
    {
        shootDamage = guns[gunList].shootDamage;
        shootDistance = guns[gunList].shootDistance;
        shootRate = guns[gunList].shootRate;
        gunModel.GetComponent<MeshFilter>().sharedMesh = guns[gunList].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = guns[gunList].model.GetComponent<MeshRenderer>().sharedMaterial;

    }
    void SelectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && gunList < guns.Count - 1) { gunList++; ChangeGun(); }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && gunList > 0) { gunList--; ChangeGun(); }
    }
}
