using Microsoft.Data.Sqlite;
using ORBITSTUDY.Models;

namespace ORBITSTUDY.Database
{
    public class DataBaseService
    {
        private readonly string _databasePath;
        private readonly string _connectionString;

        public DataBaseService()
        {
            _databasePath = Path.Combine(FileSystem.AppDataDirectory, "players.db3");
            _connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = _databasePath
            }.ToString();
        }

        public Task InitializeDataBaseAsync()
        {
            return Task.Run(() =>
            {
                using var conn = new SqliteConnection(_connectionString);
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS Players (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        username TEXT NOT NULL,
                        password TEXT NOT NULL, 
                        email TEXT NOT NULL
                    );
                    CREATE TABLE IF NOT EXISTS PLayerStats (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        player_id INTEGER NOT NULL UNIQUE,
                        xp INTEGER NOT NULL,
                        lvl TEXT NOT NULL)";
                cmd.ExecuteNonQuery();
            });
        }
        public Task<int> CreatePlayer(Player player)
        {
            return Task.Run(() =>
            {
                using var conn = new SqliteConnection(_connectionString);
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Players (username, password, email) VALUES (@username, @password, @email);";
                cmd.Parameters.AddWithValue("@username", player.Username ?? string.Empty);
                cmd.Parameters.AddWithValue("@password", player.Password ?? string.Empty);
                cmd.Parameters.AddWithValue("@email", player.Email ?? string.Empty);

                return cmd.ExecuteNonQuery();
            });
        }
        public Task<int> CreatePlayerStats(int id)
        {
            return Task.Run(() =>
            {
                using var conn = new SqliteConnection(_connectionString);
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO PLayerStats (player_id, xp, lvl) VALUES (@player_id, @xp, @lvl);";
                cmd.Parameters.AddWithValue("@player_id", id);
                cmd.Parameters.AddWithValue("@xp", 0);
                cmd.Parameters.AddWithValue("@lvl", "NOOB");
                return cmd.ExecuteNonQuery();
            });
        }
        public Task<bool> Login(string username, string password)
        {
            return Task.Run(() =>
            {
                using var conn = new SqliteConnection(_connectionString);
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT COUNT(1) FROM Players WHERE username = @username AND password = @password;";
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);

                var result = cmd.ExecuteScalar();
                int count = result != null ? Convert.ToInt32(result) : 0;

                return count > 0;
            });
        }
        public async Task ClearPlayersAsync()
        {
            using var conn = new SqliteConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();

            cmd.CommandText = @"
                DELETE FROM PlayerStats;
                DELETE FROM Players;
            ";

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
