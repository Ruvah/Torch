using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEditorInternal;

namespace Ruvah.NodeSystem
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
                SearchText = GUILayout.TextField(SearchText, EditorStyles.toolbarSearchField);
                if (EditorGUILayout.DropdownButton(ToolbarDropdownContent, FocusType.Passive,
                    EditorStyles.toolbarButton, ToolbarDropdownWidth))
                {
                    ToolbarMenu.ShowAsContext();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawVariables()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                reorderableVariablesList.draggable = true;
                reorderableVariablesList.list = EditedSystem.Variables;
            }
            else
            {
                var display_list = EditedSystem.Variables.FindAll((x) => x.Name.StartsWith(SearchText));
                reorderableVariablesList.draggable = false;
                reorderableVariablesList.list = display_list;
            }
            VariablesScrollPosition = EditorGUILayout.BeginScrollView(VariablesScrollPosition);
            {
                reorderableVariablesList.DoLayoutList();
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawVariable(Rect rect, int index, bool is_active, bool is_focused)
        {
            var variable = EditedSystem.Variables[index];

            if (!variable.Name.StartsWith(SearchText, StringComparison.OrdinalIgnoreCase))
            {

                return;
            }

            var field_name_rect = new Rect(rect.x, rect.y,rect.width * nameFieldWidth, EditorGUIUtility.singleLineHeight);
            var field_value_rect = new Rect(field_name_rect.x + field_name_rect.width + rect.width * nameValueSpaceWidth, rect.y,rect.width * valueFieldWidth,EditorGUIUtility.singleLineHeight);

            if (is_focused)
            {
                variable.Name = EditorGUI.DelayedTextField(field_name_rect, variable.Name);
            }
            else
            {
                EditorGUI.LabelField(field_name_rect, variable.Name);
            }

            if (variable.Type == (typeof(int)))
            {
                variable.Value = EditorGUI.DelayedIntField( field_value_rect,(int) variable.Value);
            }
            else if (variable.Type == (typeof(float)))
            {
                variable.Value = EditorGUI.DelayedFloatField(field_value_rect, (float) variable.Value);
            }
            else if (variable.Type == (typeof(bool)))
            {
                variable.Value = EditorGUI.Toggle(field_value_rect, (bool) variable.Value);
            }
            else if (variable.Type == (typeof(GameObject)))
            {
                variable.Value =
                    (GameObject) EditorGUI.ObjectField(field_value_rect, (Object) variable.Value, variable.Type, false);
            }
            else if (variable.Type == (typeof(string)))
            {
                variable.Value = EditorGUI.DelayedTextField(field_value_rect, (string) variable.Value);
            }
        }

        ///<summary>
            ///<para> Creates a new instance of type T and adds it as a variable to the edited system</para>
            ///<para> For GameObjects please use AddGameObjectVariable</para>
        ///</summary>
        private void AddVariable<T>()
        {
            var type = typeof(T);
            var new_instance = Activator.CreateInstance(type);
            EditedSystem.Variables.Add(new Variable(type, CreateNewVariableName(type), new_instance));
            EditorUtility.SetDirty(EditedSystem);
        }

        private void AddGameObjectVariable()
        {
            var type = typeof(GameObject);
            EditedSystem.Variables.Add(new Variable(type, CreateNewVariableName(type), null));
            EditorUtility.SetDirty(EditedSystem);
        }

        private string CreateNewVariableName(Type type)
        {
            string base_name = $"new {type.Name}";
            string name = base_name;
            int i = 0;
            while (EditedSystem.Variables.Any((x) => x.Name.Equals(name)))
            {
                i++;
                name = $"{base_name} ({i})";
            }

            return name;
        }

        protected virtual void InitializeVariablesView()
        {
            ToolbarMenu = new GenericMenu();
            ToolbarMenu.AddItem(new GUIContent("add int"),false, AddVariable<int>);
            ToolbarMenu.AddItem(new GUIContent("add float"),false, AddVariable<float>);
            ToolbarMenu.AddItem(new GUIContent("add bool"),false, AddVariable<bool>);
            ToolbarMenu.AddItem(new GUIContent("add GameObject"),false, AddGameObjectVariable);

            ToolbarDropdownContent.text = "+";

            reorderableVariablesList = new ReorderableList(
                EditedSystem.Variables,typeof(Variable),
                true,
                false,
                false,
                true
                );
            reorderableVariablesList.drawElementCallback = DrawVariable;
            reorderableVariablesList.headerHeight = 0;
        }
    }

}
