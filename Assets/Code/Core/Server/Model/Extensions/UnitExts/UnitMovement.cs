#if SERVER
using Pathfinding;
using Server.Model.Entities;
using Server.Model.Entities.Human;
using UnityEngine;

namespace Server.Model.Extensions.UnitExts
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
        private UnitCombat _combat;
        private bool _positionUpdate = false;
        private bool _rotationUpdate = false;

        //destination variables
        private Vector3 destination;
        private Path _path;
        private bool _lookingForPath = false;
        private bool _dontWalk;
        private float _rotationSpeed= 5f;
        private System.Action OnArrive;

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

        public ServerUnit Unit { get; private set; }

        public Vector3 Force
        {
            get { return _force; }
        }

        public float Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                _rotationUpdate = true;
            }
        }

        public bool CanMove { get; set; }

        public bool CanRotate { get; set; }

        public bool Running { get; set; }

        public float CurrentSpeed
        {
            get
            {
                return _baseSpeed * Unit.Attributes.MovementSpeed
                    *
                    (Running ? 3f : 1f);
            }
        }

        //Methods
        public override void Progress()
        {
            base.Progress();

            if (Running && Unit.Combat.Energy < 20)
            {
                Running = false;
            }

            if (_path != null)
            {
                if (_currentWaypoint >= _path.vectorPath.Count)
                {
                    _path = null;
                    if (OnArrive != null)
                        OnArrive();
                }
                else
                {
                    Vector3 waypoint = _path.vectorPath[_currentWaypoint];
                    Vector3 dir = waypoint - _position;

                    float dirMagnitude = dir.magnitude;

                    if (dirMagnitude > 0.3f)
                    {
                        if (RotateTo(Quaternion.LookRotation(dir * _rotationSpeed * Unit.Attributes.Mobility * Time.fixedDeltaTime + (Quaternion.Euler(new Vector3(0, _rotation, 0)) * Vector3.forward)).eulerAngles.y))
                        {
                            //eq holding space
                            if (!_dontWalk && Vector3.Distance(Position + Forward*1.25f, waypoint)  < Vector3.Distance(Position + Forward * -1, waypoint))
                            {
                                MoveForward(CurrentSpeed * Time.fixedDeltaTime);
                                _position.y += (waypoint.y - _position.y)/5f;
                            }
                        }
                    }

                    //Check if we are close enough to the next waypoint
                    //If we are, proceed to follow the next waypoint
                    Vector3 wayPoint = _path.vectorPath[_currentWaypoint];
                    if (Vector2.Distance(new Vector2(_position.x, _position.z), new Vector2(wayPoint.x, wayPoint.z)) < Unit.Display.Size * 0.75f)
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
            MoveTo(_position + Forward * speed);

            ServerUnit serverUnit = entity as ServerUnit;
            if (serverUnit != null)
            {
                serverUnit.Combat.ReduceEnergy(speed*0.3f);
            }
        }

        private Vector3 Forward
        {
            get { return Quaternion.Euler(0, _rotation, 0) * Vector3.forward; }
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
                if (Vector3.Distance(destination, newPosition) < 0.1f)
                    return;
                destination = newPosition;
                _seeker.StartPath(_position, destination, OnPathWasFound);
                _lookingForPath = true;
                OnArrive = null;
            }
        }

        public void WalkTo(Vector3 newPosition, System.Action<int,string> OnArrive, int unitId, string action)
        {
            WalkTo(newPosition, () => OnArrive(unitId, action));
        }

        public void WalkTo(Vector3 newPosition, System.Action OnArrive)
        {
            if (CanMove && !_lookingForPath)
            {
                /*if (Vector3.Distance(destination, newPosition) < 0.1f)
                {
                    this.OnArrive = OnArrive;
                    return;
                }*/

                destination = newPosition;
                _seeker.StartPath(_position, destination, OnPathWasFound);
                _lookingForPath = true;

                this.OnArrive = OnArrive;
            }
        }

        public void WalkWay(Vector3 direction)
        {
            WalkTo(_position + direction * (Unit.Display.Size +0.5f));
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
            Teleported = true;
            _wasUpdate = true;
        }

        public bool Teleported { get; private set; }

        private void OnPathWasFound(Path path)
        {
            _lookingForPath = false;
            if (!path.error)
            {
                _currentWaypoint = 0;
                _path = path;
                
            }
            else
            {
                Debug.LogError("Error finding path: "+path.errorLog);
            }
        }

        protected override void OnExtensionWasAdded()
        {
            base.OnExtensionWasAdded();

            Unit = entity as ServerUnit;

            _seeker = entity.gameObject.AddComponent<Seeker>();
            entity.gameObject.AddComponent<SimpleSmoothModifier>();

            if (entity is Human)
            {
                Teleport(new Vector3(20, 10, 20));
                WalkTo(new Vector3(21, 10, 21));

                CanMove = true;
                CanRotate = true;
            }
        }

        #region StateSerialization
        protected override void pSerializeState(Code.Code.Libaries.Net.ByteStream packet)
        {
            packet.addFlag(true, true, true);
            packet.addPosition6B(_position);

            packet.addFloat4B(_rotation);
        }

        protected override void pSerializeUpdate(Code.Code.Libaries.Net.ByteStream packet)
        {
            packet.addFlag(_positionUpdate, _rotationUpdate, Teleported);
            if (_positionUpdate)
            {
                packet.addPosition6B(_position);
            }
            if (_rotationUpdate)
            {
                packet.addFloat4B(_rotation);
            }

            Teleported = false;
        }
        #endregion StateSerialization
        
        //Update flag
        public override byte UpdateFlag()
        {
            return 0x01;
        }

    }
}
#endif
