using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void LoadScenes(string scene)
    {   
        SceneManager.LoadSceneAsync(scene);
    }
}
