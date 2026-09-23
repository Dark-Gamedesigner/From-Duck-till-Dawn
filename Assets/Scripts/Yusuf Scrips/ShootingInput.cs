using UnityEngine;

public class ShootingInput : MonoBehaviour
{
    [Header("Referenzen")]
    [Tooltip("Die Kamera, aus der geschossen wird. Falls leer gelassen, wird automatisch die Kamera auf diesem GameObject verwendet")]
    [SerializeField] private Transform cameraTransform;

    private void Awake()
    {
        if (cameraTransform == null)
        {
            cameraTransform = transform;
        }
    }

    [Header("Einstellungen")]
    [Tooltip("Maximale Schussreichweite")]
    [SerializeField] private float shootRange = 50f;

    [Tooltip("Layer, auf dem die Flaschen liegen (im Inspector auswaehlen)")]
    [SerializeField] private LayerMask bottleLayer;

    [Header("Sichtbare Kugel")]
    [Tooltip("Kleines Kugel-Prefab (z.B. eine Sphere), das beim Schuss sichtbar zum Ziel fliegt")]
    [SerializeField] private GameObject bulletPrefab;

    [Tooltip("Startpunkt der Kugel, z.B. die Muendung der Pistole. Falls leer, wird die Kamera-Position genutzt")]
    [SerializeField] private Transform muzzlePoint;

    private bool shootingEnabled = false;

    private void Update()
    {
        if (!shootingEnabled) return;

        // Linke Maustaste = schiessen
        if (Input.GetButtonDown("Fire1"))
        {
            TryShoot();
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
            // Kein Treffer = einfach daneben geschossen. Die Kugel fliegt trotzdem
            // sichtbar geradeaus bis zur maximalen Reichweite, statt einfach zu fehlen.
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

    public void EnableShooting()
    {
        shootingEnabled = true;
    }
    
    public void DisableShooting()
    {
        shootingEnabled = false;
    }
}