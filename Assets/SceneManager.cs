using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public void LoadScenes(int a)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(a);
    }
}
