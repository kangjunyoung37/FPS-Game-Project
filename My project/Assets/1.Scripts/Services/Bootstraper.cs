using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Bootstraper
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        ServiceLocator.Initialize();

        ServiceLocator.Current.Register<IGameModeService>(new GameModeService());

        #region Sound Manager Service

        var roomMagaerObjecet = new GameObject("Room Manager");
        roomMagaerObjecet.AddComponent<RoomManager>();

        var soundManagerObject = new GameObject("Sound Manager");
        var soundManagerService = soundManagerObject.AddComponent<AudioManagerService>();

        var gameManagerObject = new GameObject("Game Manager");
        gameManagerObject.AddComponent<GameManager>();
        
        Object.DontDestroyOnLoad(gameManagerObject);
        Object.DontDestroyOnLoad(soundManagerObject);
        Object.DontDestroyOnLoad(roomMagaerObjecet);

        ServiceLocator.Current.Register<IAudioManagerService>(soundManagerService);
        #endregion
    }
}

