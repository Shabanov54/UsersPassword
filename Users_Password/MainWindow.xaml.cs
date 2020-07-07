using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Users_Password
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<User> usersdb = new List<User>();
        Context context;
        public MainWindow()
        {

            var files = new FileContext();
            var getfile = files.GetSettings();
            InitializeComponent();
            Textbox_filter.TextChanged += Textbox_filter_TextChanged;
            //Textbox_Login.TextChanged += Textbox_Login_TextChanged;
            
            context = new Context();
            context.InitializeComponent(getfile);
            Window_Loaded();
            Data_Grid.MouseLeftButtonUp += MouseLeftButtonUpClick;

        }
        internal User SelectRow()
        {
            var select_row = (User)Data_Grid.SelectedItem;
            return select_row;
        }

        private void MouseLeftButtonUpClick(object sender, MouseButtonEventArgs e)
        {
            var select_row = SelectRow();
            context.OutPutUser(select_row, Update);
        }

        internal void Window_Loaded()
        {
            try
            {
                var users = new ObservableCollection<User>();
                usersdb = context.GetUsers();
                foreach (var item in usersdb)
                {
                    User user = item;
                    users.Add(user);
                }
                Data_Grid.ItemsSource = users;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void Button_Update_Click(object sender, RoutedEventArgs e)
        {
            var select_row = (User)Data_Grid.SelectedItem;
            context.Change_Pass(select_row, Text_Update.Text);
            usersdb.Clear();
            Window_Loaded();
        }

        private void Textbox_filter_TextChanged(object sender, TextChangedEventArgs e)
        {

            var user = new ObservableCollection<User>();
            List<User> users = context.SelectUser(Textbox_filter.Text);
            foreach (var item in users)
            {
                User user1 = item;
                user.Add(user1);
            }
            Data_Grid.ItemsSource = users;
        }

        //private void Textbox_Login_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    var user = new ObservableCollection<User>();
        //    List<User> users = context.Select_Login(Textbox_Login.Text);
        //    foreach (var item in users)
        //    {
        //        User user1 = item;
        //        user.Add(user1);
        //    }
        //    Data_Grid.ItemsSource = users;
        //}
    }
}
