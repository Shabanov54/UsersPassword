using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;
using FirebirdSql.Data.FirebirdClient;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Controls;
using System.Globalization;


namespace Users_Password
{
    internal class Context
    {
        FbDataAdapter adapter;
        DataTable dataTable;
        FbConnection connection = null;
        string sql = "SELECT FULL_NAME , PASSWRD , USERDB , USER_ID FROM Users";
        List<User> users_all = new List<User>();

        internal void InitializeComponent(Server server)
        {
            FbConnectionStringBuilder stringBuilder = new FbConnectionStringBuilder();
            stringBuilder.UserID = server.User;
            stringBuilder.DataSource = server.DataSource;
            stringBuilder.Database = server.Database;
            stringBuilder.Port = server.Port;
            stringBuilder.Password = server.Password;
            connection = new FbConnection(stringBuilder.ToString());

        }
        internal List<User> GetUsers()
        {
            dataTable = new DataTable();
            try
            {
                FbCommand cmd = new FbCommand(sql, connection);
                adapter = new FbDataAdapter(cmd);
                connection.Open();
                adapter.Fill(dataTable);
                foreach (var item in dataTable.AsEnumerable())
                {
                    User users = new User();
                    users.UserID = (int)item["USER_ID"];
                    if (!string.IsNullOrEmpty(item["PASSWRD"].ToString()))
                    {
                        users.Password = (string)item["PASSWRD"];
                    }
                    if (!string.IsNullOrEmpty(item["FULL_NAME"].ToString()))
                    {
                        users.User_name = (string)item["FULL_NAME"];
                    }
                    if (!string.IsNullOrEmpty(item["USERDB"].ToString()))
                    {
                        users.UserNik = (string)item["USERDB"];
                    }
                    if(users.User_name == null)
                    {
                        continue;
                    }
                    if(users.UserID ==0 ||users.UserID==5 ||users.UserID ==6 || users.UserID==2001)
                    {
                        continue;
                    }
                    users_all.Add(users);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            return users_all;
        }

        internal void OutPutUser(User select_row, TextBox update)
        {
            string output = select_row.User_name;
            string outputforlable = $"{output}";
            update.Text = outputforlable;
        }

        internal List<User> SelectUser(string text)
        {
            var select = from user_select in users_all
                         where user_select.User_name.Contains(text) || user_select.UserNik.Contains(text)
                         select user_select;
            return select.ToList();

        }

        internal void Change_Pass(User select_row, string Text_Update)
        {
            string sql = "UPDATE Users SET PASSWRD='" + Text_Update+"' WHERE USER_ID = " + select_row.UserID + ";";
            connection.Open();
            try
            {
                FbCommand cmd = new FbCommand(sql, connection);
                cmd.ExecuteNonQuery();

                using (FbTransaction tr = connection.BeginTransaction())
                {
                    tr.Commit();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        
    }
}

