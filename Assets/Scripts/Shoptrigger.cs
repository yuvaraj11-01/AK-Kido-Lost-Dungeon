using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoptrigger : MonoBehaviour
{
    [SerializeField] GameObject ShopInteraction;
    [SerializeField] GameObject ShopPanel;
    

    bool CanTrigger;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ShopInteraction.SetActive(true);
            CanTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ShopInteraction.SetActive(false);
            CanTrigger= false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CanTrigger)
            {
                ShopPanel.SetActive(true);
                Movement.instance.canMove = false;
                ShopInteraction.SetActive(false);
            }
        }
    }

    public void release()
    {
        Movement.instance.canMove = true;
    }

}
