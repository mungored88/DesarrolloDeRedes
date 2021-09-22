using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    public Grid GridReference;
    public Transform StartPosition;
    public Transform TargetPosition;
    private List<Transform> PlayerList = new List<Transform>();

    private void OnEnable()
    {
        Player.NotifyPlayerDie += RemovePlayerToList;
    }

    public List<Node> EnemyPath;


    public void Awake()
    {
        PlayerList = FindObjectsOfType<Player>().Select(player1 => player1.transform).ToList();
    }
    private void Start()
    {
        GridReference = GetComponent<Grid>();
    }


    private void Update()
    {
        GetNearestPlay();


        EnemyPath = FindPath(StartPosition.position, TargetPosition.position);

    }
    private void  GetNearestPlay()
    {
        if (PlayerList.Count == 1)
        {
            TargetPosition = PlayerList[0];
        }
        else
        {
            if (Vector3.Distance(transform.position, PlayerList[0].position) < Vector3.Distance(transform.position, PlayerList[1].position))
            {
                TargetPosition = PlayerList[0];
            }
            else
            {
                TargetPosition = PlayerList[1];
            }
        }
    }

    public void RemovePlayerToList(Player T) 
    { 

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
