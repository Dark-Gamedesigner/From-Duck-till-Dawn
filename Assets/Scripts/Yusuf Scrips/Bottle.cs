using UnityEngine;

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
    
 
    
    public void Hit()
    {
     
        if (alreadyHit) return;
        alreadyHit = true;

        PlayHitEffects();
        SpawnShatterEffect();
        
        BottleShootingGame.Instance.OnBottleHit(this);

       
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

          
            Renderer shardRenderer = shard.GetComponent<Renderer>();
            shardRenderer.material.color = shardColor;

            
            Rigidbody shardRb = shard.AddComponent<Rigidbody>();
            Vector3 explosionDirection = Random.onUnitSphere + Vector3.up; // leicht nach oben bevorzugt
            shardRb.AddForce(explosionDirection.normalized * Random.Range(2f, 5f), ForceMode.Impulse);
            shardRb.AddTorque(Random.insideUnitSphere * 5f, ForceMode.Impulse);

    
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