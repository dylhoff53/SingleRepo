using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Chapter.State
{
    public interface IBikeState
    {
        void Handle(BikeController controller);
    }
    public class BikeStateContext
    {
        public IBikeState CurrentState
        {
            get; set;
        }

        private readonly BikeController _bikeController;
        public BikeStateContext(BikeController bikeController)
        {
            _bikeController = bikeController;
        }

        public void Transition()
        {
            CurrentState.Handle(_bikeController);
        }

        public void Transition(IBikeState state)
        {
            CurrentState = state;
            CurrentState.Handle(_bikeController);
        }
    }
    public class BikeController : MonoBehaviour
    {
        public float maxSpeed = 2.0f;
        public float turnDistance = 2.0f;
        public float CurrentSpeed { get; set; }
        public Direction CurrentTurnDirection
        {
            get; private set;
        }

        private IBikeState
        _startState, _stopState, _turnState;
        private BikeStateContext _bikeStateContext;

        private void Start()
        {
            _bikeStateContext =
                new BikeStateContext(this);
            _startState = gameObject.AddComponent<BikeStartState>();
            _stopState = gameObject.AddComponent<BikeStopState>();
            _turnState = gameObject.AddComponent<BikeTurnState>();
            _bikeStateContext.Transition(_stopState);
        }

        public void StartBike()
        {
            _bikeStateContext.Transition(_startState);
        }

        public void StopBike()
        {
            _bikeStateContext.Transition(_stopState);
        }
        public void Turn(Direction direction)
        {
            CurrentTurnDirection = direction;
            _bikeStateContext.Transition(_turnState);
        }
    }

    public class BikeStopState : MonoBehaviour, IBikeState
    {
        private BikeController _bikeController;
        public void Handle(BikeController bikeController)
        {
            if (!_bikeController)
                _bikeController = bikeController;
            _bikeController.CurrentSpeed = 0;
        }
    }

    public class BikeStartState : MonoBehaviour, IBikeState
    {
        private BikeController _bikeController;
        public void Handle(BikeController bikeController)
        {
            if (!_bikeController)
                _bikeController = bikeController;
            _bikeController.CurrentSpeed =
            _bikeController.maxSpeed;
        }
        void Update()
        {
            if (_bikeController)
            {
                if (_bikeController.CurrentSpeed > 0)
                {
                    _bikeController.transform.Translate(
                    Vector3.forward * (
                    _bikeController.CurrentSpeed *
                    Time.deltaTime));
                }
            }
        }
    }

    public class BikeTurnState : MonoBehaviour, IBikeState
    {
        private Vector3 _turnDirection;
        private BikeController _bikeController;
        public void Handle(BikeController bikeController)
        {
            if (!_bikeController)
                _bikeController = bikeController;
            _turnDirection.x =
            (float)_bikeController.CurrentTurnDirection;
            if (_bikeController.CurrentSpeed > 0)
            {
                transform.Translate(_turnDirection *
                _bikeController.turnDistance);
            }
        }
    }
    public enum Direction
    {
        Left = -1,
        Right = 1
    }
}
