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
            SqlConnection connection = new SqlConnection(ConnectionString);
            using (connection)
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
                SqlCommand cmd = new SqlCommand(query, connection);
                using (cmd)
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            Pet pet = new Pet
                            {
                                ID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Age = reader.GetInt32(8),
                                Weight = reader.GetDecimal(9),
                                Gender = reader.GetString(10),
                                PetStory = reader.GetString(11),
                                Status = reader.GetString(12),
                                B64Image = reader.GetString(19),
                                PetTypeName = reader.GetString("TypeName"),
                                BreedName = reader.GetString("BreedName"),
                                LocationName = reader.GetString("LocationName"),
                                PostedByFirstName = reader.GetString("PostedByFirstName"),
                                PostedByLastName = reader.GetString("PostedByLastName"),
                                AdoptedByFirstName = reader.IsDBNull("AdoptedByFirstName") ? null : reader.GetString("AdoptedByFirstName"),
                                AdoptedByLastName = reader.IsDBNull("AdoptedByLastName") ? null : reader.GetString("AdoptedByLastName")
                            };
                            pets.Add(pet);
                        }
                    }
                }
            }
            return pets;
        }

        public List<Pet> getFilteredPets(string petType, string breed, string location)
        {
            List<Pet> pets = new List<Pet>();
            SqlConnection connection = new SqlConnection(ConnectionString);
            using (connection)
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
                                WHERE (@petType = 'All' OR pt.TypeName = @petType)
                                AND (@breed = 'All' OR pb.BreedName = @breed)
                                AND (@location = 'All' OR l.LocationName = @location)";

                SqlCommand cmd = new SqlCommand(query, connection);
                using (cmd)
                {
                    cmd.Parameters.AddWithValue("@petType", petType);
                    cmd.Parameters.AddWithValue("@breed", breed);
                    cmd.Parameters.AddWithValue("@location", location);

                    SqlDataReader reader = cmd.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            Pet pet = new Pet
                            {
                                ID = reader.GetInt32("ID"),
                                Name = reader.GetString("Name"),
                                Age = reader.GetInt32("Age"),
                                Weight = reader.GetDecimal("Weight"),
                                Gender = reader.GetString("Gender"),
                                PetStory = reader.GetString("PetStory"),
                                Status = reader.GetString("Status"),
                                B64Image = reader.GetString("B64Image"),
                                PetTypeName = reader.GetString("TypeName"),
                                BreedName = reader.GetString("BreedName"),
                                LocationName = reader.GetString("LocationName"),
                                PostedByFirstName = reader.GetString("PostedByFirstName"),
                                PostedByLastName = reader.GetString("PostedByLastName"),
                                AdoptedByFirstName = reader.IsDBNull("AdoptedByFirstName") ? null : reader.GetString("AdoptedByFirstName"),
                                AdoptedByLastName = reader.IsDBNull("AdoptedByLastName") ? null : reader.GetString("AdoptedByLastName")
                            };
                            pets.Add(pet);
                        }
                    }
                }
            }
            return pets;
        }

        public Pet getPetById(int id)
        {
            Pet pet = null;
            SqlConnection connection = new SqlConnection(ConnectionString);
            using (connection)
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

                SqlCommand cmd = new SqlCommand(query, connection);
                using (cmd)
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    using (reader)
                    {
                        if (reader.Read())
                        {
                            pet = new Pet
                            {
                                ID = reader.GetInt32("ID"),
                                Name = reader.GetString("Name"),
                                Age = reader.GetInt32("Age"),
                                Weight = reader.GetDecimal("Weight"),
                                Gender = reader.GetString("Gender"),
                                PetStory = reader.GetString("PetStory"),
                                Status = reader.GetString("Status"),
                                B64Image = reader.GetString("B64Image"),
                                PetTypeName = reader.GetString("TypeName"),
                                BreedName = reader.GetString("BreedName"),
                                LocationName = reader.GetString("LocationName"),
                                PostedByFirstName = reader.GetString("PostedByFirstName"),
                                PostedByLastName = reader.GetString("PostedByLastName")
                            };
                        }
                    }
                }
            }
            return pet;
        }

        public List<User> getAllUsers()
        {
            List<User> users = new List<User>();
            SqlConnection connection = new SqlConnection(ConnectionString);
            using (connection)
            {
                connection.Open();
                string query = "SELECT ID, FirstName, LastName, PhoneNumber FROM Users";
                SqlCommand cmd = new SqlCommand(query, connection);
                using (cmd)
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            User user = new User
                            {
                                ID = reader.GetInt32("ID"),
                                FirstName = reader.GetString("FirstName"),
                                LastName = reader.GetString("LastName"),
                                PhoneNumber = reader.GetString("PhoneNumber")
                            };
                            users.Add(user);
                        }
                    }
                }
            }
            return users;
        }

        public List<PetType> getAllPetTypes()
        {
            List<PetType> petTypes = new List<PetType>();
            SqlConnection connection = new SqlConnection(ConnectionString);
            using (connection)
            {
                connection.Open();
                string query = "SELECT ID, TypeName FROM PetTypes";
                SqlCommand cmd = new SqlCommand(query, connection);
                using (cmd)
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            PetType petType = new PetType
                            {
                                ID = reader.GetInt32("ID"),
                                TypeName = reader.GetString("TypeName")
                            };
                            petTypes.Add(petType);
                        }
                    }
                }
            }
            return petTypes;
        }

        public List<PetBreed> getAllPetBreeds()
        {
            List<PetBreed> petBreeds = new List<PetBreed>();
            SqlConnection connection = new SqlConnection(ConnectionString);
            using (connection)
            {
                connection.Open();
                string query = "SELECT ID, BreedName, PetTypeID FROM PetBreeds";
                SqlCommand cmd = new SqlCommand(query, connection);
                using (cmd)
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            PetBreed petBreed = new PetBreed
                            {
                                ID = reader.GetInt32("ID"),
                                BreedName = reader.GetString("BreedName"),
                                PetTypeID = reader.GetInt32("PetTypeID")
                            };
                            petBreeds.Add(petBreed);
                        }
                    }
                }
            }
            return petBreeds;
        }

        public List<Location> getAllLocations()
        {
            List<Location> locations = new List<Location>();
            SqlConnection connection = new SqlConnection(ConnectionString);
            using (connection)
            {
                connection.Open();
                string query = "SELECT ID, LocationName FROM Locations";
                SqlCommand cmd = new SqlCommand(query, connection);
                using (cmd)
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            Location location = new Location
                            {
                                ID = reader.GetInt32("ID"),
                                LocationName = reader.GetString("LocationName")
                            };
                            locations.Add(location);
                        }
                    }
                }
            }
            return locations;
        }

        public void adoptPet(int petId, int userId)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            using (connection)
            {
                connection.Open();
                string query = "UPDATE Pets SET Status = 'Adopted', AdoptedByUserID = @userId WHERE ID = @petId";
                SqlCommand cmd = new SqlCommand(query, connection);
                using (cmd)
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@petId", petId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void postPet(string name, int petTypeId, int breedId, int locationId, int age, decimal weight, string gender, string petStory, int postedByUserId, string b64Image)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            using (connection)
            {
                connection.Open();
                string query = @"INSERT INTO Pets (Name, PetTypeID, BreedID, LocationID, Age, Weight, Gender, PetStory, Status, PostedByUserID, B64Image) 
                                VALUES (@name, @petTypeId, @breedId, @locationId, @age, @weight, @gender, @petStory, 'Available', @postedByUserId, @b64Image)";
                SqlCommand cmd = new SqlCommand(query, connection);
                using (cmd)
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
                    cmd.Parameters.AddWithValue("@b64Image", b64Image);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void makeDonation(int userId, decimal amount)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            using (connection)
            {
                connection.Open();
                string query = "INSERT INTO Donations (UserID, Amount) VALUES (@userId, @amount)";
                SqlCommand cmd = new SqlCommand(query, connection);
                using (cmd)
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Donation> getAllDonations()
        {
            List<Donation> donations = new List<Donation>();
            SqlConnection connection = new SqlConnection(ConnectionString);
            using (connection)
            {
                connection.Open();
                string query = "SELECT ID, UserID, Amount, DonationDate FROM Donations";
                SqlCommand cmd = new SqlCommand(query, connection);
                using (cmd)
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            Donation donation = new Donation
                            {
                                ID = reader.GetInt32("ID"),
                                UserID = reader.GetInt32("UserID"),
                                Amount = reader.GetDecimal("Amount"),
                                DonationDate = reader.GetDateTime("DonationDate")
                            };
                            donations.Add(donation);
                        }
                    }
                }
            }
            return donations;
        }

        public decimal getTotalDonations()
        {
            decimal total = 0;
            SqlConnection connection = new SqlConnection(ConnectionString);
            using (connection)
            {
                connection.Open();
                string query = "SELECT ISNULL(SUM(Amount), 0) FROM Donations";
                SqlCommand cmd = new SqlCommand(query, connection);
                using (cmd)
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        total = Convert.ToDecimal(result);
                    }
                }
            }
            return total;
        }

        public int getTotalAdoptions()
        {
            int total = 0;
            SqlConnection connection = new SqlConnection(ConnectionString);
            using (connection)
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Pets WHERE Status = 'Adopted'";
                SqlCommand cmd = new SqlCommand(query, connection);
                using (cmd)
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        total = Convert.ToInt32(result);
                    }
                }
            }
            return total;
        }

        public List<Pet> getAdoptedPets()
        {
            List<Pet> pets = new List<Pet>();
            SqlConnection connection = new SqlConnection(ConnectionString);
            using (connection)
            {
                connection.Open();
                string query = @"SELECT p.Name, u.FirstName, u.LastName
                                FROM Pets p
                                INNER JOIN Users u ON p.AdoptedByUserID = u.ID
                                WHERE p.Status = 'Adopted'";
                SqlCommand cmd = new SqlCommand(query, connection);
                using (cmd)
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            Pet pet = new Pet
                            {
                                Name = reader.GetString("Name"),
                                AdoptedByFirstName = reader.GetString("FirstName"),
                                AdoptedByLastName = reader.GetString("LastName")
                            };
                            pets.Add(pet);
                        }
                    }
                }
            }
            return pets;
        }
    }