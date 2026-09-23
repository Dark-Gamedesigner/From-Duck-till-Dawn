using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    public WeaponData weaponData;

    public int CountAmmo{ get; private set; }

    private bool _reloading;
    private bool _shooting;
    private bool _readyToShoot =true;
    [SerializeField]
    private ShootSound _shootSound;
    [SerializeField]
    private AudioSource source;

    public List<ZielscheibeScript> zielscheibeScript;

    [Header("BulletTracer")] 
    [SerializeField]
    private BulletTracer bulletTracer;

    public Transform muzzle;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        CountAmmo = weaponData.maxAmmo;
        bulletTracer.InitializeTracers();
    }

    // Wenn die linke Maustaste gedrueckt wird die Methode schießen aufgerufen 
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame){
            _shootSound.PlayShootingClip(source);
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
        //muzzleFlash.Play();
        RaycastHit hit;

        // 
        switch (weaponData.Type){
            case WeaponType.Patrone:
                if (Physics.Raycast(transform.position, Camera.main.transform.forward, out hit, 
                        weaponData.weaponRange)){
                    //Debug.DrawLine(transform.position,hit.point, Color.blue, 1);
                    bulletTracer.PlayTracing(muzzle.transform.position, hit.point);
                    HitTarget(hit);
                    var zielScheibe = hit.collider.GetComponent<ZielscheibeScript>();
                    if (zielScheibe){
                        zielScheibe.Hited();
                    }
                }
                else{
                    bulletTracer.PlayTracing(muzzle.transform.position, hit.point);
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