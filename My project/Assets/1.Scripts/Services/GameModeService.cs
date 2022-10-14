using UnityEngine;

public class GameModeService : IGameModeService
{

    #region FIELDS

    private CharacterBehaviour playerCharacter;
    #endregion FLELDS

    #region FUNCTION

    public CharacterBehaviour GetPlayerCharacter()
    {
        if(playerCharacter == null)
        {
            playerCharacter = Object.FindObjectOfType<CharacterBehaviour>();
        }

        return playerCharacter;
    }
    #endregion FUNCTION
}
