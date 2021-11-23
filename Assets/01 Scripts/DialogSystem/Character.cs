using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Character")]
public class Character : ScriptableObject
{
    [Header("Character Info:")]
    public string characterName;
    [TextArea(3,5)]public string characterDescription;

    [Space]
    public GameObject characterModel;
    public Sprite characterTextBox;

    public override string ToString()
    {
        return characterName;
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
