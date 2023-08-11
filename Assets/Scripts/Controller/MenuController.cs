using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void LoadScene(string url)
    {
        SceneManager.LoadSceneAsync(url);
    }
}
