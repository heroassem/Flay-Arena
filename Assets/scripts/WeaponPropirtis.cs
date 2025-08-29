using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;

public class WeaponPropirtis : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] int weponDamage;
    [SerializeField] float weponRange;
    [SerializeField] float weponFireTime;
    [SerializeField] float weponReloadTime;
    [SerializeField] public int weaponAmo;
    [SerializeField] public int weaponMaxAmo;
    [SerializeField] int weaponBulletCount;
    [SerializeField] float weaponBulletRangeX;
    [SerializeField] float weaponBulletRangeY;
    [SerializeField] string weaponAnimeParametarName;

    [Header("network Settings")]
    public PhotonView weaponPhotonView;

    [Header("Weapon Components")]
    [SerializeField] public Transform firePoint;
    [SerializeField] Animator animator;

    [Header("Weapon Effect")]
    [SerializeField] ParticleSystem weaponBulletEffect;
    [SerializeField] ParticleSystem weaponFireEffect;

    bool isWeaponReady = true;
    bool isWeaponReloading = false;

    float fireTime, reloadTime, timeGet;

    int weaponPlayerOwnerID;

    string weaponTeem;

    RaycastHit hit;
    Transform Owner;

    public void Start()
    {
        weaponFireEffect.Stop();
    }

    public void Update()
    {
        if (weaponPhotonView.IsMine)
        {
            if (Input.GetButton("Relode") && weaponAmo < weaponMaxAmo)
            {
                StartCoroutine(Relode(weponReloadTime));
            }

            if (Input.GetMouseButton(0))
            {
                Fire();

                foreach (PlayerControler owner in FindObjectsOfType<PlayerControler>())
                {
                    if (owner.photonView.IsMine)
                    {
                        weaponPlayerOwnerID = owner.photonView.ViewID;
                        break;
                    }
                }

                if (isWeaponReady && !isWeaponReloading)
                {
                    animator.SetInteger(weaponAnimeParametarName, 1);
                    weaponPhotonView.RPC("Shote", RpcTarget.AllBuffered, weaponPlayerOwnerID);
                    weaponFireEffect.Play();
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                animator.SetInteger(weaponAnimeParametarName, 0);
            }
        }
    }

    [PunRPC]
    public void Shote(int playerOwnerId)
    {
        for (int i = 0; i < weaponBulletCount; i++)
        {
            Vector3 direction = Quaternion.Euler(
                Random.Range(-weaponBulletRangeX, weaponBulletRangeX),
                Random.Range(-weaponBulletRangeY, weaponBulletRangeY), 0f) * firePoint.forward;

            if (Physics.Raycast(firePoint.transform.position, direction, out hit, weponRange))
            {
                Debug.DrawRay(firePoint.transform.position, direction * weponRange, Color.red);

                if (hit.transform.tag == "Player")
                {
                    int playerHitID = hit.transform.GetComponent<PhotonView>().ViewID;

                    GameObject enemyPlayer = PhotonView.Find(playerHitID).gameObject;
                    PlayerControler playerControlerEnemy = enemyPlayer.GetComponent<PlayerControler>();

                    GameObject weaponPlayerOwner = PhotonView.Find(playerOwnerId).gameObject;
                    PlayerControler playerControlerOwner = weaponPlayerOwner.GetComponent<PlayerControler>();

                    if (playerControlerEnemy.playerTeem != playerControlerOwner.playerTeem)
                    {
                        if (playerControlerEnemy.canDameg == true)
                            playerControlerEnemy.playerHealth -= weponDamage;
                    }
                }

                GameObject particl = Instantiate(weaponBulletEffect.gameObject, hit.point, Quaternion.LookRotation(hit.normal));
                ParticleSystem particleSystem = particl.GetComponent<ParticleSystem>();

                if (particleSystem != null)
                {
                    particleSystem.Play();
                    StartCoroutine(DestroyParticle(particleSystem, 5f));
                }
            }
            else
            {
                Debug.DrawRay(firePoint.transform.position, direction * weponRange, Color.green);
            }
        }

        weaponAmo--;
    }

    public void Fire()
    {
        fireTime += Time.deltaTime;

        if (fireTime >= weponFireTime && weaponAmo > 0)
        {
            isWeaponReady = true;
            fireTime = 0f;
        }
        else
        {
            isWeaponReady = false;
            weaponFireEffect.Stop();
        }
        if (weaponAmo == 0)
        {
            animator.SetInteger(weaponAnimeParametarName, 0);
            weaponFireEffect.Stop();
        }
    }

    IEnumerator Relode(float time)
    {
        GameObject owner = PhotonView.Find(weaponPlayerOwnerID).gameObject;
        PlayerControler playerOwner = owner.GetComponent<PlayerControler>();

        isWeaponReloading = true;
        isWeaponReady = false;
        animator.SetInteger(weaponAnimeParametarName, 2);
        weaponFireEffect.Stop();
        if (playerOwner.isCrawshing == false)
            playerOwner.playerSpeed = playerOwner.croushSpeed;

        yield return new WaitForSeconds(time);

        isWeaponReloading = false;
        isWeaponReady = true;
        if (playerOwner.isCrawshing == false)
            playerOwner.playerSpeed = playerOwner.normaleSpeed;

        weaponAmo = weaponMaxAmo;
    }

    IEnumerator DestroyParticle(ParticleSystem particleSystem, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(particleSystem.gameObject);
    }
}