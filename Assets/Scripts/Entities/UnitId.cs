using System;

namespace Entities
{
    public struct UnitId : IEquatable<UnitId>
    {
        public readonly string Value;

        public UnitId(string value)
        {
            Value = value;
        }

        public static bool operator ==(UnitId a, UnitId b)
        {
            if (!(a is { }) || !(b is { }))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(UnitId a, UnitId b)
        {
            return !(a == b);
        }


        public bool Equals(UnitId other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is UnitId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}