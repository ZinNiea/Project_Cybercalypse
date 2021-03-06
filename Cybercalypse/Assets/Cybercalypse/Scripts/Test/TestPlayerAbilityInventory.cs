﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TestPlayerAbilityInventory : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler
{
    public EAbilitySlot slot;

    private Vector3 startPosition;
    private Vector3 mousePosition;
    private GameObject select;

    private GameObject slotAbility1;
    private GameObject slotAbility2;
    private GameObject slotAbility3;

    private void Awake()
    {
        select = GameObject.Find("Select").gameObject;

        slotAbility1 = this.transform.parent.transform.parent.GetChild(0).transform.GetChild(0).gameObject;
        slotAbility2 = this.transform.parent.transform.parent.GetChild(1).transform.GetChild(0).gameObject;
        slotAbility3 = this.transform.parent.transform.parent.GetChild(2).transform.GetChild(0).gameObject;
    }

    private void OnMouseOver()
    {
        select.transform.SetAsFirstSibling();
    }

    private void OnMouseExit()
    {
        select.transform.SetAsLastSibling();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.parent.SetAsLastSibling();
        transform.parent.transform.parent.SetAsLastSibling();
        startPosition = this.transform.position;
        this.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = new Vector3(mousePosition.x, mousePosition.y);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!CGameManager.instance.testAbilityList.Contains(eventData.pointerDrag.GetComponent<Image>().sprite))
        {
            return;
        }

        Sprite dragSprite = eventData.pointerDrag.transform.gameObject.GetComponent<Image>().sprite;
        Sprite enterSprite = eventData.pointerEnter.transform.gameObject.GetComponent<Image>().sprite;

        //Ability Change (Test 날림으로 만듬)
        if (slot == EAbilitySlot.Ability1 && this.GetComponent<Image>().sprite.name == "NullAbility")
        {
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(dragSprite), 0);
        }
        else if (slot == EAbilitySlot.Ability2 && this.GetComponent<Image>().sprite.name == "NullAbility")
        {
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(dragSprite), 1);
        }
        else if (slot == EAbilitySlot.Ability3 && this.GetComponent<Image>().sprite.name == "NullAbility")
        {
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(dragSprite), 2);
        }
        else if (slot == EAbilitySlot.Ability1 && slotAbility1.GetComponent<Image>().sprite.name != "NullAbility" && slotAbility2.GetComponent<Image>().sprite.name != "NullAbility" && slotAbility3.GetComponent<Image>().sprite.name != "NullAbility" && (eventData.pointerEnter.gameObject == slotAbility2 || eventData.pointerEnter.gameObject == slotAbility3))
        {
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(dragSprite), 0);
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(enterSprite), 1);
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(enterSprite), 2);
        }
        else if (slot == EAbilitySlot.Ability2 && slotAbility1.GetComponent<Image>().sprite.name != "NullAbility" && slotAbility2.GetComponent<Image>().sprite.name != "NullAbility" && slotAbility3.GetComponent<Image>().sprite.name != "NullAbility" &&  (eventData.pointerEnter.gameObject == slotAbility1 || eventData.pointerEnter.gameObject == slotAbility3))
        {
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(dragSprite), 1);
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(enterSprite), 0);
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(enterSprite), 2);
        }
        else if (slot == EAbilitySlot.Ability3 && slotAbility1.GetComponent<Image>().sprite.name != "NullAbility" && slotAbility2.GetComponent<Image>().sprite.name != "NullAbility" && slotAbility3.GetComponent<Image>().sprite.name != "NullAbility" && (eventData.pointerEnter.gameObject == slotAbility1 || eventData.pointerEnter.gameObject == slotAbility2))
        {
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(enterSprite), 2);
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(dragSprite), 1);
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(enterSprite), 0);
        }
        else if (slot == EAbilitySlot.Ability1 && slotAbility1.GetComponent<Image>().sprite.name != "NullAbility")
        {
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(dragSprite), 0);
        }
        else if (slot == EAbilitySlot.Ability2 && slotAbility2.GetComponent<Image>().sprite.name != "NullAbility")
        {
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(dragSprite), 1);
        }
        else if (slot == EAbilitySlot.Ability3 && slotAbility3.GetComponent<Image>().sprite.name != "NullAbility")
        {
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(dragSprite), 2);
        }

         this.GetComponent<Image>().sprite = dragSprite;
         eventData.pointerDrag.transform.gameObject.GetComponent<Image>().sprite = enterSprite;     
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Ability Check (Test 날림으로 만듬)
        if (slotAbility1.GetComponent<Image>().sprite.name == "NullAbility")
        {
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(slotAbility1.GetComponent<Image>().sprite), 0);
        }

        if (slotAbility2.GetComponent<Image>().sprite.name == "NullAbility")
        {
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(slotAbility2.GetComponent<Image>().sprite), 1);
        }

        if (slotAbility3.GetComponent<Image>().sprite.name == "NullAbility")
        {
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(slotAbility3.GetComponent<Image>().sprite), 2);
        }

        if (slotAbility1.GetComponent<Image>().sprite.name != "NullAbility")
        {
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(slotAbility1.GetComponent<Image>().sprite), 0);
        }

        if (slotAbility2.GetComponent<Image>().sprite.name != "NullAbility")
        {
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(slotAbility2.GetComponent<Image>().sprite), 1);
        }

        if (slotAbility3.GetComponent<Image>().sprite.name != "NullAbility")
        {
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(slotAbility3.GetComponent<Image>().sprite), 2);
        }

        select.transform.SetAsLastSibling();
        this.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
        this.transform.localPosition = new Vector3(0, 0);
    }
    

    public void SetItemUseKeyBoard(GameObject inventoryItem)
    {
        Sprite dragSprite = inventoryItem.transform.GetChild(0).GetComponent<Image>().sprite;

        if (!CGameManager.instance.testAbilityList.Contains(dragSprite))
        {
            return;
        }

        if (slotAbility1.GetComponent<Image>().sprite.name == "NullAbility")
        {
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(dragSprite), 0);
            inventoryItem.transform.GetChild(0).GetComponent<Image>().sprite = slotAbility1.GetComponent<Image>().sprite;
            slotAbility1.GetComponent<Image>().sprite = dragSprite;
            return;
        }
        else if (slotAbility2.GetComponent<Image>().sprite.name == "NullAbility")
        {
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(dragSprite), 1);
            inventoryItem.transform.GetChild(0).GetComponent<Image>().sprite = slotAbility2.GetComponent<Image>().sprite;
            slotAbility2.GetComponent<Image>().sprite = dragSprite;
            return;
        }
        else if (slotAbility3.GetComponent<Image>().sprite.name == "NullAbility")
        {
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(dragSprite), 2);
            inventoryItem.transform.GetChild(0).GetComponent<Image>().sprite = slotAbility3.GetComponent<Image>().sprite;
            slotAbility3.GetComponent<Image>().sprite = dragSprite;
            return;
        }

    }

    public void ResetItemUseKeyBoard(GameObject emptyInventorySlot)
    {
        if (emptyInventorySlot == null)
        {
            return;
        }

        Sprite dragSprite;
        Sprite enterSprite = CGameManager.instance.testAbilityList[0];

        if (slotAbility1.GetComponent<Image>().sprite.name != "NullAbility")
        {
            dragSprite = slotAbility1.GetComponent<Image>().sprite;
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(enterSprite), 0);
            emptyInventorySlot.transform.GetChild(0).GetComponent<Image>().sprite = slotAbility1.GetComponent<Image>().sprite;
            slotAbility1.GetComponent<Image>().sprite = dragSprite;
            return;
        }
        else if (slotAbility2.GetComponent<Image>().sprite.name != "NullAbility")
        {
            dragSprite = slotAbility2.GetComponent<Image>().sprite;
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(enterSprite), 1);
            emptyInventorySlot.transform.GetChild(0).GetComponent<Image>().sprite = slotAbility2.GetComponent<Image>().sprite;
            slotAbility2.GetComponent<Image>().sprite = dragSprite;
            return;
        }
        else if (slotAbility3.GetComponent<Image>().sprite.name != "NullAbility")
        {
            dragSprite = slotAbility2.GetComponent<Image>().sprite;
            CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().ChangeSlot(CGameManager.instance.abilityLibrary.GetComponent<CAbilityLibrary>().FindAbilityToAbilityIcon(dragSprite), 2);
            emptyInventorySlot.transform.GetChild(0).GetComponent<Image>().sprite = slotAbility3.GetComponent<Image>().sprite;
            slotAbility3.GetComponent<Image>().sprite = dragSprite;
            return;
        }
    }

    public enum EAbilitySlot
    {
        Ability1,
        Ability2,
        Ability3,
        Nothing
    }
}
