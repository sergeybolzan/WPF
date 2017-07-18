using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace ChatService
{
    [ServiceBehavior]
    public class Chat : IChat
    {
        private IClientCallback channel = OperationContext.Current.GetCallbackChannel<IClientCallback>();
        private static Dictionary<string, IClientCallback> users = new Dictionary<string, IClientCallback>();

        public void Join(string name)
        {
            users.Add(name, channel);
            foreach (var item in users.Values.Where(x => x != channel))
            {
                new Thread(() => item.AddUserToList(name, true)).Start();
            }
            foreach (var item in users.Keys)
            {
                new Thread(() => channel.AddUserToList(item, false)).Start();
            }
        }

        public void Leave(string name)
        {
            users.Remove(name);
            foreach (var item in users.Values)
            {
                new Thread(() => item.DeleteUserFromList(name, true)).Start();
            }
            new Thread(() => channel.DeleteUserFromList(name, false)).Start();
        }

        public void SendMessage(string name, string message)
        {
            foreach (var item in users.Values)
            {
                new Thread(() => item.PrintMessage(name, message)).Start();
            }
        }

        public void SendPrivateMessage(string nameFrom, string message, string nameTo)
        {
            new Thread(() => users.First(u => u.Key == nameTo).Value.PrintPrivateMessage(nameFrom, message, nameTo)).Start();
            if (nameFrom != nameTo)
            {
                new Thread(() => channel.PrintPrivateMessage(nameFrom, message, nameTo)).Start();
            }
        }

        public string[] GetNames()
        {
            return users.Keys.ToArray();
        }
    }
}
