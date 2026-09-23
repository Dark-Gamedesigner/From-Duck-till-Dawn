using UnityEngine;

public class EquipGegenstand : MonoBehaviour
{
    public string item = "WesternItem";

    public Equipmentscript targetSlot;

    private Rigidbody _rigidB;
    private Collider _collid;

    // Beim Start bekommt der Slot am Charakter einen RigidBody und einen Collider zugewiesen
    private void Start(){
        _rigidB = GetComponent<Rigidbody>();
        _collid = GetComponent<Collider>();
    }

    public void Equipt(Transform slotTransform){
        if (_rigidB != null) _rigidB.isKinematic = true;      // setzt den Rigidbody am Slot auf Kinematic
        if (_collid != null) _collid.enabled = false;         // setzt die Collision am Slot zum ausruesten aus
        
        // Bestimmt wie der Gegenstand am Slot ausgeruestet wird
        transform.SetParent(slotTransform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}