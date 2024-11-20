using Cysharp.Threading.Tasks;
using MapModCore;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace GOBA
{
    public class ProjectileProvider : IProjectileProvider
    {
        private static Dictionary<int, IResourceLocation> _idProjectile;
        //private Dictionary<int, AbilityProjectile> _idProjectile;

        public async UniTask Initialize()
        {
            _idProjectile = new Dictionary<int, IResourceLocation>();
            var locations = GetLocations<GameObject>("AbilityProjectile");
            foreach (var location in locations)
            {
                var handle = Addressables.LoadAssetAsync<GameObject>(location);
                await handle;

                if (handle.Status == AsyncOperationStatus.Failed)
                {
                    MyLogger.Log(handle.OperationException, LogLevel.Error);
                    throw handle.OperationException;
                }

                var projectile = handle.Result.GetComponent<AbilityProjectile>();
                _idProjectile.Add(projectile.Id, location);
                Addressables.Release(handle);
            }
        }

        public AbilityProjectile GetProjectile(int id)
        {
            var projectileLocation = _idProjectile[id];
            return LoadByLocation<GameObject>(projectileLocation).GetComponent<AbilityProjectile>();
        }

        private T LoadByLocation<T>(IResourceLocation location)
        {
            var handle = Addressables.LoadAssetAsync<T>(location);
            handle.WaitForCompletion();
            return handle.Result;
        }

        private IList<IResourceLocation> GetLocations<T>(object key)
        {
            var catalogHandle = Addressables.LoadResourceLocationsAsync(key, typeof(T));
            catalogHandle.WaitForCompletion();
            return catalogHandle.Result;
        }
    }
}