using System;
using UnityEditor;
using UnityEngine;

// :HACK: not clean IMO, hack to have all these types stored in the same container AND HAVE THEM SERIALIZABLE
// System.Object is not serializable by Unity
namespace Ruvah.AI.NodeSystem.ParameterContainers
{
    [Serializable]
    public class ParameterContainer : ScriptableObject
    {
    }
}

