using System;
using System.Globalization;
using Fusion;
using ReadyPlayerMe.Core;
using Unity.VisualScripting;
using UnityEngine;

namespace ReadyPlayerMe.Samples
{
    public class ThirdPersonLoader : NetworkBehaviour
    {
        private readonly Vector3 avatarPositionOffset = new Vector3(0, -0.08f, 0);

        /*[SerializeField]*/
        [Tooltip("RPM avatar URL or shortcode to load")]
        public string avatarUrl;

        private GameObject avatar;
        private AvatarObjectLoader avatarObjectLoader;

        [SerializeField] [Tooltip("Animator to use on loaded avatar")]
        private RuntimeAnimatorController animatorController;

        [SerializeField] [Tooltip("If true it will try to load avatar from avatarUrl on start")]
        private bool loadOnStart = true;

        [SerializeField]
        [Tooltip("Preview avatar to display until avatar loads. Will be destroyed after new avatar is loaded")]
        private GameObject previewAvatar;

        private void Awake()
        {
           // avatarObjectLoader = new AvatarObjectLoader();
        }

        public override void Spawned()
        {
            
            Debug.Log("spawned player network object id: " + GetComponent<NetworkObject>().Id);

           /*GetComponent<NetworkPlayerSettings>().avatarUrl =
               "https://api.readyplayer.me/v1/avatars/632d58be9b4c6a4352a9aba2.glb";*/
           
            if (this.GetComponent<NetworkObject>().HasStateAuthority)
            {
                Debug.Log("local avatar url: " + RPMAvatarManager.AvatarUrl);
                Debug.Log("download on locally");
                GetComponent<NetworkPlayerSettings>().avatarUrl =
                    /*"https://api.readyplayer.me/v1/avatars/632d58be9b4c6a4352a9aba2.glb"*/RPMAvatarManager.AvatarUrl;
            }
            
            else
            {
                Debug.Log("server avatar url: " + RPMAvatarManager.AvatarUrl);
                Debug.Log("Download on server");
                LoadAvatar(GetComponent<NetworkPlayerSettings>().avatarUrl.Value/*"https://models.readyplayer.me/623b1a6bcc9780a0691c31e2.glb"*/);
                Debug.Log("avatar url on server: " + GetComponent<NetworkPlayerSettings>().avatarUrl.Value);
                GetComponent<ThirdPersonController>().enabled = false;
            }
        }

        private void OnLoadFailed(object sender, FailureEventArgs args)
        {
            Debug.Log("Loading avatar failed: " + args.Message);
        }

        private void OnLoadCompleted(object sender, CompletionEventArgs args)
        {
            SetupAvatar(args.Avatar);
        }

        private void SetupAvatar(GameObject targetAvatar)
        {
            avatarObjectLoader.OnCompleted -= OnLoadCompleted;
            avatarObjectLoader.OnFailed -= OnLoadFailed;
            
            if (targetAvatar == null)
            {
                Debug.LogError("targetAvatar is null");
                return;
            }

            if (GetComponent<NetworkObject>().HasStateAuthority)
            {
                avatar = targetAvatar; //Instantiate(targetAvatar);
                //targetAvatar.SetActive(false);
                // Re-parent and reset transforms
                avatar.transform.parent = transform;
                avatar.transform.localPosition = avatarPositionOffset;
                avatar.transform.localRotation = Quaternion.Euler(0, 0, 0);

                avatar.GetComponent<Animator>().runtimeAnimatorController = animatorController;

               // if(this.GetComponent<NetworkObject>().HasStateAuthority == false) return;
            
                var controller = GetComponent<ThirdPersonController>();
                if (controller != null)
                {
                    controller.Setup(avatar, animatorController);
                }
            }
            else if (!GetComponent<NetworkObject>().HasStateAuthority)
            {
                avatar = targetAvatar; //Instantiate(targetAvatar);
                //targetAvatar.SetActive(false);
                // Re-parent and reset transforms
                avatar.transform.parent = transform;
                avatar.transform.localPosition = avatarPositionOffset;
                avatar.transform.localRotation = Quaternion.Euler(0, 0, 0);

                avatar.GetComponent<Animator>().runtimeAnimatorController = animatorController;

                //if(this.GetComponent<NetworkObject>().HasStateAuthority == false) return;
            
                var controller = GetComponent<ThirdPersonController>();
                if (controller != null)
                {
                    controller.Setup(avatar, animatorController);
                }
            }
            
            // Set avatar to model to fusion Interpolation Target
            /*GetComponentInParent<NetworkTransform>().InterpolationTarget =
                targetAvatar.GetComponentInChildren<SkinnedMeshRenderer>().transform;*/
        }

        public void LoadAvatar(string url)
        {
            avatarObjectLoader = new AvatarObjectLoader();
            avatarObjectLoader.OnCompleted += OnLoadCompleted;
            avatarObjectLoader.OnFailed += OnLoadFailed;
            
            //remove any leading or trailing spaces
            avatarUrl = url.Trim(' ');
            avatarObjectLoader.LoadAvatar(avatarUrl);
            
            Debug.Log("loading avatar for: " + GetComponent<NetworkObject>().Id + " , with url " + avatarUrl);
        }
    }
}