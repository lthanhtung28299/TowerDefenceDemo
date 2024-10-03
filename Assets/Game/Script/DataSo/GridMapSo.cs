using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Script", menuName = "Data/GripMapData")]
public class GridMapSo : ScriptableObject
{
    [field: SerializeField] public List<GridMapInfo> MapInfos { get; private set; } = new List<GridMapInfo>();

    public static GridMapSo GetInstance()
    {
        return DataHolder.GetInstance().GetData<GridMapSo>();
    }

    public void SaveGridMapData(GridMapInfo gridMapInfo)
    {
        var gridMap = MapInfos.Find(s => s.mapId == gridMapInfo.mapId);
        if (gridMap != null)
        {
            gridMap.UpdateGridMapData(gridMapInfo);
        }
        else
        {
            MapInfos.Add(gridMapInfo);
        }
    }
}

[Serializable]
public class GridMapInfo
{
    public string mapId;
    public Vector2Int gridSize;
    public List<Node> gridNodes = new List<Node>();
    public List<WalkablePathInfo> walkablePaths = new List<WalkablePathInfo>();

    public GridMapInfo()
    {
    }

    public GridMapInfo(string mapId, Vector2Int gridSize,List<Node> gridNodes, List<WalkablePathInfo> walkablePaths)
    {
        this.mapId = mapId;
        this.gridSize = gridSize;
        this.walkablePaths.Clear();
        foreach (var walkablePath in walkablePaths)
        {
            this.walkablePaths.Add(new WalkablePathInfo(walkablePath));    
        }

        foreach (var gridNode in gridNodes)
        {
            this.gridNodes.Add(new Node(gridNode));
        }
    }

    public GridMapInfo(GridMapInfo gridMapInfo)
    {
        mapId = gridMapInfo.mapId;
        gridSize = gridMapInfo.gridSize;
        walkablePaths.Clear();
        foreach (var walkablePathInfo in gridMapInfo.walkablePaths)
        {
            walkablePaths.Add(new WalkablePathInfo(walkablePathInfo));    
        }
        gridNodes.Clear();
        foreach (var gridNode in gridMapInfo.gridNodes)
        {
            gridNodes.Add(new Node(gridNode));
        }
    }

    public void UpdateGridMapData(GridMapInfo gridMapInfo)
    {
        gridSize = gridMapInfo.gridSize;
        walkablePaths = gridMapInfo.walkablePaths;
        walkablePaths.Clear();
        foreach (var walkablePathInfo in gridMapInfo.walkablePaths)
        {
            walkablePaths.Add(new WalkablePathInfo(walkablePathInfo));    
        }
        gridNodes.Clear();
        foreach (var gridNode in gridMapInfo.gridNodes)
        {
            gridNodes.Add(new Node(gridNode));
        }
    }
}

[Serializable]
public class WalkablePathInfo
{
    public List<Vector2Int> pathNodesCoordinate = new List<Vector2Int>();

    public WalkablePathInfo()
    {
    }

    public WalkablePathInfo(WalkablePathInfo walkablePathInfo)
    {
        pathNodesCoordinate.Clear();
        foreach (var pathNode in walkablePathInfo.pathNodesCoordinate)
        {
            pathNodesCoordinate.Add(new Vector2Int(pathNode.x, pathNode.y));   
        }
    }

    public WalkablePathInfo(List<Vector2Int> pathNodesCoordinate)
    {
        this.pathNodesCoordinate.Clear();
        foreach (var pathNode in pathNodesCoordinate)
        {
            this.pathNodesCoordinate.Add(new Vector2Int(pathNode.x, pathNode.y));   
        }
    }
}