using System.Collections;
using System.Collections.Generic;
using TMPro;
using Harpaesis.Overworld;
using UnityEngine.UI;
using UnityEngine;

namespace Harpaesis.UI
{

    /**
     * @author Matthew Sommer
     * class UIManager_Combat handles basic UI logic in the combat scene */
    public class UIManager_Combat : MonoBehaviour
    {
        TurnManager turnManager;
        public GameObject[] playerTurnObjects;
        public Button moveButton;
        public TextMeshProUGUI apText;

        bool IsFriendlyTurn { get { return turnManager.activeTurn.unit.GetType() == typeof(FriendlyUnit); } }

        #region Singleton
        public static UIManager_Combat instance;

        private void Awake()
        {
            instance = this;
        }
        #endregion

        private void Start()
        {
            turnManager = TurnManager.instance;
        }

        private void FixedUpdate()
        {
            if (IsFriendlyTurn)
            {
                moveButton.interactable = turnManager.activeTurn.unit.turnData.ap > 0 && turnManager.activeTurn.unit.canMove;
                apText.text = $"Remaining AP: {turnManager.activeTurn.unit.turnData.ap}/{turnManager.activeTurn.unit.currentApStat}";
            }
        }

        public void ShowPlayerUI(bool _showUI)
        {
            foreach (GameObject button in playerTurnObjects)
            {
                button.SetActive(_showUI);
            }
        }

        public void Button_Move()
        {
            if (IsFriendlyTurn)
            {
                FriendlyUnit _unit = (FriendlyUnit)turnManager.activeTurn.unit;
                _unit.MoveAction();
            }
        }

        public void Button_UseItem()
        {
            print("UseItem");
        }

        public void Button_SwapWeapon()
        {
            if (IsFriendlyTurn)
            {
                FriendlyUnit _unit = (FriendlyUnit)turnManager.activeTurn.unit;
                _unit.SwapWeapon();
            }
        }

        public void Button_EndTurn()
        {
            if (IsFriendlyTurn)
            {
                ((FriendlyUnit)turnManager.activeTurn.unit).EndTurn();
                turnManager.NextTurn();
            }
        }

        public void Button_UseSkill(int _index)
        {
            FriendlyUnit _unit = (FriendlyUnit)turnManager.activeTurn.unit;

            _unit.BeginTargeting(_index);
        }

        public void Button_Menu()
        {
            LevelLoadManager.LoadMainMenu();
        }

        public void Button_Continue()
        {
            OverworldData.CompleteCurrentPoint();
            LevelLoadManager.LoadOverworld();
        }

        public void Button_ReplayLevel()
        {
            LevelLoadManager.ReloadLevel();
        }
    }
}