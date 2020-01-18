using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Ruvah.AI.NodeSystem
{
    [Serializable]
    public class BaseNode : NodeObject
    {
        // -- FIELDS

        public Rect WindowRect = new Rect(0,0,100,50);
        public List<BaseConnection> Connections = new List<BaseConnection>();

        // -- METHODS


        public Vector2 GetMiddle()
        {
            Vector2 middle;
            middle.x = WindowRect.x + WindowRect.width * 0.5f;
            middle.y = WindowRect.yMin + WindowRect.height * 0.5f;

            return middle;
        }

        public Vector2 GetTop()
        {
            Vector2 top;
            top.x = WindowRect.x + WindowRect.width * 0.5f;
            top.y = WindowRect.yMin;

            return top;
        }

        public Vector2 GetBottom()
        {
            Vector2 bottom;
            bottom.x = WindowRect.x + WindowRect.width * 0.5f;
            bottom.y = WindowRect.yMax;

            return bottom;
        }

        public virtual void DrawConnections()
        {
            foreach (var connection in Connections)
            {
                connection.Draw(connection.From.GetBottom(), connection.To.GetTop());
            }
        }

        public virtual void DrawContent(int id)
        {
            GUI.DragWindow();
        }

        public virtual BaseConnection AddConnection(BaseConnection connection)
        {
            if (!TryGetConnection(connection, out var added_connection))
            {
                added_connection = connection;
                Connections.Add(added_connection);
            }
            else
            {
                DestroyImmediate(connection);
            }
            added_connection.AddTransition();
            return added_connection;
        }

        public virtual bool TryGetConnection(BaseConnection connection, out BaseConnection found_connection)
        {
            foreach (var base_connection in Connections)
            {
                if (base_connection.To == connection.To)
                {
                    found_connection = base_connection;
                    return true;
                }
            }

            found_connection = null;
            return false;
        }
    }
}
