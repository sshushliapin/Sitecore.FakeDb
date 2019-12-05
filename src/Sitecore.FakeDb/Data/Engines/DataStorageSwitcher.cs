namespace Sitecore.FakeDb.Data.Engines
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Sitecore.Diagnostics;
    using Sitecore.Threading;

    public class DataStorageSwitcher : IDisposable
    {
        private const string ItemsKey = "DataStorage_Switcher_State_";

        private readonly string databaseName;

        public DataStorageSwitcher(DataStorage objectToSwitchTo)
        {
            Assert.IsNotNull(objectToSwitchTo, "objectToSwitchTo");

            this.databaseName = objectToSwitchTo.Database.Name;

            Enter(this.databaseName, objectToSwitchTo);
        }

        public static DataStorage CurrentValue(string databaseName)
        {
            var stack = GetStack(databaseName, false);
            if (stack == null || stack.Count == 0)
            {
                return default(DataStorage);
            }

            return stack.Peek();
        }

        public static void Enter(string databaseName, DataStorage objectToSwitchTo)
        {
            Assert.ArgumentNotNull(objectToSwitchTo, "objectToSwitchTo");

            GetStack(databaseName, true).Push(objectToSwitchTo);
        }

        public static void Exit(string databaseName)
        {
            var stack = GetStack(databaseName, false);

            Assert.IsTrue(stack != null && stack.Count > 0, "Stack is null or empty.");

            stack.Pop();
        }

        public static Stack<DataStorage> GetStack(string databaseName, bool createIfEmpty)
        {
            // Sitecore.Common.Switcher uses Sitecore.Context.Items which is either ThreadLocal or HttpContext.Current
            // based on whether HttpContext is available
            var store = ThreadData.GetData(ThreadData.Keys.Items) as IDictionary;
            if (store == null)
            {
                store = new Hashtable();
                ThreadData.SetData(ThreadData.Keys.Items, store);
            }

            var stack = store[ItemsKey + databaseName] as Stack<DataStorage>;
            if (stack == null && createIfEmpty)
            {
                stack = new Stack<DataStorage>();
                store[ItemsKey + databaseName] = stack;
            }

            return stack;
        }

        public virtual void Dispose()
        {
            Exit(this.databaseName);
        }
    }
}