using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacter : MonoBehaviour
{
    [HideInInspector]
    public bool isGameFinished;
    private Queue<Vector3> path = new Queue<Vector3>();
    private Vector3 targetPosition;
    public float speed = 5;
    public bool isMoving;

    void Awake()
    {
        isGameFinished = false;
        targetPosition = transform.position;
        Camera.main.transform.SetParent(this.transform);
        Camera.main.transform.localPosition = new Vector3(-3f, 8f, -6f);
        Camera.main.transform.localEulerAngles = new Vector3(50f, 28f);
    }

    public void SetPath(Queue<Vector3> path)
    {
        this.path = path;        
    }    

    void Update()
    {
        if (!isGameFinished)
        {
            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
            {
                if (path.Count <= 0)
                {
                    isMoving = false;
                    GameController.instance.SetNextReward();
                    return;
                }
                targetPosition = path.Dequeue();
            }
            isMoving = true;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}
