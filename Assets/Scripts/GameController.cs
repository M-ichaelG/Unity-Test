using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int score;
    public UIController uiController;
    public GameField gameField;
    public AstarPathFinding aStarPF;

    private AICharacter aiCharacter;
    private List<Reward> rewards;
    private int rewardIndex;


    private static GameController s_Instance = null;
    public static GameController instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(GameController)) as GameController;
                if (s_Instance == null)
                    Debug.LogWarning("Could not locate a GameController object. \n You only can have exactly one GameController object in the scene.");
            }
            return s_Instance;
        }
    }

    void Start()
    {
        gameField.InitGameField(64, 64);
        rewards = new List<Reward>();
        rewardIndex = 0;

        int blockCount = 128;

        while(blockCount > 0)
        {
            int rdX = Random.Range(0, 64);
            int rdY = Random.Range(0, 64);
            if (gameField.IsCellBlocked(rdX, rdY))
                continue;

            gameField.BlockCell(rdX, rdY);
            blockCount--;
        }

        int rewardCount = 16;

        while(rewardCount> 0)
        {
            int rdX = Random.Range(0, 64);
            int rdY = Random.Range(0, 64);

            if (gameField.IsCellBlocked(rdX, rdY))
                continue;

            rewards.Add (gameField.CreateReward(rdX, rdY));
            rewardCount--;
        }

        gameField.InitAICharacter(0, 0);


        score = 0;

        Reward firstReward = gameField.CreateReward(6, 9);

        Vector3 rewardPosition = firstReward.transform.position;

        aiCharacter = FindObjectOfType<AICharacter>();
        if (aiCharacter == null)
            Debug.LogError("aiCharacter is null");
        Queue<Vector3> queuePath = aStarPF.GetPath(aiCharacter.transform.position, rewardPosition);

        aiCharacter.SetPath(queuePath);
    }

    public void IncrementScore ()
    {
        score++;
        uiController.SetScore(score);
    }

    public void SetNextReward ()
    {
        if (rewardIndex > 15)
        {
            uiController.DisplayWinText();
            aiCharacter.isGameFinished = true;
            return;
        }
        Vector3 rewardPosition = rewards[rewardIndex].transform.position;
        Queue<Vector3> queuePath = aStarPF.GetPath(aiCharacter.transform.position, rewardPosition);
        aiCharacter.SetPath(queuePath);
        rewardIndex += 1;
    }
}
