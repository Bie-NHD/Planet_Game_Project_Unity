using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptToMain : MonoBehaviour
{

    public string nextSceneName = "SampleScene"; // T�n scene sau splash

     public void LoadNextScene()
    {
        Debug.Log("Loading next scene..."); 
        SceneManager.LoadScene(nextSceneName);
    }
}
