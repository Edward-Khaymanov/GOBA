﻿using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace GOBA
{
    public static class AddressablesHelper
    {
        public static IList<IResourceLocation> GetLocations<T>(object key)
        {
            var handle = Addressables.LoadResourceLocationsAsync(key, typeof(T));
            handle.WaitForCompletion();
            var result = handle.Result;
            Addressables.Release(handle);
            return result;
        }

        public static T LoadByLocation<T>(IResourceLocation location)
        {
            var handle = Addressables.LoadAssetAsync<T>(location);
            handle.WaitForCompletion();
            var result = handle.Result;
            Addressables.Release(handle);
            return result;
        }
    }
}