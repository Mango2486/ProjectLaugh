using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvents : MonoBehaviour
{
    [SerializeField]
    private SceneIndex.sceneIndex sceneIndex;
    public void StartGame()
    {   
        GameManager.instance.PlaySoundSFX(5);
        SceneManager.LoadScene((int)sceneIndex);
    }

    public void QuitGame()
    {   
        GameManager.instance.PlaySoundSFX(5);
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
