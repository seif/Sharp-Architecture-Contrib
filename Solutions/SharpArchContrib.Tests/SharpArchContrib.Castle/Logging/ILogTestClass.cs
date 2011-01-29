namespace Tests.SharpArchContrib.Castle.Logging
{
    using global::SharpArchContrib.Castle.Logging;

    public interface ILogTestClass
    {
        [Log]
        int Method(string name, int val);

        int NotLogged(string name, int val);

        void ThrowException();

        int VirtualMethod(string name, int val);
    }
}