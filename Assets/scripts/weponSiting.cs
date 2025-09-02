using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;

public class weponSiting : MonoBehaviour
{
    [Header("Weapon Siting")]
    [SerializeField] GameObject weaponLayer;

    [Header("Weapon network")]
    public PhotonView photonView;

    float time = 0f;
    int targetLayer;

    private void Update()
    {
        time += Time.deltaTime;

        if (time >= 0.1f)
        {
            targetLayer = photonView.IsMine && photonView != null? LayerMask.NameToLayer("gun") : LayerMask.NameToLayer("Default");

            weaponLayer.layer = targetLayer;

            foreach (Transform child in weaponLayer.transform)
            {
                child.gameObject.layer = targetLayer;

                foreach (Transform grandChild in child)
                {
                    grandChild.gameObject.layer = targetLayer;
                }
            }
        }
    }
}
