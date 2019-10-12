using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ruvah.NodeSystem
{
    public class BaseConnection : NodeObject
    {
        public BaseNode From;
        public BaseNode To;

        private static float TriangleSideLength = 5f;
        private static Shader Shader = Shader.Find("Hidden/Internal-Colored");
        private Material Material = new Material(Shader);
        private Vector2[] Triangle = new Vector2[3];

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

        private void CreateTriangle()
        {
            Vector2 middle = (From.GetBottom() + To.GetTop()) * 0.5f;
        }
    }
}
