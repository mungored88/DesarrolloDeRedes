using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    public Grid GridReference;
    public Transform StartPosition;
    public Transform TargetPosition;
    [SerializeField] private List<Player> PlayerList = new List<Player>();

    //public Player.NotifyPlayerDie _notifiPlayerDie;
    
    private void OnEnable()
    {
        Player.PlayerLost += RemovePlayerFromList;
    }

    private void OnDisable()
    {
        Player.PlayerLost -= RemovePlayerFromList;
    }

    public List<Node> EnemyPath;

    private void Start()
    {
        //Player.PlayerLost += RemovePlayerFromList;
        PlayerList = FindObjectsOfType<Player>().ToList();
        GridReference = FindObjectOfType<Grid>();
    }


    private void Update()
    {
        if (PlayerList.Count < 2)
        {
            PlayerList = FindObjectsOfType<Player>().ToList();
        }
        // if the scene has no players, then do nothing
        if (PhotonNetwork.PlayerList.Length == 0) return;
        
        
        SetNearestPlayer();
        try
        {
            EnemyPath = FindPath(StartPosition.position, TargetPosition.position);
        }
        catch (NullReferenceException)
        {
            // Do nothing. Skip to the next frame update
        }
        catch (UnassignedReferenceException)
        {
            // Do nothing. Skip to the next frame update
        }
    }
    private void SetNearestPlayer()
    {
        try
        {
            if (PhotonNetwork.PlayerList.Length == 1)
            {
                TargetPosition = PlayerList[0].transform;
            }
            else
            {
                // TODO: mejorar la seleccion del Target
                if (Vector3.Distance(transform.position, PlayerList[0].transform.position) <
                    Vector3.Distance(transform.position, PlayerList[1].transform.position))
                {
                    TargetPosition = PlayerList[0].transform;
                }
                else
                {
                    if (Vector3.Distance(transform.position, PlayerList[1].transform.position) <
                        Vector3.Distance(transform.position, PlayerList[2].transform.position))
                    {
                        TargetPosition = PlayerList[1].transform;
                    }
                    else
                    {
                        TargetPosition = PlayerList[2].transform;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (ex is MissingReferenceException || ex is ArgumentOutOfRangeException)
            {
                PlayerList = FindObjectsOfType<Player>().ToList();
                //SetNearestPlayer();
            }
        }
    }

    public void RemovePlayerFromList(Player T) 
    {
        if (PlayerList.Contains(T))
        {
            PlayerList.Remove(T);
        }
    }

    List<Node> FindPath(Vector3 a_StartPos, Vector3 a_TargetPos)
    {
        Node StartNode = Grid.instance.NodeFromWorldPoint(a_StartPos);
        Node TargetNode = Grid.instance.NodeFromWorldPoint(a_TargetPos);

        List<Node> OpenList = new List<Node>();
        List<Node> FinalPath = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(StartNode);

        while(OpenList.Count > 0)
        {
            Node CurrentNode = OpenList[0];
            for(int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].ihCost < CurrentNode.ihCost)
                {
                    CurrentNode = OpenList[i];
                }
            }
            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if (CurrentNode == TargetNode)
            {
                FinalPath = GetFinalPath(StartNode, TargetNode);
            }

            foreach (Node NeighborNode in Grid.instance.GetNeighboringNodes(CurrentNode))
            {
                if (!NeighborNode.bIsWall || ClosedList.Contains(NeighborNode))
                {
                    continue;
                }
                int MoveCost = CurrentNode.igCost + GetManhattenDistance(CurrentNode, NeighborNode);

                if (MoveCost < NeighborNode.igCost || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.igCost = MoveCost;
                    NeighborNode.ihCost = GetManhattenDistance(NeighborNode, TargetNode);
                    NeighborNode.ParentNode = CurrentNode;

                    if(!OpenList.Contains(NeighborNode))
                    {
                        OpenList.Add(NeighborNode);
                    }
                }
            }

        }

        return FinalPath;
    }



    List<Node> GetFinalPath(Node a_StartingNode, Node a_EndNode)
    {
        List<Node> FinalPath = new List<Node>();
        Node CurrentNode = a_EndNode;

        while(CurrentNode != a_StartingNode)
        {
            FinalPath.Add(CurrentNode);
            CurrentNode = CurrentNode.ParentNode;
        }

        FinalPath.Reverse();

        Grid.instance.FinalPath = FinalPath;

        return FinalPath;
    }

    int GetManhattenDistance(Node a_nodeA, Node a_nodeB)
    {
        int ix = Mathf.Abs(a_nodeA.iGridX - a_nodeB.iGridX);
        int iy = Mathf.Abs(a_nodeA.iGridY - a_nodeB.iGridY);

        return ix + iy;
    }
   
}
