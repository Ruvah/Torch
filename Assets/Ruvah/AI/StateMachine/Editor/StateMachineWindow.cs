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

        private static StateMachine StateMachine;

        // -- METHODS

        [MenuItem("Tools/Ruvah/StateMachine")]
        static void Init()
        {
            NodeEditorWindow window = (StateMachineWindow) EditorWindow.GetWindow(typeof(StateMachineWindow));
            window.Show();
        }

        static void Init(StateMachine state_machine)
        {
            StateMachine = state_machine;
            Init();
        }


        protected override void CreateContextMenu()
        {
            ContextMenu.AddItem(new GUIContent("CreateTask"), false, ContextMenuOption,
                StateMachineContextOptions.CreateState);

        }

        protected override void ContextMenuOption(object option)
        {
            var selected_option = (StateMachineContextOptions) option;
            switch (selected_option)
            {
                case StateMachineContextOptions.CreateState:
                {
                    break;
                }
            }
        }
    }
    
}
