using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheGuarden.UI
{
    public class Menuj : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void startGame() 
        {
            SceneManager.LoadScene("PlayTest");
        }
        public void quitGame() 
        {
            Application.Quit();
        }
    }
}
