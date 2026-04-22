using MySql.Data.MySqlClient;
namespace data_layer
{
    public class AvailabilityRepo
    {
        private string connectionString = "Server=localhost;Database=s2proj;User Id=root;Password=1234;";

        public List<AvailabilityDTO> GetAllAvailabilityByUser(int userId)
        {
            List<AvailabilityDTO> allAvailability = [];
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"SELECT * FROM availability WHERE user_id=@userId", conn
            );
            cmd.Parameters.AddWithValue("@userId", userId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                allAvailability.Add(new AvailabilityDTO(reader.GetInt32(0), reader.GetString(2), reader.GetTimeSpan(3), reader.GetTimeSpan(4)));
            }
            conn.Close();
            return allAvailability;
        }

        public void UpdateAvailability(AvailabilityDTO updatedAvailability)
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"UPDATE availability SET weekday=@weekday , start_time=@newStartTime , end_time=@newEndTime WHERE id=@id", conn
            );
            cmd.Parameters.AddWithValue("@id", updatedAvailability.Id);
            cmd.Parameters.AddWithValue("@weekday", updatedAvailability.Day);
            cmd.Parameters.AddWithValue("@newStartTime", updatedAvailability.StartTime);
            cmd.Parameters.AddWithValue("@newEndTime", updatedAvailability.EndTime);
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
