using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour { 
    
      public void LoadByindex(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
        Time.timeScale = 1f;
      }
    
}