using KartGame.KartSystems;
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

    float m_Acceleration;
    float m_Steering;
    bool m_HopPressed;
    bool m_HopHeld;
    bool m_BoostPressed;
    bool m_FirePressed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
