using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using MIConvexHull;
using Poly2Tri;
using Prefabs.Obstacles;
using UnityEditor;
using Utilities;

namespace Prefabs.Person.Soldier
{
    public class Soldier : Person
    {
        public MeshFilter sightMeshFilter;
        
        private List<Vector3> intersections;
        
        private readonly HashSet<Vector3> vertices = new HashSet<Vector3>();

        private int _obstacleLayerMask;

        protected override void Start()
        {
            base.Start();

            foreach (var o in GameObject.FindGameObjectsWithTag(TagManager.Obstacle))
            {
                MeshFilter meshFilter = o.GetComponentInChildren<MeshFilter>();

                foreach (var sharedMeshVertex in meshFilter.mesh.vertices)
                {
               
                    Vector3 vertex = meshFilter.transform.TransformPoint(sharedMeshVertex);
                    vertices.Add(new Vector3(vertex.x, 0f, vertex.z));
                }
            }

            _obstacleLayerMask = 1 << LayerMask.NameToLayer(LayerManager.Obstacle);
        }

        protected override void Update()
        {
            base.Update();

            /*
             *  Get all intersection points by using RayCasts to all obstacle vertices
             */
            intersections = new List<Vector3>();

            foreach (var vertex in vertices)
            {
                Vector3 middlePosition = new Vector3(transform.position.x, 0f, transform.position.z);

                RaycastHit hit;
                Vector3 straightDirection = vertex - middlePosition;

                Vector3 slightLeftDirection = Quaternion.AngleAxis(-1f, Vector3.up) * straightDirection;
                Vector3 slightRightDirection = Quaternion.AngleAxis(1f, Vector3.up) * straightDirection;

                Vector3[] directions = {slightLeftDirection, slightRightDirection};

                // Does the ray intersect with the edge
                if (Physics.Raycast(middlePosition, straightDirection, out hit, Mathf.Infinity,
                    _obstacleLayerMask))
                {
                    intersections.Add(hit.point);

                    // Check if there is an intersection slightly left/right. Handles collisions with obstacles outside of windows
                    foreach (var direction in directions)
                    {
                        // Does the ray intersect any objects excluding the player layer
                        if (Physics.Raycast(middlePosition, direction, out hit, Mathf.Infinity, _obstacleLayerMask))
                        {
                            intersections.Add(hit.point);
                        }
                    }
                }
            }

            /*
             * Sort intersection points clockwise
             */
            intersections.Sort((i1, i2) => IsClockwise(i1, i2, transform.position));
            
            /*
             * Convert from world positions to local positions
             */
            List<Vector3> localIntersections = new List<Vector3>();

            foreach (var intersection in intersections)
            {
                localIntersections.Add(intersection - transform.position);
            }

            /*
             * Triangulate
             */
            Vector2[] triangulatorPoints = new Vector2[localIntersections.Count];
            for (int localIntersectionIndex = 0;
                localIntersectionIndex < localIntersections.Count;
                localIntersectionIndex++)
            {
                Vector3 localIntersection = localIntersections[localIntersectionIndex];
                triangulatorPoints[localIntersectionIndex] = new Vector2(localIntersection.x, localIntersection.z);
            }

            Triangulator triangulator = new Triangulator(triangulatorPoints);
            int[] triangles = triangulator.Triangulate();

            /*
             * Create mesh
             */
            Mesh newMesh = new Mesh();

            newMesh.SetVertices(localIntersections);
            newMesh.SetTriangles(triangles, 0);

            newMesh.RecalculateBounds();
            newMesh.RecalculateNormals();
        
            sightMeshFilter.mesh = newMesh;
        }
        
        void OnDrawGizmos()
        {
            int i = 0;
            foreach (var intersection in intersections)
            {
                Handles.Label(intersection, i.ToString());
                i++;
            }
        }
        
        private static int IsClockwise(Vector3 first, Vector3 second, Vector3 origin)
        {
            if (first == second)
                return 0;

            Vector3 firstOffset = first - origin;
            Vector3 secondOffset = second - origin;

            float angle1 = Mathf.Atan2(firstOffset.x, firstOffset.z);
            float angle2 = Mathf.Atan2(secondOffset.x, secondOffset.z);

            if (angle1 < angle2)
                return 1;

            if (angle1 > angle2)
                return -1;

            // Check to see which point is closest
            return (firstOffset.sqrMagnitude < secondOffset.sqrMagnitude) ? -1 : 1;
        }
    }
}