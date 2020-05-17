using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public delegate void SpinDelegate(float spinValue);
    public static event SpinDelegate SpinEvent;

    PlayerInput playerInput;
    Audio_Manager audioManager;
    UI_Manager uiManager;

    public GameObject spinner;
    public GameObject numberPrefab;
    public Transform numbersParent;
    public GameObject combinationNumberPrefab;
    public Transform combinatinoNumbersParent;
 
    public GameObject gearPrefab;
    public Transform gearsParent;

    List<GearObject> gearsList = new List<GearObject>();
    List<CircleNumber> circleNumbers = new List<CircleNumber>();

   /* public enum Type
    {
        Numbers,
        Symbols
    };

    Type selectedType = Type.Numbers;*/

 
    string selectedType = "";

    //Setup
    int numbersUsed = 4; //What numbers will be used, 0 - ?
    int combinationLength = 5;
    float timeStep = 0.8f; //how long is each action taking to player ( selecting number ). 0.8 very hard, 2.0 makes game for exampe very easy. Level time is calculated based on this value

    bool levelEnded = false;
    bool isResetting = false;
    float resetVelocity = 0f;

    bool isLocking = false;
    bool isLocked = false;
    float lockStep = 0f;
    float lockDistance = 1.7f;
    float lockSpeed = 0.6f;
    bool isCloseToNumber = false;
    CircleNumber closeNumber;
    int lastLockedNumber = 0;

    //Combination
    List<CombinationNumberData> combinationNumbers = new List<CombinationNumberData>();
    List<int> combination = new List<int>();
    int combinationIndex = 0; //What number is player now adding

    Quaternion lastQuat;
    Vector3 spinnerZeroRot = new Vector3(0f, 180f, 0f);

    //Timer
    public Transform timerHand;
    public Material timerRingMat;
    float levelTime = 2f;
    float curTime = 0f;

    //Saving
    List<LevelObject> loadedLevels = new List<LevelObject>();
    int selectedLevelIndex = 0;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        audioManager = GetComponent<Audio_Manager>();
        uiManager = GetComponent<UI_Manager>();

        //Load Level Data        
       

        Init();
    }

    void LoadLevel()
    {
        int selectedLevel = PlayerPrefs.GetInt("Level");
        loadedLevels = SaveSystem.LoadData();      

        for (int i = 0; i < loadedLevels.Count; i++)
        {
            if (loadedLevels[i].level == selectedLevel)
            {
                selectedLevelIndex = i;
                break;
                
            }
        }

        Debug.Log("Selected Level " + selectedLevel);
    }

    void Init()
    {

        LoadLevel();
       
        spinner.transform.localEulerAngles = spinnerZeroRot;
        isLocking = false;
        isLocked = false;
        isCloseToNumber = false;
        combinationIndex = 0;
        levelEnded = false;
        closeNumber = null;
        lockStep = 0f;

        //Set Level
        selectedType = loadedLevels[selectedLevelIndex].type;
        numbersUsed = loadedLevels[selectedLevelIndex].numbersUsed;
        combinationLength = loadedLevels[selectedLevelIndex].combinationLength;
        timeStep = loadedLevels[selectedLevelIndex].timeStep;  //0.8 hard

        //print("Combination Length " + combinationLength);

        uiManager.Reset();
        DeleteCombinationNumbers();
        DeleteNumbers();
        DeleteGears();     

        CreateCombination(numbersUsed);
        CreateCombinationNumbers();
        CreateNumbers(numbersUsed);
        CreateGears();

        //Setup Timer after combination was created
        timerHand.localEulerAngles = new Vector3(0, 180f, 0);
        timerRingMat.SetFloat("_FillClock", 1.0f);
        curTime = 0f;     
        CalculateLevelTime();
    }

    void Update()
    {
        if (!isResetting)
        {
           //CheckForCloseNumber();
            LockNumber();
        }
        else
        {
            ResetSpinnerToZero();

        }

        float inputSign = Mathf.Sign(playerInput.Inputvalue) * -1f;

        float delta = Quaternion.Angle(spinner.transform.rotation, lastQuat);
        SpinEvent(delta * -1f);
     

        lastQuat = spinner.transform.rotation;

        Timer();
    }
   


    #region Init

    void CreateCombination(int maxNumber)
    {
        combination.Clear();

        int length = combinationLength;
        int lastNumber = 0; //next number cant be same as last number

        for(int i = 0; i < length; i++)
        {
            int randomNumber = Random.Range(0, maxNumber);

           //Cant have same number after each other. Also prevents first number from being zero
            while (randomNumber == lastNumber)
            {
                randomNumber = Random.Range(0, maxNumber);
            }

            lastNumber = randomNumber;

            //print(randomNumber);
            combination.Add(randomNumber);
        }
    }

    void CalculateLevelTime()
    {
        //Add difficulty settings ( adjust step value )

        //Consider selecting number one step, which takes some time. If Combinat is 3 numbers long and there are 4 numbers to select. The time for level to allow for testing each combination is as following
        // (4 * step) for each number in combination to try each possible number. For each additional number count for each failed combination also time needed to set previous combination ( ie if player is on third number
        //he has to put it in two previous numbers before he can test next possible number after failing on third number, so its +2 steps for each number test on third number
        //x-1 has to be there since that combination has to be entered each time player fails, not only once per number, -1 because one combination is succesufl, so not all will fail

        //n current number in combation
        //x amount of available numbers
        // (n*step*(x-1)) + (x+step) for each number in combination

        //Step is how long does it take to select a number + some buffer for occasional reset of hand
        float step = timeStep; //0.8 very hard
        int x = numbersUsed;
        float totalTime = 0f;

        for(int n = 0; n < combination.Count; n++)
        {
            float timeAdd = (n * step * (x-1)) + ((x-1) * step);
            totalTime += timeAdd;
        }

        //print("Level Time " + totalTime);
        levelTime = totalTime;

    }

    //Numbers at the top,showing what numbers player selected. The amount of these numbers must correspond to the amount of numbers in combination ( ie if combination is 4 numbers there has to be 4 slots for numbers )
    void CreateCombinationNumbers()
    {
        //Starting postion and size of the number slot
        float pos = 0f;
        float size = 0.4f;
        float gap = 0f;

        for (int i = 0; i < combination.Count; i++)
        {
            GameObject num = (GameObject)Instantiate(combinationNumberPrefab, Vector3.zero, Quaternion.identity);
            num.transform.parent = combinatinoNumbersParent;            

            //Set the position which is increased to the right with each new slot
            float curPos = pos;
            num.transform.localPosition = new Vector3(curPos, 0, 0);            

            CombinationNumberData dataObj = new CombinationNumberData();
            dataObj.obj = num;
            dataObj.script = num.GetComponent<CombinationNumber>();
            dataObj.script.Init(selectedType);
            combinationNumbers.Add(dataObj);


            //Move position to the right, dont do it on last object
            //pos is used as total length which is divided by 2 to find center. The lenght must represent actual size. adding one more size for element which does not exist would make it bigger than it is
            if (i < (combination.Count - 1))
            {
                pos += (size + gap);
            }
        }

        //Get Center of all combination numbers
        float offset = pos / 2f;

        //Use the offset to move all numbers so that they are centered
        for (int i = 0; i < combinationNumbers.Count; i++)
        {
            Vector3 numPos = combinationNumbers[i].obj.transform.localPosition;
            combinationNumbers[i].obj.transform.localPosition = new Vector3(numPos.x - offset, numPos.y, numPos.z);       
        }
    }

    //Create numbers around spinner, based on level, there is different amount of numbers
    void CreateNumbers(int amount)
    {
        //At what angle is each number supposed to be created
        float stepAngle = 360f / (float)amount;
        int stepAngleInt = Mathf.FloorToInt(stepAngle);       

        //Go through each number, create the GO, initialize its position and script and store it to list for later access
        for (int i = 0; i < amount; i++)
        {
            GameObject number = (GameObject)Instantiate(numberPrefab, Vector3.zero, Quaternion.identity);
            number.GetComponent<SpinnerNumber>().Init(i, selectedType);
            number.transform.parent = numbersParent;
            number.name = "Number_" + i.ToString();            

            //Find position on circle at given angle
            float angle = i * stepAngleInt;
            float angleToGetPos = 360f - ((i * stepAngleInt) - 90f); //Angle 0 is at right side of circle and it goes counter clockwise. To make it clockwise do 360 - x. To move angle 0 to North, do -90
            Vector3 pos = GetPosOnCircle(angleToGetPos);
            number.transform.localPosition = pos;

            //print("Created Number " + i + " angle " + angle + " AngleToGetPos " + angleToGetPos);

            CircleNumber circleNum = new CircleNumber();
            circleNum.obj = number;
            circleNum.angle = angle;
            circleNum.number = i;
            circleNum.script = number.GetComponent<SpinnerNumber>();        
            circleNumbers.Add(circleNum);
        }

        //Add Zero again ( always ) as it sits at angle 0 and 360. Without this zero is not found when spinner angle is around 360 (for example 350 - 360)
        CircleNumber circleNumZero = new CircleNumber();
        circleNumZero.angle = 360f;
        circleNumZero.number = 0;
        circleNumZero.script = circleNumbers[0].script;
        circleNumbers.Add(circleNumZero);
    }

    //When creating numbers around spinner, get position on a circle at given angle
    Vector3 GetPosOnCircle(float angle)
    {
        float radius = 1.77f;
        Vector3 center = Vector3.zero;
        float rad = Mathf.Deg2Rad * angle;

        Vector3 point = new Vector3(center.x + radius * Mathf.Cos(rad), center.y + radius * Mathf.Sin(rad) ,0f);
        return point;
    }

    void CreateGears()
    {
        for(int i = 0; i < combination.Count; i++)
        {
            GameObject gear = (GameObject)Instantiate(gearPrefab, Vector3.zero, Quaternion.identity);
            gear.transform.parent = gearsParent;
            gear.transform.localPosition = Vector3.zero;

            gear.GetComponent<Gear>().Init(i, combination.Count);

            GearObject gearObj = new GearObject();
            gearObj.script = gear.GetComponent<Gear>();
            gearObj.obj = gear;
            gearsList.Add(gearObj);
        }
    }

    #endregion

    

    #region Gameplay

    //Called from PlayerInput to set rotation of spinner
    public void Spin(float smoothedSpinValue)
    {        
        if (!isLocking && !isResetting && !levelEnded)
        {
            Vector3 rotVal = new Vector3(0, 0, smoothedSpinValue * -1f);
            spinner.transform.Rotate(rotVal);
        }   
    }

    

    public void NumberSelected(int index)
    {
        //print("isLocking " + isLocking + " combinationCompleted " + combinationCompleted);
        //print("index " + index + " lastLocked " + lastLockedNumber);
        if (isLocking || levelEnded) { return; }
        //if(lastLockedNumber == index) { return; }  

        isLocking = true;
        isLocked = false;
        closeNumber = circleNumbers[index];
        lockStep = 0f;

        //lastLockedNumber = index;
    }

    void Timer()
    {
        if(levelEnded) { return; }

        curTime += Time.deltaTime;
        float timeIndex = curTime / levelTime;
        timeIndex = Mathf.Clamp01(timeIndex);

        timerHand.localEulerAngles = new Vector3(0, 180f, 360f * timeIndex);
        timerRingMat.SetFloat("_FillClock", 1.0f - timeIndex);

        if(curTime >= levelTime)
        {           
            Finished(false);
        }
    }

    /*
    //When spinner is close to a number, check if it should lock it (ie select it)
    void CheckForCloseNumber()
    {
        if(selectingNumber) { return; }

        isCloseToNumber = false;
        //Inverse the angle, numbers are spawned with angles going clockwise, ie 90 degrees at right, but spinner ( unity euler angles ) has at that rotation 270 degrees
        float spinAngle = 360f - spinner.transform.localEulerAngles.z;        

        //Check for each existing number if its close
        for (int i = 0; i < circleNumbers.Count; i++)
        {
            //print("Circ " + circleNumbers[i].angle + " Spinner " + spinnerEuler);

            float distance = spinAngle - circleNumbers[i].angle;
            if(Mathf.Abs(distance) < lockDistance)
            {
                isCloseToNumber = true;
                closeNumber = circleNumbers[i];               
            }
        }
    }*/

    

    //Locking of number, spinner is close, so automaticly rotate the spinner to the correct rotation and then lock the number
    void LockNumber()
    {     
        if(isLocking && !isLocked)
        {
            //Invert spinner angle to match angles of numbers
            float spinAngle = spinner.transform.localEulerAngles.z;               
            float numberEuler = closeNumber.angle;

            //float delta = numberEuler - spinAngle;              

            Quaternion spinnerRot = Quaternion.Euler(0, 180f, spinAngle);
            Quaternion numberRot = Quaternion.Euler(0, 180f, numberEuler);

            spinner.transform.rotation = Quaternion.SlerpUnclamped(spinnerRot, numberRot, lockStep);
            lockStep += (Time.deltaTime * lockSpeed);

            float delta = Quaternion.Angle(numberRot, spinner.transform.rotation);

            //Locked
            if (delta < lockDistance)
            {
                isLocking = false;
                isLocked = true;
                lastLockedNumber = closeNumber.number;               
                closeNumber.script.Locked();                                   

                //Auto rotation finished, locked spinner at exact angle of the number ( again inverted from the number )
                spinner.transform.localEulerAngles = new Vector3(spinner.transform.localEulerAngles.x, spinner.transform.localEulerAngles.y, /*360f - */closeNumber.angle);       

                CheckCombinationNumber(closeNumber.number);
            }
        }
    }

    //Check locked number, win condintion for locking all numbers in or reset if wrong number was locked or just continue with correct number
    void CheckCombinationNumber(int number)
    {
        if(combinationIndex > combination.Count -1)
        {
            print("Index bigger than Combination");
            return;
        }

        //Correct number selected within combination
        if(number == combination[combinationIndex])
        {
            //Add it top visualy and increment index to move to next number in combination          
            combinationNumbers[combinationIndex].script.SetNumber(number);
            gearsList[combinationIndex].script.Locked = true;
            combinationIndex++;

            //All numbers locked in, completed
            if(combinationIndex >= combination.Count)
            {
                Finished(true);
            }
        }
        //Wrong number selected
        else
        {
            //If locked number which is wrong was selected and is same as first number in combinaton reset to 0 ( player should not stay on wrong number if its also same first number, he would have to move away and back to this number )
            if(lastLockedNumber == combination[0])
            {
                isResetting = true;
            }    

            //Reset Combination progress
            combinationIndex = 0;
            lastLockedNumber = 0; //Has to reset, so that player is not prevent from selecting same number after reste

            foreach (CombinationNumberData data in combinationNumbers)
            {
                data.script.Reset();
            }

            foreach(GearObject gear in gearsList)
            {
                gear.script.Locked = false;
            }
        }       
    }

    void Finished(bool success)
    {
        levelEnded = true;
        uiManager.ShowWinUI(success);        
        uiManager.ShowLoseUI(!success);

        if (success)
        {
            loadedLevels[selectedLevelIndex].finished = true;
            SaveSystem.SaveData(loadedLevels);
        }


        int curDifficulty = PlayerPrefs.GetInt("Difficulty");      
       /* float timeIndex = curTime / levelTime;
        timeIndex = Mathf.Clamp01(timeIndex);
        timeIndex -= 0.6f;
        timeIndex = Mathf.Clamp01(timeIndex);
        int reduceValue = Mathf.CeilToInt(timeIndex * 10f);*/

        int finalValue = 0;

        if (success)
        {
            finalValue = +5; // - reduceValue;               
        }
        else
        {
            finalValue = -5;
        }
        
        curDifficulty += finalValue;
        curDifficulty = Mathf.Clamp(curDifficulty, 0, 100);

        //print("Final Value " + finalValue);
        //print("Cur Dif " + curDifficulty);

        PlayerPrefs.SetInt("Difficulty", curDifficulty);
        
    }

    void ResetSpinnerToZero()
    {
        float goalAngle = 0f;

        if (spinner.transform.localEulerAngles.z > 180f)
        {
            goalAngle = 360f;
        }

        float newAngle = Mathf.SmoothDamp(spinner.transform.localEulerAngles.z, goalAngle, ref resetVelocity, 0.2f);

        spinner.transform.localEulerAngles = new Vector3(0, 180f, newAngle);

        if (newAngle > 358f || newAngle < 2f)
        {
            spinner.transform.localEulerAngles = new Vector3(0, 180f, 0f);
            isResetting = false;
        }
    }

    public void RestartGame()
    {
        Init();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    void DeleteCombinationNumbers()
    {
        foreach (CombinationNumberData data in combinationNumbers)
        {
            Destroy(data.obj);
        }

        combinationNumbers.Clear();
    }

    void DeleteNumbers()
    {
        foreach (CircleNumber num in circleNumbers)
        {
            Destroy(num.obj);
        }

        circleNumbers.Clear();
    }

    void DeleteGears()
    {
        foreach (GearObject gear in gearsList)
        {
            Destroy(gear.obj);
        }

        gearsList.Clear();
    }

    #endregion
}

public class CircleNumber
{
    public SpinnerNumber script;
    public GameObject obj;
    public float angle = 0f;
    public int number = 0;
}


public class CombinationNumberData
{
    public CombinationNumber script;
    public GameObject obj;
}

public class GearObject
{
    public GameObject obj;
    public Gear script;
}


/*
//[SerializeField] float rotStep = 5f; //how many degrees to rotate the spinner
    //float accumulatedRot = 0f;
        if (Mathf.Abs(value) > 0f)
        {
            accumulatedRot += value;

            if (Mathf.Abs(accumulatedRot) >= rotStep)
            {
                //Rotate
                int rotSteps = Mathf.FloorToInt(Mathf.Abs(accumulatedRot) / rotStep);
                float rotValue = (float)(rotSteps) * Mathf.Sign(accumulatedRot) * rotStep;
                Vector3 rotVal = new Vector3(0, 0, rotValue);
                spinner.transform.Rotate(rotVal);

                accumulatedRot += (Mathf.Abs(rotValue) * (Mathf.Sign(accumulatedRot) * -1f));
                //accumulatedRot = 0f;
                audioManager.PlaySound(Sounds.Sound.Click);
            }
        }
        else
        {
            accumulatedRot = 0f;
        }

 */
