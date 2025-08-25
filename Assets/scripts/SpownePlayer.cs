using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpownePlayer : MonoBehaviour
{
    [Header("Spowne Point")]
    [SerializeField] Transform redCorner;
    [SerializeField] Transform blueCorner;

    private void Awake()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1 || PhotonNetwork.LocalPlayer.ActorNumber == 3)
            {
                PhotonNetwork.Instantiate("Player 1", new Vector3(Random.Range(redCorner.position.x - 3, redCorner.position.x + 3), redCorner.position.y + 2, redCorner.position.z), redCorner.rotation);
            }
            else if(PhotonNetwork.LocalPlayer.ActorNumber == 2 || PhotonNetwork.LocalPlayer.ActorNumber == 4)
            {
                PhotonNetwork.Instantiate("Player 1", new Vector3(Random.Range(blueCorner.position.x - 3, blueCorner.position.x + 2), blueCorner.position.y + 2, blueCorner.position.z), blueCorner.rotation);
            }
        }
    }

    public void SpownPlayer(string playerTeem)
    {
        if (playerTeem == "red")
        {
            PhotonNetwork.Instantiate("Player 1", new Vector3(Random.Range(redCorner.position.x - 3, redCorner.position.x + 3), redCorner.position.y + 2, redCorner.position.z), redCorner.rotation);
        }
        else if (playerTeem == "blue")
        {
            PhotonNetwork.Instantiate("Player 1", new Vector3(Random.Range(blueCorner.position.x - 3, blueCorner.position.x + 2), blueCorner.position.y + 2, blueCorner.position.z), blueCorner.rotation);
        }
    }
}
