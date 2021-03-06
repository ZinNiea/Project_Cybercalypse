﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CGridDrivenContentsGenerator : MonoBehaviour
{
    // 상대 좌표에 곱할 상수
    public float tileLength = 0.16f;

    // 맵 생성 옵션
    public int numOfChamberInHorizontal;
    public int numOfChamberInVertical;
    public int chamberWidth, chamberHeight;

    public float possibilityOfUpperChamber;
    public float possibilityOfUnderChamber;
    public float possibilityOfContinuousDummy;
    public float possibilityOfDividedDummy;
    // 해당 방향으로 경로를 몇번 생성할 것인지 결정
    public int numOfSimulation;
    

    // 그리드 사각형 내에 어떤 그리드에 어떤 Chamber가 위치하는지 저장
    public Dictionary<Vector2Int, CChamber> ChamberPosition { get; private set; }
    // 각 Chamber에 배치된 타일 정보를 저장, 관리하는 Dictionary, VirtualCoordGenerator에 의해 
    public Dictionary<Vector2Int, ETileType> TileDict { get; private set; }

    // 각 Chamber에 대한 공통 정보
    public int NumOfChamberInHorizontal { get; private set; }
    public int NumOfChamberInVertical { get; private set; }
    public int ChamberWidth { get; private set; }
    public int ChamberHeight { get; private set; }
    public float PossibilityOfContinuousDummy { get; private set; }
    public float PossibilityOfDividedDummy { get; private set; }
    public float PossibilityOfUpperChamber { get; private set; }
    public float PossibilityOfUnderChamber { get; private set; }
    
    public int NumOfSimulation { get; private set; }
    public float TILE_LENGTH { get; private set; }

    // 출발 지점의 Chamber 상대 좌표
    public Vector2Int StartChamberPos { get; private set; }
    // 도착 지점의 Chamber 상대 좌표
    public Vector2Int EndChamberPos { get; private set; }

    // 플레이어 생성 지점 절대 좌표
    private Vector3 playerStartPosition;
    public Vector3 PlayerStartPosition
    {
        get { return playerStartPosition; }
        set { playerStartPosition = new Vector3(value.x * TILE_LENGTH * 2, value.y * TILE_LENGTH * 2); }
    }

    private CVirtualCoordGenerator virtualCoordGenerator;
    public AContentsGenerator gameObjectGenerator;

    void Awake()
    {
        ChamberPosition = new Dictionary<Vector2Int, CChamber>();
        TileDict = new Dictionary<Vector2Int, ETileType>();
        virtualCoordGenerator = GetComponentInChildren<CVirtualCoordGenerator>();
    }

    /// <summary>
    /// 생성할 맵에 대한 최소 정보 입력(Chamber의 가로 개수, Chamber의 세로 개수, 맵 구성요소 생성기)
    /// </summary>
    private void initGenerator()
    {
        NumOfChamberInHorizontal = numOfChamberInHorizontal;
        NumOfChamberInVertical = numOfChamberInVertical;
        ChamberHeight = chamberHeight;
        ChamberWidth = chamberWidth;
        NumOfSimulation = numOfSimulation;
        TILE_LENGTH = tileLength;
        checkPossibility();

        PossibilityOfContinuousDummy = possibilityOfContinuousDummy;
        PossibilityOfDividedDummy = possibilityOfDividedDummy;
        PossibilityOfUnderChamber = possibilityOfUnderChamber;
        PossibilityOfUpperChamber = possibilityOfUpperChamber;
    }

    /// <summary>
    /// 맵 구동기 가동
    /// </summary>
    public void StartGenerator()
    {
        initGenerator();
        makeEssentialPath();
        makeDummyPath(StartChamberPos);

        // generator를 이용해여 맵 구성요소 생성
        virtualCoordGenerator.GenerateVirtualCoord();
        gameObjectGenerator.GenerateContents();

        Vector2Int playerStartPos = new Vector2Int(StartChamberPos.x * ChamberWidth + ChamberWidth / 2, StartChamberPos.y * ChamberHeight + ChamberHeight / 2);
        while(TileDict[playerStartPos] != ETileType.Empty)
        {
            if(TileDict.ContainsKey(new Vector2Int(playerStartPos.x + 1, playerStartPos.y)))
            {
                playerStartPos = new Vector2Int(playerStartPos.x + 1, playerStartPos.y);
            }
            else if(TileDict.ContainsKey(new Vector2Int(playerStartPos.x - 1, playerStartPos.y)))
            {
                playerStartPos = new Vector2Int(playerStartPos.x - 1, playerStartPos.y);
            }
            else if(TileDict.ContainsKey(new Vector2Int(playerStartPos.x, playerStartPos.y - 1)))
            {
                playerStartPos = new Vector2Int(playerStartPos.x, playerStartPos.y - 1);
            }
            else if(TileDict.ContainsKey(new Vector2Int(playerStartPos.x, playerStartPos.y + 1)))
            {
                playerStartPos = new Vector2Int(playerStartPos.x, playerStartPos.y + 1);
            }
        }
        PlayerStartPosition = new Vector3(playerStartPos.x, playerStartPos.y, 0.0f);
    }

    private void checkPossibility()
    {
        
        if(possibilityOfContinuousDummy < 0.0f || possibilityOfDividedDummy < 0.0f ||
            possibilityOfContinuousDummy + possibilityOfDividedDummy > 100.0f)
        {
            possibilityOfDividedDummy = 20.0f;
            possibilityOfContinuousDummy = 60.0f;
        }

        if(possibilityOfUpperChamber < 0.0f || possibilityOfUpperChamber > 100.0f)
        {
            possibilityOfUpperChamber = 100.0f;
        }

        if(possibilityOfUnderChamber < 0.0f || possibilityOfUnderChamber > 100.0f)
        {
            possibilityOfUnderChamber = 100.0f;
        }
    }

    /// <summary>
    /// 필수 경로를 생성
    /// </summary>
    private void makeEssentialPath()
    {
        Vector2Int currentPosition, nextPosition;
        Vector2Int[] adjacentPosition;
        // 필수 경로 시작 지점
        currentPosition = StartChamberPos = new Vector2Int(0, (int)Random.Range(0.0f, NumOfChamberInVertical));
        ChamberPosition.Add(currentPosition, new CChamber(EChamberType.Essential, currentPosition));
        // 필수 경로 인접 지점
        adjacentPosition = getAdjacentPath(currentPosition, true);
        nextPosition = adjacentPosition[(int)Random.Range(0.0f, adjacentPosition.Length)];
        ChamberPosition.Add(nextPosition, new CChamber(EChamberType.Essential, nextPosition));
        addFromCurrentToNextChamberPassage(currentPosition, nextPosition);
        currentPosition = nextPosition;
        // 그리드의 맨 오른쪽 위치 까지 진행
        while (currentPosition.x != NumOfChamberInHorizontal - 1)
        {
            adjacentPosition = getAdjacentPath(currentPosition, true);

            nextPosition = adjacentPosition[(int)Random.Range(0.0f, adjacentPosition.Length)];
            ChamberPosition.Add(nextPosition, new CChamber(EChamberType.Essential, nextPosition));
            addFromCurrentToNextChamberPassage(currentPosition, nextPosition);
            currentPosition = nextPosition;
        }
        // 도착 지점 설정
        EndChamberPos = currentPosition;
    }

    /// <summary>
    /// 해당 좌표의 근접한 위치의 상대 좌표 배열을 반환
    /// </summary>
    /// <param name="path"> 근접한 위치의 좌표를 구할 기준 좌표 </param>
    /// <param name="isEssential"> 필수 경로의 근접좌표를 구하는가? </param>
    /// <returns> 근접한 좌표 배열 </returns>
    private Vector2Int[] getAdjacentPath(Vector2Int path, bool isEssential)
    {
        List<Vector2Int> adjacentList = new List<Vector2Int>();
        List<Vector2Int> availableList = new List<Vector2Int>();
        // 필수 경로가 아닌 경우에는 역으로 이동하는 경로도 고려
        if (!isEssential)
        {
            adjacentList.Add(new Vector2Int(path.x - 1, path.y)); // left
        }

        if (Random.Range(0.0f, 100.0f) < PossibilityOfUpperChamber)
        {
            adjacentList.Add(new Vector2Int(path.x, path.y + 1)); //up
        }
        if (Random.Range(0.0f, 100.0f) < PossibilityOfUnderChamber)
        {
            adjacentList.Add(new Vector2Int(path.x, path.y - 1)); // down
        }
        adjacentList.Add(new Vector2Int(path.x + 1, path.y)); // right

        adjacentList.ForEach(delegate (Vector2Int adjPath)
        {
        if (!ChamberPosition.ContainsKey(adjPath) && adjPath.x >= 0 && adjPath.x < NumOfChamberInHorizontal
        && adjPath.y >= 0 && adjPath.y < NumOfChamberInVertical)
            {
                availableList.Add(adjPath);
            }
        });

        return availableList.ToArray();
    }

    /// <summary>
    /// start Chamber와 end Chamber를 이어주는 메소드, 두 Chamber는 상대좌표 거리가 1만큼 차이나야 한다.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    private void addFromCurrentToNextChamberPassage(Vector2Int start, Vector2Int end)
    {
        ChamberPosition[start].NextChamberPosition.Add(end);
        ChamberPosition[end].PrevChamberPosition = start;
    }

    /// <summary>
    /// 생성된 필수 경로를 기준으로 더미 경로를 생성한다.
    /// </summary>
    /// <param name="start"></param>
    private void makeDummyPath(Vector2Int start)
    {
        if(start == EndChamberPos)
        {
            return;
        }

        // 해당 Chamber가 필수 경로 상의 Chamber인 경우
        if (ChamberPosition[start].ChamberType == EChamberType.Essential)
        {
            // 다음 필수경로를 대상으로 실행
            makeDummyPath(ChamberPosition[start].NextChamberPosition[0]);
        }

        int possibility = (int)Random.Range(0.0f, 100.0f);
        Vector2Int[] adjacentChambers = getAdjacentPath(start, false);

        // 인접한 Chamber가 존재하지 않는 경우
        if (adjacentChambers.Length == 0)
        {
            return;
        }
        int index = (int)Random.Range(0.0f, adjacentChambers.Length);

        ChamberPosition.Add(adjacentChambers[index], new CChamber(EChamberType.Dummy, adjacentChambers[index]));
        addFromCurrentToNextChamberPassage(start, adjacentChambers[index]);

        // 설정된 확률로 길이 확장
        if (possibility <= PossibilityOfContinuousDummy)
        {
            makeDummyPath(adjacentChambers[index]);
        }  // 설정된 확률로 길이 분열
        else if (possibility > PossibilityOfContinuousDummy && possibility <= PossibilityOfDividedDummy + PossibilityOfContinuousDummy)
        {
            makeDummyPath(adjacentChambers[index]);
            makeDummyPath(start);
        }
        // 이외의 확률로 길이 끊어짐
    }

    
}
