using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Game : MonoBehaviour {

    public Text levelText;
    public Text scoreText;
    public Button buttonCarrot;
    public Button buttonForward;
    public Button buttonPlay;
    public GameObject carrotPrefab;
    public GameObject hillPrefab;
    public GameObject emptyCirclePrefab;
    public GameObject yellowCircle;
    public GameObject firstPage;
    public Button buttonStartGame;

    int level;
    Task[] tasks;
    Playground playground;
    List<RectTransform> allHillsRect;
    RectTransform carrotRect;
    List<RectTransform> allCarrotsRect;
    RectTransform emptyRect;
    List<GameObject> allEmptyRect;
    float canvasWidth;
    RectTransform canvasRect;

    // Use this for initialization
    void Start () {
        level = 1;
        levelText.text = "" + level;
        scoreText.text = "0/0";
        tasks = new Task[]{
            new Task(1, 4, 1),
            new Task(2, 4, 2),
            new Task(3, 5, 2),
            new Task(4, 5, 3),
            new Task(5, 6, 2),
            new Task(6, 6, 4),
            new Task(7, 7, 3),
            new Task(8, 7, 4)
        };
        playground = new Playground(tasks[level + 6]);
        canvasRect = (RectTransform)gameObject.transform;
        carrotRect = (RectTransform)carrotPrefab.transform;
        allHillsRect = new List<RectTransform>();
        allCarrotsRect = new List<RectTransform>();
        allEmptyRect = new List<GameObject>();
        canvasWidth = canvasRect.rect.width;
        yellowCircle.SetActive(false);
        CreatePlayground();
        CreateEmpty();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))    //GetKeyDown - aby to spravilo len raz
        {
            //levelText.text = "" + level++;
            //
            //float width = rt.rect.width;
            //GameObject newCarrot = Instantiate(hillPrefab, new Vector3(0, 300, 0), Quaternion.identity);
            //newCarrot.transform.parent = gameObject.transform; //gameObject - je Canvas lebo script patri canvasu

        }
	}

    public void StartGame()
    {
        Debug.Log("start");
        firstPage.SetActive(false);
    }

    void CreatePlayground()
    {
        RectTransform hillRect = (RectTransform)hillPrefab.transform;
        float hillWidth = hillRect.rect.width;
        int countHills = playground.GetHillsCount();
        float allHillsWidth = countHills * hillWidth;
        float x = ((canvasWidth - allHillsWidth) / 2) + hillWidth/ 2;
        for (int i = 0; i < countHills; i++)
        {
            GameObject hill = Instantiate(hillPrefab, new Vector3(x + (i*hillWidth), 100, 0), Quaternion.identity);
            hill.transform.SetParent(gameObject.transform);
            allHillsRect.Add((RectTransform)hill.transform);
            if(playground.GetPlayground()[i] == 1)
            {
                GameObject carrot = Instantiate(carrotPrefab, new Vector3(x + (i * hillWidth), 120, 0), Quaternion.identity);
                carrot.transform.SetParent(gameObject.transform);
                allCarrotsRect.Add((RectTransform)carrot.transform);
            }
            
        }
        
    }

    void CreateEmpty()
    {
        RectTransform emptyRect = (RectTransform)emptyCirclePrefab.transform;
        float emptyWidth = emptyRect.rect.width;
        int countEmpty = playground.GetSolution().Count;
        float allEmptyWidth = countEmpty * emptyWidth;
        float x = ((canvasWidth - allEmptyWidth) / 2) + emptyWidth / 2;
        for(int i = 0; i < countEmpty; i++)
        {
            GameObject empty = Instantiate(emptyCirclePrefab, new Vector3(x + (i*emptyWidth), 400, 0), Quaternion.identity);
            empty.transform.SetParent(gameObject.transform);
            allEmptyRect.Add(empty);
        }

    }


    class Task
    {
        public int level;
        public int countHill;
        public int countCarrot;

        public Task(int level, int countHill, int countCarrot)
        {
            this.level = level;
            this.countHill = countHill;
            this.countCarrot = countCarrot;
        }
    }

    class Playground
    {
        int[] playground;
        List<char> solution;
        Task task;

        public Playground(Task task)
        {
            this.task = task;
             playground = GeneratePlayground(task.countCarrot, task.countHill);
            solution = GetSolution(playground, task.countCarrot);     
        }

        public int[] GeneratePlayground(int countCarrots, int countHills)
        {
            int[] carrots = new int[countHills - 1];
            for(int i=0; i<countCarrots; i++)
            {
                carrots[i] = 1;
            }
            Shuffle(carrots);
            int[] empty = new int[] { 0 };
            return empty.Concat(carrots).ToArray();

        }

        public void Shuffle(int[] array)
        {
            System.Random _random = new System.Random();
            int p = array.Length;
            for (int n = p - 1; n > 0; n--)
            {
                int r = _random.Next(0, n);
                int t = array[r];
                array[r] = array[n];
                array[n] = t;
            }
        }

        public List<char> GetSolution(int[] playground, int countCarrots)
        {
            List<char> solution = new List<char>();
            int poc = 0;
            int i = 0;
            while (poc < countCarrots){
                solution.Add('D');
                i++;
                if(i < playground.Count() && playground[i] == 1)
                {
                    solution.Add('M');
                    poc++;
                }
            }
            return solution;
        }

        public int GetCountCarrot()
        {
            return this.task.countCarrot;
        }

        public int GetHillsCount()
        {
            return this.task.countHill;
        }

        public int[] GetPlayground()
        {
            return this.playground;
        }

        public List<char> GetSolution()
        {
            return this.solution;
        }
    }





}
