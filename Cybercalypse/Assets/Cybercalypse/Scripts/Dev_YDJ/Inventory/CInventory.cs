﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CInventory : MonoBehaviour {

    public GameObject equipmentPanel;
    public GameObject consumablePanel;

    public GameObject skillPanel;
    public GameObject abilityPanel;

    private GameObject currentInventoryTab;

    private int money;

    //!<
    //!< ---

    private CInventoryAbility[] inventoryAbilities;
    private CInventorySkill[] inventorySkills;

    public int InventoryAbilityIndex;
    public int InventorySkillIndex;

    public const int maxSlotCount = 32;

	// Use this for initialization
	void Start () {
        equipmentPanel = GameObject.Find("Panel_Inventory_Equipment");
        consumablePanel = GameObject.Find("Panel_Inventory_Consumable");

        skillPanel = GameObject.Find("Panel_Inventory_Skill");
        abilityPanel = GameObject.Find("Panel_Inventory_Ability");
        consumablePanel.SetActive(false);
        skillPanel.SetActive(false);
        abilityPanel.SetActive(false);

        currentInventoryTab = equipmentPanel;

        inventoryAbilities = new CInventoryAbility[maxSlotCount];
        inventorySkills = new CInventorySkill[maxSlotCount];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ActivateTab(GameObject _object)
    {
        currentInventoryTab.SetActive(false);
        currentInventoryTab = _object;
        currentInventoryTab.SetActive(true);
        //DeActivateExcludeTab(_object);
    }

    public void DeActivateExcludeTab(GameObject _object)
    {

    }

    /// <summary>
    /// 아이템을 인벤토리에 집어넣어주는 함수
    /// </summary>
    /// <typeparam name="T">AItem을 상속받는 모든 오브젝트는 인벤토리에 Get가능</typeparam>
    /// <param name="_item">인벤토리에 집어넣을 아이템 인스턴스</param>
    public void AddItem<T>(T _item) where T : AItem
    {
        switch(_item.ItemCategory)
        {
            case AItem.EItemCategory.Equipment:
                break;
            case AItem.EItemCategory.Consumable:
                break;
            case AItem.EItemCategory.Talent:
                if (_item.GetComponent<ATalent>().TalentCategory.Equals(ATalent.ETalentCategory.Ability))
                    Debug.Log("Ability");
                else
                    Debug.Log("Skill");
                break;
            default:
                Debug.Log("Null Item reference");
                break;
        }
    }

    public void GetMoney()
    {

    }

    static public T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy as T;
    }
}