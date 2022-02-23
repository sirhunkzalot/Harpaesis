using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Harpaesis.Overworld
{
    public class PlayerInput_Overworld : MonoBehaviour
    {
        OverworldController controller;

        [ReadOnly] public bool cancel;

        [ReadOnly] public bool pause;

        public static PlayerInput_Overworld instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            controller = GetComponent<OverworldController>();
        }

        public void I_MoveNorth(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                controller.MoveNorth();
            }
        }
        public void I_MoveEast(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                controller.MoveEast();
            }
        }
        public void I_MoveSouth(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                controller.MoveSouth();
            }
        }
        public void I_MoveWest(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                controller.MoveWest();
            }
        }

        public void I_Interact(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
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
                pause = !pause;
            }
        }
    }
}