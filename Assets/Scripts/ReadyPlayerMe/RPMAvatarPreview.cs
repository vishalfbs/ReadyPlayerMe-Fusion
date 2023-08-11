using System;
using ReadyPlayerMe.Core;
using UnityEngine;

public class RPMAvatarPreview : MonoBehaviour
{
    private readonly Vector3 avatarPositionOffset = new Vector3(0, -0.08f, 0);
        
        /*[SerializeField]*/[Tooltip("RPM avatar URL or shortcode to load")] 
        public string avatarUrl;
        private GameObject avatar;
        private AvatarObjectLoader avatarObjectLoader;
        [SerializeField][Tooltip("Animator to use on loaded avatar")] 
        private RuntimeAnimatorController animatorController;
        [SerializeField][Tooltip("If true it will try to load avatar from avatarUrl on start")] 
        private bool loadOnStart = true;
        [SerializeField][Tooltip("Preview avatar to display until avatar loads. Will be destroyed after new avatar is loaded")]
        private GameObject previewAvatar;

        private void OnEnable()
        {
            avatarObjectLoader = new AvatarObjectLoader();
            avatarObjectLoader.OnCompleted += OnLoadCompleted;
            avatarObjectLoader.OnFailed += OnLoadFailed;
            
            if (previewAvatar != null)
            {
                SetupAvatar(previewAvatar);
            }
            if (loadOnStart)
            {
                if (!string.IsNullOrEmpty(RPMAvatarManager.AvatarUrl))
                {
                    avatarUrl = RPMAvatarManager.AvatarUrl;
                }
                LoadAvatar(avatarUrl);
            }
        }

        private void OnDisable()
        {
            avatarObjectLoader.OnCompleted -= OnLoadCompleted;
            avatarObjectLoader.OnFailed -= OnLoadFailed;
        }

        private void Start()
        {
            /*avatarObjectLoader = new AvatarObjectLoader();
            avatarObjectLoader.OnCompleted += OnLoadCompleted;
            avatarObjectLoader.OnFailed += OnLoadFailed;*/
            
            /*if (previewAvatar != null)
            {
                SetupAvatar(previewAvatar);
            }
            if (loadOnStart)
            {
                if (!string.IsNullOrEmpty(RPMAvatarManager.AvatarUrl))
                {
                    avatarUrl = RPMAvatarManager.AvatarUrl;
                }
                LoadAvatar(avatarUrl);
            }*/
        }

        private void OnLoadFailed(object sender, FailureEventArgs args)
        {
            Debug.Log("Loading avatar failed: " + args.Message);
        }

        private void OnLoadCompleted(object sender, CompletionEventArgs args)
        {
            if (previewAvatar != null)
            {
                Destroy(previewAvatar);
                previewAvatar = null;
            }
            SetupAvatar(args.Avatar);
        }

        private void SetupAvatar(GameObject  targetAvatar)
        {
            if (avatar != null)
            {
                Destroy(avatar);
            }
            
            avatar = targetAvatar;
            // Re-parent and reset transforms
            avatar.transform.parent = transform;
            avatar.transform.localPosition = avatarPositionOffset;
            avatar.transform.localRotation = Quaternion.Euler(0, 0, 0);
            avatar.GetComponent<Animator>().runtimeAnimatorController = animatorController;
            avatar.AddComponent<CapsuleCollider>();
            var capsuleCollider = avatar.GetComponent<CapsuleCollider>();
            capsuleCollider.radius = 0.3f;
            capsuleCollider.height = 1.8f;
            capsuleCollider.center = Vector3.up * 0.81f; 
            avatar.AddComponent<TouchRotate>();
            
        }

        public void LoadAvatar(string url)
        {
            //remove any leading or trailing spaces
            avatarUrl = url.Trim(' ');
            avatarObjectLoader.LoadAvatar(avatarUrl);
        }
}
