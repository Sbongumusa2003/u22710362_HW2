using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using u22710362_HW2.Controllers;

namespace u22710362_HW2.Models
{
    public class DefaultDataService
    {
        private String ConnectionString;

        public DefaultDataService()
        {
            ConnectionString = Globals.ConnectionString;
        }

        public List<Pet> getAllPets()
        {
            List<Pet> pets = new List<Pet>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = @"SELECT p.ID, p.Name, p.Age, p.Weight, p.Gender, p.PetStory, p.Status, p.B64Image,
                                    pt.TypeName, pb.BreedName, l.LocationName, 
                                    u1.FirstName as PostedByFirstName, u1.LastName as PostedByLastName,
                                    u2.FirstName as AdoptedByFirstName, u2.LastName as AdoptedByLastName
                                    FROM Pets p
                                    INNER JOIN PetTypes pt ON p.PetTypeID = pt.ID
                                    INNER JOIN PetBreeds pb ON p.BreedID = pb.ID
                                    INNER JOIN Locations l ON p.LocationID = l.ID
                                    INNER JOIN Users u1 ON p.PostedByUserID = u1.ID
                                    LEFT JOIN Users u2 ON p.AdoptedByUserID = u2.ID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Pet pet = new Pet
                                {
                                    ID = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Age = reader.GetInt32(2),
                                    Weight = reader.GetDecimal(3),
                                    Gender = reader.GetString(4),
                                    PetStory = reader.GetString(5),
                                    Status = reader.GetString(6),
                                    B64Image = reader.GetString(7),
                                    PetTypeName = reader.GetString(8),
                                    BreedName = reader.GetString(9),
                                    LocationName = reader.GetString(10),
                                    PostedByFirstName = reader.GetString(11),
                                    PostedByLastName = reader.GetString(12),
                                    AdoptedByFirstName = reader.IsDBNull(13) ? null : reader.GetString(13),
                                    AdoptedByLastName = reader.IsDBNull(14) ? null : reader.GetString(14)
                                };
                                pets.Add(pet);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error retrieving all pets: {ex.Message}", ex);
            }

            return pets;
        }

        public List<Pet> getFilteredPets(string petType, string breed, string location)
        {
            List<Pet> pets = new List<Pet>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = @"SELECT p.ID, p.Name, p.Age, p.Weight, p.Gender, p.PetStory, p.Status, p.B64Image,
                                    pt.TypeName, pb.BreedName, l.LocationName, 
                                    u1.FirstName as PostedByFirstName, u1.LastName as PostedByLastName,
                                    u2.FirstName as AdoptedByFirstName, u2.LastName as AdoptedByLastName
                                    FROM Pets p
                                    INNER JOIN PetTypes pt ON p.PetTypeID = pt.ID
                                    INNER JOIN PetBreeds pb ON p.BreedID = pb.ID
                                    INNER JOIN Locations l ON p.LocationID = l.ID
                                    INNER JOIN Users u1 ON p.PostedByUserID = u1.ID
                                    LEFT JOIN Users u2 ON p.AdoptedByUserID = u2.ID
                                    WHERE p.Status = 'Available'
                                    AND (@petType = 'All' OR pt.TypeName = @petType)
                                    AND (@breed = 'All' OR pb.BreedName = @breed)
                                    AND (@location = 'All' OR l.LocationName = @location)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@petType", petType ?? "All");
                        cmd.Parameters.AddWithValue("@breed", breed ?? "All");
                        cmd.Parameters.AddWithValue("@location", location ?? "All");

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Pet pet = new Pet
                                {
                                    ID = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Age = reader.GetInt32(2),
                                    Weight = reader.GetDecimal(3),
                                    Gender = reader.GetString(4),
                                    PetStory = reader.GetString(5),
                                    Status = reader.GetString(6),
                                    B64Image = reader.GetString(7),
                                    PetTypeName = reader.GetString(8),
                                    BreedName = reader.GetString(9),
                                    LocationName = reader.GetString(10),
                                    PostedByFirstName = reader.GetString(11),
                                    PostedByLastName = reader.GetString(12),
                                    AdoptedByFirstName = reader.IsDBNull(13) ? null : reader.GetString(13),
                                    AdoptedByLastName = reader.IsDBNull(14) ? null : reader.GetString(14)
                                };
                                pets.Add(pet);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error retrieving filtered pets: {ex.Message}", ex);
            }

            return pets;
        }

        public Pet getPetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Pet ID must be greater than zero", nameof(id));
            }

            Pet pet = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = @"SELECT p.ID, p.Name, p.Age, p.Weight, p.Gender, p.PetStory, p.Status, p.B64Image,
                                    pt.TypeName, pb.BreedName, l.LocationName, 
                                    u1.FirstName as PostedByFirstName, u1.LastName as PostedByLastName
                                    FROM Pets p
                                    INNER JOIN PetTypes pt ON p.PetTypeID = pt.ID
                                    INNER JOIN PetBreeds pb ON p.BreedID = pb.ID
                                    INNER JOIN Locations l ON p.LocationID = l.ID
                                    INNER JOIN Users u1 ON p.PostedByUserID = u1.ID
                                    WHERE p.ID = @id";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                pet = new Pet
                                {
                                    ID = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Age = reader.GetInt32(2),
                                    Weight = reader.GetDecimal(3),
                                    Gender = reader.GetString(4),
                                    PetStory = reader.GetString(5),
                                    Status = reader.GetString(6),
                                    B64Image = reader.GetString(7),
                                    PetTypeName = reader.GetString(8),
                                    BreedName = reader.GetString(9),
                                    LocationName = reader.GetString(10),
                                    PostedByFirstName = reader.GetString(11),
                                    PostedByLastName = reader.GetString(12)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error retrieving pet with ID {id}: {ex.Message}", ex);
            }

            return pet;
        }

        public List<User> getAllUsers()
        {
            List<User> users = new List<User>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT ID, FirstName, LastName, PhoneNumber FROM Users";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User user = new User
                                {
                                    ID = reader.GetInt32(0),
                                    FirstName = reader.GetString(1),
                                    LastName = reader.GetString(2),
                                    PhoneNumber = reader.GetString(3)
                                };
                                users.Add(user);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error retrieving all users: {ex.Message}", ex);
            }

            return users;
        }

        public List<PetType> getAllPetTypes()
        {
            List<PetType> petTypes = new List<PetType>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT ID, TypeName FROM PetTypes";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PetType petType = new PetType
                                {
                                    ID = reader.GetInt32(0),
                                    TypeName = reader.GetString(1)
                                };
                                petTypes.Add(petType);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error retrieving pet types: {ex.Message}", ex);
            }

            return petTypes;
        }

        public List<PetBreed> getAllPetBreeds()
        {
            List<PetBreed> petBreeds = new List<PetBreed>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT ID, BreedName, PetTypeID FROM PetBreeds";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PetBreed petBreed = new PetBreed
                                {
                                    ID = reader.GetInt32(0),
                                    BreedName = reader.GetString(1),
                                    PetTypeID = reader.GetInt32(2)
                                };
                                petBreeds.Add(petBreed);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error retrieving pet breeds: {ex.Message}", ex);
            }

            return petBreeds;
        }

        public List<Location> getAllLocations()
        {
            List<Location> locations = new List<Location>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT ID, LocationName FROM Locations";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Location location = new Location
                                {
                                    ID = reader.GetInt32(0),
                                    LocationName = reader.GetString(1)
                                };
                                locations.Add(location);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error retrieving locations: {ex.Message}", ex);
            }

            return locations;
        }

        public void adoptPet(int petId, int userId)
        {
            if (petId <= 0)
            {
                throw new ArgumentException("Pet ID must be greater than zero", nameof(petId));
            }

            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than zero", nameof(userId));
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = "UPDATE Pets SET Status = 'Adopted', AdoptedByUserID = @userId WHERE ID = @petId AND Status = 'Available'";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@petId", petId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            throw new InvalidOperationException($"Pet with ID {petId} is no longer available for adoption or does not exist.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error adopting pet ID {petId}: {ex.Message}", ex);
            }
        }

        public void postPet(string name, int petTypeId, int breedId, int locationId, int age, decimal weight, string gender, string petStory, int postedByUserId, string b64Image)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Pet name cannot be empty", nameof(name));
            }

            if (petTypeId <= 0)
            {
                throw new ArgumentException("Pet type ID must be greater than zero", nameof(petTypeId));
            }

            if (breedId <= 0)
            {
                throw new ArgumentException("Breed ID must be greater than zero", nameof(breedId));
            }

            if (locationId <= 0)
            {
                throw new ArgumentException("Location ID must be greater than zero", nameof(locationId));
            }

            if (age < 0)
            {
                throw new ArgumentException("Age cannot be negative", nameof(age));
            }

            if (weight <= 0)
            {
                throw new ArgumentException("Weight must be greater than zero", nameof(weight));
            }

            if (string.IsNullOrWhiteSpace(gender))
            {
                throw new ArgumentException("Gender cannot be empty", nameof(gender));
            }

            if (string.IsNullOrWhiteSpace(petStory))
            {
                throw new ArgumentException("Pet story cannot be empty", nameof(petStory));
            }

            if (postedByUserId <= 0)
            {
                throw new ArgumentException("Posted by user ID must be greater than zero", nameof(postedByUserId));
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = @"INSERT INTO Pets (Name, PetTypeID, BreedID, LocationID, Age, Weight, Gender, PetStory, Status, PostedByUserID, B64Image) 
                                    VALUES (@name, @petTypeId, @breedId, @locationId, @age, @weight, @gender, @petStory, 'Available', @postedByUserId, @b64Image)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@petTypeId", petTypeId);
                        cmd.Parameters.AddWithValue("@breedId", breedId);
                        cmd.Parameters.AddWithValue("@locationId", locationId);
                        cmd.Parameters.AddWithValue("@age", age);
                        cmd.Parameters.AddWithValue("@weight", weight);
                        cmd.Parameters.AddWithValue("@gender", gender);
                        cmd.Parameters.AddWithValue("@petStory", petStory);
                        cmd.Parameters.AddWithValue("@postedByUserId", postedByUserId);
                        cmd.Parameters.AddWithValue("@b64Image", b64Image ?? string.Empty);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            throw new InvalidOperationException("Failed to create pet record.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error posting pet: {ex.Message}", ex);
            }
        }

        public void makeDonation(int userId, decimal amount)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than zero", nameof(userId));
            }

            if (amount <= 0)
            {
                throw new ArgumentException("Donation amount must be greater than zero", nameof(amount));
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Donations (UserID, Amount) VALUES (@userId, @amount)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@amount", amount);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            throw new InvalidOperationException("Failed to record donation.");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547) // Foreign key constraint violation
                {
                    throw new InvalidOperationException("Invalid user ID provided.", ex);
                }
                throw new Exception($"Database error making donation: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error making donation: {ex.Message}", ex);
            }
        }

        public List<Donation> getAllDonations()
        {
            List<Donation> donations = new List<Donation>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT ID, UserID, Amount, DonationDate FROM Donations";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Donation donation = new Donation
                                {
                                    ID = reader.GetInt32(0),
                                    UserID = reader.GetInt32(1),
                                    Amount = reader.GetDecimal(2),
                                    DonationDate = reader.GetDateTime(3)
                                };
                                donations.Add(donation);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error retrieving donations: {ex.Message}", ex);
            }

            return donations;
        }

        public decimal getTotalDonations()
        {
            decimal total = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT ISNULL(SUM(Amount), 0) FROM Donations";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            total = Convert.ToDecimal(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error calculating total donations: {ex.Message}", ex);
            }

            return total;
        }

        public int getTotalAdoptions()
        {
            int total = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Pets WHERE Status = 'Adopted'";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            total = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database error calculating total adoptions: {ex.Message}", ex);
            }
            return total;
        }

        public List<Pet> getAdoptedPets()
        {
            List<Pet> pets = new List<Pet>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = @"SELECT p.Name, u.FirstName, u.LastName
                                    FROM Pets p
                                    INNER JOIN Users u ON p.AdoptedByUserID = u.ID
                                    WHERE p.Status = 'Adopted'";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Pet pet = new Pet
                                {
                                    Name = reader.GetString(0),
                                    AdoptedByFirstName = reader.GetString(1),
                                    AdoptedByLastName = reader.GetString(2)
                                };
                                pets.Add(pet);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error retrieving adopted pets: {ex.Message}", ex);
            }

            return pets;
        }
    }
}