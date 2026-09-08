using UnityEngine;

public class EquipGegenstand : MonoBehaviour
{
    public string item = "WesternItem";

    public Equipmentscript targetSlot;

    private Rigidbody rb;
    private Collider col;

    private void Start(){
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void Equipt(Transform slotTransform){
        if (rb != null) rb.isKinematic = true;
        if (col != null) col.enabled = false;
        
        transform.SetParent(slotTransform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
