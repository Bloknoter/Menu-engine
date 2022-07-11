using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MenuEngine
{
    public class GameEscaper : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}
