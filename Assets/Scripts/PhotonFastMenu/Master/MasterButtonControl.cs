using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MasterButtonControl : MonoBehaviourPunCallbacks
{

    private Player player;
    [SerializeField] GameObject masterIcon;
    [SerializeField] GameObject giveMasterBtn;
    public void SetUp(Player _player)
    {
        player = _player;
        SetMasterImage(PhotonNetwork.MasterClient);
        //giveMasterBtn.onClick.AddListener(delegate { TransferMaster(); });
    }
    public void TransferMaster()
    {
        PhotonNetwork.SetMasterClient(player);
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        SetMasterImage(newMasterClient);
    }

    public void SetMasterImage(Player _newMaster)
    {
        if (_newMaster.IsLocal) //check master's
        {
            if (player.IsMasterClient)
            {
                masterIcon.gameObject.SetActive(true);
                giveMasterBtn.gameObject.SetActive(false);
            }
            else
            {
                masterIcon.gameObject.SetActive(false);
                giveMasterBtn.gameObject.SetActive(true);
            }
        }
        else //check not-master's player's view
        {
            if (_newMaster == player)
            {
                masterIcon.gameObject.SetActive(true);
            }
            else
            {
                masterIcon.gameObject.SetActive(false);
            }
            giveMasterBtn.gameObject.SetActive(false);
        }
    }
}
