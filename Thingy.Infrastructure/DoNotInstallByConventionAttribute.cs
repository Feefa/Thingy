using System;

namespace Thingy.Infrastructure
{
    /// <summary>
    /// Any class marked with this attribute will not be considered for convention-based
    /// registration regardless of its name or what interfaces it implements.
    /// </summary>
    public class DoNotInstallByConventionAttribute : Attribute { }
}
