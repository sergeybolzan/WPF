using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatService
{
    [ServiceContract(CallbackContract = typeof(IClientCallback))]
    public interface IChat
    {
        [OperationContract(IsOneWay = true)]
        void Join(string name);

        [OperationContract(IsOneWay = true)]
        void Leave(string name);

        [OperationContract(IsOneWay = true)]
        void SendMessage(string name, string message);

        [OperationContract(IsOneWay = true)]
        void SendPrivateMessage(string nameFrom, string message, string nameTo);
        
        [OperationContract]
        string[] GetNames();
    }

    public interface IClientCallback
    {
        [OperationContract(IsOneWay = true)]
        void AddUserToList(string name, bool showMessage);

        [OperationContract(IsOneWay = true)]
        void DeleteUserFromList(string name, bool showMessage);

        [OperationContract(IsOneWay = true)]
        void PrintMessage(string name, string message);

        [OperationContract(IsOneWay = true)]
        void PrintPrivateMessage(string nameFrom, string message, string nameTo);

    }
}
