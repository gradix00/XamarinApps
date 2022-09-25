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

        private bool CheckAcces(MySqlConnection con)
        {
            if (con.State == System.Data.ConnectionState.Open)
                return true;
            return false;
        }

        public bool InitiateLogin(string email, string password)
        {
            Email = email;
            Password = password;

            var conn = new MySqlConnection(connAddr);
            conn.Open();

            if (CheckAcces(conn))
            {
                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM Users WHERE email='{Email}' AND password='{Password}'", conn);
                MySqlDataReader read = cmd.ExecuteReader();

                int numberOfRecord = 0;
                while (read.Read())
                {
                    GetInfoUser(read["Id"].ToString(), read["Nickname"].ToString(), read["Email"].ToString(), read["Points"].ToString(), read["Status"].ToString(), read["RequirePwdReset"].ToString());
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

            if(CheckAcces(conn))
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
                    cmd.Parameters.AddWithValue("@nickname", nick);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@points", "0");
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

            List<UserData> retList = new List<UserData>();
            if(CheckAcces(conn))
            {
                MySqlCommand cmd = new MySqlCommand("SELECT id, nickname, email, points, status, RequirePwdReset FROM Users", conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    retList.Add(new UserData
                    {
                        ID = int.Parse(reader["id"].ToString()),
                        Nickname = reader["nickname"].ToString(),
                        Email = reader["email"].ToString(),
                        Points = int.Parse(reader["points"].ToString()),
                        Status = new Dictionary.GetStatus().status[reader["status"].ToString()],
                        RequirePasswordReset = bool.Parse(reader["RequirePwdReset"].ToString())
                    });               
                conn.Close();
            }
            return retList;
        }

        public UserData GetUserDataByID(int id)
        {
            List<UserData> getList = GetListAllUsers();
            foreach(UserData user in getList)
            {
                if(id == user.ID)
                    return user;
            }
            return new UserData();
        }

        public bool UpdateUserDataBy(UserData data)
        {
            var conn = new MySqlConnection(connAddr);
            conn.Open();

            if (CheckAcces(conn))
            {
                MySqlCommand cmd = new MySqlCommand($"UPDATE Users SET email = @email, points=@points, status=@status, RequirePwdReset=@requireReset WHERE id={data.ID}", conn);
                cmd.Parameters.AddWithValue("@email", data.Email);
                cmd.Parameters.AddWithValue("@points", data.Points);
                cmd.Parameters.AddWithValue("@status", data.Status);
                cmd.Parameters.AddWithValue("@requireReset", data.RequirePasswordReset);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            else
                return false;
            return true;
        }

        private UserData GetInfoUser(string id, string nick, string email, string points, string status, string rpr)
        {
            UserData = new UserData()
            {
                ID = int.Parse(id),
                Nickname = nick,
                Email = email,
                Points = int.Parse(points),
                Status = new Dictionary.GetStatus().status[status],
                RequirePasswordReset = bool.Parse(rpr.ToString())
            };
            return UserData;
        }

    }
}
