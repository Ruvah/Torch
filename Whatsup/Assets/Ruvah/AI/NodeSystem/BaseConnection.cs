using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

namespace Ruvah.NodeSystem
{
    [Serializable]
    public class Transition
    {

    }

    [Serializable]
    public class BaseConnection : NodeObject
    {
        public BaseNode From;
        public BaseNode To;

        protected bool HasMultipleConnections => Transitions.Count > 1;

        private Shader Shader;
        private Material Material;
        private bool IsInitialized;

        private static float TriangleSideLength = 10f;
        private static float LineWidth = 3f;
        private static float ArrowOffset = 5f;

        private Vector2[] Arrow = new Vector2[6];
        private Vector2[] Line = new Vector2[4];
        private List<Transition> Transitions = new List<Transition>();
        private Rect LineRect;

        // -- METHODS


        public void AddTransition()
        {
            Transitions.Add(new Transition());
        }

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
            bool is_in_rect =
                Math.IsPointInTriangle(mouse_pos, Line[0], Line[1], Line[2])
                   || Math.IsPointInTriangle(mouse_pos, Line[3], Line[0], Line[2]);
            bool is_in_triangles =
                Math.IsPointInTriangle(mouse_pos, Arrow[0], Arrow[1], Arrow[2])
                    || (Math.IsPointInTriangle(mouse_pos, Arrow[3], Arrow[4], Arrow[5]) && HasMultipleConnections);

            return is_in_rect || is_in_triangles;
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

            Quaternion rotation = Quaternion.Euler(0,0,angle);

            for (int i = 0; i < (HasMultipleConnections ? 2 : 1); i++)
            {
                Vector2 start_point = from + line * 0.5f;
                start_point += line.normalized * (TriangleSideLength * i);

                int triangle_start_index = i * 3;

                var arrow_point = start_point;
                arrow_point.x += TriangleSideLength;

                var bottom_wing = start_point;
                bottom_wing.x -= TriangleSideLength * 0.5f;
                bottom_wing.y += TriangleSideLength;

                var top_wing = start_point;
                top_wing.x -= TriangleSideLength * 0.5f;
                top_wing.y -= TriangleSideLength;

                Arrow[triangle_start_index] = arrow_point;
                Arrow[triangle_start_index+1] = bottom_wing;
                Arrow[triangle_start_index+2] = top_wing;

                Vector2 middle = new Vector2(
                    ( Arrow[triangle_start_index].x + Arrow[triangle_start_index+1].x + Arrow[triangle_start_index+2].x) / 3f,
                    (Arrow[triangle_start_index].y + Arrow[triangle_start_index+1].y + Arrow[triangle_start_index+2].y) / 3f
                    );
                for (var j = triangle_start_index; j < triangle_start_index+3; j++)
                {
                    var point_rotated_around_origin = Arrow[j] - middle;
                    point_rotated_around_origin = rotation * point_rotated_around_origin;
                    point_rotated_around_origin += middle;
                    Arrow[j] = point_rotated_around_origin;
                }
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
            foreach (var point in Arrow)
            {
                GL.Vertex(point);
            }
            GL.End();
            GL.PopMatrix();
        }
    }
}
