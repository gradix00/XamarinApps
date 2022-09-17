using System;
using MySqlConnector;
using TestBDonline.Scripts.Structs;
using System.Collections.Generic;

namespace TestBDonline.Scripts
{
    public class Authentication
    {
        private const string defaultEmail = "adiomigames@gmail.com";
        private const string defaultPassword = "Adiomi_cf_jas";
        private const string server = "beme3cogq4rlb9kotqkx-mysql.services.clever-cloud.com";
        private const string database = "beme3cogq4rlb9kotqkx";
        private const string username = "uj3jmogbwwarnhk2";
        private const string password = "iOwBLjQIeWpliIMJEGWo";
        private string connAddr = $@"server={server};database={database};uid={username};pwd={password}";

        public string Email { get; set; }
        public string Password { get; set; }
        public UserData UserData { get; set; }

        private bool CheckAcces()
        {
            var conn = new MySqlConnection(connAddr);
            conn.Open();

            bool res = false;
            if (conn.State == System.Data.ConnectionState.Open)
                res = true;

            conn.Close();
            return res;
        }

        public bool InitiateLogin(string email, string password)
        {
            Email = email;
            Password = password;

            var conn = new MySqlConnection(connAddr);
            conn.Open();

            if (conn.State == System.Data.ConnectionState.Open)
            {
                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM Users WHERE email='{Email}' AND password='{Password}'", conn);
                MySqlDataReader read = cmd.ExecuteReader();

                int numberOfRecord = 0;
                while (read.Read())
                {
                    GetInfoUser(read["Id"].ToString(), read["Nickname"].ToString(), read["Email"].ToString(), read["Points"].ToString(), read["Status"].ToString());
                    numberOfRecord++;
                }
         
                conn.Close();
                if (numberOfRecord > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                conn.Close();
                return false;
            }
        }

        public bool InitiateRegister(string nick, string email, string password)
        {
            var conn = new MySqlConnection(connAddr);
            conn.Open();

            if(conn.State == System.Data.ConnectionState.Open)
            {
                MySqlCommand cmd = new MySqlCommand($"SELECT nickname, email FROM Users WHERE email='{email}' OR nickname='{nick}'", conn);
                int numberOfRecord = 0;

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    numberOfRecord++;

                if (numberOfRecord == 0)
                {
                    conn.Close();

                    conn.Open();
                    cmd = new MySqlCommand($"INSERT INTO Users(nickname, email, password, points) VALUES(@nickname, @email, @password, @points)", conn);
                    cmd.Parameters.AddWithValue("@nickname", $"{nick}");
                    cmd.Parameters.AddWithValue("@email", $"{email}");
                    cmd.Parameters.AddWithValue("@password", $"{password}");
                    cmd.Parameters.AddWithValue("@points", $"0");
                    Console.WriteLine("command: "+cmd.CommandText);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
            }
            conn.Close();
            return false;
        }  

        public List<UserData> GetListAllUsers()
        {
            var conn = new MySqlConnection(connAddr);
            conn.Open();

            List<UserData> retList = null;
            if(conn.State == System.Data.ConnectionState.Open)
            {
                MySqlCommand cmd = new MySqlCommand("SELECT id, nickname, email, points FROM Users");
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    retList.Add(new UserData
                    {
                        ID = int.Parse(reader["id"].ToString()),
                        Nickname = reader["nickname"].ToString(),
                        Email = reader["email"].ToString(),
                        Points = int.Parse(reader["points"].ToString())
                    });
                conn.Close();
            }
            return retList;
        }

        private UserData GetInfoUser(string id, string nick, string email, string points, string status)
        {
            Status stat = Status.user;
            if (status == "Banned")
                stat = Status.banned;
            else if (status == "Admin")
                stat = Status.admin;
            else
                stat = Status.user;

            UserData = new UserData()
            {
                ID = int.Parse(id),
                Nickname = nick,
                Email = email,
                Points = int.Parse(points),
                Status = stat
            };
            return UserData;
        }

    }
}
