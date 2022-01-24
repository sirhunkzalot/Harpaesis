using System.Collections;
using System.Collections.Generic;
using Harpaesis.UI;
using UnityEngine.Events;
using UnityEngine;

namespace Harpaesis.Chungus
{
    public class Dialogue : MonoBehaviour
    {
        bool skipLine;

        [Header("Before Dialog")]
        public UnityEvent OnStartDialog;

        [Space]
        public List<Dialogue_Line> lines = new List<Dialogue_Line>();

        [Header("After Dialogue")]
        public UnityEvent OnFinishDialog;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void StartDialog()
        {
            OnBeginDialog();
            UIManager_Dialog.instance.StartText(this);

        }

        public void OnBeginDialog()
        {
            OnStartDialog.Invoke();
        }

        public void OnEndDialog()
        {
            OnFinishDialog.Invoke();
            gameObject.SetActive(false);
        }
    }

}