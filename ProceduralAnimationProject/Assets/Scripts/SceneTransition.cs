using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{

    public void OnClickChangeScene(int _buildIndex)
    {
        SceneManager.LoadScene(_buildIndex);
    }
}
