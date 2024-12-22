using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace GOBA.CORE
{
    public interface IResourceProvider
    {
        public IList<IResourceLocation> GetLocations<T>(object key);
        public T LoadByLocation<T>(IResourceLocation location);
        public T LoadByKey<T>(object key);
    }
}