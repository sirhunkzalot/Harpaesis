using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harpaesis.UI
{
    public class UIManager_EndCombatScreen : MonoBehaviour
    {
        public GameObject victoryScreen;
        public GameObject defeatScreen;

        public static UIManager_EndCombatScreen instance;

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            victoryScreen.SetActive(false);
            defeatScreen.SetActive(false);
        }

        /*private void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                OpenVictoryScreen();
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                OpenLoseScreen();
            }
        }*/

        public void OpenVictoryScreen()
        {
            victoryScreen.SetActive(true);
        }

        public void OpenLoseScreen()
        {
            defeatScreen.SetActive(true);
        }
    }
}