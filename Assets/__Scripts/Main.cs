using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;  // Singleton for Main

    private void Awake()
    {
        S = this;
    }

    public void Restart()
    {
        SceneManager.LoadScene("SceneLevel1");
    }
}
