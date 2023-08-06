using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Vector2 roomDimensionsMinMax;
    [SerializeField] Vector2 densityMinMax;
    [SerializeField] float tileSize;
    [SerializeField] int powerUpRoomChance;

    [SerializeField] GameObject gravityHole;

    [SerializeField] GameObject[] powerUps;
    [SerializeField] GameObject powerUpRoom;

    GameObject[] powerUpsSpawned = new GameObject[3];

    bool inPowerUpRoom = false, levelStarted = true;
    public bool powerUpCollected = false;

    List<GameObject> gravityHolesCurrentLevel = new List<GameObject>();
    [SerializeField] List<int> powerUpRoomIndices = new List<int>();

    // Start is called before the first frame update
    RoomGenerator roomGenerator;
    EnemyManager enemyManager;

    GameObject player;
    [SerializeField] GameObject deathScreenUI;
    
    TextMeshPro infoText;

    Color initBackgroundColor;

    bool spawnNewRoom = true;
    int roomIndex = -1;
    [SerializeField] int totalRoomIndex = -1;
    int powerUpRoomListIndex = 0;

    float timeTillLevelStart;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        infoText = GameObject.FindGameObjectWithTag("Info Display").GetComponent<TextMeshPro>();

        roomGenerator = FindAnyObjectByType<RoomGenerator>();
        enemyManager = FindAnyObjectByType<EnemyManager>();

        initBackgroundColor = Camera.main.backgroundColor;

        // Pregenerate all the rooms for this dungeon / level / floor

        // Spawn each room when the time comes

        int numRooms = Random.Range(20, 25);
        roomDimensionsMinMax = new Vector2(5, 10);

        for (int i = 0; i < numRooms; i++)
        {
            if (i % 3 == 0 && i > 0)
            {
                int roomTypeRand = Random.Range(0, 100);
                if (roomTypeRand < 50)
                {
                    powerUpRoomIndices.Add(i);
                    continue;
                }
            }

            int density = (int)Random.Range(densityMinMax.x, densityMinMax.y);

            int baseDimension = (int)Mathf.Lerp(roomDimensionsMinMax.x, roomDimensionsMinMax.y, i / (float)numRooms);
            int roomWidthUnitsCurrentRoom = baseDimension + Random.Range(-1, 2);
            int roomHeightUnitsCurrentRoom = baseDimension + Random.Range(-1, 2);

            int rand = Random.Range(0, 100);
            bool powerUpRoom = false;
            if (rand < powerUpRoomChance) { powerUpRoom = true; }

            roomGenerator.GenerateRoom(density, roomWidthUnitsCurrentRoom, 
                roomHeightUnitsCurrentRoom, tileSize, powerUpRoom);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnNewRoom)
        {
            totalRoomIndex++;
            infoText.text = "";

            if (roomIndex + 1 == roomGenerator.Rooms.Count)
            {
                SceneManager.LoadScene("Win Screen");
            }

            if (Camera.main.backgroundColor != initBackgroundColor)
            {
                Camera.main.backgroundColor = initBackgroundColor;
            }

            // Check if previous room was the power-up room. If so deactivate
            if (inPowerUpRoom) 
            {
                levelStarted = true;

                // Destroy power-ups
                Destroy(powerUpsSpawned[0]);
                Destroy(powerUpsSpawned[1]);
                Destroy(powerUpsSpawned[2]);

                inPowerUpRoom = false;
                powerUpCollected = false;
                powerUpRoom.SetActive(false);
            }
            else if (roomIndex >= 0) 
            // Otherwise previous room was a normal encounter room if it existed
            // Clean everything up
            { 
                roomGenerator.Rooms[roomIndex].SetActive(false);

                // TODO: Instead of adding and clearing - maybe preassign a fixed large size

                // Destroy all holes of this room before switching to the next room
                for(int i = 0; i < gravityHolesCurrentLevel.Count; i++)
                {
                    Destroy(gravityHolesCurrentLevel[i]);
                }
                gravityHolesCurrentLevel.Clear();
            }

            // If room number (index) matches the next power up room index
            // Player goes to PowerUp Room
            if (powerUpRoomListIndex < powerUpRoomIndices.Count)
            {
                if (totalRoomIndex == powerUpRoomIndices[powerUpRoomListIndex])
                {
                    powerUpRoomListIndex++;
                    powerUpRoom.SetActive(true);

                    player.transform.position = Vector3.up * 1.3f;


                    // Spawn 3 random distinct powerups
                    int xPos = -5;
                    List<int> indices = new List<int>() { 0, 1, 2, 3, 4, 5 };
                    for (int i = 0; i < 3; i++)
                    {
                        int rand = Random.Range(0, indices.Count);
                        powerUpsSpawned[i] = 
                            Instantiate(powerUps[indices[rand]], 
                                new Vector3(xPos, 2, 5), 
                                Quaternion.identity);

                        indices.Remove(indices[rand]);
                        xPos += 5;
                    }

                    spawnNewRoom = false;
                    inPowerUpRoom = true;
                    powerUpCollected = false;

                    // Don't execute the rest of this if block
                    return;
                }
            }

            // Otherwise setup for the next encounter room

            // Activate the next room

            // TODO: Use room generator's roomIndex.
            // roomIndex in this class is redundant
            roomGenerator.Rooms[++roomIndex].SetActive(true);

            // Spawn player at the desired spawn position
            player.transform.position = roomGenerator.PlayerSpawnPositions[roomIndex];

            // Build the navmesh for this room
            roomGenerator.Rooms[roomIndex].GetComponent<NavMeshSurface>().BuildNavMesh();

            // Spawn all the enemies
            enemyManager.SpawnEnemies(roomIndex);

            //int curseRand = Random.Range(0, 100);
            //if (curseRand < 20) { enemyManager.ApplyCurse(); }

            // Spawn gravity holes in the required places
            var holePosThislevel = roomGenerator.HolePositions[roomIndex];

            foreach (Vector3 gravityHolePosition in holePosThislevel)
            {
                gravityHolesCurrentLevel.Add(Instantiate(gravityHole, gravityHolePosition, Quaternion.identity));
            }

            float facingAngle = 90.0f;

            // First 4 holes are level boundaries. Adjust their transforms
            for (int i = 0; i < 4; i++)
            {
                Transform gravityHoleTransform = gravityHolesCurrentLevel[i].transform;
                gravityHoleTransform.Rotate(new Vector3(0.0f, facingAngle, 0.0f));

                facingAngle -= 90.0f;

                gravityHoleTransform.localScale =
                    new Vector3(
                        500.0f,
                        gravityHoleTransform.localScale.y,
                        gravityHoleTransform.localScale.z
                    );
            }

            spawnNewRoom = false;
        }

        if (timeTillLevelStart > 0)
        {
            Time.timeScale = 0.1f;
            timeTillLevelStart -= Time.deltaTime;
            return;
        }
        else if(!levelStarted)
        {
            Time.timeScale = 1.0f;
            levelStarted = true;
            spawnNewRoom = true;
        }

        // for debugging only
        if (Input.GetKeyDown(KeyCode.L) && inPowerUpRoom)
        {
            powerUpCollected = true;
        }

        if (inPowerUpRoom)
        {
            if (powerUpCollected) { spawnNewRoom = true; }
        }
        else if (levelStarted && enemyManager.EnemyMap.Count == 0 && !spawnNewRoom) 
        {
            timeTillLevelStart = 0.1f;
            levelStarted = false;
        }

        if (player.GetComponent<CharacterStats>().Hp <=0)
        {
            Destroy(FindObjectOfType<PauseScreenManager>());
            deathScreenUI.SetActive(true);

            // Disable player capabilities
            player.GetComponent<PlayerAttack>().enabled = false;
            player.GetComponent<PlayerDash>().enabled = false;
            player.GetComponent<PlayerInteractionLogic>().enabled = false;
            player.GetComponent<PlayerMovement>().enabled = false;
        }
    }
}
