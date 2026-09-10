using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Weapon")] 
    public WeaponType Type;
    public int maxAmmo = 6;
    public int demage = 1;
    public float fireRate = 0.5f;
    public float reload = 0.5f;
    public float weaponRange = 40f;
    public float headShot = 1.5f;

    [Header("Raycast")] 
    public GameObject hitEffect;

    [Header("Prefab")] 
    public WeaponController WeaponPrefab;

}

public enum WeaponType
{
    Patrone
}
