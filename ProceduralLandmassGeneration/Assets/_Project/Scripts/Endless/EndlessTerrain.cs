using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    public const int maxViewDistance = 450;
    public Transform viewer;
    public static Vector2 viewerPosition;
    private int chunkSize;

    Dictionary<Vector2,TerrainChunk> terrainChunkdDictionary;
    private List<TerrainChunk> terrainChunksVisibleLastUpdate;
    private int chunkVisibleInViewDst;
    // Start is called before the first frame update
    void Start()
    {
        chunkSize = MapGenerator.mapChunkSize - 1;
        chunkVisibleInViewDst = Mathf.RoundToInt(maxViewDistance / chunkSize);
        terrainChunkdDictionary = new Dictionary<Vector2, TerrainChunk>();
        terrainChunksVisibleLastUpdate = new List<TerrainChunk>();
    }

    void Update()
    {
        viewerPosition = new Vector2(viewer.position.x,viewer.position.z);
        UpdateVisibleChunk();
    }

    // UpdateTerrainChunk is called once per frame
    void UpdateVisibleChunk()
    {
        foreach (var terrainChunk in terrainChunksVisibleLastUpdate)
        {
            terrainChunk.SetVisible(false);
        }
        terrainChunksVisibleLastUpdate.Clear();
        ;
        int currentChunkCordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);
        for (int yOffset = -chunkVisibleInViewDst; yOffset <= chunkVisibleInViewDst; yOffset++)
        {
            for (int xOffset = -chunkVisibleInViewDst; xOffset <= chunkVisibleInViewDst; xOffset++)
            {
                Vector2 viewedChunkCord = new Vector2(currentChunkCordX + xOffset, currentChunkCordY + yOffset);
                if (terrainChunkdDictionary.ContainsKey(viewedChunkCord))
                {
                    terrainChunkdDictionary[viewedChunkCord].UpdateTerrainChunk();
                    if (terrainChunkdDictionary[viewedChunkCord].IsVisible())
                    {
                        terrainChunksVisibleLastUpdate.Add(terrainChunkdDictionary[viewedChunkCord]);
                    }
                }
                else
                {
                    terrainChunkdDictionary.Add(viewedChunkCord,new TerrainChunk(viewedChunkCord,chunkSize,transform));
                }
            }
        }
    }

    public class TerrainChunk
    {
        private GameObject meshObject;
        private Vector2 position;
        private Bounds bounds;
        public TerrainChunk(Vector2 coord, int size, Transform parent)
        {
            position = coord * size;
            bounds = new Bounds(position,Vector3.one*size);
            Vector3 positionV3 = new Vector3(position.x,0,position.y);
            meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            meshObject.transform.parent = parent;
            meshObject.transform.position = positionV3;
            meshObject.transform.localScale = Vector3.one * size / 10f; //because unity's default plne is 10 in scale
            SetVisible(false);
        }

        public void UpdateTerrainChunk()
        {
            var viewerDistanceFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
            bool visible = viewerDistanceFromNearestEdge <= maxViewDistance;
            SetVisible(visible);
        }

        public void SetVisible(bool isVisible)
        {
            meshObject.SetActive(isVisible);
        }

        public bool IsVisible() => meshObject.activeSelf;
    }
}
