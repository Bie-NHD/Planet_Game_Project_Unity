using UnityEngine;

/// <summary>
/// Interface for managing player preferences of a specific type.
/// </summary>
/// <typeparam name="T">The type of the preference value.</typeparam>
public interface IPlayerPrefConfig<T>
{
    /// <summary>
    /// Retrieves the stored preference value of type <typeparamref name="T"/>.
    /// </summary>
    /// <returns>The preference value.</returns>
    T GetPref();

    /// <summary>
    /// Stores the specified preference value of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="value">The value to store.</param>
    void SetPref(T value);
}

/// <summary>
/// Abstract base class for managing player preferences using Unity's PlayerPrefs system.
/// </summary>
/// <typeparam name="T">The type of the preference value.</typeparam>
public abstract class BasePlayerPrefConfig<T> : ScriptableObject, IPlayerPrefConfig<T>
{
    /// <summary>
    /// The key used to identify the stored value in PlayerPrefs.
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Retrieves the value of type <typeparamref name="T"/> from PlayerPrefs.
    /// </summary>
    /// <returns>The value retrieved from PlayerPrefs.</returns>
    public abstract T GetPref();

    /// <summary>
    /// Stores the specified value of type <typeparamref name="T"/> in PlayerPrefs.
    /// </summary>
    /// <param name="value">The value to store in PlayerPrefs.</param>
    public abstract void SetPref(T value);

    /// <summary>
    /// Ensures that any changes to PlayerPrefs are saved to disk when the object is destroyed.
    /// </summary>
    void OnDestroy()
    {
        PlayerPrefs.Save();
    }
}
