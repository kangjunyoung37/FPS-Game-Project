
using System;
using System.Collections.Generic;

public class ServiceLocator
{
    /// <summary>
    /// ���� ��ϵ� ���񽺵�
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
    /// ���� ���񽺿� ����ϱ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    public void Register<T>(T service) where T : IGameService
    {
        string key = typeof(T).Name;
        if(services.ContainsKey(key))
        {
            InfimaGames.LowPolyShooterPack.Log.kill($" {key}��� Ű���� {GetType().Name}���� ��ϵǾ��ֽ��ϴ�.");
            return;
        }
        services.Add(key, service);
    }

    /// <summary>
    /// ���� ���� �����ϱ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void Unregister<T>() where T : IGameService
    {
        string key = typeof(T).Name;
        if(!services.ContainsKey(key))
        {
            InfimaGames.LowPolyShooterPack.Log.kill($"{GetType().Name}�� ����{key}���񽺰� �����ϴ�.");
        }
        services.Remove(key);
    }

}
