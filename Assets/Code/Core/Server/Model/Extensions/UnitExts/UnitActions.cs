using System.Collections.Generic;
using Code.Core.Client.UI;
using Code.Core.Shared.Content.Types;
using Code.Libaries.Generic.Managers;
using Code.Libaries.Net.Packets.ForServer;
using Code.Libaries.UnityExtensions.Independent;
using Server.Model.Entities;
using Server.Model.Entities.Human;
using Server.Model.Entities.Items;
using UnityEngine;

namespace Server.Model.Extensions.UnitExts
{
    public class UnitActions : EntityExtension
    {
        public ServerUnit Unit { get; private set; }

        private List<Spell> Spells = new List<Spell>(4);
        private int _currentCastingSpellId = -1;
        private float _currentSpellTime;

        public override void Progress()
        {
            if (CurrentCastingSpell != null)
            {
                if (CurrentCastingSpell.HasEnergyCost)
                {
                    Unit.Combat.ReduceEnergy(Time.fixedDeltaTime * CurrentCastingSpell.EnergyCost * Unit.Combat.EnergyRatio);
                }
                _currentSpellTime += Time.fixedDeltaTime * Unit.Combat.EnergyRatio;
                CurrentCastingSpell.StrenghtChanged(Unit, CurrentCastStrenght);
            }
        }

        public float CurrentCastStrenght
        {
            get { return _currentSpellTime / CurrentCastingSpell.MaxDuration; }
        }

        public Spell CurrentCastingSpell
        {
            get
            {
                if (_currentCastingSpellId == -1) { return null; }

                return Spells[_currentCastingSpellId];
            }
        }

        protected override void OnExtensionWasAdded()
        {
            base.OnExtensionWasAdded();
            Unit = entity as ServerUnit;

            for (int i = 0; i < 4; i++)
            {
                Spells.Add(null);
                EquipSpell(ContentManager.I.Spells[i], i);
            }
        }

        private void EquipSpell(Spell spell, int i)
        {
            Spells[i] = spell;

            Player p = Unit as Player;

            if (p != null)
            {
                CorotineManager.Instance.RunCoroutine(
                    delegate
                    {
                        UIInterfaceEvent packet = new UIInterfaceEvent();
                        packet.interfaceId = InterfaceType.ActionBars;
                        packet.controlID = 5 + i;
                        packet._eventType = UIInterfaceEvent.EventType.SEND_DATA;

                        var floats = new List<float>();
                        floats.Add(spell.InContentManagerId + 0.01f);
                        packet.values = floats;

                        p.Client.ConnectionHandler.SendPacket(packet);
                    }, 1f);
            }
        }

        public void StartSpell(int id)
        {
            if (CurrentCastingSpell == null)
                if (Spells[id] != null)
                {
                    _currentCastingSpellId = id;
                    Spells[id].StartCasting(Unit);
                }
        }

        public void CancelSpell(int id)
        {
            if (CurrentCastingSpell == null)
                if (Spells[id] != null)
                {
                    _currentCastingSpellId = id;
                    Spells[id].StartCasting(Unit);
                }
        }

        public void FinishSpell(int id)
        {
            if (Spells[id] == CurrentCastingSpell)
            {
                CurrentCastingSpell.FinishCasting(Unit, _currentSpellTime);
                _currentSpellTime = 0;
                _currentCastingSpellId = -1;
            }
        }

        public void DoAction(int unitId, string actionName)
        {

            ServerUnit selectedUnit = Unit.CurrentWorld[unitId];

            if (selectedUnit == null)
            {
                //Debug.LogError("Trying to execute action ["+actionName+"] on a null unit.");
                return;
            }

            if (Vector3.Distance(Unit.Movement.Position, selectedUnit.Movement.Position) > Unit.Display.Size + selectedUnit.Display.Size)
            {
                if (selectedUnit != null)
                {
                    if (Unit.CurrentBranch.ObjectsVisible.Contains(selectedUnit))
                    {
                        Unit.Movement.WalkTo(selectedUnit.Movement.Position, Unit.Actions.DoAction, unitId, actionName);
                    }
                    else
                    {
                        Debug.Log("Object not visible.");
                    }
                }
                else
                {
                    Debug.Log("Object not existing. " + unitId);
                }
            }
            else
            {

                //Debug.Log("UnitActionPacket " + selectedUnit + " : " + actionName);
                selectedUnit.Actions.HandleIncommingAction(Unit, actionName);
            }
        }

        private void HandleIncommingAction(ServerUnit fromUnit, string actionName)
        {
            if (Unit is DroppedItem)
            {
                if (actionName == "Take")
                {
                    DroppedItem item = Unit as DroppedItem;

                    item.Display.PickupingUnit = fromUnit;

                    return;
                }

                if (actionName == "Pick-up")
                {
                    DroppedItem item = Unit as DroppedItem;

                    item.Display.PickupingUnit = fromUnit;

                    return;
                }
            }

            if (actionName == "Open")
            {
                UnitInventory inventory = Unit.GetExt<UnitInventory>();

                if (inventory == null)
                    Debug.LogError("Not an inventory.");

                if (inventory.HasAcces(Unit))
                {
                    if (fromUnit is Player)
                    {
                        Player p = fromUnit as Player;
                        p.ClientUi.Inventories.ShowInventory(inventory);
                    }
                }
                else
                {
                    Debug.Log("i have no access.");
                }

                return;
            }

            Debug.LogError("Unhandled action: "+actionName);
        }
    }
}
