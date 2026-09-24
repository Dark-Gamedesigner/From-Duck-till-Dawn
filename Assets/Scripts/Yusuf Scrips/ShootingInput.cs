using UnityEngine;

public class ShootingInput : MonoBehaviour
{
    [Header("Referenzen")]
    [SerializeField] private Transform cameraTransform;

    [Header("Einstellungen")]
    [SerializeField] private float shootRange = 500f;
    [SerializeField] private LayerMask bottleLayer;

    [Header("Munition")]
    [SerializeField] private int maxAmmo = 8;

    [Header("Sichtbare Kugel")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform muzzlePoint;

    [Header("Sound")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip emptySound;

    private bool shootingEnabled = false;
    private int currentAmmo;

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

                if (shootSound != null)
                {
                    AudioSource.PlayClipAtPoint(shootSound, transform.position);
                }

                TryShoot();
            }
            else
            {
                if (emptySound != null)
                {
                    AudioSource.PlayClipAtPoint(emptySound, transform.position);
                }
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
    }

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