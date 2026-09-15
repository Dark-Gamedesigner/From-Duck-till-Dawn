using UnityEngine;

/// <summary>
/// Erkennt Maus-Klicks waehrend eines aktiven Schiess-Minispiels
/// und prueft per Raycast, ob eine Flasche getroffen wurde.
/// Ist normalerweise deaktiviert - wird nur waehrend eines laufenden
/// Minispiels von BottleShootingGame.cs ein- und ausgeschaltet.
/// </summary>
public class ShootingInput : MonoBehaviour
{
    [Header("Referenzen")]
    [Tooltip("Die Kamera, aus der geschossen wird. Falls leer gelassen, wird automatisch die Kamera auf diesem GameObject verwendet")]
    [SerializeField] private Transform cameraTransform;

    private void Awake()
    {
        // In Minispiel-Szenen sitzt dieses Script direkt auf der Kamera zusammen
        // mit MinigameLook.cs - dann reicht die eigene Transform als Ursprung.
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

        if (Physics.Raycast(ray, out RaycastHit hit, shootRange, bottleLayer))
        {
            if (hit.collider.TryGetComponent(out Bottle bottle))
            {
                bottle.Hit();
            }
        }
        // Kein Treffer = einfach daneben geschossen, kein Fehlerfall,
        // hier bewusst keine Fehlermeldung noetig.
    }

    /// <summary>
    /// Wird von BottleShootingGame beim Rundenstart aufgerufen.
    /// </summary>
    public void EnableShooting()
    {
        shootingEnabled = true;
    }

    /// <summary>
    /// Wird von BottleShootingGame beim Rundenende aufgerufen.
    /// </summary>
    public void DisableShooting()
    {
        shootingEnabled = false;
    }
}