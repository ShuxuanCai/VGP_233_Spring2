using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //public Button restartButton;

    //public void Update()
    //{
    //    if (GameStats.gameOver == true)
    //        restartButton.gameObject.SetActive(true);
    //}

    public void RestartGame()
    {
        SceneManager.LoadScene("GamePlay");
    }
}
