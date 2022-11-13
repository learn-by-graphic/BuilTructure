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

    public void reload(GameObject popup)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        popup.SetActive(false);
    }
}
