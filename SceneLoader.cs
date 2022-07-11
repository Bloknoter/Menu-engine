using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;


namespace MenuEngine
{
    public class SceneLoader : MonoBehaviour
    {
        public void LoadScene(string Name)
        {
            SceneManager.LoadScene(Name);
        }
    }
}
