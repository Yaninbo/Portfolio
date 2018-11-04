public class BoardManager : MonoBehaviour
{
    //This script randomly setups the play area of Space plunderer at the beginning of each new round.
    //Count class for easier setting of minimum and maximum values.
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    //Tile ammounts are set throuh inspector.
    public int m_columns = 17;                  //Integer value of how many colums play area will have.
    public int m_rows = 10;                     //Integer value of how many rows play area will have.
    public int m_possibleBoardPlacements;       //Integer value of how many tiles play area will have all together.

    public int m_resourceAmount;                //Integer value of how many resource area tiles play area will have.
    public int m_emptyAmount;                   //Integer value of how many empty tiles play area will have.
    public int m_trasureAmount;                 //Integer value of how many treasure tiles play area will have.
    public int m_enemy1Amount;                  //Integer value of how many type 1 enemies play area will have.
    public int m_enemy2Amount;                  //Integer value of how many type 2 enemies play area will have.
    public int m_enemy3Amount;                  //Integer value of how many type 3 enemies play area will have.
    public int m_enemy4Amount;                  //Integer value of how many type 4 enemies play area will have.
    public int m_enemy5Amount;                  //Integer value of how many type 5 enemies play area will have.
    public int m_sRRAmount;                     //Integer value of how many short radar power ups play area will have.
    public int m_lRR3Amount;                    //Integer value of how many long radar power ups play area will have.
    public int m_kitAmount;                     //Integer value of how many health kit supplies play area will have.
    public int m_ammoAmount;                    //Integer value of how many ammo crate supplies play area will have.
    public int m_hazardAmount;                  //Integer value of how many hazard tiles play area will have.

    public GameObject m_placeholder;            //Gameobject where new board will be built.

    public GameObject m_wallTiles;              //Wall tile prefab. Surrounds play area so that player can't get outside of the play area.
    public GameObject m_emptyTiles;             //Empty tile prefab.
    public GameObject m_startTile;              //Start tile prefab.
    public GameObject[] m_resourceTiles;        //Array of resourse tile prefabs.
    public GameObject m_tresureTile;            //Treasure tile prefab.
    public GameObject m_sRRTile;                //Short radar tile prefab.
    public GameObject m_lRRTile;                //Long radar tile prefab.
    public GameObject m_kitTile;                //Health tile prefab.
    public GameObject m_ammoTile;               //Ammo tile prefab.
    public GameObject m_hazardTile;             //Hazard tile prefab.

    public GameObject m_enemy1Tile;             //Enemy 1 tile prefab. Space pirates.
    public GameObject m_enemy2Tile;             //Enemy 2 tile prefab. Phantom ukko.
    public GameObject m_enemy3Tile;             //Enemy 3 tile prefab. Otsonian droid.
    public GameObject m_enemy4Tile;             //Enemy 4 tile prefab. Grob cube.
    public GameObject m_enemy5Tile;             //Enemy 5 tile prefab. Galactic offender.

    //List of gameboard grid positions.
    private List<Vector3> gridPositions = new List<Vector3>();

    //When game starts setup gameboard.
    void Start()
    {
        SetupScene();
    }

    //Clear any old data and add new grid positions.
    void InitialiseList()
    {
        gridPositions.Clear();

        for (int x = 1; x < m_columns - 1; x++)
        {
            for (int y = 1; y < m_rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    //Set boards placement in scene and wall off outer perimiter of board.
    void BoardSetup()
    {
        for (int x = -1; x < m_columns + 1; x++)
        {
            for (int y = -1; y < m_rows + 1; y++)
            {
                GameObject toInstantiate = m_placeholder.transform;
                if (x == -1 || x == m_columns || y == -1 || y == m_rows)
                    toInstantiate = m_wallTiles[Random.Range(0, m_wallTiles.Length)];

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(m_boardHolder);
            }
        }
    }

    //Get random free position from list and then remove it from list after it has been filled.
    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    //Method to randomize array of prefabs at random amounts between minimum and maximum.
    void LayoutObjectArrayAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum);
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoise = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoise, randomPosition, Quaternion.identity);
        }
    }

    //Method to randomize position for single type of tile that has specific ammount.
    void LayoutObjectAtRandom(GameObject tile, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoise = tile;
            Instantiate(tileChoise, randomPosition, Quaternion.identity);
        }
    }

    //Setupping play area.
    public void SetupScene()
    {
        //Calculate how many emptytiles there are going to be.
        m_emptyAmount = m_possibleBoardPlacements - (m_resourceAmount + m_enemy1Amount + m_enemy2Amount + m_enemy3Amount + m_enemy4Amount + m_enemy5Amount + m_trasureAmount + m_lRR3Amount + m_sRRAmount + m_kitAmount + m_ammoAmount + m_hazardAmount + m_tresureAmount+1);
        //Setup board and create list of possible tile positions.
        BoardSetup();
        InitialiseList();
        //Layout objects at random on the board one tile type at a time.
        LayoutObjectArrayAtRandom(m_resourceTiles, m_resourceAmount, m_resourceAmount);
        LayoutObjectAtRandom(m_startTile, 1);
        GameObject.Find("PlayerExploration").GetComponent<PlayerExploration>().GameStart();
        LayoutObjectAtRandom(m_enemy1Tile, m_enemy1Amount);
        LayoutObjectAtRandom(m_enemy2Tile, m_enemy2Amount);
        LayoutObjectAtRandom(m_enemy3Tile, m_enemy3Amount);
        LayoutObjectAtRandom(m_enemy4Tile, m_enemy4Amount);
        LayoutObjectAtRandom(m_enemy5Tile, m_enemy5Amount);
        LayoutObjectAtRandom(m_clueTile, m_trasureAmount);
        LayoutObjectAtRandom(m_tresureTile, m_tresureAmount);
        LayoutObjectAtRandom(m_sRRTile, m_sRRAmount);
        LayoutObjectAtRandom(m_lRRTile, m_lRR3Amount);
        LayoutObjectAtRandom(m_kitTile, m_kitAmount);
        LayoutObjectAtRandom(m_ammoTile, m_ammoAmount);
        LayoutObjectAtRandom(m_hazardTile, m_hazardAmount);

        //Fill remaining grid positions with empty tiles.
        LayoutObjectAtRandom(m_emptyTiles, m_emptyAmount);
    }
}

