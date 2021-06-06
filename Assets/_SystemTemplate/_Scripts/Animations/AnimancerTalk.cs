//using Animancer;
//using UnityEngine;

//[RequireComponent(typeof(AnimancerComponent))]
//[ExecuteInEditMode]
//public class AnimancerTalk : MonoBehaviour
//{
//    [SerializeField] private AnimancerComponent _layeredAnimancer;
//    [SerializeField] private AnimationClip _idle;
//    [SerializeField] private AnimationClip _talking;
//    [SerializeField] private AvatarMask _talkMask;
//    private const int ActionLayer = 1;
//    private const float FadeDuration = 0.25f;

//    private void Start()
//    {
//        _layeredAnimancer = GetComponent<AnimancerComponent>();

//        _idle = Resources.Load<AnimationClip>("TalkingIdle");
//        _talking = Resources.Load<AnimationClip>("TalkingTalk");
//        _talkMask = Resources.Load<AvatarMask>("TalkingMask");
//    }

//    private void OnEnable()
//    {
//        // Set the mask for layer 1 (this automatically creates the layer).
//        _layeredAnimancer.Layers[ActionLayer].SetMask(_talkMask);

//        // Since we set a mask it will use the name of the mask in the Inspector by default. But we can also
//        // replace it with a custom name. Either way, layer names are only used in the Inspector and any calls to
//        // this method will be compiled out of runtime builds.
//        _layeredAnimancer.Layers[ActionLayer].SetDebugName("Action Layer");
//    }

//    public void EnableTalk(bool isEnabled, float speed)
//    {
//        if (_layeredAnimancer.Animator==null)
//        {
//            _layeredAnimancer.Animator = GetComponentInParent<Animator>();
//            if (_layeredAnimancer.Animator == null)
//            {
//                _layeredAnimancer.Animator = transform.parent.gameObject.AddComponent<Animator>();
//            }
//        }

//        var info = _layeredAnimancer.Animator?.GetCurrentAnimatorClipInfo(0);
//        if (info!=null && info.Length > 0)
//        {
//            var clip = info[0].clip;

//            if (clip !=null)
//            {
//                _layeredAnimancer.Play(clip);
//                var state = _layeredAnimancer.Layers[ActionLayer].Play(isEnabled ? _talking : _idle, FadeDuration, FadeMode.FromStart);
//                state.Speed = speed;
//            }
//        }

//        //state.Events.OnEnd = () => _layeredAnimancer.Layers[ActionLayer].StartFade(0, FadeDuration);
//    }
//}
