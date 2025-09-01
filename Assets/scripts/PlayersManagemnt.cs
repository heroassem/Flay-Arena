using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class PlayersManagemnt : RoomManager
{
    [SerializeField] GameManagemnt gameManagemnt;

    public override void OnMatchStart()
    {
        gameManagemnt.SpownPlayer();
    }
}   
