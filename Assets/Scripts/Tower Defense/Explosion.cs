using System;
using UnityEngine;

namespace Tower_Defense
{
    public class Explosion : DefenceWarEntity
    {
        [SerializeField, Range(0f, 1f)] private float _duration = 0.5f;

        [SerializeField] private AnimationCurve _opcityCurve = default;

        [SerializeField] private AnimationCurve _scaleCurve = default;

        private float _age;

        private MeshRenderer _meshRenderer;

        private static int s_colorPropertyId = Shader.PropertyToID("_Color");

        private static MaterialPropertyBlock s_propertyBlock;

        private float _scale;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void Initialize(Vector3 position, float blastRadius, float damage = 0f)
        {
            if (damage > 0)
            {
                TargetPoint.FillBuffers(position, blastRadius);
                for (int i = 0; i < TargetPoint.BufferedCount; i++)
                {
                    TargetPoint.GetBuffered(i).Enemy.ApplyDamage(damage);
                }
            }


            transform.localPosition = position;
            _scale = 2 * blastRadius;
        }

        public override bool GameUpdate()
        {
            _age += Time.deltaTime;
            if (_age >= _duration)
            {
                OriginFactory.Reclaim(this);
                return false;
            }

            if (s_propertyBlock == null)
            {
                s_propertyBlock = new MaterialPropertyBlock();
            }

            float t = _age / _duration;

            Color c = Color.yellow;
            c.a = _opcityCurve.Evaluate(t);
            s_propertyBlock.SetColor(s_colorPropertyId, c);
            _meshRenderer.SetPropertyBlock(s_propertyBlock);
            transform.localScale = Vector3.one * (_scale * _scaleCurve.Evaluate(t));


            return true;
        }
    }
}