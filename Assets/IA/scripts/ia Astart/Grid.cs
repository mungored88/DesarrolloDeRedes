using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public static Grid instance;
    public LayerMask ObstacleMask;
    public Vector2 vGridWorldSize;
    public float fNodeRadius;
    public float fDistanceBetweenNodes;

    public Node[,] NodeArray;
    public List<Node> FinalPath;

   public float fNodeDiameter;

    public float fnotediametermulti;
    int iGridSizeX, iGridSizeY;


    private void Awake()
    {
        instance = this;
        fNodeDiameter = fNodeRadius * fnotediametermulti;
        iGridSizeX = Mathf.RoundToInt(vGridWorldSize.x / fNodeDiameter);
        iGridSizeY = Mathf.RoundToInt(vGridWorldSize.y / fNodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        NodeArray = new Node[iGridSizeX, iGridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * vGridWorldSize.x / 2 - Vector3.forward * vGridWorldSize.y / 2;
        for (int x = 0; x < iGridSizeX; x++)
        {
            for (int y = 0; y < iGridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * fNodeDiameter + fNodeRadius) + Vector3.forward * (y * fNodeDiameter + fNodeRadius);
                bool Wall = true;


                if (Physics.CheckSphere(worldPoint, fNodeRadius, ObstacleMask))
                {
                    Wall = false;
                }

                NodeArray[x, y] = new Node(Wall, worldPoint, x, y);
            }
        }
    }


    public List<Node> GetNeighboringNodes(Node a_NeighborNode)
    {
        List<Node> NeighborList = new List<Node>();
        int icheckX;
        int icheckY;


        icheckX = a_NeighborNode.iGridX + 1;
        icheckY = a_NeighborNode.iGridY;
        if (icheckX >= 0 && icheckX < iGridSizeX)
        {
            if (icheckY >= 0 && icheckY < iGridSizeY)
            {
                NeighborList.Add(NodeArray[icheckX, icheckY]);
            }
        }

        icheckX = a_NeighborNode.iGridX - 1;
        icheckY = a_NeighborNode.iGridY;
        if (icheckX >= 0 && icheckX < iGridSizeX)
        {
            if (icheckY >= 0 && icheckY < iGridSizeY)
            {
                NeighborList.Add(NodeArray[icheckX, icheckY]);
            }
        }

        icheckX = a_NeighborNode.iGridX;
        icheckY = a_NeighborNode.iGridY + 1;
        if (icheckX >= 0 && icheckX < iGridSizeX)
        {
            if (icheckY >= 0 && icheckY < iGridSizeY)
            {
                NeighborList.Add(NodeArray[icheckX, icheckY]);
            }
        }

        icheckX = a_NeighborNode.iGridX;
        icheckY = a_NeighborNode.iGridY - 1;
        if (icheckX >= 0 && icheckX < iGridSizeX)
        {
            if (icheckY >= 0 && icheckY < iGridSizeY)
            {
                NeighborList.Add(NodeArray[icheckX, icheckY]);
            }
        }

        return NeighborList;
    }


    public Node NodeFromWorldPoint(Vector3 a_vWorldPos)
    {
        int posX = 0;
        int posY = 0;
        float minDistance = Vector3.Distance(NodeArray[posX, posY].vPosition, a_vWorldPos);

        for (int x = 1; x < NodeArray.GetLength(0); x++)
        {
            for (int y = 1; y < NodeArray.GetLength(1); y++)
            {
                var actualDist = Vector3.Distance(NodeArray[x, y].vPosition, a_vWorldPos);
                if (actualDist < minDistance)
                {
                    posX = x;
                    posY = y;
                    minDistance = actualDist;
                }
            }
        }

        return NodeArray[posX, posY];
    }



    private void OnDrawGizmos()
    {

        Gizmos.DrawWireCube(transform.position, new Vector3(vGridWorldSize.x, 1, vGridWorldSize.y));

        if (NodeArray != null)
        {
            foreach (Node n in NodeArray)
            {
                if (n.bIsWall)
                {
                    Gizmos.color = Color.blue;
                }
                else
                {
                    Gizmos.color = Color.white;
                }


                if (FinalPath != null)
                {
                    if (FinalPath.Contains(n))
                    {
                        Gizmos.color = Color.red;
                    }

                }

                Gizmos.DrawCube(n.vPosition, Vector3.one * (fNodeDiameter - fDistanceBetweenNodes));
            }
        }
    }
}
