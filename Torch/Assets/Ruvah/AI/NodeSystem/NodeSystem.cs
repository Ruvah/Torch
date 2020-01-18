using System;
using System.Collections;
using System.Collections.Generic;
using Ruvah.AI.NodeSystem.ParameterContainers;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using Object = System.Object;

namespace Ruvah.AI.NodeSystem
{
    [Serializable]
    public class NodeSystem : ScriptableObject
    {
        // -- FIELDS


        public List<BaseNode> NodesList = new List<BaseNode>();
        public List<ParameterContainer> Variables = new List<ParameterContainer>();
    }
}
