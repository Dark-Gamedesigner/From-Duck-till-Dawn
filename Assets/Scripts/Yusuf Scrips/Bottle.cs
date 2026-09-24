using UnityEngine;


// Sitzt auf jeder einzelnen Flasche in der Szene.
// Kennt keine Spiellogik selbst - meldet nur "ich wurde getroffen" an das BottleShootingGame.

public class Bottle : MonoBehaviour
{
    [Header("Effekte (optional)")]
    [Tooltip("Partikeleffekt, der beim Treffer abgespielt wird - kann leer bleiben")]
    [SerializeField] private GameObject hitEffectPrefab;

    [Tooltip("Sound, der beim Treffer abgespielt wird - kann leer bleiben")]
    [SerializeField] private AudioClip hitSound;

    [Header("Zerbrechen-Effekt")]
    [Tooltip("Anzahl der Bruchstuecke, die beim Treffer erzeugt werden")]
    [SerializeField] private int shardCount = 6;

    [Tooltip("Farbe der generierten Bruchstuecke (Glas-Look)")]
    [SerializeField] private Color shardColor = new Color(0.4f, 0.3f, 0.1f, 0.8f);

    private bool alreadyHit = false;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    // Wird von ShootingInput aufgerufen, wenn der Spieler diese Flasche trifft.
    
    public void Hit()
    {
        // Verhindert, dass eine bereits getroffene Flasche nochmal zählt
        // (z.B. falls der Klick-Raycast aus Versehen zweimal denselben Frame trifft)
        if (alreadyHit) return;
        alreadyHit = true;

        PlayHitEffects();
        SpawnShatterEffect();

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

        if (hitSound != null)
        {
            
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }
    }

   
    private void SpawnShatterEffect()
    {
        for (int i = 0; i < shardCount; i++)
        {
            GameObject shard = GameObject.CreatePrimitive(PrimitiveType.Cube);
            shard.name = "BottleShard";
            shard.transform.position = transform.position;
            shard.transform.localScale = Vector3.one * Random.Range(0.05f, 0.12f);

            // Farbe setzen (eigenes Material-Instance, damit nicht alle Shards dasselbe teilen)
            Renderer shardRenderer = shard.GetComponent<Renderer>();
            shardRenderer.material.color = shardColor;

            // Kein eigener Collider-Aerger mit anderen Flaschen/Spieler noetig -
            // der Standard Box Collider von CreatePrimitive reicht fuer die kurze Lebensdauer.
            Rigidbody shardRb = shard.AddComponent<Rigidbody>();
            Vector3 explosionDirection = Random.onUnitSphere + Vector3.up; // leicht nach oben bevorzugt
            shardRb.AddForce(explosionDirection.normalized * Random.Range(2f, 5f), ForceMode.Impulse);
            shardRb.AddTorque(Random.insideUnitSphere * 5f, ForceMode.Impulse);

            // Bruchstuecke nach kurzer Zeit wieder entfernen, damit die Szene nicht vollmuellt
            Destroy(shard, 2f);
        }
    }

    
    // Setzt die Flasche für eine neue Runde zurück (z.B. bei erneutem Versuch).
  
    public void ResetBottle()
    {
        alreadyHit = false;
        gameObject.SetActive(true);
    }
}