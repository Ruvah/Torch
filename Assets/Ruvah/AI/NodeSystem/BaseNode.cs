﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Ruvah.NodeSystem
{
    [Serializable]
    public abstract class BaseNode : NodeObject
    {
        // -- FIELDS
        
        public Rect WindowRect = new Rect(0,0,200,100);
        public string WindowTitle;
        public List<BaseConnection> FromConnections = new List<BaseConnection>();
        public List<BaseConnection> ToConnections = new List<BaseConnection>();

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
            Vector2 bottom;
            bottom.x = WindowRect.x + WindowRect.width * 0.5f;
            bottom.y = WindowRect.yMin;

            return bottom;
        }
        
        public Vector2 GetBottom()
        {
            Vector2 top;
            top.x = WindowRect.x + WindowRect.width * 0.5f;
            top.y = WindowRect.yMax;

            return top;
        }

        public virtual void DrawConnections()
        {
            foreach (var connection in FromConnections)
            {
                connection.Draw(connection.From.GetBottom(), connection.To.GetTop()); 
            }
        }

        public virtual void DrawWindow(int id)
        {
            GUI.DragWindow();
        }

        public virtual void OnClicked(Vector2 mouse_pos)
        {
            
        }

        public virtual void AddConnection(BaseConnection connection)
        {
            if (connection.From == this)
            {
                FromConnections.Add(connection);
            }
            else
            {
                ToConnections.Add(connection);
            }
        }


    }
}
