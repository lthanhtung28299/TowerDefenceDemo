using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TileNode : MonoBehaviour
{
    [SerializeField] private GameObject buildGo, debugGo, towerBuildGo, selectGo;
    [SerializeField] private TMP_Text txtDebugPosition;
    public Node Node { get; private set; }


    public void Init(Node node)
    {
        Node = node;
        UpdateViewTileNode();
    }

    public void SelectNode(bool isSelected)
    {
        if (Node == null) return;
        if (Node.isSelected == isSelected) return;
        Node.isSelected = isSelected;
        selectGo.SetActive(isSelected);
    }

    public void SetNodeCanBuild()
    {
        if (Node == null) return;
        Node.canBuild = true;
        UpdateViewTileNode();
    }

    public void SetBuiltNode(bool isBuilt)
    {
        if (Node == null) return;
        Node.hasBeenBuilt = isBuilt;
        UpdateViewTileNode();
    }

    private void UpdateViewTileNode()
    {
        buildGo.SetActive(Node.canBuild);
        towerBuildGo.SetActive(false);
        debugGo.SetActive(GameSceneManager.Instance.IsTesting);
        if (GameSceneManager.Instance.IsTesting) txtDebugPosition.text = Node.coordinates.ToString();
    }

    public void ResetTileNode()
    {
        Node.hasBeenBuilt = false;
        Node.isWalkable = false;
        buildGo.Hide();
        debugGo.Hide();
        towerBuildGo.Hide();
        selectGo.Hide();
    }
}

[Serializable]
public class Node
{
    public Vector2Int coordinates;
    public bool isWalkable;
    public bool canBuild;
    [HideInInspector] public bool hasBeenBuilt;
    [HideInInspector] public bool isSelected;

    public Node(Node node)
    {
        coordinates = node.coordinates;
        isWalkable = node.isWalkable;
        canBuild = node.canBuild;
        isSelected = node.isSelected;
        hasBeenBuilt = false;
    }

    public Node(Vector2Int coordinates, bool isWalkable)
    {
        this.coordinates = coordinates;
        this.isWalkable = isWalkable;
    }
}