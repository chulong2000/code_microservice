using System;

namespace GHM.Infrastructure.ModelBinders
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class DefaultFromBodyAttribute : Attribute
    {
    }
}
