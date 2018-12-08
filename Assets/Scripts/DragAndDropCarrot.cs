using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropCarrot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Game game;
    bool dragging;
    Vector2 originSize;
    RectTransform thisRT;
    int position;

    private void Start()
    {
        thisRT = (RectTransform)transform;
        originSize = thisRT.sizeDelta;
        dragging = false;
        var canvas = GameObject.FindWithTag("Canvas");
        game = canvas.GetComponent<Game>();
    }

    public void SetPosition(int position)
    {
        this.position = position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //thisRT.sizeDelta = originSize;
        //dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        List<GameObject> empties = game.GetEmpties();
        char[] state = game.GetState();
        bool touched = false;
        for (var i = 0; i < empties.Count; i++)
        {
            var empty = empties[i];
            RectTransform emptyRT = (RectTransform)empty.transform;
            if (empty.transform.position.x - 50 < gameObject.transform.position.x && empty.transform.position.x + 50 > gameObject.transform.position.x
                && empty.transform.position.y - 50 < gameObject.transform.position.y && empty.transform.position.y + 50 > gameObject.transform.position.y)
            {
                transform.position = empty.transform.position;
                thisRT.sizeDelta = emptyRT.sizeDelta;
                touched = true;
                if (position != -1)
                {
                    game.RemoveState(position);
                }
                if (state[i] != 'E')
                {
                    game.RemoveCard(i);
                }
                game.AddCard(gameObject, i, 'M');
                position = i;
            }
        }
        if (!touched)
        {
            if (position != -1)
            {
                game.RemoveState(position);
            }
            Destroy(gameObject);
        }
    }
}