using System.Collections;
using System.Collections.Generic;
using Harpaesis.UI;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Harpaesis.Combat
{
    public class PlayerInput_Combat : MonoBehaviour
    {
        [ReadOnly] public bool mouseDownLeft;
        [ReadOnly] public bool mouseDownRight;
        [ReadOnly] public Vector2 mousePosition;

        [ReadOnly] public Vector2 cameraMovement;
        [ReadOnly] public float cameraScroll;

        UIManager_Combat uiCombat;
        GridCamera gridCamera;

        

        public static PlayerInput_Combat instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            uiCombat = UIManager_Combat.instance;
            gridCamera = GridCamera.instance;
        }

        public void I_MoveShortcut(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                uiCombat.Button_Move();
            }
        }
        public void I_ItemsShortcut(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                uiCombat.Button_Items();
            }
        }
        public void I_DefendShortcut(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                uiCombat.Button_Defend();
            }
        }
        public void I_EndTurnShortcut(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                uiCombat.Button_EndTurn();
            }
        }
        public void I_BasicAttack(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                uiCombat.Button_UseSkill(0);
            }
        }
        public void I_PrimarySkill(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                uiCombat.Button_UseSkill(1);
            }
        }
        public void I_SecondarySkill(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                uiCombat.Button_UseSkill(2);
            }
        }
        public void I_TertiarySkill(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                uiCombat.Button_UseSkill(3);
            }
        }
        public void I_SignatureSkill(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                uiCombat.Button_UseSkill(4);
            }
        }
        public void I_SpeedUp(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                uiCombat.SpeedUp(true);
            }
            else if (_ctx.canceled)
            {
                uiCombat.SpeedUp(false);
            }
        }
        public void I_ResetCamera(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                gridCamera.JumpToCurrentUnit();
            }
        }
        public void I_CameraMovement(InputAction.CallbackContext _ctx)
        {
            cameraMovement = _ctx.ReadValue<Vector2>();
        }
        public void I_CameraZoom(InputAction.CallbackContext _ctx)
        {
            cameraScroll = _ctx.ReadValue<float>();
        }
        public void I_CameraRotateLeft(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                gridCamera.RotateLeft();
            }
        }
        public void I_CameraRotateRight(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                gridCamera.RotateRight();
            }
        }
        public void I_MousePosition(InputAction.CallbackContext _ctx)
        {
            mousePosition = _ctx.ReadValue<Vector2>();
        }
        public void I_Pause(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                if (uiCombat.settings.activeSelf)
                {
                    uiCombat.settings.SetActive(false);
                }
                else uiCombat.settings.SetActive(true);
            }
        }
        public void I_Affirm(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                mouseDownLeft = true;
            }
            else if (_ctx.canceled)
            {
                mouseDownLeft = false;
            }
        }
        public void I_Cancel(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                mouseDownRight = true;
            }
            else if (_ctx.canceled)
            {
                mouseDownRight = false;
            }
        }
        public void I_ProgressDialog(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                UIManager_Dialog.instance?.SkipLine();
            }
        }

        public void I_PartyInfo(InputAction.CallbackContext _ctx)
        {
            if (_ctx.started)
            {
                if (uiCombat.partyInfo.activeSelf)
                {
                    uiCombat.partyInfo.SetActive(false);
                }
                else uiCombat.partyInfo.SetActive(true);
            }
        }
    }
}