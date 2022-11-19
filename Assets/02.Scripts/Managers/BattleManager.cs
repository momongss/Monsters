using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager I { get; private set; }

    public UIGameResult UI_GameResult;

    private void Awake()
    {
        I = this;
    }

    public void OnBattleEnd(bool isWin)
    {
        UI_GameResult.OnGameEnd(isWin);

        if (isWin)
        {
            
        }
    }
}
