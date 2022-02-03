using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject spawnedPlayerPrefab;
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.Instantiate("NetworkPlayer", transform.position, transform.rotation);
    }
    public override void OnLeftRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
    }
    // Start is called before the first frame update

}
