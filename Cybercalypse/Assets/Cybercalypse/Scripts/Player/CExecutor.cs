﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CExecutor : MonoBehaviour
{
    /// <summary>
    /// 작성자 : 구용모, 윤동준
    /// 스크립트 : Player의 내부 속성들과 HP,Shield 등등을 체크하고 스탯을 나타내는 스크립트
    /// 최초 작성일 : . . .
    /// 최종 수정일 : 2018.07.09
    /// </summary>

    //!< private 변수이름은 첫글자 소문자
    public string name;

    private float currentHealth;

    private float currentShield;

    private float currentEnergy;

    private float defensivePower;

    private float moveForce; //값을 할당 후 변하였을 때만 변하는 전역변수 그 외에는 값이 상실되면 안된다.
    private float jumpForce; //값을 할당 후 변하였을 때만 변하는 전역변수 그 외에는 값이 상실되면 안된다.
    private float dodgeForce;
    private float knockbackForce;

    private Image HP;
    private Image SP;
    private Image EP;

    #region //!< get, set
    public float MaximumHealth { get; set; }
    [SerializeField]
    public float CurrentHealth { get; set; }

    public float MaximumShield { get; set; }
    [SerializeField]
    public float CurrentShield { get; set; }
    public float ShieldRecoveryRate { get; set; }

    public float MaximumEnergy { get; set; }
    [SerializeField]
    public float CurrentEnergy { get; set; }
    public float EnergyRecvoeryRate { get; set; }

    public int MaximumBulletCount { get; set; }
    public int CurrentBulletCount { get; set; }


    //아래 템플릿으로 바꿔놓을것
    //Player Physics component
    public float MoveForce
    {
        get
        {
            return moveForce;
        }
        set
        {
            moveForce = value;
        }
    }
    public float JumpForce
    {
        get
        {
            return jumpForce;
        }
        set
        {
            jumpForce = value;
        }
    }
    public float DodgeForce
    {
        get
        {
            return dodgeForce;
        }
        set
        {
            dodgeForce = value;
        }
    }
    public float KnockbackForce
    {
        get
        {
            return knockbackForce;
        }
        set
        {
            knockbackForce = value;
        }
    }
    #endregion

    private void Awake()
    {
        MaximumHealth = 2000.0f; // --> 정확한 수치가 나오면 JSON 파일로 파싱하여 할당함
        CurrentHealth = 2000.0f;// --> 정확한 수치가 나오면 JSON 파일로 파싱하여 할당함

        MaximumShield = 2000.0f;// --> 정확한 수치가 나오면 JSON 파일로 파싱하여 할당함
        CurrentShield = 2000.0f;// --> 정확한 수치가 나오면 JSON 파일로 파싱하여 할당함

        ShieldRecoveryRate = 1.0f;// --> 정확한 수치가 나오면 JSON 파일로 파싱하여 할당함

        MaximumEnergy = 100.0f;// --> 정확한 수치가 나오면 JSON 파일로 파싱하여 할당함
        CurrentEnergy = 100.0f;// --> 정확한 수치가 나오면 JSON 파일로 파싱하여 할당함
        EnergyRecvoeryRate = 3.0f;// --> 정확한 수치가 나오면 JSON 파일로 파싱하여 할당함

        MaximumBulletCount = 30;// --> 정확한 수치가 나오면 JSON 파일로 파싱하여 할당함
        CurrentBulletCount = 0;// --> 정확한 수치가 나오면 JSON 파일로 파싱하여 할당함
    }

    private void Start()
    {
        StartCoroutine(RecoverShield());
        StartCoroutine(RecoveryEnergy());
    }

    IEnumerator RecoverShield()
    {
        while(true)
        {
            if(CurrentShield >= MaximumShield)
            {
                yield return new WaitUntil(() => CurrentShield < MaximumShield);
            }
            CurrentShield += ShieldRecoveryRate;
            yield return new WaitForSeconds(Time.deltaTime * 60f);
        }
    }

    IEnumerator RecoveryEnergy()
    {
        while(true)
        {
            if(CurrentEnergy >= MaximumEnergy)
            {
                yield return new WaitUntil(() => CurrentEnergy < MaximumEnergy);
            }
            CurrentEnergy += ShieldRecoveryRate;

            yield return new WaitForSeconds(Time.deltaTime * 60f); ;
        }
    }

    public void GetStatusPoint(float amount)
    {

    }

    public void GetDamage(float damage)
    {
        if(CurrentShield >= damage)
        {
            CurrentShield -= damage;
        }

        else    //!< currentShield < damage
        {
            if(CurrentShield > 0.0f)
            {
                damage -= CurrentShield;    //!< 남은 실드를 깎고
                CurrentShield = 0.0f;
            }
            CurrentHealth -= damage;
            if(CurrentHealth <= 0.0f)
            {
                CurrentHealth = 0.0f;
            }
        }
    }

    /// <summary>
    /// EP를 소모하는 함수
    /// </summary>
    /// <param name="consumption">소모량을 float로 전달</param>
    public void ConsumeEnergy(float consumption)
    {
        CurrentEnergy -= consumption;
        if (CurrentEnergy <= 0.0f)
            CurrentEnergy = 0.0f;
    }
}