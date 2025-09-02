using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class PlayersManagemnt : RoomManager
{
    public override void OnMatchStart()
    {
        gameManagemnt.SpownPlayer();
    }

    public override void OnAllPlayersReady(Player playerCount)
    {
        PhotonView gameManagemntView = gameManagemnt.GetComponent<PhotonView>();

        if (gameManagemntView != null && gameManagemntView.IsMine)
        {
            gameManagemnt.Timer(ref gameManagemnt.gameTime);
            gameManagemnt.timerUI.SetActive(true);
            gameManagemntView.RPC("PlayersReady", RpcTarget.AllBuffered);
        }
    }
}   
