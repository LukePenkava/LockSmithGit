using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.Analytics;


public class MenuManager : MonoBehaviour
{
    Ads_Manager adsManager;

    public Camera cam;
    public GameObject levelPrefab;
    public Transform levelsParent;
    public GameObject futureLevelPrefab;
    public GameObject linePrefab;

    float targetY = 0f;
    float scrollSpeed = 0.4f;
    float totalDelta = 0f;
    float levelStep = 4.0f;

    
    float scrollVelocity;
    float smoothedScroll = 0f;

    public Sprite[] futureLevelSprites;

    List<LevelObject> levels = new List<LevelObject>();
    int currentLevel = 0;
    public int CurrentLevel
    {
        get { return currentLevel; }
    }

    private RewardedAd rewardedAd;

    void Start()
    {
        GameObject adsManagerGO = GameObject.FindGameObjectWithTag("AdManager");

        if(adsManagerGO)
        {
            adsManager = adsManagerGO.GetComponent<Ads_Manager>();
        }


        Init();
    }  

    void Init()
    {
        foreach(Transform lvl in levelsParent)
        {
            Destroy(lvl.gameObject);
        }

        levels.Clear();

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
        else
        {
            float mod = levelsParent.transform.localPosition.y % levelStep;

            if (Mathf.Abs(mod) > levelStep / 2f)
            {
                mod = (levelStep * -1) - mod;
                scrollValue = mod * Time.deltaTime * scrollSpeed * 20f;
            }
            else
            {
                scrollValue = mod * -1f * Time.deltaTime * scrollSpeed * 20f;
            }
        }

        //Smoothed Scrolling, has to be outside of touch to have the smoothed effect even after player lifts finger, so it slows down
        smoothedScroll = Mathf.SmoothDamp(smoothedScroll, scrollValue, ref scrollVelocity, 0.35f);
        //smoothedScroll = Mathf.Lerp(smoothedScroll, scrollValue, 1.0f * Time.deltaTime);
        if(Mathf.Abs(smoothedScroll) > Mathf.Abs(scrollValue))
        {
            smoothedScroll = scrollValue;
        }        

        //Limit swiping, so that player does not swipe under first level ( posY = 0 is limit, cant be higher value )
        float movedPos = levelsParent.transform.localPosition.y + smoothedScroll;

        //Limit movement by checking how much is the scroll delta over the limit and remove the delta which is over the limit from current delta. This way new delta always adjusted to be just enough to hit the limit
        //Ie there is no hard limit setting or cutting off, which should prevent hard positiong setting
        if (movedPos > 0f)
        {
            smoothedScroll -= movedPos;
        }

        float lowLimit = (( (levels.Count + 3) - 1) * levelStep * -1f);
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
        int levelsToCreate = 1 - (levels.Count - finishedLevels);
        //Have to store before, or the index would change in the loop as objects are added and level index would be messed up
        int levelsCount = levels.Count; 

        for (int i = 0; i < levelsToCreate; i++)
        {
            LevelObject levelObj = CreateLevelData(levelsCount + i); 
            levels.Add(levelObj);
        }

        SaveSystem.SaveData(levels);

      
        //Create actual level game objects
        for (int i = 0; i < levels.Count; i++)
        {   
            GameObject levelGO = Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);
            levelGO.name = "Level" + i.ToString();
            levelGO.transform.parent = levelsParent;
            levelGO.transform.localPosition = new Vector3(0, i * levelStep, -0.2f);

            levelGO.GetComponent<LevelButton>().Init(levels[i], this);      
        }

        int tempI = 0;
        for(int i = levels.Count; i < levels.Count + 2; i++)
        {
            GameObject futureLevel = Instantiate(futureLevelPrefab, Vector3.zero, Quaternion.identity);
            futureLevel.name = "FutureLevel" + i.ToString();
            futureLevel.transform.parent = levelsParent;
            futureLevel.transform.localPosition = new Vector3(0, i * levelStep, -0.2f);
            futureLevel.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = futureLevelSprites[tempI];
            tempI++;
        }

        /*
        for(int i = 0; i < levels.Count + 2; i++)
        {
            GameObject lineGO = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
            lineGO.name = "Line" + i.ToString();
            lineGO.transform.parent = levelsParent;
            lineGO.transform.localPosition = new Vector3(0, (levelStep * 0.5f) + (i * levelStep) + (levelStep * 0.12f), -0.1f);
            lineGO.transform.localScale = new Vector3(0.25f, 0.25f, 0.35f);
        }*/

        SetCurrentLevel();
    }

    LevelObject CreateLevelData(int levelIndex)
    {
        LevelObject levelObj = new LevelObject();
        float difIndex = PlayerPrefs.GetInt("Difficulty") / 100f;  //0.0 easy, 1.0 most difficult

        //First Level
        if (levelIndex == 0)
        {
            levelObj.level = levelIndex;
            levelObj.type = Helper.Types[0];
            levelObj.numbersUsed = 4;
            levelObj.combinationLength = 3;
            levelObj.timeStep = 3.0f;
        }
        else
        {
            levelObj.level = levelIndex;
           
            int lastAd = PlayerPrefs.GetInt("LastAd");
            int lastAdDif = levelIndex - lastAd;
            if(lastAdDif > 3)
            {
                int adChance = UnityEngine.Random.Range(0, 100);
                adChance = (adChance - 30) + (lastAdDif * 10); //increase chance with each additional level
                if(adChance > 45)
                {
                    levelObj.isAd = true;
                    PlayerPrefs.SetInt("LastAd", levelIndex);
                }
            }            

            levelObj.type = GetRandomType();
            if(levelIndex < 5) { levelObj.type = Helper.Types[0]; } //Make first few levels numbers

            int maxNumbers = Helper.minNumbers + Mathf.CeilToInt(difIndex * (Helper.maxNumbers - Helper.minNumbers)); 
            levelObj.numbersUsed = UnityEngine.Random.Range(Helper.minNumbers, maxNumbers);

            int minRange = 2; //Provide always some range, otherwise the combination would be for example always only 3 for a long time
            int maxComb = Helper.minCombinationLength + minRange + Mathf.CeilToInt(difIndex * ((Helper.maxCombinationLength- minRange) - Helper.minCombinationLength));
            levelObj.combinationLength = UnityEngine.Random.Range(Helper.minCombinationLength, maxComb);

            float minStep = Helper.minTimeStep + ((1f - difIndex) * (Helper.maxTimeStep - Helper.minTimeStep));
            levelObj.timeStep = UnityEngine.Random.Range(minStep, minStep + 0.1f);

            /*print("Difficulty " + PlayerPrefs.GetInt("Difficulty"));
            print("MaxNumbers " + maxNumbers + " maxComb " + maxComb + " minStep " + minStep);
            print("NumbersUsed " + levelObj.numbersUsed + " combinationLength " + levelObj.combinationLength + " timeStep " + levelObj.timeStep);*/
        }

        return levelObj;
    }

    string GetRandomType()
    {
        //Game_Manager.Type[] types = (Game_Manager.Type[])System.Enum.GetValues(typeof(Game_Manager.Type));
        int typeLength = Helper.Types.Length;
        int rnd = UnityEngine.Random.Range(0, typeLength);
        string rndType = Helper.Types[rnd];       

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

        if(levelData.isAd)
        {
            if (Application.isEditor)
            {
                AdFinished();
            }
            else
            {
                adsManager.ShowRewardedAd();
            }
        }
        else
        {
            Analytics.CustomEvent("LevelSelected", new Dictionary<string, object>
            {
                { "LevelIndex", levelData.level }
            });

            PlayerPrefs.SetInt("Level", levelData.level);
            SceneManager.LoadScene("GameScene");
        }        
    }

    public void AdFinished()
    {
        StartCoroutine("AdFinishedCoroutine");
    }

    IEnumerator AdFinishedCoroutine()
    {
        yield return new WaitForSeconds(0.25f);

        if (levels[currentLevel].isAd)
        {
            levels[currentLevel].finished = true;
            SaveSystem.SaveData(levels);
            Init();
        }
    }

    public void DeleteSaves()
    {
        SaveSystem.DeleteData();
        PlayerPrefs.DeleteAll();
    }

}
