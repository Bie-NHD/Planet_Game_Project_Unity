using UnityEngine;

namespace Configs
{
    public class IntPlayerPrefConfig : BasePlayerPrefConfig<int>
    {
        [SerializeField]
        protected int _value = 0;

        /// <summary>
        /// The key used to store and retrieve the integer preference from PlayerPrefs.
        /// </summary>
        [SerializeField]
        private new string Key;

        void Awake()
        {
            if (!PlayerPrefs.HasKey(Key))
            {
                SetPref(0);
            }
            else
            {
                _value = PlayerPrefs.GetInt(Key);
            }
        }

        public override int GetPref()
        {
            return _value;
        }

        public override void SetPref(int value)
        {
            PlayerPrefs.SetInt(Key, value);
            PlayerPrefs.Save(); // TODO: Check performance impact
            _value = value;
        }
    }
}
