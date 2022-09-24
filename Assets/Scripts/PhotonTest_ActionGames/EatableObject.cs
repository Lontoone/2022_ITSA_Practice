using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatableObject : MonoBehaviour
{
    PhotonView pv;
    private void Start()
    {
        pv = gameObject.GetComponent<PhotonView>(); 
        //pv = PhotonView.Get(this);
    }
    public void GotEat() {
        Debug.Log(pv);
        pv.RPC("DestorySelf",RpcTarget.All);
    }
    [PunRPC]
    private void DestorySelf() {
        Destroy(gameObject);
    }

}
