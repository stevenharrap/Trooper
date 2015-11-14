namespace Trooper.Thorny.Business.Operation.Core
{
    using System;

    public class Process
    {
        public Process(string name, Action action)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            this.Name = name;
            this.Action = action;
        }

        public Action Action { get; }

        public string Name { get; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}