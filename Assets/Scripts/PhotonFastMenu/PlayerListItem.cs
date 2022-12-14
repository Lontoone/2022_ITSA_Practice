using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] Text text;
    [SerializeField] MasterButtonControl masterButton;
    [SerializeField] MenuReadyButton readyButton;
    Player player;
    public void SetUp(Player _player)
    {
        player = _player;
        text.text = _player.NickName;

        masterButton.SetUp(player);
        readyButton.Init(player);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
