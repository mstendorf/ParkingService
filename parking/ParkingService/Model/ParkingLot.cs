namespace ParkingService.Parking
{
    using CarTypeService.Models;
    using EventFeed;

    // we make an interface since this could be useful for another parkinglot implementation
    public interface IparkingLot
    {
        Registration Register(
            ParkingRequest request,
            CarDescription carType,
            IEventStore eventStore
        );
        Registration? CheckParking(string licensePlate, string parkingSpot);
        void Delete(string licensePlate);
        List<Registration> GetRegistrations();
    }

    public class PersistentParkingLot : IparkingLot
    {
        private string connectionstring =
            "Host=localhost;Username=postgres;Password=testing123;Database=parkinglot;Include Error Detail=true";

        public Registration Register(
            ParkingRequest request,
            CarDescription carType,
            IEventStore eventStore
        )
        {
            using var connection = new Npgsql.NpgsqlConnection(connectionstring);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            using var command = new Npgsql.NpgsqlCommand(
                "INSERT INTO registrations (licenseplate, contactinformation, parkingspot, parkedtime, car_make, car_model, car_variant) VALUES (@licenseplate, @contactinformation, @parkingspot, @parkedtime, @car_make, @car_model, @car_variant)",
                connection
            );
            command.Parameters.AddWithValue("licenseplate", request.LicensePlate);
            command.Parameters.AddWithValue("contactinformation", request.ContactInformation);
            command.Parameters.AddWithValue("parkingspot", request.ParkingSpot);
            command.Parameters.AddWithValue("parkedtime", DateTime.Now);
            command.Parameters.AddWithValue("car_make", carType.Make);
            command.Parameters.AddWithValue("car_model", carType.Model);
            command.Parameters.AddWithValue("car_variant", carType.Variant);
            command.ExecuteNonQuery();
            transaction.Commit();
            var registration = new Registration(
                request.LicensePlate,
                request.ContactInformation,
                request.ParkingSpot,
                DateTime.Now,
                carType
            );
            eventStore.Raise("car_parked", registration);
            return registration;
        }

        public Registration? CheckParking(string licensePlate, string parkingSpot)
        {
            using var connection = new Npgsql.NpgsqlConnection(connectionstring);
            connection.Open();
            using var command = new Npgsql.NpgsqlCommand(
                "SELECT * FROM registrations WHERE licenseplate = @licenseplate AND parkingspot = @parkingspot",
                connection
            );
            command.Parameters.AddWithValue("licenseplate", licensePlate);
            command.Parameters.AddWithValue("parkingspot", parkingSpot);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Registration(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetDateTime(3),
                    new CarDescription(
                        reader.GetString(4),
                        reader.GetString(5),
                        reader.GetString(6)
                    )
                );
            }
            return null;
        }

        public void Delete(string licensePlate)
        {
            using var connection = new Npgsql.NpgsqlConnection(connectionstring);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            using var command = new Npgsql.NpgsqlCommand(
                "DELETE FROM registrations WHERE licenseplate = @licenseplate",
                connection
            );
            command.Parameters.AddWithValue("licenseplate", licensePlate);
            command.ExecuteNonQuery();
            transaction.Commit();
        }

        public List<Registration> GetRegistrations()
        {
            using var connection = new Npgsql.NpgsqlConnection(connectionstring);
            connection.Open();
            using var command = new Npgsql.NpgsqlCommand("SELECT * FROM registrations", connection);

            using var reader = command.ExecuteReader();
            var registrations = new List<Registration>();
            while (reader.Read())
            {
                registrations.Add(
                    new Registration(
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetDateTime(3),
                        new CarDescription(
                            reader.GetString(4),
                            reader.GetString(5),
                            reader.GetString(6)
                        )
                    )
                );
            }
            return registrations;
        }
    }

    // public class ParkingLot : IparkingLot
    // {
    //     private static List<Registration> registrations = new List<Registration>();
    //
    //     public Registration Register(
    //         ParkingRequest request,
    //         CarDescription carType,
    //         IEventStore eventStore
    //     )
    //     {
    //         var registration = new Registration(
    //             request.LicensePlate,
    //             request.ContactInformation,
    //             request.ParkingSpot,
    //             DateTime.Now,
    //             carType
    //         );
    //         registrations.Add(registration);
    //         eventStore.Raise("car_parked", registration);
    //         return registration;
    //     }
    //
    //     public Registration? CheckParking(string licensePlate, string parkingSpot)
    //     {
    //         var registration = registrations.LastOrDefault<Registration>(
    //             r => r.LicensePlate == licensePlate && r.ParkingSpot == parkingSpot
    //         );
    //         return registration;
    //     }
    //
    //     public void Delete(string licensePlate)
    //     {
    //         registrations.RemoveAll(r => r.LicensePlate == licensePlate);
    //     }
    //
    //     public List<Registration> GetRegistrations()
    //     {
    //         return registrations;
    //     }
    // }

    public record Registration(
        string LicensePlate,
        string ContactInformation,
        string ParkingSpot,
        DateTime ParkedTime,
        CarDescription CarType
    );

    public record ParkingRequest(
        string LicensePlate,
        string ParkingSpot,
        string ContactInformation
    );
}
