using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ruvah.AI.Statemachine
{
    [CreateAssetMenu(menuName = "Ruvah/AI/Statemachine", fileName = "StateMachine")]
    public class StateMachine : ScriptableObject
    {
        public List<StateNode> NodesList = new List<StateNode>();



    }
}
