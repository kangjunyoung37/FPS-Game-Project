
using System;
using System.Collections.Generic;

public class ServiceLocator
{
    /// <summary>
    /// 현재 등록된 서비스들
    /// </summary>
    private readonly Dictionary<string, IGameService> services = new Dictionary<string, IGameService>();

    public static ServiceLocator Current { get; private set; }

    public static void Initialize()
    {
        Current = new ServiceLocator();
    }

    public T Get<T>() where T : IGameService
    {
        string key = typeof(T).Name;
        if(!services.ContainsKey(key))
        {
            InfimaGames.LowPolyShooterPack.Log.kill($"{key} not registered with {GetType().Name}");
            throw new InvalidOperationException();
        }
        return (T)services[key];
    }

    /// <summary>
    /// 게임 서비스에 등록하기
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    public void Register<T>(T service) where T : IGameService
    {
        string key = typeof(T).Name;
        if(services.ContainsKey(key))
        {
            InfimaGames.LowPolyShooterPack.Log.kill($" {key}라는 키값이 {GetType().Name}으로 등록되어있습니다.");
            return;
        }
        services.Add(key, service);
    }

    /// <summary>
    /// 게임 서비스 삭제하기
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void Unregister<T>() where T : IGameService
    {
        string key = typeof(T).Name;
        if(!services.ContainsKey(key))
        {
            InfimaGames.LowPolyShooterPack.Log.kill($"{GetType().Name}를 가진{key}서비스가 없습니다.");
        }
        services.Remove(key);
    }

}
