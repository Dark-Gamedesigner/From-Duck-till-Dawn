using UnityEngine;

public class BulletTravel : MonoBehaviour
{
    [Tooltip("Wie schnell die Kugel zum Ziel fliegt (Einheiten pro Sekunde)")]
    [SerializeField] private float speed = 60f;

    private Vector3 targetPosition;
    private bool hasTarget = false;
    
    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
        hasTarget = true;
    }

    private void Update()
    {
        if (!hasTarget) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Ziel erreicht (kleine Toleranz, da MoveTowards selten exakt 0 erreicht)
        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            Destroy(gameObject);
        }
    }
}