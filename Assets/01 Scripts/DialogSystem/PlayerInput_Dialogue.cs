using System.Collections;
using System.Collections.Generic;
using Harpaesis.Chungus;
using Harpaesis.UI;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInput_Dialogue : MonoBehaviour
{
    UIManager_Dialog ui;

    private void Start()
    {
        ui = UIManager_Dialog.instance;
    }
    public void I_ProgressDialog(InputAction.CallbackContext _ctx)
    {
        if (_ctx.started)
        {
            ui.SkipLine();
        }
    }
}
