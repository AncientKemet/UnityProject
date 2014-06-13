using System.Security.Cryptography;
using Pathfinding;
using UnityEngine;
using Code.Core.Server.Model.Entities;

namespace Code.Core.Server.Model.Extensions.UnitExts
{

    public class UnitMovement : UnitUpdateExt
    {
        //how fast does force fadeout?
        private const float ForceFade = 0.80f;

        //how strong the force is?
        private const float ForceWeight = 0.0f;

        //important variables
        private Vector3 _position = Vector3.one;
        private float _rotation = 0;

        private float _baseSpeed = 2f;

        private Vector3 _force = Vector3.zero;

        //Pathfinding seeker
        private Seeker _seeker;
        private int _currentWaypoint = 0;

        //other variables
        private MovementState currentState = new MovementState();
        private UnitCombat _combat;
        private bool _positionUpdate = false;
        private bool _rotationUpdate = false;

        //destination variables
        private Vector3 destination;
        private Path _path;
        private bool _lookingForPath = false;
        private bool _dontWalk;
        private float _rotationSpeed= 5f;

        //property getters
        public Vector3 Position
        {
            get { return _position; }
            private set
            {
                if (_position != value)
                {
                    entity.transform.position = value;
                    _position = value;
                    _positionUpdate = true;
                    _wasUpdate = true;
                }
            }
        }

        public Vector3 Force
        {
            get { return _force; }
        }

        public float Rotation
        {
            get { return _rotation; }
            private set
            {
                _rotation = value;
                _rotationUpdate = true;
            }
        }

        public bool CanMove
        {
            get { return !currentState.stunned; }
        }

        public bool CanRotate
        {
            get { return !currentState.stunned; }
        }

        public bool Running { get; set; }

        public float CurrentSpeed { get { return _baseSpeed*(Running ? 5f* (Combat == null ? 0 : (Combat.Energy / 100f) / 2f) : 1f); } }

        private UnitCombat Combat
        {
            get
            {
                if (_combat == null)
                    _combat = entity.GetExt<UnitCombat>();
                return _combat;
            }
        }

        //Methods
        public override void Progress()
        {
            base.Progress();

            if (Running && Combat.Energy < 40)
            {
                Running = false;
            }

            if (_path != null)
            {
                if (_currentWaypoint >= _path.vectorPath.Count)
                {
                    _path = null;
                }
                else
                {
                    Vector3 waypoint = _path.vectorPath[_currentWaypoint];
                    Vector3 dir = waypoint - _position;

                    float dirMagnitude = dir.magnitude;

                    if (dirMagnitude > 0.3f && (dir + _force).magnitude > 0.1f)
                    {
                        if (RotateTo(Quaternion.LookRotation(dir * _rotationSpeed * Time.fixedDeltaTime + (Quaternion.Euler(new Vector3(0, _rotation, 0)) * Vector3.forward)).eulerAngles.y))
                        {
                            //eq holding space
                            if (!_dontWalk && (dir + _force).magnitude > 0.03f)
                            {
                                MoveForward(CurrentSpeed * Time.fixedDeltaTime);
                                _position.y += (waypoint.y - _position.y)/5f;
                            }
                        }
                    }

                    //Check if we are close enough to the next waypoint
                    //If we are, proceed to follow the next waypoint
                    Vector3 wayPoint = _path.vectorPath[_currentWaypoint];
                    if (Vector2.Distance(new Vector2(_position.x, _position.z), new Vector2(wayPoint.x, wayPoint.z)) < 0.5f)
                    {
                        _currentWaypoint++;
                    }
                }
            }

            if (_dontWalk)
            {
                _path = null;
            }

            if (_force.magnitude > 0.017f)
            {
                Position += Force;
            }
            _force *= ForceFade;
            
            /*
            RotateTo(Quaternion.LookRotation(new Vector3(destination.x, 0, destination.z) - new Vector3(_position.x, 0, _position.z)).eulerAngles.y);

            if (Vector2.Distance(new Vector2(destination.x, destination.z), new Vector2()) > 0.5f)
            {
                MoveTo(_position + Quaternion.Euler(new Vector3(0,_rotation,0)) * Vector3.forward * _baseSpeed);
            }*/
        }

        private void MoveForward(float speed)
        {
            MoveTo(_position + Quaternion.Euler(0, _rotation, 0) * Vector3.forward * speed);

            ServerUnit serverUnit = entity as ServerUnit;
            if (serverUnit != null)
            {
                serverUnit.unitCombat.ReduceEnergy(speed*(Running ? 2f : 1f));
            }
        }

        /// <summary>
        /// Instantly moves to new DirecionVector, adds force.
        /// </summary>
        /// <param name="newPosition"></param>
        private void MoveTo(Vector3 newPosition)
        {
            if (CanMove)
            {
                _force += (newPosition - _position) * ForceWeight;
                Position = newPosition;
                _wasUpdate = true;
            }
        }


        public void StopWalking()
        {
            _dontWalk = true;
        }

        public void ContinueWalking()
        {
            _dontWalk = false;
        }

        /// <summary>
        /// Smoothly walks to the destination.
        /// </summary>
        /// <param name="newPosition">Destination of walk.</param>
        public void WalkTo(Vector3 newPosition)
        {
            if (CanMove && !_lookingForPath)
            {
                destination = newPosition;
                _seeker.StartPath(_position, destination, OnPathWasFound);
                _lookingForPath = true;
            }
        }

        public void WalkWay(Vector3 direction)
        {
            WalkTo(_position + direction);
        }

        private bool RotateTo(float newRotation)
        {
            if (CanRotate)
            {
                Rotation = newRotation;
                _wasUpdate = true;
                return true;
            }
            return false;
        }
        
        public void Teleport(Vector3 location)
        {
            Position = location;
            _wasUpdate = true;
        }

        private void OnPathWasFound(Path path)
        {
            _lookingForPath = false;
            if (!path.error)
            {
                _path = path;
                _currentWaypoint = 0;
            }
        }

        
        
        protected override void OnExtensionWasAdded()
        {
            base.OnExtensionWasAdded();

            _seeker = entity.gameObject.AddComponent<Seeker>();
            entity.gameObject.AddComponent<SimpleSmoothModifier>();

            Teleport(new Vector3(20, 10, 20));
            WalkTo(new Vector3(21, 10, 21));
        }

        #region StateSerialization
        protected override void pSerializeState(Code.Libaries.Net.ByteStream packet)
        {
            packet.addFlag(true, true);
            packet.addPosition6B(_position);

            packet.addFloat4B(_rotation);
        }

        protected override void pSerializeUpdate(Code.Libaries.Net.ByteStream packet)
        {
            packet.addFlag(_positionUpdate, _rotationUpdate);
            if (_positionUpdate)
            {
                packet.addPosition6B(_position);
            }
            if (_rotationUpdate)
            {
                packet.addFloat4B(_rotation);
            }
        }
        #endregion StateSerialization

        
        
        //Update flag
        public override byte UpdateFlag()
        {
            return 0x01;
        }

    }

    internal class MovementState
    {
        public bool stunned = false;
    }
}
