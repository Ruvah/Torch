using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ruvah.NodeSystem
{
    public partial class NodeEditorWindow
    {
        // -- FIELDS


        private string SearchText = string.Empty;
        private GUILayoutOption ToolbarDropdownWidth = GUILayout.Width(20);
        private GUIContent ToolbarDropdownContent = new GUIContent();
        private GenericMenu ToolbarMenu;
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
            VariablesScrollPosition = EditorGUILayout.BeginScrollView(VariablesScrollPosition);
            {
                foreach (var variable in EditedSystem.Variables)
                {
                    DrawVariable(variable);
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawVariable(Variable variable)
        {
            EditorGUILayout.BeginHorizontal();
            {
                variable.Name = EditorGUILayout.DelayedTextField(variable.Name);
                if (variable.Type == (typeof(int)))
                {
                    variable.Value = EditorGUILayout.DelayedIntField((int) variable.Value);
                }
                else if (variable.Type == (typeof(float)))
                {
                    variable.Value = EditorGUILayout.DelayedFloatField((float) variable.Value);
                }
                else if (variable.Type == (typeof(bool)))
                {
                    variable.Value = EditorGUILayout.Toggle((bool) variable.Value);
                }
                else if (variable.Type == (typeof(GameObject)))
                {
                    variable.Value = (GameObject) EditorGUILayout.ObjectField((Object) variable.Value, variable.Type, false);
                }
                else if (variable.Type == (typeof(string)))
                {
                    variable.Value = EditorGUILayout.DelayedTextField((string) variable.Value);
                }
            }
            EditorGUILayout.EndHorizontal();
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
        }

        private void AddGameObjectVariable()
        {
            var type = typeof(GameObject);
            EditedSystem.Variables.Add(new Variable(type, CreateNewVariableName(type), null));
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
        }
    }

}
