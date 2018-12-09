using System;
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

    float time;
    char[] state;
    //List<GameObject> allEmpty;
    //GameObject[] allCarrots;
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

    void Start()
    {
        startAnimate = false;
        cardD = false;
        cardM = false;
        cardE = false;
    }

    void Update()
    {
        if (startAnimate)
        {
            if (yellowIndex < state.Length)
            {
                if (!cardD && state[yellowIndex] == 'D')
                {
                    cardD = true;
                    time = 0f;
                    animator.SetBool("jump", true);
                    rabbitPos++;
                }
                else if (!cardM && state[yellowIndex] == 'M')
                {
                    cardM = true;
                    time = 0f;
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
                    time = 0f;
                    AnimateYellow(yellowIndex + 1);
                }

                if (cardD)
                {
                    if (time >= 1.971448f && time < 2f)
                    {
                        animator.SetBool("jump", false);
                        AnimateYellow(yellowIndex + 1);
                    }
                    else if (time >= 3f && time < 3.1f)
                    {
                        yellowIndex++;
                        cardD = false;
                    }
                    if (animator.GetBool("jump"))
                    {
                        gameObject.transform.Translate(Vector3.right * Time.deltaTime * 72f);
                    }
                }

                if (cardM)
                {
                    time += Time.deltaTime;
                    if (time >= 0.9f && time < 1f)
                    {
                        yellowIndex++;
                        cardM = false;
                    }
                }

                if (cardE)
                {
                    time += Time.deltaTime;
                    if (time >= 0.9f && time < 1f)
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
        yellow.transform.position = game.GetAllEmptyRect()[index].transform.position;
        yellow.transform.SetAsLastSibling();
        GameObject currentCard = game.GetAllPutCards()[index];
        if (currentCard != null)
        {
            currentCard.transform.SetAsLastSibling();
        }
    }

    public void StartAnimation()
    {
        if (!startAnimate)
        {
            game = canvas.GetComponent<Game>();
            state = game.GetState();
            //state = new char[5] { 'D', 'D', 'D', 'D', 'D' };
            //allEmpty = game.GetAllEmptyRect();
            yellowIndex = 0;
            time = 0f;
            carrots = 0;
            //allCarrots = game.getCarrots();
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
        for (int i = 0; i < solution.Count; i++)
        {
            if (solution[i] != state[i])
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
        for (int i = 0; i < playground.Length; i++)
        {
            if (playground[i] == 1)
            {
                index++;
            }
            if (i == position)
            {
                return index;
            }
        }
        return -1;
    }

    void RemoveCarrot(int rabbitPos)
    {
        int index = GetIndexOfCarrot(rabbitPos);
        if (index > -1)
        {
            game.RemoveCarrot(index);
        }
    }


}