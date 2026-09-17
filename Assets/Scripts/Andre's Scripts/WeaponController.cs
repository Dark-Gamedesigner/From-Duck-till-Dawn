using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    public Transform muzzle;
    public WeaponData weaponData;

    public int countAmmo{ get; private set; }

    private bool reloading = false;
    private bool shooting = false;
    private bool readyToShoot =true;

    public List<ZielscheibeScript> zielscheibeScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        countAmmo = weaponData.maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame){
            Shoot();
        }
    }

    public void Shoot(){
        if (! readyToShoot || shooting || reloading || weaponData == null) return;
        if (countAmmo <= 0){Reload();return;}

        readyToShoot = false;
        shooting = true;

        UseAmmo();
        Invoke(nameof(ResetAttack), weaponData.fireRate);
        AttackRaycast();
        Instantiate(weaponData);
        
    }

    private void AttackRaycast(){
        RaycastHit hit;
        RaycastHit[] hits;

        switch (weaponData.Type){
            case WeaponType.Patrone:
                if (Physics.Raycast(transform.position, Camera.main.transform.forward, out hit, weaponData.weaponRange)){
                    Debug.DrawLine(transform.position,hit.point, Color.blue, 1);
                    HitTarget(hit);
                    var zielScheibe = hit.collider.GetComponent<ZielscheibeScript>();
                    if (zielScheibe){
                        zielScheibe.Hited(1);
                    }
                } 
                Debug.DrawLine(transform.position, transform.position + Camera.main.transform.forward * 100, Color.red, 1);
                break;
        }
    }

    private void HitTarget(RaycastHit hit){
        GameObject obj = Instantiate(weaponData.hitEffect, hit.point, Quaternion.identity);
        Destroy(obj, 4);
    }

    void ResetAttack(){
        shooting = false;
        readyToShoot = true;
    }
    private void UseAmmo(){
        countAmmo--;
    }
    
    public void Reload(){
        if (countAmmo == weaponData.maxAmmo || reloading) return;
        reloading = true;
        Invoke(nameof(ResetReload), weaponData.reload);
    }

    void ResetReload(){
        reloading = false;
        countAmmo = weaponData.maxAmmo;
    }
}