using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Harpaesis.Overworld
{
    public class PlayerInput_Overworld : MonoBehaviour
    {
        OverworldController controller;

        private void Start()
        {
            controller = OverworldController.instance;
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
    }
}