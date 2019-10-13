using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ruvah.NodeSystem
{
    [Serializable]
    public class NodeSystem : ScriptableObject
    {
        public List<BaseNode> NodesList = new List<BaseNode>();
    }
}
