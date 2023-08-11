using Fusion;
using ReadyPlayerMe.Samples;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            var networkObj = Runner.Spawn(PlayerPrefab, new Vector3( 0, 1, 0), Quaternion.identity, player);
            /*networkObj.GetComponent<ThirdPersonLoader>().Start1();*/
        }
    }
}
