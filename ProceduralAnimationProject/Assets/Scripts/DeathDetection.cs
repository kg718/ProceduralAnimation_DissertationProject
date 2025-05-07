using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathDetection : MonoBehaviour
{
    void Update()
    {
        if(transform.position.y < -30)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
