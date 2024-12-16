using UnityEngine;

namespace GOBA
{
    public static class PlayerLocalDependencies
    {
        public static PlayerCamera PlayerCamera { get; private set; }
        public static PlayerInput PlayerInput { get; private set; }
        public static PlayerUI PlayerUI { get; private set; }
        public static BarsRenderer BarsRenderer { get; private set; }


        public static void Init()
        {
            PlayerCamera = GameObject.FindAnyObjectByType<PlayerCamera>();
            PlayerInput = GameObject.FindAnyObjectByType<PlayerInput>();
            PlayerUI = GameObject.FindAnyObjectByType<PlayerUI>();
            BarsRenderer = GameObject.FindAnyObjectByType<BarsRenderer>();
        }

        public static void Dispose()
        {
            PlayerCamera = null;
            PlayerInput = null;
            PlayerUI = null;
            BarsRenderer = null;
        }
    }
}