using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ScoreBoardManager : MonoBehaviourPunCallbacks
{
    public static ScoreBoardManager sbm_instance;
    public ScoreListItem listItemPrefab;
    public Transform listContainer;

    private Dictionary<Player, ScoreListItem> itemPairs =new Dictionary<Player, ScoreListItem>();

    private void Awake()
    {
        sbm_instance = this;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        UpdatePoint(targetPlayer);
        Debug.Log("Update Point");
    }

    public void SetupPlayer(Player p) {
        ScoreListItem _li = GameObject.Instantiate<ScoreListItem>(listItemPrefab);
        _li.transform.SetParent(listContainer);

        itemPairs.Add(p, _li);
    }

    public void UpdatePoint(Player p) {
        itemPairs[p].SetPointText(p.CustomProperties[PlayerManager.PLAYER_SCORE].ToString());
    }
}
