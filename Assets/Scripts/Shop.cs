using Org.MetaFab.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] ShopItem[] items;

    Animator anim;
    List<ExchangeOffer> offers = new List<ExchangeOffer>();
    List<CollectionItem> itemsList = new List<CollectionItem>();
    void OnEnable()
    {
        MetafabManager.GetExchangeOffers((res) =>
        {
            offers = res;
        });

        itemsList = MetafabManager.GetItemBalance();

        //fix
        var temp = itemsList[0];
        itemsList[0] = itemsList[1];
        itemsList[1] = temp;

        anim = GetComponent<Animator>();
        Refresh();
    }

    void Refresh()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].Setup(offers[i], itemsList[i].count>0,false);
            items[i].OnEquipSelected += OnEquip;
        }
    }

    void OnEquip(ShopItem shopItem)
    {
        foreach (var item in items)
        {
            if(shopItem != item)
            item.UnEquip();
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            anim.SetTrigger("Close");
        }
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
        var player = GameObject.FindObjectOfType<Movement>();
        if (player != null)
        {
            player.canMove = true;
        }
    }



}
