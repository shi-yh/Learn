using System;
using UnityEngine;

namespace Tower_Defense
{
    public enum DefenceEnemyType
    {
        Small,
        Medium,
        Large,
        max
    }

    public class DefenceEnemy : GameBehavior
    {
        [SerializeField] private Transform _model = default;

        [SerializeField] private DefenceAnimationConfig _animationConfig = default;

        private DefenceEnemyFactory _originEnemyFactory;

        private DefenceEnemyAnimator _animator;

        /// <summary>
        /// 记录的当前瓦片和目的地瓦片
        /// </summary>
        private DefenceGameTile _tileFrom, _tileTo;

        private Vector3 _positionFrom, _positionTo;

        private float _progress, _progressFactor;

        private Direction _direction;

        private DirectionChange _directionChange;

        private float _directionAngleFrom, _directionAngleTo;

        private float _pathOffse;

        private float _speed;

        private Collider _targetPointCollider;


        private float Health { get; set; }
        public float Scale { get; private set; }

        public DefenceEnemyFactory OriginFactory
        {
            get => _originEnemyFactory;
            set { _originEnemyFactory = value; }
        }

        public Collider TargetPointCollider
        {
            set { _targetPointCollider = value; }
        }

        public bool IsValidTarget => _animator.CurrentClip == DefenceEnemyAnimator.Clip.Move;

        private void Awake()
        {
            _animator.Configure(_model.GetChild(0).gameObject.AddComponent<Animator>(), _animationConfig);
        }


        public void SpawnOn(DefenceGameTile tile)
        {
            _tileFrom = tile;
            _tileTo = tile.NextTileOnPath;
            _progress = 0f;
            PrepareIntro();
        }

        private void PrepareIntro()
        {
            _positionFrom = _tileFrom.transform.localPosition;
            transform.localPosition = _positionFrom;
            _positionTo = _tileFrom.ExitPoint;
            _direction = _tileFrom.PathDirection;
            _directionChange = DirectionChange.None;
            _directionAngleFrom = _directionAngleTo = _direction.GetAngle();
            _model.localPosition = new Vector3(_pathOffse, 0f);
            transform.localRotation = _direction.GetRotation();
            _progressFactor = 2 * _speed;
        }

        private void PrepareOutro()
        {
            _positionTo = _tileFrom.transform.localPosition;
            _directionChange = DirectionChange.None;
            _directionAngleTo = _direction.GetAngle();
            _model.localPosition = new Vector3(_pathOffse, 0f);
            transform.localRotation = _direction.GetRotation();
            _progressFactor = 2 * _speed;
        }

        private void PrepareNextState()
        {
            _tileFrom = _tileTo;
            _tileTo = _tileFrom.NextTileOnPath;
            _positionFrom = _positionTo;

            if (_tileTo == null)
            {
                PrepareOutro();
                return;
            }

            _positionTo = _tileFrom.ExitPoint;
            _directionChange = _direction.GetDirectionChangeTo(_tileFrom.PathDirection);
            _direction = _tileFrom.PathDirection;
            _directionAngleFrom = _directionAngleTo;

            switch (_directionChange)
            {
                case DirectionChange.None:
                    PrepareForward();
                    break;
                case DirectionChange.TurnRight:
                    PrepareTurnRight();
                    break;
                case DirectionChange.TurnLeft:
                    PrepareTurnLeft();
                    break;
                case DirectionChange.TurnAround:
                    PrepareTurnAround();
                    break;
                default:
                    PrepareTurnAround();
                    break;
            }
        }

        private void PrepareForward()
        {
            transform.localRotation = _direction.GetRotation();

            _directionAngleTo = _direction.GetAngle();

            _model.localPosition = new Vector3(_pathOffse, 0f);

            _progressFactor = _speed;
        }

        void PrepareTurnRight()
        {
            _directionAngleTo = _directionAngleFrom + 90f;
            _model.localPosition = new Vector3(_pathOffse - 0.5f, 0f);
            transform.localPosition = _positionFrom + _direction.GetHalfVector();
            ///四分之一圆半径
            _progressFactor = _speed / (Mathf.PI * 0.5f * (0.5f - _pathOffse));
        }

        void PrepareTurnLeft()
        {
            _directionAngleTo = _directionAngleFrom - 90f;
            _model.localPosition = new Vector3(_pathOffse + 0.5f, 0f);
            transform.localPosition = _positionFrom + _direction.GetHalfVector();
            _progressFactor = _speed / (Mathf.PI * 0.5f * (0.5f + _pathOffse));
        }

        void PrepareTurnAround()
        {
            _directionAngleTo = _directionAngleFrom + (_pathOffse < 0f ? 180f : -180f);
            _model.localPosition = new Vector3(_pathOffse, 0f);
            transform.localPosition = _positionFrom;
            _progressFactor = _speed / (Mathf.PI * Mathf.Max(Mathf.Abs(_pathOffse), 0.2f));
        }


        public override bool GameUpdate()
        {
            _animator.GameUpdate();


            if (_animator.CurrentClip == DefenceEnemyAnimator.Clip.Intro)
            {
                if (!_animator.IsDone)
                {
                    return true;
                }

                _animator.PlayMove(_speed / Scale);
                _targetPointCollider.enabled = true;
            }
            else if (_animator.CurrentClip >= DefenceEnemyAnimator.Clip.Outro)
            {
                if (_animator.IsDone)
                {
                    Recycle();
                    return false;
                }

                return true;
            }


            if (Health <= 0)
            {
                _animator.PlayDying();
                _targetPointCollider.enabled = false;

                return true;
            }

            _progress += Time.deltaTime * _progressFactor;
            while (_progress >= 1f)
            {
                if (_tileTo == null)
                {
                    DefenceGame.EnemyReachedDestination();
                    _animator.PlayOutro();
                    _targetPointCollider.enabled = false;

                    return true;
                }

                _progress = (_progress - 1f) / _progressFactor;

                PrepareNextState();

                ///如果进度随状态变化，则剩余的进度不能直接应用到下一个状态
                _progress *= _progressFactor;
            }

            if (_directionChange == DirectionChange.None)
            {
                transform.localPosition = Vector3.LerpUnclamped(_positionFrom, _positionTo, _progress);
            }
            else
            {
                float angle = Mathf.LerpUnclamped(_directionAngleFrom, _directionAngleTo, _progress);
                transform.localRotation = Quaternion.Euler(0, angle, 0f);
            }

            return true;
        }

        public override void Recycle()
        {
            _animator.Stop();
            _originEnemyFactory.Reclaim(this);
        }

        public void Initialize(float scale, float pathOffset, float speed, float health)
        {
            _model.localScale = new Vector3(scale, scale, scale);
            this._pathOffse = pathOffset;
            _speed = speed;
            Scale = scale;
            Health = health;
            _animator.PlayIntro();
            _targetPointCollider.enabled = false;
        }

        public void ApplyDamage(float damage)
        {
            Health -= damage;
        }
    }
}