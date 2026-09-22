using UnityEngine;

public class Equipmentscript : MonoBehaviour
{
    [Header("BodySlot")]
    public Transform headSlot;
    public Transform breastSlot;
    public Transform hipSlot;
    
    [Header("EquipSlot")]
    public EquipGegenstand headEquipt;
    public EquipGegenstand breastEquipt;
    public EquipGegenstand hipEquipt;

    public float interaction = 3.0f; 
    
    private Equipment _equip;
    //public Equipment InRange;

    // Wenn E gedrueckt wird, wird der Gegenstand ausgeruestet
    private void Update(){
        if (Input.GetKeyDown(KeyCode.E)){
            GetEquip();
        }
    }

    // Funktion zum ausruesten der Gegenstaende
    void GetEquip(){
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        
        
        if (Physics.Raycast(ray, out hit, interaction )){                                   // Wenn ein Raycast in der Range der Gegenstaende ist
            EquipGegenstand newItem = hit.collider.GetComponent<EquipGegenstand>();         // Bekommt der Gegenstand die Komponente EquipGegenstand
            
            // Abfrage nach Item ob schon vorhanden 
            if (newItem != null){                                                        // Wenn newItem nicht null ist
                Transform targetTrans = null;                                               // wird targetTrans null...
                EquipGegenstand currentEquip = null;                                        // ... sowie auch currentEquip

                //hier wird der neue Gegenstand an den slot angeheftet
                switch (newItem.targetSlot._equip){
                    case Equipment.Cowboyhat:
                        targetTrans = headSlot;
                        currentEquip = headEquipt;
                        break;
                    case Equipment.Sheriffstar:
                        targetTrans = breastSlot;
                        currentEquip = breastEquipt;
                        break;
                    case Equipment.Revolver:
                        targetTrans = hipSlot;
                        currentEquip = hipEquipt;
                        break;
                }

                // hier wird es ausgeruestet
                if (targetTrans == null){
                    if (currentEquip != null){
                        newItem.Equipt(targetTrans);
                        UpdateSlot(_equip, newItem);
                    }
                }
            }
        }
    }

    // prueft gegenwaertigen Slot, ob Item vorhanden
    void UpdateSlot(Equipment slot, EquipGegenstand item){
        switch (slot){
            case Equipment.Cowboyhat: headEquipt = item; break;
            case Equipment.Sheriffstar: breastEquipt = item; break;
            case Equipment.Revolver: hipEquipt = item; break;
        }
    }
    
}


public enum Equipment
{
    Sheriffstar, 
    Cowboyhat, 
    Revolver
}