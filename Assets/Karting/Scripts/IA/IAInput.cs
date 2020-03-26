using KartGame.KartSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAInput : MonoBehaviour , IInput
{
    public float Acceleration
    {
        get { return m_Acceleration; }
    }
    public float Steering
    {
        get { return m_Steering; }
    }
    public bool BoostPressed
    {
        get { return m_BoostPressed; }
    }
    public bool FirePressed
    {
        get { return m_FirePressed; }
    }
    public bool HopPressed
    {
        get { return m_HopPressed; }
    }
    public bool HopHeld
    {
        get { return m_HopHeld; }
    }

    internal void SetDNA(float[] bestiaturnDNA)
    {
        iaturnDNA = bestiaturnDNA;
    }

    float m_Acceleration;
    [SerializeField]
    float m_Steering;
    bool m_HopPressed;
    bool m_HopHeld;
    bool m_BoostPressed;
    bool m_FirePressed;
    [SerializeField]
    float[] iaturnDNA;
    float timeindex;
    int checkpoint;
    void Start()
    {
        m_Acceleration = 1;
    }

    internal void CreateDNA()
    {
        iaturnDNA = new float[37];
        for (int i = 0; i < iaturnDNA.Length; i++)
        {
            iaturnDNA[i] = UnityEngine.Random.Range(-1, 2);

        }
    }

    // Update is called once per frame
    void Update()
    {
        //timeindex += Time.deltaTime*2;
        m_Steering = iaturnDNA[checkpoint];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CheckPointIA"))
        {
            checkpoint++;
        }
    }
}
