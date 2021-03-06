using System.Collections;
using System.Collections.Generic;
using TMPro;
using Harpaesis.Combat;
using Harpaesis.Overworld;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        public GameObject movementItemButtons;
        public TextMeshProUGUI rightClickText;

        public GameObject speedUpIcon;

        [Header("Pause")]
        public GameObject settings;
        public GameObject mainSettings;
        public GameObject audioSettings;
        public GameObject visualSettings;
        public GameObject Items;
        public GameObject partyInfo;

        public AudioSource sfx;

        SkillSlot[] skillSlots;

        public string postCombatLevelToLoad;

        public bool buttonPressedThisFrame;

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
            skillSlots = GetComponentsInChildren<SkillSlot>();
            ActivateCancelPopup(false);
        }

        private void FixedUpdate()
        {
            if (IsFriendlyTurn)
            {
                moveButton.interactable = turnManager.activeTurn.unit.turnData.ap > 0 && turnManager.activeTurn.unit.canMove;
                apText.text = $"Remaining AP: {turnManager.activeTurn.unit.turnData.ap}/{turnManager.activeTurn.unit.unitData.apStat}";
                buttonPressedThisFrame = false;
            }
        }

        public void ActivateCancelPopup(bool _cancelPopup)
        {
            if (_cancelPopup)
            {
                movementItemButtons.SetActive(false);
                rightClickText.gameObject.SetActive(true);
            }
            else
            {
                movementItemButtons.SetActive(true);
                rightClickText.gameObject.SetActive(false);
            }
        }

        public void ShowPlayerUI(bool _showUI)
        {
            foreach (GameObject button in playerTurnObjects)
            {
                button.SetActive(_showUI);
            }

            if (_showUI)
            {
                foreach (SkillSlot _slot in skillSlots)
                {
                    _slot.UpdateSkillSlot();
                }
            }
        }

        public void Button_Move()
        {
            if (IsFriendlyTurn)
            {
                FriendlyUnit _unit = (FriendlyUnit)turnManager.activeTurn.unit;
                _unit.MoveAction();
                buttonPressedThisFrame = true;
            }
        }

        public void Button_Items()
        {
            buttonPressedThisFrame = true;
        }

        public void Button_Defend()
        {
            if (IsFriendlyTurn)
            {
                FriendlyUnit _unit = (FriendlyUnit)turnManager.activeTurn.unit;
                _unit.DefendAction();
                buttonPressedThisFrame = true;
            }
        }

        public void Button_SwapWeapon()
        {
            if (IsFriendlyTurn)
            {
                FriendlyUnit _unit = (FriendlyUnit)turnManager.activeTurn.unit;
                _unit.SwapWeapon();
                foreach (SkillSlot _slot in skillSlots)
                {
                    _slot.UpdateSkillSlot();
                }
                buttonPressedThisFrame = true;
            }
        }

        public void Button_EndTurn()
        {
            if (IsFriendlyTurn)
            {
                ((FriendlyUnit)turnManager.activeTurn.unit).EndTurn();
                turnManager.NextTurn();
                buttonPressedThisFrame = true;
            }
        }

        public void Button_UseSkill(int _index)
        {
            FriendlyUnit _unit = (FriendlyUnit)turnManager.activeTurn.unit;
            buttonPressedThisFrame = true;
            if (_unit.currentState == FriendlyUnit.FriendlyState.Active)
            {
                _unit.BeginTargeting(_index);
            }

        }

        public void Button_Menu()
        {
            LevelLoadManager.LoadMainMenu();
        }

        public void Button_Continue()
        {
            OverworldData.CompleteCurrentPoint();
            LevelLoadManager.LoadLevel(postCombatLevelToLoad);
        }

        public void Button_ReplayLevel()
        {
            LevelLoadManager.ReloadLevel();
        }

        public void Pause()
        {
            settings.SetActive(!settings.activeInHierarchy);
            audioSettings.SetActive(false);
            visualSettings.SetActive(false);
            mainSettings.SetActive(true);
            sfx.Play();
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void SpeedUp(bool _speedUp)
        {
            if (_speedUp)
            {
                Time.timeScale = 5f;
                speedUpIcon.SetActive(true);
            }
            else
            {
                Time.timeScale = 1f;
                speedUpIcon.SetActive(false);
            }
        }

        public void QuitOverworld()
        {
            SceneManager.LoadScene("OverworldTesting");
        }
    }
}