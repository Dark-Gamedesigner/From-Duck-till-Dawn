using System;
using NUnit.Framework;
using Unity.VisualScripting;
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
    
    private Equipment Equip;
    //public Equipment InRange;

    private void Update(){
        if (Input.GetKeyDown(KeyCode.E)){
            GetEquip();
        }
    }

    void GetEquip(){
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interaction )){
            EquipGegenstand newItem = hit.collider.GetComponent<EquipGegenstand>();

            if (newItem != null){
                Transform targetTrans = null;
                EquipGegenstand currentEquip = null;

                switch (newItem.targetSlot.Equip){
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

                if (targetTrans == null){
                    if (currentEquip != null){
                        newItem.Equipt(targetTrans);
                        UpdateSlot(Equip, newItem);
                    }
                }
            }
        }
    }

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
