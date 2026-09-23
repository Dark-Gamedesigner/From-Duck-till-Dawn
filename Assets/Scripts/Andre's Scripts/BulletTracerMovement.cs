using System.Collections;
using UnityEngine;

public class BulletTracerMovement : MonoBehaviour
{
    private Vector3 _targetPositionPoint;
    private float _moveSpeed;
    private float _lifeTime;

    private bool _canMove = true;
    
    // Update is called once per frame
    void Update()
    {
        if (!_canMove) return;
        transform.position = Vector3.MoveTowards(transform.position, _targetPositionPoint, _moveSpeed * Time.deltaTime);
        float distance = Vector3.Distance(transform.position, _targetPositionPoint);

        if (distance <= 0.5f){
            _canMove = false;
        }
    }

    public void Iniziieren(Vector3 endPos, float speed, float lifeTime){
        _targetPositionPoint = endPos;
        _moveSpeed = speed;
        _lifeTime = lifeTime;

        StartCoroutine(Hide(_lifeTime));
    }

    IEnumerator Hide(float lifeTime){
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }
}
