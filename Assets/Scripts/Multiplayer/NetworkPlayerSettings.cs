using Fusion;
using ReadyPlayerMe.Samples;
using UnityEngine;

public class NetworkPlayerSettings : NetworkBehaviour
{
    [Networked(OnChanged = nameof(AvatarUrlChanged))]
    public NetworkString<_128> avatarUrl { get; set; }
    
    [Networked(OnChanged = nameof(PlayerNameChanged))]
    public NetworkString<_16> playerName { get; set; }

    public static void AvatarUrlChanged(Changed<NetworkPlayerSettings> change)
    {
        if (change.Behaviour.HasStateAuthority)
        {
            Debug.Log("avatar url changed: " + change.Behaviour.avatarUrl.Value);
            change.Behaviour.GetComponent<ThirdPersonLoader>().LoadAvatar(change.Behaviour.avatarUrl.Value);
        }
    }
    
    public static void PlayerNameChanged(Changed<NetworkPlayerSettings> change)
    {
        
    }
}
