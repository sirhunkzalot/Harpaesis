using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Game_Data
{
    public List<CharacterToken> characterTokens = new List<CharacterToken>();

    public Game_Data(List<GameObject> characters)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            //characters[i].GetInstanceID != Able to do what you think
            //Use a GUID like a champ!
            //Hint: string guid = System.Guid.NewGuid().ToString();
            CharacterToken temp = new CharacterToken(characters[i]);
            characterTokens.Add(temp);
        }
    }
}

/// <summary>
/// use as a reference for all save data
/// </summary>
[System.Serializable]
public class VectorToken
{
    private float _x;
    private float _y;
    private float _z;

    public Vector3 ReturnVector { get { return new Vector3(_x, _y, _z); } }

    public VectorToken(Vector3 vector)
    {
        _x = vector.x;
        _y = vector.y;
        _z = vector.z;
    }
}

[System.Serializable]
public class CharacterToken
{
    private VectorToken _position;

    public CharacterToken(GameObject myObject)
    {
        _position = new VectorToken(myObject.transform.position);
    }
}

