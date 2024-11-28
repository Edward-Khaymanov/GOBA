using Cysharp.Threading.Tasks;
using GOBA.CORE;
using System;
using Unity.Netcode;
using UnityEngine;

namespace GOBA.Assets._Project.Sources._Test
{
    public class MoveCommand : ICommand
    {
        private IUnit _target;
        private Vector3 _targetPosition;

        public MoveCommand(IUnit target, Vector3 targetPosition)
        {
            _target = target;
            _targetPosition = targetPosition;
        }

        public void Execute()
        {
            _target.MoveTo(_targetPosition);
        }
    }
}