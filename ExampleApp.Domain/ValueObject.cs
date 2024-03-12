using ExampleApp.Core;

namespace ExampleApp.Domain
{
    /// <summary>
    /// Value object base
    /// </summary>
    public abstract class ValueObject
    {
        public abstract Result IsValid();
        public override abstract bool Equals(object? other);
        public override abstract int GetHashCode();
    }
}
