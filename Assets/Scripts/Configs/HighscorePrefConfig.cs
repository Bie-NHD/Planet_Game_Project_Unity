using UnityEngine;
using UnityEngine.Events;

namespace Configs
{
    [CreateAssetMenu(
        fileName = "HighscorePrefConfig",
        menuName = "Scriptable Objects/Configs/Player Preferences/Highscore Config"
    )]
    public class HighscorePrefConfig : IntPlayerPrefConfig
    {
        public int HighScore => _value;

        public static UnityEvent<int> UpdateHighScoreEvent { get; private set; } = new();

        void OnDestroy()
        {
            UpdateHighScoreEvent.RemoveAllListeners();
        }
    }
}
