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
        private Shader Shader;
        private Material Material;
        private bool IsInitialized;

        private static float TriangleSideLength = 10f;
        private static float LineWidth = 3f;

        private Vector2[] Arrow = new Vector2[3];
        private Vector2[] Line = new Vector2[4];

        // -- METHODS
        
        
        public void Draw(Vector2 from, Vector2 to)
        {
            if (!IsInitialized)
            {
                Initialize();
            }
            
            var half_width = LineWidth * 0.5f;
            Vector2 line = to - from;
            float angle = Mathf.Atan(line.y / line.x);
            angle = Mathf.Rad2Deg * angle + 90;

            Quaternion rotation = Quaternion.Euler(0,0,angle);
            
            var left = (Vector2)(rotation * (Vector2.left * half_width));
            var right = (Vector2)(rotation * (Vector2.right * half_width));

            Line[0] = to + left;
            Line[1] = from + left;
            Line[2] = from + right;
            Line[3] = to + right;
           
            GL.PushMatrix();
            Material.SetPass(0);
            GL.LoadPixelMatrix();
            GL.Color(Color.white);
            GL.Begin(GL.QUADS);
            GL.Vertex(Line[0]);
            GL.Vertex(Line[1]);
            GL.Vertex(Line[2]);
            GL.Vertex(Line[3]);
            GL.End();
            GL.PopMatrix();

            DrawArrowHead(from, to);
        }

        public void DrawToMouse(Vector2 from, Vector2 mouse_pos)
        {
            Draw(from, mouse_pos);
        }

        public bool Contains(Vector2 mouse_pos)
        {
            var rect = new Rectangle2D(Line);
            return rect.Contains(mouse_pos);
        }

        public void Initialize()
        {
            Shader = Shader.Find("Hidden/Internal-Colored");
            Material = new Material(Shader);
            Material.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            Material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            Material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            Material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            Material.SetInt("_ZWrite", 0);
            IsInitialized = true;
        }

        private void CalculateArrowHead(Vector2 from, Vector2 to)
        {
            Vector2 line = to - from;

            float angle = Mathf.Atan(line.y / line.x);
            angle = Mathf.Rad2Deg * angle;
            if (from.x > to.x)
            {
                angle += 180;
            }

            Vector2 line_middle = (from + to) * 0.5f;

            var arrow_point = line_middle;
            arrow_point.x += TriangleSideLength;

            var bottom_wing = line_middle;
            bottom_wing.x -= TriangleSideLength * 0.5f;
            bottom_wing.y += TriangleSideLength;

            var top_wing = line_middle;
            top_wing.x -= TriangleSideLength * 0.5f;
            top_wing.y -= TriangleSideLength;

            Arrow[0] = arrow_point;
            Arrow[1] = bottom_wing;
            Arrow[2] = top_wing;
            
            Quaternion rotation = Quaternion.Euler(0,0,angle);
            Vector2 middle = new Vector2((Arrow[0].x + Arrow[1].x + Arrow[2].x) / 3f,(Arrow[0].y + Arrow[1].y + Arrow[2].y) / 3f );
            for (var i = 0; i < Arrow.Length; i++)
            {
                var point_rotated_around_origin = Arrow[i] - middle;
                point_rotated_around_origin = rotation * point_rotated_around_origin;
                point_rotated_around_origin += middle;
                Arrow[i] = point_rotated_around_origin;
            }
        }

        private void DrawArrowHead(Vector2 from, Vector2 to)
        {
            CalculateArrowHead(from, to);
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
