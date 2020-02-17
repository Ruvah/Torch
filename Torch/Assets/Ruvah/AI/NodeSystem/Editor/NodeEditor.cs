using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Ruvah.AI.NodeSystem;
using UnityEditor;
using UnityEngine;

public static class NodeEditor
{
    public static void CreateBaseConnection(BaseNode from, BaseNode to)
    {
        var connection = (BaseConnection) ScriptableObject.CreateInstance(typeof(BaseConnection));
        connection.From = from;
        connection.To = to;

    }
}
