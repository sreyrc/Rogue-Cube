using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] Material floorMaterial;

    List<MeshFilter> sourceMeshFilters = new List<MeshFilter>();
    List<GameObject> tempTileGameObjects = new List<GameObject>();

    List<GameObject> rooms = new List<GameObject>();
    List<GameObject> powerUpRooms = new List<GameObject>(); 

    Dictionary<int, List<Vector3>> gravityHolePositions =
    new Dictionary<int, List<Vector3>>();

    Dictionary<int, Vector3> playerSpawnPositions =
        new Dictionary<int, Vector3>();

    MeshFilter targetMeshFilter;
    bool playerSpawnPositionSet = false;
    int roomNumber = -1;

    public ref List<GameObject> Rooms { get { return ref rooms; } }

    public ref Dictionary<int, List<Vector3>> HolePositions
    {
        get { return ref gravityHolePositions; }
    }

    public ref Dictionary<int, Vector3> PlayerSpawnPositions
    {
        get { return ref playerSpawnPositions; }
    }

    public void GenerateRoom(
        int density, 
        int roomWidthUnits, 
        int roomHeightUnits, 
        float tileSize, 
        bool powerUpRoom)
    {
        roomNumber++;

        // Step 1: Lay Tiles

        float roomHalfWidth = (roomWidthUnits * tileSize) / 2.0f;
        float roomHalfHeight = (roomHeightUnits * tileSize) / 2.0f;

        float tileHalfSize = tileSize / 2.0f; 

        Vector2 pos = new Vector2(-roomHalfWidth + tileHalfSize, roomHalfHeight - tileHalfSize);

        gravityHolePositions.Add(roomNumber, new List<Vector3>());

        // First 4 in the list will be positions of holes surrounding the level
        // Order: West -> South -> East -> North
        gravityHolePositions[roomNumber].Add(new Vector3(-roomHalfWidth - tileHalfSize, 1.0f, 0.0f));
        gravityHolePositions[roomNumber].Add(new Vector3(0.0f, 1.0f, -roomHalfHeight - tileHalfSize));
        gravityHolePositions[roomNumber].Add(new Vector3(roomHalfWidth + tileHalfSize, 1.0f, 0.0f));
        gravityHolePositions[roomNumber].Add(new Vector3(0.0f, 1.0f, roomHalfHeight + tileHalfSize));

        for (int i = 1; i <= roomHeightUnits; i++)
        {
            for (int j = 1; j <= roomWidthUnits; j++)
            {
                int rand = Random.Range(0, 100);

                // TODO: Remove repetitive code
                if (rand < density)
                {
                    var tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    tile.transform.position = new Vector3(pos.x, 0, pos.y);
                    tile.transform.localScale = new Vector3(tileSize, 1.0f, tileSize);
                    
                    tempTileGameObjects.Add(tile);
                    sourceMeshFilters.Add(tile.GetComponent<MeshFilter>());

                    if(!playerSpawnPositionSet)
                    {
                        playerSpawnPositions.Add(roomNumber, tile.transform.position + 1.3f * Vector3.up);
                        playerSpawnPositionSet = true;
                    }
                }
                else
                {
                    gravityHolePositions[roomNumber].Add(new Vector3(pos.x, 1.0f, pos.y));
                }
                pos.x += tileSize;
            }
            pos.x -= (2 * roomHalfWidth);
            pos.y -= tileSize;
        }

        GameObject newRoom = new GameObject();

        newRoom.name = "Room " + roomNumber.ToString();

        targetMeshFilter = newRoom.AddComponent<MeshFilter>();
        newRoom.AddComponent<MeshRenderer>();

        // Step 2: Combine Tile Meshes Into One Mesh

        var combine = new CombineInstance[sourceMeshFilters.Count];

        // Go through each source mesh filter and add them to the combine array
        for (int i = 0; i < sourceMeshFilters.Count; i++)
        {
            combine[i].mesh = sourceMeshFilters[i].mesh;
            combine[i].transform = sourceMeshFilters[i].transform.localToWorldMatrix;
        }

        // Combine all meshes
        var mesh = new Mesh();
        mesh.CombineMeshes(combine);
        targetMeshFilter.mesh = mesh;

        for(int i = 0; i < tempTileGameObjects.Count; i++)
        {
            Destroy(tempTileGameObjects[i]);
        }

        newRoom.AddComponent<NavMeshSurface>();
        newRoom.GetComponent<MeshRenderer>().material = floorMaterial;

        sourceMeshFilters.Clear();
        tempTileGameObjects.Clear();

        playerSpawnPositionSet = false;

        newRoom.SetActive(false);

        rooms.Add(newRoom);
    }
}
