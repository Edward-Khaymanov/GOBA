using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Netcode;
using UnityEngine;

namespace GOBA
{
    public class Terrain : NetworkBehaviour
    {
        [field: SerializeField] public List<TeamSpawnpoint> Spawnpoints { get; private set; }
    }
}