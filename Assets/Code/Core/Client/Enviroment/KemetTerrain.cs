using System.Collections;
using Code.Core.Client.Net;
using Code.Core.Client.UI.Controls;
using Code.Core.Client.Units;
using Code.Core.Client.Units.Extensions;
using Code.Core.Shared.NET;
using Code.Libaries.Generic;
using Code.Libaries.Net.Packets.ForServer;
using UnityEngine;

namespace Code.Core.Client.Terrain
{
    [RequireComponent(typeof(Clickable))]
    internal class KemetTerrain : MonoSingleton<KemetTerrain>
    {

        [SerializeField]
        private TerrainCollider
            _cachedTerrainColliderReference;

        private bool _wasMovingLastFrame = false;

        public TerrainCollider TerrainCollider
        {
            get
            {
                if (_cachedTerrainColliderReference == null)
                {
                    _cachedTerrainColliderReference = GetComponent<TerrainCollider>();
                }
                return _cachedTerrainColliderReference;
            }
        }

        private void Start()
        {
            /*var clickable = GetComponent<Clickable>();
            clickable.OnRightMouseHold += MoveMyPlayerToMouse;*/
        }

        private void Update()
        {
            if (Input.GetMouseButton(1))
            {
                if (PlayerUnit.MyPlayerUnit != null)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    float distance = 1000;
                    int layerMask = 1 << 7;
                    layerMask = ~layerMask;
                    if (Physics.Raycast(ray, out hit, distance, layerMask))
                    {
                        SendPacket(hit, true);
                        _wasMovingLastFrame = true;
                    }
                }
            }
            else if(_wasMovingLastFrame)
            {
                //This will make the player stop, once he's not holding right mouse anymore
                RaycastHit hit = new RaycastHit();
                StartCoroutine(StopAndResumeWalk());
                _wasMovingLastFrame = false;
            }
        }

        #region DISABLED

        private void MoveMyPlayerFromMouse()
        {
            if (PlayerUnit.MyPlayerUnit != null)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float distance = 100;
                int layerMask = 1 << 7;
                layerMask = ~layerMask;
                if (Physics.Raycast(ray, out hit, distance, layerMask))
                {
                    SendPacket(hit, true, true);
                }
            }
        }

        private void MoveMyPlayerToClick()
        {
            if (PlayerUnit.MyPlayerUnit != null)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float distance = 100;
                int layerMask = 1 << 7;
                layerMask = ~layerMask;
                if (Physics.Raycast(ray, out hit, distance, layerMask))
                {
                    SendPacket(hit, false);
                }
            }
        }

        #endregion


        private void SendPacket(RaycastHit hit)
        {
            SendPacket(hit, false, false);
        }

        private void SendPacket(RaycastHit hit, bool hold)
        {
            SendPacket(hit, hold, false);
        }

        private void SendPacket(RaycastHit hit, bool hold, bool invert)
        {
            WalkRequestPacket update = new WalkRequestPacket();

            if (hold)
            {
                Vector3 myPos = PlayerUnit.MyPlayerUnit.MovementTargetPosition;

                if (invert)
                {
                    //used for going away from the hitpoint
                    Vector3 inverted = (hit.point - myPos).normalized;
                    inverted.x *= -1;
                    inverted.z *= -1;
                    update.DirecionVector = inverted;
                }
                else
                {
                    Vector3 difference = (hit.point - myPos);
                    if (difference.magnitude > 1f)
                    {
                        update.DirecionVector = difference.normalized;
                    }
                    else
                    {
                        update.DirecionVector = difference;
                    }
                }
            }
            else
            {
                update.DirecionVector = hit.point;
            }

            ClientCommunicator.Instance.SendToServer(update);
        }


        IEnumerator StopAndResumeWalk()
        {
            ClientCommunicator.Instance.SendToServer(new InputEventPacket(PacketEnums.INPUT_TYPES.StopWalk));
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            ClientCommunicator.Instance.SendToServer(new InputEventPacket(PacketEnums.INPUT_TYPES.ContinueWalk));
        }
    
    }
}
