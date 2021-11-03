using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BattleLog : MonoBehaviour
{
    bool toggleBattleLog = false;
    public Text battleLogTextBox;

    private static string battleLogText;

    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(toggleBattleLog);
        StartCoroutine(UpdateLog());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            toggleBattleLog = !toggleBattleLog;
            transform.GetChild(0).gameObject.SetActive(toggleBattleLog);
        }
    }

    private IEnumerator UpdateLog()
    {
        while (true)
        {
            battleLogTextBox.text = battleLogText;
            yield return new WaitForEndOfFrame();
        }
    }

    public static void Log(string _message, BattleLogType _logType)
    {
        battleLogText += $"\n[{_logType}]: {_message}";
    }
}
public enum BattleLogType { CombatLog, SystemLog }