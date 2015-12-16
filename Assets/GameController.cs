using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameController : MonoBehaviour {
    public int CurrentTurn = 0;
    public Text text;
    public void NextTurnButton_Click()
    {
        CurrentTurn++;
        text.text = CurrentTurn.ToString();
    }
}
