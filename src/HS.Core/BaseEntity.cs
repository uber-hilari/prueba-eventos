using System;
using System.Collections.Generic;
using System.Text;

#pragma warning disable IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
namespace HS
#pragma warning restore IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; protected set; } = false;

        public bool Equals(BaseEntity? obj)
        {
            if (obj is null) return false;

            if (ReferenceEquals(this, obj)) return true;

            if (obj is BaseEntity entidad)
            {
                return Id.Equals(entidad.Id);
            }
            return false;
        }

        // override object.Equals
        public override bool Equals(object? obj)
        {
            return Equals(obj as BaseEntity);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public void Delete()
        {
            IsDeleted = true;
        }

        public static bool operator ==(BaseEntity? left, BaseEntity? right)
        {
            if (left is null) 
                return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(BaseEntity? left, BaseEntity? right) => !(left == right);
    }
}
