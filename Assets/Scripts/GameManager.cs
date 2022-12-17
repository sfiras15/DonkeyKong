using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private int level;
    private int lives;
    private int score;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject); //this gameobject will not be destroyed when a new scene is loaded
        NewGame();
        LoadLevel(1); // 1 and not 0  cuz the scene 0 in our build settings is preload 
    }
    private void LoadLevel(int index){
        level = index;
        Camera camera = Camera.main;
        if (camera != null){
            camera.cullingMask = 0;
        }
        Invoke(nameof(LoadScene),1f);
    }
    private void LoadScene(){
        SceneManager.LoadScene(level);
    }
    private void NewGame(){
        lives = 3;
        score = 0;
        LoadLevel(1);
    }
    public void  LevelCompleted(){
        score += 1000;
        int nextLevel = level + 1 ;
        if (nextLevel < SceneManager.sceneCountInBuildSettings){
            LoadLevel(nextLevel);
        }else {
            LoadLevel(1);
        }
    }
    public void LevelFailed(){
        lives--;
        if (lives == 0){
            NewGame();
        }
        else{
            LoadLevel(level);
        }
    }
}
