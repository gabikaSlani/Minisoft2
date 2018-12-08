using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RabbitMove : MonoBehaviour
{

    public Animator animator;
    public GameObject yellow;
    public Canvas canvas;
    public Text scoreText;
    public GameObject succesMW;
    public GameObject failMW;

    int frame;
    char[] state;
    List<GameObject> allEmpty;
    GameObject[] allCarrots;
    Game game;
    int yellowIndex;
    List<char> solution;
    int[] playground;
    int carrots;
    bool startAnimate;
    bool cardD;
    bool cardM;
    bool cardE;
    int rabbitPos;

    // Use this for initialization
    void Start()
    {
        startAnimate = false;
        cardD = false;
        cardM = false;
        cardE = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startAnimate)
        {
            if (yellowIndex < state.Length)
            {
                if (!cardD && state[yellowIndex] == 'D')
                {
                    cardD = true;
                    frame = 0;
                    animator.SetBool("jump", true);
                    rabbitPos++;
                }
                else if (!cardM && state[yellowIndex] == 'M')
                {
                    cardM = true;
                    frame = 0;
                    if (rabbitPos < playground.Length && playground[rabbitPos] == 1)
                    {
                        RemoveCarrot(rabbitPos);
                        IncreaseScore();
                    }
                    AnimateYellow(yellowIndex + 1);

                }
                else if (!cardE && state[yellowIndex] == 'E')
                {
                    cardE = true;
                    frame = 0;
                    AnimateYellow(yellowIndex + 1);
                }

                if (cardD)
                {
                    frame++;
                    if (frame == 119)
                    {
                        animator.SetBool("jump", false);
                        AnimateYellow(yellowIndex + 1);
                    }
                    if (frame == 200)
                    {
                        yellowIndex++;
                        cardD = false;
                    }
                    if (animator.GetBool("jump"))
                    {
                        gameObject.transform.position = new Vector3(
                            gameObject.transform.position.x + 1.18f,
                            gameObject.transform.position.y,
                            gameObject.transform.position.z);
                    }
                }

                if (cardM)
                {
                    frame++;
                    if (frame == 60)
                    {
                        yellowIndex++;
                        cardM = false;
                    }
                }

                if (cardE)
                {
                    frame++;
                    if (frame == 60)
                    {
                        yellowIndex++;
                        cardE = false;
                    }
                }
            }
            else
            {
                startAnimate = false;
                if (SolutionIsGood())
                {
                    game.ShowSuccessModalWindow();
                    succesMW.transform.SetAsLastSibling();
                }
                else
                {
                    game.ShowFailModalWindow();
                    failMW.transform.SetAsLastSibling();
                }
            }
        }
    }

    public void AnimateYellow(int index)
    {
        yellow.transform.position = allEmpty[index].transform.position;
        yellow.transform.SetAsLastSibling();
        game.GetAllPutCards()[index].transform.SetAsLastSibling();
    }

    public void StartAnimation()
    {
        if (!startAnimate)
        {
            game = canvas.GetComponent<Game>();
            state = game.GetState();
            //state = new char[5] { 'D', 'D', 'D', 'D', 'D' };
            allEmpty = game.GetAllEmptyRect();
            yellowIndex = 0;
            frame = 0;
            carrots = 0;
            allCarrots = game.getCarrots();
            solution = game.GetSolution();
            rabbitPos = 0;
            playground = game.GetPlayground();
            AnimateYellow(yellowIndex);
            yellow.SetActive(true);
            startAnimate = true;
        }
    }

    public bool SolutionIsGood()
    {
        for(int i=0; i < solution.Count; i++)
        {
            if(solution[i] != state[i])
            {
                return false;
            }
        }
        return true;
    }

    void IncreaseScore()
    {
        carrots++;
        string vsetky = game.GetCountCarrots().ToString();
        scoreText.text = carrots.ToString() + "/" + vsetky;
    }

    int GetIndexOfCarrot(int position)
    {
        int index = -1;
        for (int i=0; i < playground.Length; i++)
        {
            if(playground[i] == 1)
            {
                index++;
            }
            if(i == position)
            {
                return index;
            }
        }
        return -1;
    }

    void RemoveCarrot(int rabbitPos)
    {
        int index = GetIndexOfCarrot(rabbitPos);
        if (index > -1) {      
            GameObject carrot = allCarrots[index];
            allCarrots[index] = null;
            Destroy(carrot);
        }
    }
}