using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Core;

public class RestartBtn : MonoBehaviour
{
    public void RestartGame()
    {
        SceneLoader.instance.RestartGame();
    }
}
