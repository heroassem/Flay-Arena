using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSiting : MonoBehaviour
{
    [Header("Map Settings")]
    [SerializeField] Transform cameraMap;

    private void Update()
    {
        cameraMap.transform.position = new Vector3(transform.position.x, 22.41f, transform.position.z);                           
    }
}
