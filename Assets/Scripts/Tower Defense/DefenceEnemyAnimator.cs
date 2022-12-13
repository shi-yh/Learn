using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Tower_Defense
{
    ///仅仅使用animator来记录动画，而非控制动画，状态机由自己的脚本控制
    public struct DefenceEnemyAnimator
    {
        public enum Clip
        {
            Move,
            Intro,
            Outro,
            Dying,
            
            Max,
        }

        private PlayableGraph _graph;

        private AnimationMixerPlayable _mixer;

        private Clip _previousClip;

        private float _translationProgress;

        public Clip CurrentClip { get; private set; }

        public bool IsDone => GetPlayable(CurrentClip).IsDone();

        private const float transitionSpeed = 5f;

        public void Configure(Animator animator, DefenceAnimationConfig config)
        {
            ///初始化
            _graph = PlayableGraph.Create();
            ///设置更新类型
            _graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

            _mixer = AnimationMixerPlayable.Create(_graph, (int)Clip.Max);


            AnimationClipPlayable clip = AnimationClipPlayable.Create(_graph, config.Intro);
            ///需要明确设置非循环动画的周期，以确保它的结束可以被正确的获取            
            clip.SetDuration(config.Intro.length);
            _mixer.ConnectInput((int)Clip.Intro, clip, 0);


            clip = AnimationClipPlayable.Create(_graph, config.Move);
            ///如果不暂停，那么这个动画也会进入播放状态，只是因为后续设置了权重，所以这个动画的播放不响应
            /// 为了让动画切换后，表现效果更为正确，所以需要先暂停
            clip.Pause();
            _mixer.ConnectInput((int)Clip.Move, clip, 0);


            clip = AnimationClipPlayable.Create(_graph, config.Outro);
            clip.SetDuration(config.Outro.length);
            clip.Pause();
            _mixer.ConnectInput((int)Clip.Outro, clip, 0);

            
            clip = AnimationClipPlayable.Create(_graph, config.Dying);
            clip.SetDuration(config.Dying.length);
            clip.Pause();
            _mixer.ConnectInput((int)Clip.Dying, clip, 0);
            

            var output = AnimationPlayableOutput.Create(_graph, "Enemy", animator);

            output.SetSourcePlayable(_mixer);
        }

        public void PlayIntro()
        {
            SetWeight(Clip.Intro, 1f);
            CurrentClip = Clip.Intro;
            _graph.Play();
            _translationProgress = -1f;
        }

        public void PlayOutro()
        {
            BeginTransition(Clip.Outro);
        }

        public void PlayDying()
        {
            BeginTransition(Clip.Dying);
        }


        void SetWeight(Clip clip, float weight)
        {
            _mixer.SetInputWeight((int)clip, weight);
        }


        public void PlayMove(float speed)
        {
            GetPlayable(Clip.Move).SetSpeed(speed);
            BeginTransition(Clip.Move);
        }

        Playable GetPlayable(Clip clip)
        {
            return _mixer.GetInput((int)clip);
        }


        public void Stop()
        {
            _graph.Stop();
        }

        public void OnDestroy()
        {
            _graph.Destroy();
        }

        public void GameUpdate()
        {
            if (_translationProgress>=0)
            {
                _translationProgress += Time.deltaTime * transitionSpeed;

                if (_translationProgress >= 1f)
                {
                    SetWeight(CurrentClip, 1f);
                    SetWeight(_previousClip, 0f);
                    GetPlayable(_previousClip).Pause();
                    _translationProgress = -1f;
                }
                else
                {
                    SetWeight(CurrentClip, _translationProgress);
                    SetWeight(_previousClip, 1 - _translationProgress);
                }
            }
            
        }


        void BeginTransition(Clip nextClip)
        {
            _previousClip = CurrentClip;
            CurrentClip = nextClip;
            _translationProgress = 0f;
            GetPlayable(nextClip).Play();
        }
    }
}