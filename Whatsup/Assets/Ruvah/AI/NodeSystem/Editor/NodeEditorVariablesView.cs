﻿using System;
using System.Collections.Generic;
using System.Linq;
using Ruvah.AI.NodeSystem.ParameterContainers;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEditorInternal;
using UnityEngine.Assertions.Comparers;

namespace Ruvah.AI.NodeSystem
{
    public partial class NodeEditorWindow
    {
        // -- FIELDS

        private const float nameFieldWidth = 0.6f;
        private const float nameValueSpaceWidth = 0.05f;
        private const float valueFieldWidth = 0.35f;

        private string SearchText = string.Empty;
        private GUILayoutOption ToolbarDropdownWidth = GUILayout.Width(20);
        private GUIContent ToolbarDropdownContent = new GUIContent();
        private GenericMenu ToolbarMenu;
        private ReorderableList reorderableVariablesList;
        private SearchField SearchField;

        private List<ParameterContainer> DisplayList;

        [SerializeField] private Vector2 VariablesScrollPosition;

        // -- METHODS

        protected virtual void DrawVariablesView()
        {
            DrawVariablesToolbar();
            DrawVariables();
        }

        private void DrawVariablesToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                var search_text = SearchField.OnToolbarGUI(SearchText);
                if (SearchText != search_text)
                {
                    SearchText = search_text;
                    UpdateSearchResults();
                }

                if (EditorGUILayout.DropdownButton(ToolbarDropdownContent, FocusType.Passive,
                    EditorStyles.toolbarButton, ToolbarDropdownWidth))
                {
                    ToolbarMenu.ShowAsContext();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void UpdateSearchResults()
        {
            bool is_search_empty = string.IsNullOrEmpty(SearchText);
            DisplayList = EditedSystem.Variables.FindAll(x => x.name.StartsWith(SearchText, StringComparison.OrdinalIgnoreCase));
            reorderableVariablesList.draggable = is_search_empty;
        }

        private void DrawVariables()
        {
            VariablesScrollPosition = EditorGUILayout.BeginScrollView(VariablesScrollPosition);
            {
                reorderableVariablesList.DoLayoutList();
            }
            EditorGUILayout.EndScrollView();
        }

        public void ReorderableVariablesList_OnChanged(ReorderableList list)
        {
            EditedSystem.Variables.Clear();
            EditedSystem.Variables.AddRange(DisplayList);
        }

        private void DrawVariable(Rect rect, int index, bool is_active, bool is_focused)
        {
            var container = DisplayList[index];

            var field_name_rect = new Rect(rect.x, rect.y,rect.width * nameFieldWidth, EditorGUIUtility.singleLineHeight);
            var field_value_rect = new Rect(field_name_rect.x + field_name_rect.width + rect.width * nameValueSpaceWidth, rect.y,rect.width * valueFieldWidth,EditorGUIUtility.singleLineHeight);

            if (is_active)
            {
                container.name = EditorGUI.DelayedTextField(field_name_rect, container.name);
                EditorUtility.SetDirty(EditedSystem);
            }
            else
            {
                EditorGUI.LabelField(field_name_rect, container.name);
            }

            switch (container)
            {
                case IntContainer int_variable:
                {
                    int_variable.Value = EditorGUI.DelayedIntField(field_value_rect, int_variable.Value);
                    break;
                }
                case FloatContainer float_variable:
                {
                    float_variable.Value = EditorGUI.DelayedFloatField(field_value_rect, float_variable.Value);
                    break;
                }
                case BoolContainer bool_variable:
                {
                    bool_variable.Value = EditorGUI.Toggle(field_value_rect, (bool) bool_variable.Value);
                    break;
                }
                case GameObjectContainer game_object_container:
                {
                    game_object_container.Value =
                        (GameObject) EditorGUI.ObjectField(field_value_rect, game_object_container.Value,
                            typeof(GameObject), false);
                    break;
                }
            }
        }

        private void AddIntVariable()
        {
            var container = CreateInstance<IntContainer>();
            container.Initialize(CreateNewVariableName("Int"), default);
            container.hideFlags = HideFlags.HideInHierarchy;
            AssetDatabase.AddObjectToAsset(container, EditedSystem);
            EditedSystem.Variables.Add(container);
            EditorUtility.SetDirty(EditedSystem);
            DisplayList = EditedSystem.Variables.FindAll(x => x.name.StartsWith(SearchText, StringComparison.OrdinalIgnoreCase));

        }

        private void AddFloatVariable()
        {
            var container = CreateInstance<FloatContainer>();
            container.Initialize(CreateNewVariableName("Float"), 0);
            container.hideFlags = HideFlags.HideInHierarchy;
            AssetDatabase.AddObjectToAsset(container, EditedSystem);
            EditedSystem.Variables.Add(container);
            EditorUtility.SetDirty(EditedSystem);
            DisplayList = EditedSystem.Variables.FindAll(x => x.name.StartsWith(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        private void AddBoolVariable()
        {
            var container = CreateInstance<BoolContainer>();
            container.Initialize(CreateNewVariableName("Bool"), default);
            container.hideFlags = HideFlags.HideInHierarchy;
            AssetDatabase.AddObjectToAsset(container, EditedSystem);
            EditedSystem.Variables.Add(container);
            EditorUtility.SetDirty(EditedSystem);
            DisplayList = EditedSystem.Variables.FindAll(x => x.name.StartsWith(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        private void AddGameObjectVariable()
        {
            var container = CreateInstance<GameObjectContainer>();
            container.Initialize(CreateNewVariableName("GameObject"), null);
            container.hideFlags = HideFlags.HideInHierarchy;
            AssetDatabase.AddObjectToAsset(container, EditedSystem);
            EditedSystem.Variables.Add(container);
            EditorUtility.SetDirty(EditedSystem);
            DisplayList = EditedSystem.Variables.FindAll(x => x.name.StartsWith(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        private string CreateNewVariableName(string name)
        {
            string base_name = $"New {name}";
            string result = base_name;
            int i = 0;
            while (EditedSystem.Variables.Any((x) => x.name.Equals(result)))
            {
                i++;
                result = $"{base_name} ({i})";
            }

            return result;
        }

        protected virtual void InitializeVariablesView()
        {
            ToolbarMenu = new GenericMenu();
            ToolbarMenu.AddItem(new GUIContent("Add new Int"),false, AddIntVariable);
            ToolbarMenu.AddItem(new GUIContent("Add new Float"),false, AddFloatVariable);
            ToolbarMenu.AddItem(new GUIContent("Add new Bool"),false, AddBoolVariable);
            ToolbarMenu.AddItem(new GUIContent("Add new GameObject"),false, AddGameObjectVariable);

            ToolbarDropdownContent.text = "+";

            DisplayList = new List<ParameterContainer>(EditedSystem.Variables);

            reorderableVariablesList = new ReorderableList(
                DisplayList,
                typeof(ParameterContainer),
                true,
                false,
                false,
                true
                );
            reorderableVariablesList.drawElementCallback = DrawVariable;
            reorderableVariablesList.headerHeight = 0;
            reorderableVariablesList.onChangedCallback = ReorderableVariablesList_OnChanged;
            SearchField = new SearchField();
        }
    }

}
