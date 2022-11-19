using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGameResult : MonoBehaviour
{
    public TextMeshProUGUI game_result_text;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnGameEnd(bool game_result)
    {
        if (game_result == true)
        {
            game_result_text.text = "WIN !!";
        } else
        {
            game_result_text.text = "DEFEAT";
        }

        gameObject.SetActive(true);
    }
}
