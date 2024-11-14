using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{
    [SerializeField] private int nextLevelIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (PlayerPrefs.GetInt("currentLevel") < 5)
            {
                PlayerPrefs.SetInt("currentLevel", SceneManager.GetActiveScene().buildIndex + 1);

            }
            ChangeScene();
        }
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(nextLevelIndex);
    }
}