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

        var soundManagerObject = new GameObject("Sound Manager");
        var soundManagerService = soundManagerObject.AddComponent<AudioManagerService>();

        Object.DontDestroyOnLoad(soundManagerObject);

        ServiceLocator.Current.Register<IAudioManagerService>(soundManagerService);
        #endregion
    }
}

