using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public GameObject gear;
    public GameObject gearAxis;
    public GameObject leg;

    //Ratio between leg scale and position of gear
    float legScale = 1;
    float gearPos = 0.2f;

    float gearMinScale = 0.6f;  //0.6 .1
    float gearMaxScale = 2.0f;  //2.0 .3

    float minGearPos = 0.5f;
    float maxGearPos = 1.35f;

    float startPosZ = 0.25f;
    float posZIncrement = 0.15f;

    float minSpeed = 0.1f;
    float maxSpeed = 0.8f;
    float spinSpeed = 0;


    bool locked = false;
    public bool Locked
    {
        get { return locked; }
        set { locked = value; }
    }


    //public Material[] materialVariations;

    private void OnEnable()
    {
        Game_Manager.SpinEvent += GearRot;
    }

    private void OnDisable()
    {
        Game_Manager.SpinEvent -= GearRot;
    }


    public void Init(int index, int totalGears)
    {
        transform.name = "Gear" + index.ToString();
        //int randomMat = Random.Range(0, materialVariations.Length);
        //gear.GetComponent<Renderer>().material = materialVariations[0];

        transform.localPosition = new Vector3(0, 0, startPosZ + (index * posZIncrement));

        float angleStep = 360f / (float)totalGears;
        float angle = angleStep * index;
        transform.localEulerAngles = new Vector3(0, 0, angle);

        float randomPos = Random.Range(minGearPos, maxGearPos);
        float randomScale = Random.Range(gearMinScale, gearMaxScale);

        float newLegScale = randomPos / 0.2f;
        leg.transform.localScale = new Vector3(newLegScale, leg.transform.localScale.y, leg.transform.localScale.z);

        gear.transform.localPosition = new Vector3(randomPos * -1f, gear.transform.localPosition.y, gear.transform.localPosition.z);
        gear.transform.localScale = new Vector3(randomScale, randomScale, gear.transform.localScale.z);
        gearAxis.transform.localPosition = new Vector3(randomPos * -1f, gearAxis.transform.localPosition.y, gearAxis.transform.localPosition.z);

        spinSpeed = Random.Range(minSpeed, maxSpeed);
    }


    void GearRot(float spinDelta)
    {
        if(locked) { return; }

        Vector3 rot = new Vector3(0, 0, spinDelta * spinSpeed);
        transform.Rotate(rot);

        rot *= -1f;
        gear.transform.Rotate(rot);
    }

    private void Update()
    {
        /*
        Vector3 rot = new Vector3(0, 0, 10f);
        gear.transform.Rotate(rot);
        */
    }
}
