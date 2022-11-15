using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolGun : MonoBehaviour
{
    [SerializeField] GameObject flash;
    [SerializeField] Transform projectile;
    [SerializeField] Transform startPoint;
    [SerializeField] float impactForce;
    [SerializeField] int MagazenSize;
    [SerializeField] float ReloadTime;

    Animator anim;
    bool canFire;
    Camera cam;
    int currentAmmo;
    float lastShot = .0f;
    bool reload;

    private void Awake()
    {
        anim = GetComponentInParent<Animator>();
        cam = Camera.main;
    }

    private void Start()
    {
        currentAmmo = MagazenSize;
        WeaponHUD.instance.SetAmmo(currentAmmo, MagazenSize);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!reload)
            {
                currentAmmo = 0;
                Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
                Shoot(mousePosition, startPoint.position);
                WeaponHUD.instance.SetAmmo(currentAmmo, MagazenSize);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
            
            if(!reload)
            Shoot(mousePosition, startPoint.position);
            else
            {
                if (Time.time > ReloadTime + lastShot)
                {
                    reload= false;
                    currentAmmo = MagazenSize;
                }
            }
        }
    }

    public void Shoot(Vector3 gunEndPointPosition, Vector3 shootPosition)
    {

        if (currentAmmo > 0)
        {
            anim.SetTrigger("singleshot");
            flash.SetActive(true);
            ShootProjectile(shootPosition, gunEndPointPosition);
            CinemachineShake.Instance.ShakeCamera(2f, .1f);
            Invoke(nameof(disableFlash), .2f);
            lastShot = Time.time;

            currentAmmo--;
            WeaponHUD.instance.SetAmmo(currentAmmo, MagazenSize);

        }
        else
        {
            reload = true;
            var reload_ = GameObject.FindObjectOfType<Reload>();
            if (reload_)
            {
                reload_.gameObject.SetActive(true);
                reload_.StartReloading(ReloadTime);

            }
        }
    }

    void disableFlash()
    {
        flash.SetActive(false);
    }

    private void ShootProjectile(Vector3 gunEndPointPosition, Vector3 shootPosition)
    {
        Transform bulletTransform = Instantiate(projectile, gunEndPointPosition, Quaternion.identity);

        Vector3 shootDir = (shootPosition - gunEndPointPosition).normalized;
        bulletTransform.GetComponent<Bullet>().Setup(shootDir,.5f);
    }

}
