using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public GunData gunData;
    public BaseController controller;
    public Transform GunMuzzle;
    public Transform cameraTransform;


    WeaponAnim weaponanim;
    Animator Reloadanim;
    private CinemachineImpulseSource recoilShakeImpulseSource;

    [SerializeField] GameObject muzzleFlash;

    [HideInInspector] public bool isShooting = false;

    [SerializeField] private TextMeshProUGUI AmmoCount;

    private float currentAmmo = 0f;
    private float nextTimeToFire = 0f;

    public bool isReloading = false;




    private void Start()
    {
        currentAmmo = gunData.magazineSize;
        Reloadanim = GetComponentInChildren<Animator>(); ;
        weaponanim = GetComponent<WeaponAnim>();

        controller = transform.root.GetComponent<BaseController>();
        recoilShakeImpulseSource = GetComponent<CinemachineImpulseSource>();

    }

    private void Update()
    {

        if (!isReloading)
        {
            AmmoCount.text = currentAmmo.ToString() + " / " + gunData.magazineSize.ToString();
        }
        else
        {
            AmmoCount.text = "Reloading...";
        }

        if (!isReloading && currentAmmo <= 0)
        {
            StartCoroutine(Reload());
        }


    }

    public void TryReloading()
    {
        if (!isReloading && currentAmmo < gunData.magazineSize)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {

        isReloading = true;
        Reloadanim.SetBool("Reloading", true);

        yield return new WaitForSeconds(gunData.reloadTime);

        currentAmmo = gunData.magazineSize;
        Reloadanim.SetBool("Reloading", false);
        isReloading = false;
    }

    public void TryShoot()
    {
        if (isReloading == true || currentAmmo <= 0)
        {
            return;
        }

        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + (1 / gunData.fireRate);

            HandleShoot();
        }

    }

    private void HandleShoot()
    {
        isShooting = true;
        StartCoroutine(MuzzleFlash());
        currentAmmo--;
        Debug.Log("shoot");
        weaponanim.isRecoiling = true;
        Shoot();
        recoilShakeImpulseSource.GenerateImpulse();

    }

    public abstract void Shoot();

    private IEnumerator MuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        muzzleFlash.SetActive(false);
    }
}

