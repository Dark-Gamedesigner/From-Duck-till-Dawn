using UnityEngine;

public class ShootingInput : MonoBehaviour
{
    [Header("Referenzen")]
    [Tooltip("Die Kamera, aus der geschossen wird. Falls leer, wird automatisch die Kamera auf diesem GameObject verwendet")]
    [SerializeField] private Transform cameraTransform;

    [Header("Einstellungen")]
    [Tooltip("Maximale Schussreichweite")]
    [SerializeField] private float shootRange = 500f;

    [Tooltip("Layer, auf dem die Flaschen liegen")]
    [SerializeField] private LayerMask bottleLayer;

    [Header("Munition")]
    [Tooltip("Wie viele Schuesse der Spieler pro Runde hat")]
    [SerializeField] private int maxAmmo = 8;

    [Header("Sichtbare Kugel")]
    [Tooltip("Kleines Kugel-Prefab, das beim Schuss sichtbar zum Ziel fliegt")]
    [SerializeField] private GameObject bulletPrefab;

    [Tooltip("Startpunkt der Kugel, z.B. die Muendung der Pistole. Falls leer, wird die Kamera-Position genutzt")]
    [SerializeField] private Transform muzzlePoint;

    private bool shootingEnabled = false;
    private int currentAmmo;

    /// <summary>
    /// Aktuell verbleibende Schuesse - kann von der UI ausgelesen werden.
    /// </summary>
    public int CurrentAmmo => currentAmmo;
    public int MaxAmmo => maxAmmo;

    private void Awake()
    {
        if (cameraTransform == null)
        {
            cameraTransform = transform;
        }
    }

    private void Update()
    {
        if (!shootingEnabled) return;

        if (Input.GetButtonDown("Fire1"))
        {
            if (currentAmmo > 0)
            {
                currentAmmo--;
                TryShoot();
            }
            else
            {
                Debug.Log("Keine Munition mehr!");
                // Hier koennte spaeter ein "Klick"-Sound fuer leere Waffe rein
            }
        }
    }

    private void TryShoot()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        Vector3 bulletTarget;

        if (Physics.Raycast(ray, out RaycastHit hit, shootRange, bottleLayer))
        {
            bulletTarget = hit.point;

            if (hit.collider.TryGetComponent(out Bottle bottle))
            {
                bottle.Hit();
            }
        }
        else
        {
            bulletTarget = ray.origin + ray.direction * shootRange;
        }

        SpawnBulletVisual(bulletTarget);
    }

    private void SpawnBulletVisual(Vector3 target)
    {
        if (bulletPrefab == null) return;

        Vector3 startPosition = muzzlePoint != null ? muzzlePoint.position : cameraTransform.position;

        GameObject bullet = Instantiate(bulletPrefab, startPosition, Quaternion.identity);
        BulletTravel travel = bullet.GetComponent<BulletTravel>();

        if (travel != null)
        {
            travel.SetTarget(target);
        }
        else
        {
            Debug.LogWarning("ShootingInput: bulletPrefab hat kein BulletTravel-Script!");
        }
    }

    /// <summary>
    /// Wird von BottleShootingGame beim Rundenstart aufgerufen.
    /// Setzt die Munition auf den vollen Wert zurueck.
    /// </summary>
    public void EnableShooting()
    {
        shootingEnabled = true;
        currentAmmo = maxAmmo;
    }

    public void DisableShooting()
    {
        shootingEnabled = false;
    }
}