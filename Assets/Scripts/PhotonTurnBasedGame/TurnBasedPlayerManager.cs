using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class TurnBasedPlayerManager : MonoBehaviourPunCallbacks
{
    public GameObject optButton;
    int turnCount = -1;
    Player[] roomPlayers { get { return (PhotonNetwork.PlayerList); } }
    PhotonView pv;

    private void Start()
    {
        pv = gameObject.GetComponent<PhotonView>();
        SetupPlayer();
        //由Master決定玩家順序
        if (PhotonNetwork.IsMasterClient)
        {
            ChangeTurn();
        }

    }
    private void SetupPlayer()
    {
        //Create player list
        //roomPlayers = PhotonNetwork.PlayerList; //NULL!
        foreach (Player _p in roomPlayers)
        {
            ScoreBoardManager.sbm_instance.SetupPlayer(_p);
            //**注意: Custom property需要初始化
            _p.SetCustomProperties(
            MyPhotonExtension.WrapToHash(new object[] { PlayerManager.PLAYER_SCORE, 100 }));
        }
    }

    [PunRPC]
    private void ChangeTurn()
    {
        turnCount=(turnCount+1)%roomPlayers.Length;
        Player _currentOpter = roomPlayers[turnCount % roomPlayers.Length];
        var _data = MyPhotonExtension.WrapToHash(new object[] { "CURRENT_OPTERS", _currentOpter.NickName });
        PhotonNetwork.CurrentRoom.SetCustomProperties(_data);
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        //改變操作者時:
        var data = propertiesThatChanged["CURRENT_OPTERS"];
        Debug.Log(data);

        if (data!=null)
        {
            //Player _currentOpter = MyPhotonExtension.GetPlayerByName(data.ToString());
            Debug.Log("目前操作者: " + data.ToString());
            CheckIsMyTurn(data.ToString());
        }
    }

    private void CheckIsMyTurn(string _nkName)
    {
        if (PhotonNetwork.LocalPlayer.NickName == _nkName)
        {
            optButton.SetActive(true);
        }
        else
        {
            optButton.SetActive(false);
        }
    }

    public void AttackNext()
    {
        pv.RPC("DoAttackFromMaster", PhotonNetwork.MasterClient);
    }

    [PunRPC]
    private void DoAttackFromMaster()
    {
        int _dmg = Random.Range(0, 20);

        Player gotHurtPlayer = roomPlayers[turnCount];
        //Debug.Log(roomPlayers[turnCount].NickName);
        var _currentScore = roomPlayers[turnCount].CustomProperties[PlayerManager.PLAYER_SCORE];
        var _newScore = (int)_currentScore - _dmg;

        gotHurtPlayer.SetCustomProperties(
            MyPhotonExtension.WrapToHash(new object[] { PlayerManager.PLAYER_SCORE, _newScore }));

        //死亡=>結束
        if (_newScore <= 0)
        {
            pv.RPC("FinishGame", RpcTarget.All);
        }
        else {
            pv.RPC("ChangeTurn", RpcTarget.MasterClient);
        }

    }

    [PunRPC]
    private void FinishGame() {
        Debug.Log("Game Finish");

    }
}
