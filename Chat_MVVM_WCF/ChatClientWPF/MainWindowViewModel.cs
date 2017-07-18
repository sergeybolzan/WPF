using ChatClientWPF.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClientWPF
{
    class MainWindowViewModel : IChatCallback, INotifyPropertyChanged
    {
        private static InstanceContext context;
        private static ChatClient proxy;
        public MainWindowViewModel()
        {
            context = new InstanceContext(this);
            proxy = new ChatClient(context);
        }


        #region Fields and properties
        private ObservableCollection<string> users;
        /// <summary>
        /// Коллекция активных пользователей
        /// </summary>
        public ObservableCollection<string> Users
        {
            get { return users ?? (users = new ObservableCollection<string>()); }
        }

        private string name;
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name
        {
            get { return name; }
            set 
            { 
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private string message;
        /// <summary>
        /// Вводимое пользователем сообщение
        /// </summary>
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged("Message");
            }
        }

        private string textOfChat;
        /// <summary>
        /// Текст чата
        /// </summary>
        public string TextOfChat
        {
            get { return textOfChat; }
            set
            {
                textOfChat = value;
                OnPropertyChanged("TextOfChat");
            }
        }

        private string selectedUser;
        /// <summary>
        /// Выбранный пользователь в списке
        /// </summary>
        public string SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
                UpdateTextBoxMessageFocus();
            }
        }

        private bool isTextBoxMessageFocused;
        /// <summary>
        /// Свойство, отвечающее за фокус на текст, вводимый пользователем
        /// </summary>
        public bool IsTextBoxMessageFocused
        {
            get { return isTextBoxMessageFocused; }
            set
            {
                isTextBoxMessageFocused = value;
                OnPropertyChanged("IsTextBoxMessageFocused");
            }
        }
        #endregion


        #region Commands
        private RelayCommand joinCommand;
        /// <summary>
        /// Команда присоединения пользователя к чату. Осуществляется проверка на наличие и уникальность введенного имени пользователя, после чего вызывается метод службы Join().
        /// </summary>
        public RelayCommand JoinCommand
        {
            get
            {
                return joinCommand ?? (joinCommand = new RelayCommand(obj =>
                    {
                        if (!string.IsNullOrWhiteSpace(Name))
                        {
                            if (!proxy.GetNames().Contains(Name))
                            {
                                proxy.Join(Name);
                            }
                            else
                            {
                                MessageBox.Show("Такой никнейм уже существует.");
                            }
                        }
                        else MessageBox.Show("Нельзя использовать пустое имя.");
                    }));
            }
        }

        private RelayCommand leaveCommand;
        /// <summary>
        /// Команда, вызывающая метод службы Leave(). Осуществляется выход пользователя из чата
        /// </summary>
        public RelayCommand LeaveCommand
        {
            get
            {
                return leaveCommand ?? (leaveCommand = new RelayCommand(obj =>
                    {
                        proxy.Leave(Name);
                        Users.Clear();
                        TextOfChat = "";
                        Message = "";
                    }));
            }
        }

        private RelayCommand sendMessageCommand;
        /// <summary>
        /// Отправка сообщения всем
        /// </summary>
        public RelayCommand SendMessageCommand
        {
            get
            {
                return sendMessageCommand ?? (sendMessageCommand = new RelayCommand(obj =>
                    {
                        proxy.SendMessage(Name, Message);
                        Message = "";
                        UpdateTextBoxMessageFocus();
                    }));
            }
        }

        private RelayCommand sendPrivateMessageCommand;
        /// <summary>
        /// Отправка приватного сообщения
        /// </summary>
        public RelayCommand SendPrivateMessageCommand
        {
            get
            {
                return sendMessageCommand ?? (sendPrivateMessageCommand = new RelayCommand(obj =>
                {
                    if (SelectedUser != null)
                    {
                        proxy.SendPrivateMessage(Name, Message, SelectedUser);
                        Message = "";
                        UpdateTextBoxMessageFocus();
                    }
                    else
                    {
                        MessageBox.Show("Не выбран получатель.");
                        UpdateTextBoxMessageFocus();
                    }
                }));
            }
        }

        private RelayCommand closeWindowCommand;
        /// <summary>
        /// Выход пользователя из чата при закрытии приложения
        /// </summary>
        public RelayCommand CloseWindowCommand
        {
            get
            {
                return closeWindowCommand ?? (closeWindowCommand = new RelayCommand(obj =>
                    {
                        if (Users.Count > 0) proxy.Leave(Name);
                    }));
            }
        }
        #endregion


        #region IChatCallback
        /// <summary>
        /// Метод, вызываемый службой. Происходит добавление нового пользователя в чат.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="showMessage"></param>
        public void AddUserToList(string name, bool showMessage)
        {
            Users.Add(name);
            if (showMessage)
            {
                TextOfChat += string.Format("<К нам присоединился {0}>\n", name);
            }
        }

        /// <summary>
        /// Метод, вызываемый службой. Происходит удаление пользователя из чата.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="showMessage"></param>
        public void DeleteUserFromList(string name, bool showMessage)
        {
            Users.Remove(name);
            if (showMessage)
            {
                TextOfChat += string.Format("<Нас покинул {0}>\n", name);
            }
        }

        /// <summary>
        /// Метод, вызываемый службой. Происходит вывод сообщения в чат.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="message"></param>
        public void PrintMessage(string name, string message)
        {
            TextOfChat += string.Format("<{0}>: {1}\n", name, message);
        }

        /// <summary>
        /// Метод, вызываемый службой. Происходит вывод приватного сообщения в чат.
        /// </summary>
        /// <param name="nameFrom"></param>
        /// <param name="message"></param>
        /// <param name="nameTo"></param>
        public void PrintPrivateMessage(string nameFrom, string message, string nameTo)
        {
            if (nameTo != Name)
            {
                TextOfChat += string.Format("<{0} (личное {1})>: {2}\n", nameFrom, nameTo, message);
            }
            else
            {
                TextOfChat += string.Format("<{0} (личное)>: {1}\n", nameFrom, message);
            }
        }
        #endregion


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion


        /// <summary>
        /// Установка фокуса на вводимый пользователем текст.
        /// </summary>
        private void UpdateTextBoxMessageFocus()
        {
            IsTextBoxMessageFocused = false;
            IsTextBoxMessageFocused = true;
        }
    }
}
