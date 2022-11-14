using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] float sortYLimit;
    [SerializeField] Transform hands;
    [SerializeField] WeaponContainer weaponContainer;
    Camera cam;
    SpriteRenderer weaponVisual;

    public void EquipWeapon(string ID)
    {
        foreach (var weapon in weaponContainer.weapons)
        {
            if(weapon.weaponID.ToString() == ID)
            {
                Instantiate(weapon.prefab,hands);
                StaticPlayerData.EquipedWeapon = ID;
                break;
            }
        }
    }

    public void UnEquipWeapon()
    {
        for (int i = 0; i < hands.childCount; i++)
        {
            Destroy(hands.GetChild(i).gameObject);
        }
    }

    private void Awake()
    {
        GetVisual();
    }

    void GetVisual()
    {
        weaponVisual = GetComponentInChildren<SpriteRenderer>();

    }

    private void Start()
    {
        cam = Camera.main;
        SetAmmoVisual();
        if (StaticPlayerData.EquipedWeapon != "")
        {
            EquipWeapon(StaticPlayerData.EquipedWeapon);
        }
    }

    void SetAmmoVisual()
    {
        if (weaponVisual) WeaponHUD.instance.SetImage(weaponVisual.sprite);

    }
    void Update()
    {
        GetVisual();
        SetAmmoVisual();
        faceMouse();


    }

    void faceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = cam.ScreenToWorldPoint(mousePosition);

        if (weaponVisual!=null)
        {
            // snap weapon
            if (mousePosition.x > transform.position.x) weaponVisual.flipY = false;
            else if (mousePosition.x < transform.position.x) weaponVisual.flipY = true;

            if (mousePosition.y > transform.position.y + sortYLimit) weaponVisual.sortingOrder = 0;
            else if (mousePosition.y < transform.position.y + sortYLimit) weaponVisual.sortingOrder = 2;
        }


        Vector2 direction = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        );

        transform.right = direction;
    }


}
