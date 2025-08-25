using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class GetWepon : MonoBehaviour
{
    [Header("Weapon spowne")]
    [SerializeField] Transform weaponsParent;
    [SerializeField] TextMeshProUGUI amoText;
    [SerializeField] public string weaponName;

    [Header("Network Siting")]
    public PhotonView photonView;

 
    string amo;

    int amoID;

    bool done = false;

    public void Update()
    {
        if (photonView.IsMine)
        { 
            if (Input.GetKeyDown(KeyCode.LeftAlt) && weaponName != null && done == false)
            {
                GameObject wepone = PhotonNetwork.Instantiate(weaponName, Vector3.zero, Quaternion.identity);
                int weaponID = wepone.GetComponent<PhotonView>().ViewID;

                photonView.RPC("GetWeaponFP", RpcTarget.AllBuffered, weaponID);

                foreach (weponProbertis wp in FindObjectsOfType<weponProbertis>())
                {
                    if (wp.weaponPhotonView.IsMine)
                    {
                        amoID = wp.weaponPhotonView.ViewID;
                        break;
                    }
                }

                done = true;
            }

            if(done)
            {
                weponProbertis w = PhotonView.Find(amoID).GetComponent<weponProbertis>();

                if (w != null) 
                {
                    amoText.text = w.weaponAmo.ToString() + "/" + w.weaponMaxAmo.ToString();
                }
            }
        }
    }

    [PunRPC]
    public void GetWeaponFP(int weaponID)
    {
        GameObject wepone = PhotonView.Find(weaponID).gameObject;
        wepone.transform.SetParent(weaponsParent);
        wepone.transform.localPosition = Vector3.zero;
        wepone.transform.localRotation = Quaternion.identity;
    }
}
