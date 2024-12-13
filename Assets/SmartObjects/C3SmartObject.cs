using System;
using System.Globalization;
using Unity.Mathematics;
using UnityEngine;

namespace OrbitGames{
    public struct C3SmartObject : IEquatable<C3SmartObject>
    {
        public enum Type : byte
        {
            Null,
            Bool,
            Number,
            String,
            Object
        }

        public readonly Type currentType;
        public readonly double numberData;
        public readonly object otherData;

        private C3SmartObject(Type type, double numberData, object otherData)
        {
            this.currentType = type;
            this.numberData = numberData;
            this.otherData = otherData;
        }

        public override string ToString()
        {
            return currentType switch
            {
                Type.Bool => (numberData > 0).ToString(),
                Type.Number => numberData.ToString(CultureInfo.InvariantCulture),
                Type.String => (string)otherData,
                Type.Object => otherData?.ToString() ?? "null",
                _ => "null"
            };
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)currentType, numberData, otherData);
        }

        public bool IsNumberNaN()
        {
            return double.IsNaN((double)this);
        }

        #region SET FROM

        public static implicit operator C3SmartObject(double value)
        {
            return new C3SmartObject(Type.Number, value, null);
        }

        public static implicit operator C3SmartObject(float value)
        {
            return new C3SmartObject(Type.Number, value, null);
        }

        public static implicit operator C3SmartObject(int value)
        {
            return new C3SmartObject(Type.Number, value, null);
        }

        public static implicit operator C3SmartObject(bool value)
        {
            return new C3SmartObject(Type.Bool, value ? 1 : 0, null);
        }

        public static implicit operator C3SmartObject(string value)
        {
            return new C3SmartObject(Type.String, double.NaN, value ?? string.Empty);
        }

        public static implicit operator C3SmartObject(GameObject value)
        {
            return new C3SmartObject(Type.Object, double.NaN, value);
        }

        #endregion

        #region GET BACK

        public static implicit operator double(C3SmartObject value)
        {
            return value.currentType switch
            {
                Type.Number => value.numberData,
                Type.Bool => value.numberData,
                Type.String => double.TryParse((string)value.otherData, out var r) ? r : double.NaN,
                _ => double.NaN
            };
        }

        public static implicit operator bool(C3SmartObject value)
        {
            return value.currentType switch
            {
                Type.Number => value.numberData != 0,
                Type.Bool => value.numberData != 0,
                Type.String => double.TryParse((string)value.otherData, out var parsedDouble)
                    ? parsedDouble != 0
                    : (bool.TryParse((string)value.otherData, out var b) && b),
                _ => false
            };
        }

        public static implicit operator string(C3SmartObject value)
        {
            return value.currentType switch
            {
                Type.Number => value.numberData.ToString(CultureInfo.InvariantCulture),
                Type.Bool => value.numberData > 0 ? "true" : "false",
                Type.String => (string)value.otherData,
                _ => ""
            };
        }

        #endregion

        #region Operators

        public static C3SmartObject operator +(C3SmartObject a, C3SmartObject b)
        {
            //如果完全相同的类型，只要是数字就加，否则就是字符串拼接。
            if (a.currentType == b.currentType)
            {
                if (a.currentType == Type.Number)
                    return (double)a + (double)b;
                else
                    return (string)a + (string)b;
            }
            //否则，如果其中一个是字符串, 另一个是数字，则判断字符串那个能否被解析成数字。
            else if (a.currentType == Type.String && double.TryParse((string)a.otherData, out var aDouble) &&
                     b.currentType == Type.Number)
            {
                return aDouble + (double)b;
            }
            else if (b.currentType == Type.String && double.TryParse((string)b.otherData, out var bDouble) &&
                     a.currentType == Type.Number)
            {
                return (double)a + bDouble;
            }

            //其他情况，全都是字符串直接加
            return (string)a + (string)b;
        }

        public static C3SmartObject operator -(C3SmartObject a, C3SmartObject b)
        {
            return (double)a - (double)b;
        }
        
        public static C3SmartObject operator *(C3SmartObject a, C3SmartObject b)
        {
            return (double)a * (double)b;
        }
        
        public static C3SmartObject operator /(C3SmartObject a, C3SmartObject b)
        {
            return (double)a / (double)b;
        }

        public static C3SmartObject operator ^(C3SmartObject a, C3SmartObject b)
        {
            return math.pow((double)a, (double)b);
        }
        
        public static bool operator ==(C3SmartObject a, C3SmartObject b)
        {
            return a.Equals(b);
        }
        
        public static bool operator !=(C3SmartObject a, C3SmartObject b)
        {
            return !a.Equals(b);
        }

        #endregion

        #region EQUALS

        public bool Equals(C3SmartObject other)
        {
            if (currentType == other.currentType)
            {
                return numberData == other.numberData && otherData == other.otherData;
            }
            //否则，如果其中一个是字符串, 另一个是数字，则判断字符串那个能否被解析成数字。

            if (other.currentType == Type.Number && currentType == Type.String &&
                double.TryParse((string)otherData, out var aDouble))
            {
                return aDouble == other.numberData;
            }

            if (currentType == Type.Number && other.currentType == Type.String &&
                double.TryParse((string)other.otherData, out var bDouble))
            {
                return numberData == bDouble;
            }

            if (other.currentType == Type.Bool && currentType == Type.String &&
                bool.TryParse((string)otherData, out var aBool))
            {
                return aBool == (bool)other;
            }

            if (currentType == Type.Bool && other.currentType == Type.String &&
                bool.TryParse((string)other.otherData, out var bBool))
            {
                return bBool == (bool)this;
            }

            return otherData == other.otherData;
        }

        public override bool Equals(object obj)
        {
            return obj is C3SmartObject other && Equals(other);
        }
        
        #endregion
    }
}

