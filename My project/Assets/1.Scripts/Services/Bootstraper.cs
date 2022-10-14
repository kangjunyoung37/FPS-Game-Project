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

    }
}

