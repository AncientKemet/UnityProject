using System.Collections.Generic;
using Code.Code.Libaries.Net;
using Code.Libaries.Generic.Trees;
using Code.Libaries.Net.Packets.ForClient;
using Server.Model.Entities.Human;
using Server.Model.Extensions;
using Server.Model.Extensions.UnitExts;
using UnityEngine;

namespace Server.Model.Entities
{
    public class ServerUnit : WorldEntity, IQuadTreeObject
    {
        public UnitMovement Movement;
        public UnitCombat Combat;
        public UnitDisplay Display;
        public UnitAnim Anim;
        public UnitAttributes Attributes;
        public UnitFocus Focus;
        public UnitActions Actions;

        private List<UnitUpdateExt> _updateExtensions;

        public override World CurrentWorld
        {
            get { return base.CurrentWorld; }
            set
            {
                if (base.CurrentWorld != null)
                {
                    if (CurrentBranch != null)
                    {
                        CurrentBranch.RemoveObject(this);
                    }
                }
                base.CurrentWorld = value;
                value.Tree.AddObject(this);
            }
        }

        public virtual void Awake()
        {
            _updateExtensions = new List<UnitUpdateExt>();

            AddExt(Movement = new UnitMovement());
            AddExt(Display = new UnitDisplay());
            
            AddExt(Actions = new UnitActions());
            AddExt(Focus = new UnitFocus());

            //in the end find all updatable extensions
            foreach (EntityExtension extension in Extensions)
            {
                if (extension is UnitUpdateExt)
                {
                    _updateExtensions.Add(extension as UnitUpdateExt);
                }
            }

            _updateExtensions.Sort(
                (ext, updateExt) => ext.UpdateFlag().CompareTo(updateExt.UpdateFlag())
                );
        }

        public override void Progress()
        {
            ReCreateUpdatePacket();

            if (_updatePacket != null)
            {
                SendPacketToPlayersAround(_updatePacket);
            }

            base.Progress();
        }

        private void SendPacketToPlayersAround(BasePacket packet)
        {
            foreach (IQuadTreeObject objectAround in CurrentBranch.ObjectsVisible)
            {
                Player playerAround = objectAround as Player;
                if (playerAround != null)
                {
                    playerAround.Client.ConnectionHandler.SendPacket(packet);
                }
            }
        }

        #region QUAD TREE IMPL
        private QuadTree _currentBranch;
        private Vector2 _quadTreePos = Vector2.zero;

        public QuadTree CurrentBranch
        {
            get { return _currentBranch; }
            set { _currentBranch = value; }
        }


        public Vector2 GetPosition()
        {
            return _quadTreePos + new Vector2(0.01f, 0.01f);
        }

        public Vector2 PositionChange()
        {
            Vector2 r = new Vector2(Movement.Position.x, Movement.Position.z) - _quadTreePos;
            _quadTreePos = new Vector2(Movement.Position.x, Movement.Position.z);
            return r;
        }
        #endregion QUAD TREE IMPL

        #region UPDATE_PACKET
        private UnitUpdatePacket _updatePacket;

        private void ReCreateUpdatePacket()
        {

            _updatePacket = null;
            //Crate mask
            int mask = 0;
            foreach (UnitUpdateExt updateExtension in _updateExtensions)
            {
                if (updateExtension.WasUpdate())
                {
                    mask = mask | updateExtension.UpdateFlag();
                }
            }

            if (mask == 0)
                return;

            _updatePacket = new UnitUpdatePacket();
            _updatePacket.UnitID = ID;

            //add mask
            _updatePacket.SubPacketData.addByte(mask);

            //serialize the rest of the packet
            foreach (UnitUpdateExt updateExtension in _updateExtensions)
            {
                if (updateExtension.WasUpdate())
                {
                    updateExtension.SerializeUpdate(_updatePacket.SubPacketData);
                }
            }

#if DEBUG_NETWORK
            string log = "";
            log += "\n" + "Server created packet size " + _updatePacket;
            Debug.Log(log);
#endif
        }

        #endregion

    }
}

