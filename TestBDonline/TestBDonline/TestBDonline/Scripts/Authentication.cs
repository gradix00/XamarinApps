﻿using System.Net;
using System;
using MySqlConnector;
using TestBDonline.Scripts.Structs;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    if (con.State == System.Data.ConnectionState.Open)
                        return true;
                }
            }
            catch
            {
                con.CloseAsync();                        
            }
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
                    UserData = GetInfoUser(read["Id"].ToString(), read["Nickname"].ToString(), read["Email"].ToString(), read["Points"].ToString(), read["Status"].ToString(), read["Gender"].ToString(), read["RequirePwdReset"].ToString());
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

        public List<PostData> GetListAllPosts(bool isRandom = false)
        {
            var conn = new MySqlConnection(connAddr);
            conn.Open();

            var retList = new List<PostData>();
            if (CheckAcces(conn))
            {
                string txtCmd = null;

                if (isRandom)
                    txtCmd = "SELECT * FROM Posts ORDER BY RAND() LIMIT 4";
                else
                    txtCmd = "SELECT * FROM Posts";

                MySqlCommand cmd = new MySqlCommand(txtCmd, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retList.Add(new PostData
                    {
                        Autor = reader["autor"].ToString(),
                        Date = Convert.ToDateTime(reader["date"].ToString()),
                        Title = reader["title"].ToString(),
                        Description = reader["description"].ToString(),
                        UrlImage = reader["UrlImage"].ToString(),
                        Likes = int.Parse(reader["likes"].ToString())
                    });
                }
            }
            conn.Close();
            return retList;
        }

        public List<MessageData> GetAllMessagesData(int limit = 10)
        {
            var conn = new MySqlConnection(connAddr);
            conn.Open();

            var retList = new List<MessageData>();
            if (CheckAcces(conn))
            {
                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM GlobalChat ORDER BY date DESC LIMIT {limit}", conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retList.Add(new MessageData
                    {
                        ID = int.Parse(reader["id"].ToString()),
                        Autor = reader["autor"].ToString(),
                        Message = reader["message"].ToString(),
                        Date = Convert.ToDateTime(reader["date"].ToString())
                    });
                }
            }
            conn.Close();
            return retList;
        }

        public List<EventData> GetListAllEventLog(int numberRecords = 10)
        {
            var conn = new MySqlConnection(connAddr);
            conn.OpenAsync();

            var retList = new List<EventData>();
            if (CheckAcces(conn))
            {
                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM EventLog ORDER BY id DESC LIMIT {numberRecords}", conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retList.Add(new EventData
                    {
                        ID = int.Parse(reader["ID"].ToString()),
                        Autor = reader["Autor"].ToString(),
                        Details = reader["Details"].ToString(),
                        Date = Convert.ToDateTime(reader["Date"].ToString())
                    });
                }
            }
            conn.CloseAsync();

            foreach(var item in retList)
                Console.WriteLine(item.Details);
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

        public bool CreateNewMessageGlobalChat(MessageData data)
        {
            var conn = new MySqlConnection(connAddr);
            conn.Open();

            if (CheckAcces(conn))
            {
                MySqlCommand cmd = new MySqlCommand("INSERT INTO GlobalChat(autor, message, date) VALUES(@autor, @message, @date)", conn);
                cmd.Parameters.AddWithValue("@autor", data.Autor);
                cmd.Parameters.AddWithValue("@message", data.Message);
                cmd.Parameters.AddWithValue("@date", data.Date);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            conn.Close();
            return false;
        }

        public bool CreatePost(PostData data)
        {
            var conn = new MySqlConnection(connAddr);
            conn.Open();

            if (CheckAcces(conn))
            {
                MySqlCommand cmd = new MySqlCommand("INSERT INTO Posts(autor, date, title, description, UrlImage, likes) VALUES(@autor, @date, @title, @description, @url, @likes)", conn);
                cmd.Parameters.AddWithValue("@autor", data.Autor);
                cmd.Parameters.AddWithValue("@date", data.Date);
                cmd.Parameters.AddWithValue("@title", data.Title);
                cmd.Parameters.AddWithValue("@description", data.Description);
                cmd.Parameters.AddWithValue("@url", data.UrlImage);
                cmd.Parameters.AddWithValue("@likes", "0");
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            conn.Close();
            return false;
        }

        public bool CreateNewLog(EventData data)
        {
            var conn = new MySqlConnection(connAddr);
            conn.Open();

            if (CheckAcces(conn))
            {
                MySqlCommand cmd = new MySqlCommand("INSERT INTO EventLog(autor, details, date) VALUES(@autor, @details, @date)", conn);
                cmd.Parameters.AddWithValue("@autor", data.Autor);
                cmd.Parameters.AddWithValue("@details", data.Details);
                cmd.Parameters.AddWithValue("@date", data.Date);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            conn.Close();
            return false;
        }

        public bool UpdateUserData(UserData data)
        {
            var conn = new MySqlConnection(connAddr);
            conn.Open();

            if (CheckAcces(conn))
            {
                Console.WriteLine("loll");
                MySqlCommand cmd = new MySqlCommand($"UPDATE Users SET email = @email, points=@points, status=@status, RequirePwdReset=@requireReset WHERE id={data.ID}", conn);
                cmd.Parameters.AddWithValue("@email", data.Email);
                cmd.Parameters.AddWithValue("@points", data.Points);
                cmd.Parameters.AddWithValue("@status", data.Status.ToString());
                cmd.Parameters.AddWithValue("@requireReset", data.RequirePasswordReset);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            else
                return false;
            return true;
        }

        private UserData GetInfoUser(string id, string nick, string email, string points, string status, string gender, string rpr)
        {
            UserData = new UserData()
            {
                ID = int.Parse(id),
                Nickname = nick,
                Email = email,
                Points = int.Parse(points),
                Status = new Dictionary.GetStatus().status[status],
                Gender = new Dictionary.GetStatus().gender[gender],
                RequirePasswordReset = bool.Parse(rpr.ToString())
            };
            return UserData;
        }

    }
}
