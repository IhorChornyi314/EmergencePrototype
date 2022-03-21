using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class MoveOrder: Order
{
        private int _targetID = -1;
        [NonSerialized] private Vector2 _targetPosition;
        [NonSerialized] private Vector2 _targetPreviousPosition;
        private float _stoppingRadius;
        private float _targetPosX, _targetPosY;
        
        public MoveOrder(int targetID, float stopRadius)
        {
                _targetID = targetID;
                _stoppingRadius = stopRadius;
        }

        public MoveOrder(Vector2 targetPosition, float stopRadius)
        {
                _targetPosition = targetPosition;
                _stoppingRadius = stopRadius;
        }

        public override void AddToBuilding(Building building, bool reset)
        {
                if (reset) building.SpawnOrders.Clear();
                building.SpawnOrders.Add(this);
        }

        public override void AddToUnit(Unit unit, bool reset)
        {
                unit.AddOrResetOrder(this, reset);
        }
        
        public override OrderResult Execute(Entity self)
        {
                Vector2 targetPosition = _targetID == -1 ?  _targetPosition : EntityManager.Instance.entities[_targetID].Position;
                if (Vector2.Distance(targetPosition, self.Position) <= _stoppingRadius)
                        return OrderResult.Finish;
                Vector2 direction = (targetPosition - self.Position).normalized;

                self.Position = self.Position + direction * ((UnitData)self.data).speed * Time.deltaTime;
                return OrderResult.Continue;
        }

        public override void Serialize()
        {
                _targetPosX = _targetPosition.x;
                _targetPosY = _targetPosition.y;
        }
        public override void Deserialize()
        {
                _targetPosition = new Vector2(_targetPosX, _targetPosY);
        }
}