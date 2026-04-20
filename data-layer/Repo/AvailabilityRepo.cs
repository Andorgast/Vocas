using MySql.Data.MySqlClient;
namespace data_layer
{
    public class AvailabilityRepo
    {
        private string connectionString = "Server=localhost;Database=s2proj;User Id=root;Password=1234;";
        public List<AvailabilityDTO> AvailabilityDTOList { get; private set; } = [];

        public List<AvailabilityDTO> GetAllAvailabilityByUser(int userId)
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"SELECT * FROM availability WHERE user_id=@userId", conn
            );
            cmd.Parameters.AddWithValue("@userId", userId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                AvailabilityDTOList.Add(new AvailabilityDTO(reader.GetInt32(0), reader.GetString(2), TimeSpan.Parse(reader.GetString(3)), TimeSpan.Parse(reader.GetString(4))));
            }
            conn.Close();
            return AvailabilityDTOList;
        }

        public void UpdateAvailability(int id, string weekday, TimeSpan newStartTime, TimeSpan newEndTime)
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"UPDATE availability SET weekday=@weekday , start_time=@newStartTime , end_time=@newEndTime WHERE id=@id", conn
            );
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@weekday", weekday);
            cmd.Parameters.AddWithValue("@newStartTime", newStartTime);
            cmd.Parameters.AddWithValue("@newEndTime", newEndTime);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void RemoveAvailability(int id)
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"DELETE FROM availability WHERE id=@id", conn
            );
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
