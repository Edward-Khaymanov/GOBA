using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace GOBA
{
    public class Level : NetworkBehaviour
    {
        [field: SerializeField] public List<TeamSpawnpoint> Spawnpoints { get; private set; }
    }
}