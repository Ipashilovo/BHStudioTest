using Mirror;
using UnityEngine;

namespace Entities
{
    public struct PositionMassage : NetworkMessage
    {
        public Vector3 Position;
    }
}