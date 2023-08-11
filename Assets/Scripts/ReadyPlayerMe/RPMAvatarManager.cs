using UnityEngine;

public class RPMAvatarManager : MonoBehaviour
{
    private const string Avatar_url = "avatar-url";
    
    private void Awake()
    {
        if(!PlayerPrefs.HasKey(Avatar_url))
            PlayerPrefs.SetString(Avatar_url, null);
    }
    
    public static string AvatarUrl { get => PlayerPrefs.GetString(Avatar_url); set => PlayerPrefs.SetString(Avatar_url, value); }

    public void SetRmpAvatar(string _url)
    {
        Debug.Log("Created avatar url: " + _url);
        AvatarUrl = _url;
    }
}
