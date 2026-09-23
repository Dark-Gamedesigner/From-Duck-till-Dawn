using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    public WeaponData weaponData;

    public int CountAmmo{ get; private set; }

    private bool _reloading = false;
    private bool _shooting = false;
    private bool _readyToShoot =true;

    public List<ZielscheibeScript> zielscheibeScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        CountAmmo = weaponData.maxAmmo;
    }

    // Wenn die linke Maustaste gedrueckt wird die Methode schießen aufgerufen 
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame){
            Shoot();
        }
    }

    // Methode für das Schießen
    public void Shoot(){
        // wenn bereit zum Schießen, Schuss, Nachladen, und das scriptable Object null ist passiert nichts 
        if (! _readyToShoot || _shooting || _reloading || weaponData == null) return; 
        
        // Wenn Munition alle ist Nachladen (durch Nachladen Methode), danach zurueck 
        if (CountAmmo <= 0){Reload();return;}

        // Bereit zum Schießen wird auf false gesetzt und Schuss auf true 
        _readyToShoot = false;
        _shooting = true;

        // Munition wird abgezogen 
        UseAmmo();
        // dann wird ResetAttack iniziert mit der feuerrate aus dem Waffendaten Script 
        Invoke(nameof(ResetAttack), weaponData.fireRate);
        // wohin geschossen wird 
        AttackRaycast();
        // Instanziieren von weaponData
        Instantiate(weaponData);
    }

    // Methode wohin geschossen wird
    private void AttackRaycast(){
        RaycastHit hit;

        // 
        switch (weaponData.Type){
            case WeaponType.Patrone:
                if (Physics.Raycast(transform.position, Camera.main.transform.forward, out hit, 
                        weaponData.weaponRange)){
                    //Debug.DrawLine(transform.position,hit.point, Color.blue, 1);
                    HitTarget(hit);
                    var zielScheibe = hit.collider.GetComponent<ZielscheibeScript>();
                    if (zielScheibe){
                        zielScheibe.Hited();
                    }
                } 
                //Debug.DrawLine(transform.position, transform.position + Camera.main.transform.forward * 100, Color.red, 1);
                break;
        }
    }

    private void HitTarget(RaycastHit hit){
        GameObject obj = Instantiate(weaponData.hitEffect, hit.point, Quaternion.identity);
        Destroy(obj, 4);
    }

    // Methode, wenn der Angriff zurueckgesetzt wird 
    void ResetAttack(){
        _shooting = false;
        _readyToShoot = true;
    }
    
    // Methode für das Verbrauchen von Munition
    private void UseAmmo(){
        CountAmmo--;
    }
    
    // Methode zum Nachladen
    public void Reload(){
        if (CountAmmo == weaponData.maxAmmo || _reloading) return;
        _reloading = true;
        Invoke(nameof(ResetReload), weaponData.reload);
    }

    // Methode zum Reseten des Nachladen
    void ResetReload(){
        _reloading = false;
        CountAmmo = weaponData.maxAmmo;
    }
}