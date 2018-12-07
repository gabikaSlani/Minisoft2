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

    int frame;
    char[] state;
    List<GameObject> allEmpty;
    Game game;
    int yellowIndex;
    List<char> solution;
    int[] playground;
    int carrots;

    // Use this for initialization
    void Start()
    {
        //len kvoli tomu ze ked bezi update na zaciatku ani nepadal na NullReferenceException a OutOfRangeException
        state = new char[1] { 'E' };
        yellowIndex = -1;
    }

    // Update is called once per frame
    void Update()
    {
        //kvoli animacii a posunu zaja
        frame++;
        //ked sa doanimuje zajac
        if (frame == 155)
        {
            animator.SetBool("jump", false);
            Debug.Log("konci animacia, yellowIndex=" + yellowIndex);
            AnimateYellow(yellowIndex++);
        }
        //ked je animacia v stave jump - zajac sa hybe
        if (animator.GetBool("jump"))
        {
            gameObject.transform.position = new Vector3(
                gameObject.transform.position.x + 0.88f,
                gameObject.transform.position.y,
                gameObject.transform.position.z);
        }

    }

    public void AnimateRabbit()
    {
        animator.SetBool("jump", true);
        frame = 0;
    }

    public void AnimateYellow(int index)
    {
        yellow.transform.position = allEmpty[index].transform.position;
        yellow.transform.SetAsLastSibling();
    }

    public void StartAnimation()
    {
        game = canvas.GetComponent<Game>();
        //state = game.GetState();
        allEmpty = game.GetAllEmptyRect();
        yellowIndex = 0;
        frame = 0;
        carrots = 0;
        solution = game.GetSolution();
        state = solution.ToArray();
        playground = game.GetPlayground();
        AnimateYellow(yellowIndex);
        yellow.SetActive(true);
        while (yellowIndex < solution.Count)
        {
            if (state[yellowIndex] == 'D')
            {
                AnimateRabbit();
                Debug.Log("nasla som D animujem zajaca");
            }
            else if (state[yellowIndex] == 'M')
            {
                if (playground[yellowIndex] == 1)
                {
                    scoreText.text = (carrots++).ToString() + "/" + game.GetCountCarrots().ToString();
                    Debug.Log("nasla som M, menim skore");
                }
                AnimateYellow(yellowIndex++);
            }
            else
            {
                AnimateYellow(yellowIndex++);
            }
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
}