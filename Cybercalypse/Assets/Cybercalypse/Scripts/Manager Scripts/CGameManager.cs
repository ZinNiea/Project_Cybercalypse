﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CGameManager : SingleTonManager<CGameManager>
{
    /// <summary>
    /// 작성자 : 구용모
    /// 스크립트 : CyberCalypse의 전체적인 게임 흐름을 관리하는 매니저 스크립트
    /// 최초 작성일 : . . .
    /// 최종 수정일 : 2018.07.08
    /// </summary>

    public bool isPlayerInvincible;
    public GameObject playerObject;
    public GameObject skillLibrary;
    public GameObject abilityLibrary;
    public GameObject equipmentLibrary;

    public bool isDead;
    public bool isGameOver;
    public bool isMenuClose;

    public List<Sprite> testSkillList;
    public List<Sprite> testAbilityList;
    public List<Sprite> testWeaponList;
    public List<Sprite> testArmorList;
    //public List<Sprite> testSuitList;

    private SpriteRenderer sprite;


    private new void Awake()
    {
        base.Awake();

        playerObject = GameObject.Find("Player").gameObject;
        skillLibrary = GameObject.Find("Skill Library").gameObject;
        abilityLibrary = GameObject.Find("Ability Library").gameObject;
        equipmentLibrary = GameObject.Find("Equipment Library").gameObject;

        sprite = playerObject.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        CInputManager.instance.GamePause += GamePause;
        CInputManager.instance.GameRetry += isRetry;

        Time.timeScale = 1.0f;
    }

    public void GamePause(bool isGamePauseNow)
    {
        //crossHair 중심을 마우스 좌표로 사용하기 위해 crossHair 텍스쳐의 정 중앙의 포지션으로 놓는다.

        if(isGamePauseNow)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
    }

    /// <summary>
    /// 플레이어가 세이브 지점에 도달하거나 던전이 끝날때마다 이곳으로 세이브 이벤트를 던져준다.
    /// 세이브 이벤트가 실행되면 CDataManager의 GameSavaJson.json으로 저장이 된다.(일단은 플레이어의 최근 위치(Position))
    /// </summary>
    public void GameSave()
    {
        //save event 실행
    }

    public void PlayerHasInvincible()
    {
        StartCoroutine(HasInvincibleTime());
        StartCoroutine(HasBlink());
    }

    private void isRetry(bool isRetryInput)
    {
        if (!isGameOver)
        {
            return;
        }
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SceneReload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator HasBlink()
    {
        sprite.color = new Vector4(1,0,0,0.75f);
        yield return new WaitForSeconds(0.05f);
        sprite.color = new Vector4(1, 1, 1, 1);

        for (float i = 0; i < 1.5f; i += Time.deltaTime)
        {
            if(!isPlayerInvincible)
            {
                sprite.enabled = true;
                yield break;
            }

            sprite.enabled = false;

            yield return new WaitForSeconds(0.01f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator HasInvincibleTime()
    {
        isPlayerInvincible = true;
        Physics2D.IgnoreLayerCollision(9, 25, true);
        yield return new WaitForSeconds(1.5f);
        isPlayerInvincible = false;
        Physics2D.IgnoreLayerCollision(9, 25, false);
    }
}