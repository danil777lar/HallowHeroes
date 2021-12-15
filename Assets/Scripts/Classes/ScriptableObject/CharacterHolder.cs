using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/CharacterHolder")]
public class CharacterHolder : ScriptableObject
{
    #region Character
    [Serializable]
    public class Character
    {
        public int characterCost;
        public string characterName;
        public GameObject characterPrefab;
    }
    #endregion

    [SerializeField] private List<Character> _characters;

    public int Count => _characters.Count;
    public Character Current
    {
        get
        {
            int id = PlayerPrefs.GetInt("Character", 0);
            return _characters[id];
        }
        set
        {
            PlayerPrefs.SetInt("Character", _characters.IndexOf(value));
        }
    }


    public bool IsUnlocked(Character character) 
    {
        bool result = character.characterCost < 1;
        result |= (PlayerPrefs.GetInt("CharacterUnlock"+_characters.IndexOf(character), 0) == 1);
        return result;
    }

    public void Unlock(Character character) 
    {
        PlayerPrefs.SetInt("CharacterUnlock" + _characters.IndexOf(character), 1);
    }

    public Character GetCharacter(int id) 
    {
        return _characters[id];
    }
}
