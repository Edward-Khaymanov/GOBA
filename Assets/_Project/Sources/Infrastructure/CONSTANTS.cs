using UnityEngine;

namespace GOBA
{
    public class CONSTANTS
    {
        public const int NEUTRAL_TEAM_ID = -1;
        public const float RAYCAST_MAX_DISTANCE = 1000;
        public const float ARMOR_REDUCE_MULTIPLIER = 0.05f;

        public const int COLLIDERS_MAX_GET = 100;
        public const ushort DEFAULT_PORT = 42532;
        public const string DEFAULT_IP = "127.0.0.1";


        public const int MAIN_MENU_SCENE_INDEX = 1;
        public const int LOBBY_SCENE_INDEX = 2;
        public const int GAME_SCENE_INDEX = 3;


        public const string GAME_SCENE_NAME = "Game";
        public const string MAIN_MENU_SCENE_NAME = "MainMenu";
        public const string LOBBY_SCENE_NAME = "Lobby";


        public class Layers
        {
            public static LayerMask AllLayers = Physics.AllLayers;
            public static LayerMask None = new LayerMask();
            public static LayerMask Unit = LayerMask.GetMask(nameof(Unit));
            public static LayerMask Terrain = LayerMask.GetMask(nameof(Terrain));
        }

        public class AddresablesMarks
        {
            public const string UNIT = "unit";
        }
    }
}