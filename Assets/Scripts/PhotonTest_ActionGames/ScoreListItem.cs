using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreListItem : MonoBehaviour
{
    public Text pointText;
    public void SetPointText(string _p) {
        pointText.text = _p;
    }
}
