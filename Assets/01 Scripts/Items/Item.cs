using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Item")]
public abstract class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    public abstract void UseEffect(Unit _unit);
}
