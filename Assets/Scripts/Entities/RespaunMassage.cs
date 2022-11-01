using Mirror;
using UnityEngine;

namespace Entities
{
    public struct RespaunMassage : NetworkMessage
    {
    }

    public struct RespaunPositionMassage : NetworkMessage
    {
        public Vector3 Position;
        public UnitId UnitId;
    }
}