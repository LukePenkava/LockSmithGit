using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Camera cam;
    public GameObject levelPrefab;
    public Transform levelsParent;
   
    float scrollSpeed = 0.2f;
    float totalDelta = 0f;
    float levelStep = 3.0f;

    float scrollVelocity;
    float smoothedScroll = 0f;

    List<LevelObject> levels = new List<LevelObject>();
    int currentLevel = 0;



    void Start()
    {
        List<LevelObject> data = SaveSystem.LoadData();
        if(data != null)
        {
            levels = data;
        }
       
        CreateLevels();  
    }


    void Update()
    {
        GetInput();
    }


    void GetInput()
    {
        float scrollValue = 0f;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                totalDelta = 0f;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                float delta = touch.deltaPosition.y;

                //Used for recognizing swipe from tap
                totalDelta += Mathf.Abs(delta); 

                scrollValue = delta * Time.deltaTime * scrollSpeed;               

            }
            if (touch.phase == TouchPhase.Ended)
            {
                //Dont register selection if player was swiping
                if (Mathf.Abs(totalDelta) < 1.0f)
                {
                    RaycastHit hit;
                    Ray ray = cam.ScreenPointToRay(touch.position);

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.transform.tag == "Level")
                        {
                            hit.collider.gameObject.GetComponent<LevelButton>().Selected();
                        }
                    }
                }
            }
        }

        //Smoothed Scrolling, has to be outside of touch to have the smoothed effect even after player lifts finger, so it slows down
        smoothedScroll = Mathf.SmoothDamp(smoothedScroll, scrollValue, ref scrollVelocity, 0.15f);

        //Limit swiping, so that player does not swipe under first level ( posY = 0 is limit, cant be higher value )
        float movedPos = levelsParent.transform.localPosition.y + smoothedScroll;

        //Limit movement by checking how much is the scroll delta over the limit and remove the delta which is over the limit from current delta. This way new delta always adjusted to be just enough to hit the limit
        //Ie there is no hard limit setting or cutting off, which should prevent hard positiong setting
        if (movedPos > 0f)
        {
            smoothedScroll -= movedPos;
        }

        float lowLimit = ((levels.Count - 1) * levelStep * -1f);
        if (movedPos < lowLimit)
        {
            float dif = movedPos - lowLimit;
            smoothedScroll -= dif;
        }

        levelsParent.transform.Translate(Vector3.up * smoothedScroll);

    }


    void CreateLevels()
    {
        //Amount of fnished levels
        int finishedLevels = 0; 

        //Count all finished levels
        for (int i = 0; i < levels.Count; i++)
        {
            //print("i " + i + " Level " + levels[i].level + " Finished " + levels[i].finished);

            if (levels[i].finished)
            {
                finishedLevels++;
            }
        }

        currentLevel = finishedLevels;

        //Always have +2 levels to last finished one, current one and next one player will playe after visible, to see there is something after current level
        int levelsToCreate = 2 - (levels.Count - finishedLevels);
        //Have to store before, or the index would change in the loop as objects are added and level index would be messed up
        int levelsCount = levels.Count; 

        for (int i = 0; i < levelsToCreate; i++)
        {
            LevelObject levelObj = CreateLevel(levelsCount + i); 
            levels.Add(levelObj);
        }

        SaveSystem.SaveData(levels);

      
        //Create actual level game objects
        for (int i = 0; i < levels.Count; i++)
        {   
            GameObject levelGO = Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);
            levelGO.name = "Level" + i.ToString();
            levelGO.transform.parent = levelsParent;
            levelGO.transform.localPosition = new Vector3(0, i * levelStep, 0);

            levelGO.GetComponent<LevelButton>().Init(levels[i], this);      
        }

        SetCurrentLevel();
    }

    LevelObject CreateLevel(int levelIndex)
    {
        LevelObject levelObj = new LevelObject();

        //First Level
        if (levelIndex == 0)
        {
            levelObj.level = levelIndex;
            levelObj.type = Helper.Types[0];
            levelObj.numbersUsed = 4;
            levelObj.combinationLength = 4;
            levelObj.timeStep = 3.0f;
        }
        else
        {
            levelObj.level = levelIndex;
            levelObj.type = GetRandomType();
            levelObj.numbersUsed = Random.Range(Helper.minNumbers, Helper.maxNumbers);
            levelObj.combinationLength = Random.Range(Helper.minCombinationLength, Helper.maxCombinationLength);
            levelObj.timeStep = Random.Range(Helper.minTimeStep, Helper.maxTimeStep);
        }

        return levelObj;
    }

    string GetRandomType()
    {
        //Game_Manager.Type[] types = (Game_Manager.Type[])System.Enum.GetValues(typeof(Game_Manager.Type));
        int typeLength = Helper.Types.Length;
        int rnd = Random.Range(0, typeLength);
        string rndType = Helper.Types[rnd];

        print(rndType);

        return rndType;
    }

    //Set scroll position on current level
    void SetCurrentLevel()
    {
        float pos = (currentLevel) * levelStep * -1f;
        levelsParent.transform.localPosition = new Vector3(0, pos, 0);
    }

    //Called from level button when tapped
    public void SelectLevel(LevelObject levelData)
    {
        if (levelData.finished) { return; }
        if(levelData.level != currentLevel) { return; }

        PlayerPrefs.SetInt("Level", levelData.level);
        SceneManager.LoadScene("GameScene");
    }

    public void DeleteSaves()
    {
        SaveSystem.DeleteData();
    }

}
