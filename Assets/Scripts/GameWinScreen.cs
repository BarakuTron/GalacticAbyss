using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameWinScreen : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    public PlayerStats playerStats;
    

    public void Setup(int score) {
        gameObject.SetActive(true);
        pointsText.text = "Score: " + score.ToString();
    }

    public void RestartButton() {
        Destroy(playerStats.gameObject);
        SceneManager.LoadScene("Level00");
        
    }

     public void ExitButton() {
        Destroy(playerStats.gameObject);
        SceneManager.LoadScene("MainMenu");
    }
}
