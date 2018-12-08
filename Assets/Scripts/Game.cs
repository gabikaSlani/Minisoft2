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
    public GameObject forwardCardPrefab;
    public GameObject carrotCardPrefab;
    public GameObject yellowCircle;
    public GameObject successModalWindow;
    public GameObject failModalWindow;
    public GameObject rabbit;

    public Button showSuccessModal;
    public Button showFailModal;

    int level;
    Task[] tasks;
    Playground playground;
    List<GameObject> allHills;
    RectTransform carrotRect;
    List<GameObject> allCarrots;
    RectTransform emptyRect;
    List<GameObject> allEmptyRect;
    GameObject[] allPutCards;
    float canvasWidth;
    RectTransform canvasRect;
    char[] state;

    // Use this for initialization
    void Start() {
        level = 1;
        levelText.text = level + "";
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
        playground = new Playground(tasks[level]);
        canvasRect = (RectTransform)gameObject.transform;
        carrotRect = (RectTransform)carrotPrefab.transform;
        allHills = new List<GameObject>();
        allCarrots = new List<GameObject>();
        allEmptyRect = new List<GameObject>();
        canvasWidth = canvasRect.rect.width;
        yellowCircle.SetActive(false);
        successModalWindow.SetActive(false);
        failModalWindow.SetActive(false);
        scoreText.text = "0/" + tasks[level].countCarrot.ToString();
        CreatePlayground();
        CreateEmpty();
        //state v takejto forme lebo ho taham cez getter - zatial som nepotrebovala, testovala som na solution
        //state = new char[4] { 'D', 'D', 'M', 'D'};
        rabbit.transform.position = new Vector3(allHills[0].transform.position.x, allHills[0].transform.position.y + 58, 0);
        rabbit.transform.SetAsLastSibling();

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))    //GetKeyDown - aby to spravilo len raz
        {
            //levelText.text = "" + level++;
            //
            //float width = rt.rect.width;
            //GameObject newCarrot = Instantiate(hillPrefab, new Vector3(0, 300, 0), Quaternion.identity);
            //newCarrot.transform.parent = gameObject.transform; //gameObject - je Canvas lebo script patri canvasu

        }
    }

    void CreatePlayground()
    {
        RectTransform hillRect = (RectTransform)hillPrefab.transform;
        float hillWidth = hillRect.rect.width;
        int countHills = playground.GetHillsCount();
        float allHillsWidth = countHills * hillWidth;
        float x = ((canvasWidth - allHillsWidth) / 2) + hillWidth / 2;
        for (int i = 0; i < countHills; i++)
        {
            GameObject hill = Instantiate(hillPrefab, new Vector3(x + (i * hillWidth), 100, 0), Quaternion.identity);
            hill.transform.SetParent(gameObject.transform);
            allHills.Add(hill);
            if (playground.GetPlayground()[i] == 1)
            {
                GameObject carrot = Instantiate(carrotPrefab, new Vector3(x + (i * hillWidth), 120, 0), Quaternion.identity);
                carrot.transform.SetParent(gameObject.transform);
                //allCarrotsRect.Add((RectTransform)carrot.transform);
                allCarrots.Add(carrot);
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
        for (int i = 0; i < countEmpty; i++)
        {
            GameObject empty = Instantiate(emptyCirclePrefab, new Vector3(x + (i * emptyWidth), 400, 0), Quaternion.identity);
            empty.transform.SetParent(gameObject.transform);
            allEmptyRect.Add(empty);
        }
        allPutCards = new GameObject[countEmpty];
        state = new char[countEmpty];
        for (int i = 0; i < countEmpty; i++)
        {
            state[i] = 'E';
        }

    }

    public void onClickForwardButton()
    {
        int lastEmpty = GetLastEmptyIndex();
        if (lastEmpty == -1)
        {
            return;
        }
        Vector3 pos = allEmptyRect[lastEmpty].transform.position;
        GameObject forward = Instantiate(forwardCardPrefab, pos, Quaternion.identity);
        RectTransform forwardRect = (RectTransform)forward.transform;
        RectTransform empty = (RectTransform)allEmptyRect[lastEmpty].transform;
        forward.transform.SetParent(gameObject.transform);
        forwardRect.sizeDelta = empty.sizeDelta;
        AddCard(forward, lastEmpty, 'D');
        forward.SendMessage("SetPosition", lastEmpty);
    }

    public void onClickCarrotButton()
    {
        int lastEmpty = GetLastEmptyIndex();
        if (lastEmpty == -1)
        {
            return;
        }
        Vector3 pos = allEmptyRect[lastEmpty].transform.position;
        GameObject carrot = Instantiate(carrotCardPrefab, pos, Quaternion.identity);
        RectTransform carrotRect = (RectTransform)carrot.transform;
        RectTransform empty = (RectTransform)allEmptyRect[lastEmpty].transform;
        carrot.transform.SetParent(gameObject.transform);
        Vector2 originalSize = carrotRect.sizeDelta;
        carrotRect.sizeDelta = empty.sizeDelta;
        AddCard(carrot, lastEmpty, 'M');
        carrot.SendMessage("SetPosition", lastEmpty);
    }

    public void AddCard(GameObject card, int index, char type)
    {
        allPutCards[index] = card;
        state[index] = type;
    }

    public void RemoveCard(int index)
    {
        Destroy(allPutCards[index]);
        allPutCards[index] = null;
        RemoveState(index);
    }

    public char[] GetState()
    {
        return state;
    }

    public void RemoveState(int index)
    {
        state[index] = 'E';
    }

    private int GetLastEmptyIndex()
    {
        bool bolo = false;
        for (int i = state.Length - 1; i >= 0; i--)
        {
            if (state[i] != 'E')
            {
                bolo = true;
            }
            else if (state[i] == 'E' && (i == 0 || state[i - 1] != 'E') && !bolo)
            {
                return i;
            }
        }
        return -1;
    }

    public void ShowSuccessModalWindow()
    {
        this.successModalWindow.SetActive(true);
    }

    public void ShowFailModalWindow()
    {
        this.failModalWindow.SetActive(true);
    }

    public List<GameObject> GetAllEmptyRect()
    {
        return allEmptyRect;
    }

    public List<GameObject> GetEmpties()
    {
        return allEmptyRect;
    }

    public List<char> GetSolution()
    {
        return playground.GetSolution();
    }

    public int[] GetPlayground()
    {
        return playground.GetPlayground();
    }

    public int GetCountCarrots()
    {
        return playground.GetCountCarrot();
    }

    public GameObject[] getCarrots()
    {
        return allCarrots.ToArray();
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
