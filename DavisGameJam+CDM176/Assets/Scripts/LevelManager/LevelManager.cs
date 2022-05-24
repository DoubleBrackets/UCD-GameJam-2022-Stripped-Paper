using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public int startLevel;
    public PlayerStateController player;
    public LevelDataAsset levelData;

    // Starts at 0
    private int currentLevelNumber = 0;
    private GameObject currentLevelObject;

    public int levelSceneOffset;

    private bool isLoading = false;

    private float transitionTime = 1.5f;

    public Action OnScreenTransition;

    private void Awake()
    {
        instance = this;
        isLoading = true;
        player.gameObject.SetActive(false);
        Action loadIn = () =>
        {
            StartCoroutine(Corout_TransitionIntoLevel(startLevel));
            player.gameObject.SetActive(true);
        };
        StartCoroutine(Corout_Delay(1.5f,loadIn));
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }

    public void RestartGame()
    {
        LoadLevel(0);
    }

    private AsyncOperation LoadLevel(int level)
    {
        currentLevelNumber = level;
        var loadOp = SceneManager.LoadSceneAsync(levelSceneOffset + level, LoadSceneMode.Additive);
        return loadOp;
    }

    private AsyncOperation UnloadLevel(int level)
    {
        return SceneManager.UnloadSceneAsync(levelSceneOffset + level);
    }

    private void ChangeLevel(int level)
    {
        isLoading = true;
        StartCoroutine(Corout_ChangeLevel(level));
    }

    private IEnumerator Corout_ChangeLevel(int level)
    {
        yield return TransitionManager.instance.TransitionOut(transitionTime);        
        GameLayerManager.instance.UnloadLayerObjects();
        yield return UnloadLevel(currentLevelNumber);
        yield return new WaitForEndOfFrame();
        OnScreenTransition?.Invoke();
        yield return new WaitForSeconds(0.1f);
        yield return Corout_TransitionIntoLevel(level);
        isLoading = false;
    }

    private IEnumerator Corout_TransitionIntoLevel(int level)
    {
        yield return LoadLevel((level) % (SceneManager.sceneCountInBuildSettings - levelSceneOffset));
        yield return new WaitForEndOfFrame();
        GameLayerManager.instance.ResetLayerManager();
        LoadPlayer();
        Camera.main.GetComponent<Cinemachine.CinemachineBrain>().ActiveVirtualCamera.UpdateCameraState(Vector3.up, 10000000);
        //TerrainManager.instance.SetupStaticTerrain();
        yield return TransitionManager.instance.TransitionIn(transitionTime);
        isLoading = false;
    }

    [ContextMenu("Progress level")]
    public void ProgressLevel()
    {
        ChangeLevel(currentLevelNumber + 1);
    }

    public void RestartLevel()
    {
        if(!isLoading)
            ChangeLevel(currentLevelNumber);
    }

    public void LoadPlayer()
    {
        player.RespawnPlayer();
        player.transform.position = levelData[currentLevelNumber].respawnPosition;
    }

    private IEnumerator Corout_Delay(float delay, Action toInvoke)
    {
        yield return new WaitForSeconds(delay);
        toInvoke?.Invoke();
    }
}
