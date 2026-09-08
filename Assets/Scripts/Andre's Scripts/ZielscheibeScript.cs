using UnityEngine;
using UnityEngine.InputSystem;

public class ZielscheibeScript : MonoBehaviour
{
    private float speed = 1.5f;

    private float distance = 4.25f;

    private float start;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        start = transform.position.z;
    }

    // Update is called once per frame
    void Update(){
        float newPosZ = start + Mathf.PingPong(Time.time * speed, distance);
        transform.position = new Vector3(transform.position.x, transform.position.y, newPosZ);
    }
}
