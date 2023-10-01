using UnityEngine;

namespace GOBA
{
    public enum LogLevel
    {
        Log,
        Warning,
        Error
    }

    public class MyLogger : MonoBehaviour
    {
        [SerializeField] private bool _isEnable;
        [SerializeField] private LogLevel _minimumDisplayLevel;

        private static MyLogger _instance;

        private void Awake()
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public static void Log(object message, LogLevel logLevel = LogLevel.Log)
        {
            if (_instance == null)
                return;

            _instance.LogInternal(message, logLevel);
        }

        private void LogInternal(object message, LogLevel logLevel = LogLevel.Log)
        {
            if (_isEnable == false)
                return;

            if (logLevel < _minimumDisplayLevel)
                return;

            switch (logLevel)
            {
                case LogLevel.Log:
                    Debug.Log(message);
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning(message);
                    break;
                case LogLevel.Error:
                    Debug.LogError(message);
                    break;
            }
        }
    }
}