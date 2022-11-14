using Org.MetaFab.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    
    [SerializeField] Button BuyBtn, EquipBtn;
    [SerializeField] Text equipText,costText;

    public Action<ShopItem> OnEquipSelected;
    WeaponHolder weaponHolder;

    ExchangeOffer offer;
    bool brought,Equip;
    public void Setup(ExchangeOffer offer,bool brought,bool Equip)
    {
        this.offer = offer;
        this.brought = brought;
        this.Equip = Equip;

        SetUpVisual();
        BuyBtn.onClick.AddListener(BuyWeapon);
        EquipBtn.onClick.AddListener(EquipWeapon);
        weaponHolder = FindObjectOfType<WeaponHolder>();
    }

    void SetUpVisual()
    {
        if (brought)
        {
            BuyBtn.gameObject.SetActive(false);
            EquipBtn.gameObject.SetActive(true);
        }
        if (Equip) equipText.text = "Equiped";
        else equipText.text = "Equip";
        costText.text = ((int)offer.InputCurrencyAmount).ToString();
    }

    public void BuyWeapon()
    {
        BuyBtn.interactable = false;
        MetafabManager.BuyOffer(offer.Id.ToString(), (res) =>
        {
            if (res)
            {
                brought = true;
                SetUpVisual();
                LobbyHUD.instance.refresh();
            }
        });
    }

    public void EquipWeapon()
    {
        // equip weapon
        OnEquipSelected?.Invoke(this);
        Equip = true;
        SetUpVisual();
        weaponHolder.EquipWeapon(((int)offer.OutputCollectionItemIds[0]).ToString());
    }

    public void UnEquip()
    {
        // un equip;
        Equip = false;
        SetUpVisual();
        weaponHolder.UnEquipWeapon();

    }

}
