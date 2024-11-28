using UnityEngine;

namespace GOBA.CORE
{
    public interface IMove
    {
        //public bool CanMove { get; }
        //public bool IsMoving { get; }

        public void MoveTo(Vector3 position);
        //public void Move();
        //public void FollowEntity(IUnit unit);
    }
}