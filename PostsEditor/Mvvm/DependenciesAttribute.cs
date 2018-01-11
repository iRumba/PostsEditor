using System;

namespace Mvvm
{
    public sealed class DependenciesAttribute : Attribute
    {
        public string[] DependentProperties { get; }
        public DependenciesAttribute(params string[] names)
        {
            DependentProperties = names;
        }
    }
}
