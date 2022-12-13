using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tower_Defense
{
    [CreateAssetMenu]
    public class DefenceAnimationConfig : ScriptableObject
    {
        [SerializeField] private AnimationClip _move = default, _intro = default, _outro = default,_dying;


        public AnimationClip Move => _move;
        public AnimationClip Intro => _intro;
        public AnimationClip Outro => _outro;

        public AnimationClip Dying => _dying;

    }
}