using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BulletTracer
{
    private List<GameObject> _bulletTracerList = new List<GameObject>();
    
    [SerializeField] 
    private GameObject bulletTracerPrefab;
    [SerializeField] 
    private Transform tracerHolder;

    [Header("Tracer Einstellungen")] 
    [SerializeField]
    private int noOffBulletTracer = 10;
    [SerializeField]
    private float tracerSpeed = 100f;
    [SerializeField]
    private float tracerLifeTime = 1.5f;

    public void InitializeTracers(){
        for (int i = 0; i < noOffBulletTracer; i++){
            GameObject newTracer = UnityEngine.Object.Instantiate(bulletTracerPrefab, tracerHolder);
            _bulletTracerList.Add(newTracer);
            _bulletTracerList[i].SetActive(false);
        }
    }

    public void PlayTracing(Vector3 startPos, Vector3 endPos){
        GameObject currentTracer = GetBulletPool(startPos);
        currentTracer.transform.parent = null;
        currentTracer.SetActive(true);
        BulletTracerMovement tracerMovement = currentTracer.GetComponent<BulletTracerMovement>();

        if (tracerMovement != null){
            tracerMovement.Iniziieren(endPos, tracerSpeed, tracerLifeTime);
        }
    }

    private GameObject GetBulletPool(Vector3 emitPos){
        int index = UnityEngine.Random.Range(0, _bulletTracerList.Count);
        for (int i = 0; i < _bulletTracerList.Count; i++){
            if (!_bulletTracerList[index].activeInHierarchy){
                _bulletTracerList[index].transform.position = emitPos;
                return _bulletTracerList[index];
            }
            else{
                index = UnityEngine.Random.Range(0, _bulletTracerList.Count);
            }
        }
        return null;
    }
}
