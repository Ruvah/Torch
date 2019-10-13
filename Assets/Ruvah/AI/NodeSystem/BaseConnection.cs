using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ruvah.NodeSystem
{
    [Serializable]
    public class BaseConnection : NodeObject
    {
        public BaseNode From;
        public BaseNode To;

        private static float TriangleSideLength = 10f;
        private static Shader Shader = Shader.Find("Hidden/Internal-Colored");
        private Material Material = new Material(Shader);
        private Vector2[] Arrow = new Vector2[3];

        public BaseConnection()
        {
            Material.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            Material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            Material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            Material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            Material.SetInt("_ZWrite", 0);
        }
        
        public void Apply()
        {
            From.AddConnection(this);
            To.AddConnection(this);
        }
        
        public void Draw()
        {

            GL.PushMatrix();
            Material.SetPass(0);
            GL.LoadPixelMatrix();
            GL.Color(Color.white);
            GL.Begin(GL.LINES);
            GL.Vertex(From.GetBottom());
            GL.Vertex(To.GetTop());
            GL.End();
            GL.PopMatrix();

            DrawTriangle();
        }

        public void DrawToMouse(Vector2 mouse_pos)
        {
            GL.PushMatrix();
            Material.SetPass(0);
            GL.LoadPixelMatrix();
            GL.Color(Color.white);
            GL.Begin(GL.LINES);
            GL.Vertex(From.GetBottom());
            GL.Vertex(mouse_pos);
            GL.End();
            GL.PopMatrix();
        }

        private void CreateArrow()
        {
            Vector2 middle = (From.GetBottom() + To.GetTop()) * 0.5f;

            var point = middle;
            point.x += TriangleSideLength;

            var bottom_wing = middle;
            bottom_wing.x -= TriangleSideLength * 0.5f;
            bottom_wing.y += TriangleSideLength;

            var top_wing = middle;
            top_wing.x -= TriangleSideLength * 0.5f;
            top_wing.y -= TriangleSideLength;

            Arrow[0] = point;
            Arrow[1] = bottom_wing;
            Arrow[2] = top_wing;
        }

        private void DrawTriangle()
        {
            CreateArrow();
            GL.PushMatrix();
            Material.SetPass(0);
            GL.LoadPixelMatrix();
            GL.Color(Color.white);
            GL.Begin(GL.TRIANGLES);
            for (int i = 0; i < Arrow.Length; i++)
            {
                GL.Vertex(Arrow[i]);
            }
            GL.End();
            GL.PopMatrix();
        }
    }
}
