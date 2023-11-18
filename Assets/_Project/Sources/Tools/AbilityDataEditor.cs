#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GOBA
{
    [CustomEditor(typeof(AbilityData))]
    public class AbilityDataEditor : Editor
    {
        private IList<Type> _types;
        private string[] _typesName;
        private readonly string _abilityPropertyName = nameof(AbilityData.Ability);
        private string _currentTypeName;
        private bool _selectStarted;
        private bool _errorShowing;
        private int _selectIndex = -1;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Current type", _currentTypeName);
            base.OnInspectorGUI();

            if (_selectStarted == false)
            {
                _selectStarted = GUILayout.Button("Select type");
                return;
            }

            _selectIndex = EditorGUILayout.Popup(_selectIndex, _typesName);
            var isCreation = GUILayout.Button("Confirm");
            var isCancellation = GUILayout.Button("Cancel");

            if (isCancellation)
            {
                _selectIndex = -1;
                _selectStarted = false;
                _errorShowing = false;
            }

            if (isCreation && _selectIndex >= 0)
            {
                Create(_selectIndex);
            }

            ShowError();
            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable()
        {
            _types = GetAllAbilityTypes();
            _typesName = _types.Select(t => t.Name).ToArray();
            var prop = serializedObject.FindProperty(_abilityPropertyName);
            _currentTypeName = prop.managedReferenceValue?.GetType()?.Name;
        }

        private void Create(int typeNameIndex)
        {
            var typeName = _typesName[typeNameIndex];
            if (_currentTypeName == typeName)
            {
                _errorShowing = true;
                return;
            }

            var type = _types.FirstOrDefault(t => t.Name == typeName);
            SetAbility(type);

            _selectIndex = -1;
            _selectStarted = false;
            _errorShowing = false;
        }

        private void ShowError()
        {
            if (_errorShowing)
                EditorGUILayout.HelpBox("This type is already selected", MessageType.Error);
        }

        private void SetAbility(Type abilityType)
        {
            var prop = serializedObject.FindProperty(_abilityPropertyName);
            var ability = Activator.CreateInstance(abilityType) as AbilityBase;
            prop.managedReferenceValue = ability;
            _currentTypeName = ability.GetType().Name;
        }

        private IList<Type> GetAllAbilityTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(AbilityBase)) && type.IsAbstract == false)
                .ToList();
        }
    }
}
#endif