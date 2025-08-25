using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingOrganize : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(CheckPingRoutine());
    }

    private IEnumerator CheckPingRoutine()
    {
        while (true)
        {
            PingManagemnt();
            yield return new WaitForSeconds(5f);
        }
    }

    private void PingManagemnt()
    {
        int ping = PhotonNetwork.GetPing();

        if (ping > 300)
        {
            PhotonNetwork.SendRate = 15;
        }
        else if (ping >= 200 && ping <= 300)
        {
            PhotonNetwork.SendRate = 20;
        }
        else if (ping >= 100 && ping < 200)
        {
            PhotonNetwork.SendRate = 25;
        }
        else
        {
            PhotonNetwork.SendRate = 30;
        }

        PhotonNetwork.SerializationRate = PhotonNetwork.SendRate / 2;

        Debug.Log($"Ping: {ping} | SendRate: {PhotonNetwork.SendRate} | SerializationRate: {PhotonNetwork.SerializationRate}");
    }
}
