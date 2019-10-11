using System.Collections;
using System.Collections.Generic;
using Ruvah.NodeSystem;
using UnityEditor;
using UnityEngine;

namespace Ruvah.AI.Statemachine
{
    public class StateMachineWindow : NodeEditorWindow
    {
        public enum StateMachineContextOptions
        {
            CreateState,
        }

        // -- FIELDS

        // -- METHODS

        //[MenuItem("Tools/Ruvah/StateMachine")]
        static void Init()
        {
            NodeEditorWindow window = (StateMachineWindow) EditorWindow.GetWindow(typeof(StateMachineWindow));
            window.Show();
        }

        public static void Init(StateMachine state_machine)
        {
            EditedSystem = state_machine;
            Init();
        }


        protected override void CreateContextMenu()
        {
            ContextMenu.AddItem(new GUIContent("AddState"), false, ContextMenuOption,
                StateMachineContextOptions.CreateState);
        }

        protected override void ContextMenuOption(object option)
        {
            var selected_option = (StateMachineContextOptions) option;
            switch (selected_option)
            {
                case StateMachineContextOptions.CreateState:
                {
                    CreateState();
                    break;
                }
            }
        }

        private void CreateState()
        {
            var new_state = new StateNode();
            new_state.WindowRect.center = MousePos;
            EditedSystem.NodesList.Add(new_state);
            EditorUtility.SetDirty(EditedSystem);
        }
    }
    
}
