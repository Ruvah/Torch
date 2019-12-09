using System;
using System.Collections;
using System.Collections.Generic;
using Ruvah.NodeSystem.ParameterContainers;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using Object = System.Object;

namespace Ruvah.NodeSystem
{
    [Serializable]
    public class NodeSystem : ScriptableObject
    {
        // -- FIELDS


        public List<BaseNode> NodesList = new List<BaseNode>();
        public List<ParameterContainer> Variables = new List<ParameterContainer>();
    }
}
