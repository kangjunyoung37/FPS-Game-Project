using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPInventory : MonoBehaviour
{
    //[SerializeField]
    //private Transform Character;

    [SerializeField]
    private CharacterBehaviour characterBehaviour;

    public CharacterBehaviour GetCharacterBehaviour() => characterBehaviour;
}
