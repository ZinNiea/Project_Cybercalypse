﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {
    private CGridDrivenContentsGenerator generator;

    float x;
    float y;

	void Start ()
    {
        generator = LevelManager.instance.GridGenerator;
        generator.StartGenerator();
        CGameManager.instance.playerObject.transform.position = generator.PlayerStartPosition;
    }
}
