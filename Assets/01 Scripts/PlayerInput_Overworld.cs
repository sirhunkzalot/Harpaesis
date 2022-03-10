using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Harpaesis.Overworld
{
    public class PlayerInput_Overworld : MonoBehaviour
    {
        OverworldController controller;

        public GameObject partyInfo;

        [ReadOnly] public bool cancel;

        [ReadOnly] public bool pause;

        OverworldTestingScenes testingScenes;

        public static PlayerInput_Overworld instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            controller = GetComponent<OverworldController>();
            testingScenes = OverworldTestingScenes.instance;
        }

        public void I_MoveNorth(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started && !pause)
            {
                controller.MoveNorth();
            }
        }
        public void I_MoveEast(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started && !pause)
            {
                controller.MoveEast();
            }
        }
        public void I_MoveSouth(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started && !pause)
            {
                controller.MoveSouth();
            }
        }
        public void I_MoveWest(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started && !pause)
            {
                controller.MoveWest();
            }
        }

        public void I_Interact(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started && !pause)
            {
                controller.Interact();
            }
        }

        public void I_Cancel(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                cancel = true;
            }
            else if (_ctx.canceled)
            {
                cancel = false;
            }
        }

        public void I_Pause(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                TogglePause();
            }
        }

        public void TogglePause()
        {
            pause = !pause;
            testingScenes.Pause();
        }

        public void I_PartyInfo(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                if (partyInfo.activeSelf)
                {
                    partyInfo.SetActive(false);
                }
                else partyInfo.SetActive(true);
            }
        }
    }
}