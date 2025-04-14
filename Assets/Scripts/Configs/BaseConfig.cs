using UnityEngine;
using Utils;

public interface IPlayerPrefConfig<T>
{
    public T GetPref();
    public void SetPref(T value);
}

public abstract class BasePlayerPrefConfig<T> : ScriptableObject, IPlayerPrefConfig<T>
{
    protected string _key { get; set; } = string.Empty;
    public abstract T GetPref();
    public abstract void SetPref(T value);
}
