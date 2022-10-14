using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameModeService : IGameService
{
    CharacterBehaviour GetPlayerCharacter();
}
