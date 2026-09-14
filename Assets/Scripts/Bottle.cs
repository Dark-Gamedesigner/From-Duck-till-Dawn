using UnityEngine;

/// <summary>
/// Sitzt auf jeder einzelnen Flasche in der Szene.
/// Kennt keine Spiellogik selbst - meldet nur "ich wurde getroffen" an das BottleShootingGame.
/// </summary>
public class Bottle : MonoBehaviour
{
    [Header("Effekte (optional)")]
    [Tooltip("Partikeleffekt, der beim Treffer abgespielt wird - kann leer bleiben")]
    [SerializeField] private GameObject hitEffectPrefab;

    [Tooltip("Sound, der beim Treffer abgespielt wird - kann leer bleiben")]
    [SerializeField] private AudioClip hitSound;

    private bool alreadyHit = false;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Wird von ShootingInput aufgerufen, wenn der Spieler diese Flasche trifft.
    /// </summary>
    public void Hit()
    {
        // Verhindert, dass eine bereits getroffene Flasche nochmal zählt
        // (z.B. falls der Klick-Raycast aus Versehen zweimal denselben Frame trifft)
        if (alreadyHit) return;
        alreadyHit = true;

        PlayHitEffects();

        // Der eigentliche Spielfortschritt läuft über BottleShootingGame,
        // nicht über die einzelne Flasche - deshalb hier die zentrale Instanz informieren.
        BottleShootingGame.Instance.OnBottleHit(this);

        // Flasche optisch verschwinden lassen (statt zerstören,
        // damit sie beim naechsten Rundenstart wieder aktiviert werden kann)
        gameObject.SetActive(false);
    }

    private void PlayHitEffects()
    {
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }

    /// <summary>
    /// Setzt die Flasche für eine neue Runde zurück (z.B. bei erneutem Versuch).
    /// </summary>
    public void ResetBottle()
    {
        alreadyHit = false;
        gameObject.SetActive(true);
    }
}