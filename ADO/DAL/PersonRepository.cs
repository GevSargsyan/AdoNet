using CORE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public class PersonRepository : IPersonRepository
    {

        private readonly string _connectionstring = "Server=(localdb)\\mssqllocaldb;Database=AdoLesson;Trusted_Connection=True;";

        public PersonRepository()
        {

        }

        public PersonRepository(string con)
        {
            _connectionstring = con;
        }
        public int CreatePersonAsync(Person newPerson)
        {
            if (newPerson is null)
            {
                throw new ArgumentNullException(nameof(newPerson));
            }

            var newPersonEntity = new Entites.Person
            {
               
                Age = newPerson.Age,
                FirstName = newPerson.FirstName,
                LastName = newPerson.LastName
            };

            using (var connection = new SqlConnection(_connectionstring))
            {
                connection.Open();
                string querycom = "insert into Person (Age,FirstName,LastName,CreatedDate, UpdatedDate) Values(@Age,@FirstName,@LastName,@CreatedDate, @UpdatedDate)";
                querycom += " SELECT SCOPE_IDENTITY()";

                var query = new SqlCommand(querycom, connection);
               
                query.Parameters.AddWithValue("@Age", newPersonEntity.Age);
                query.Parameters.AddWithValue("@FirstName", newPersonEntity.FirstName);
                query.Parameters.AddWithValue("@LastName", newPersonEntity.LastName);
                query.Parameters.AddWithValue("@CreatedDate", newPersonEntity.CreatedDate);
                query.Parameters.AddWithValue("@UpdatedDate", newPersonEntity.UpdatedDate);

                return  Convert.ToInt32(query.ExecuteScalar());
            }
        }
     
        public void DeletePersonAsync(int id)
        {
            if (id < 0)
            {
                throw new ArgumentNullException(nameof(id));
            }

            using (var connection = new SqlConnection(_connectionstring))
            {
                connection.Open();
                string querycom = @"
					UPDATE Person
					SET DeletedDate = @DeletedDate
					WHERE Id = @Id";

                var query = new SqlCommand(querycom, connection);

                query.Parameters.AddWithValue("@Id", id);
                query.Parameters.AddWithValue("@DeletedDate", DateTime.Now);

                query.ExecuteNonQuery();
            }
        }

        public Person GetPersonByIdAsync(int id)
        {
           
                Person person = default;
            using (var connection = new SqlConnection(_connectionstring))
            {
                connection.Open();
                string querycom = "select Id,FirstName,LastName,Age from Person where Id=@Id and DeletedDate is NULL";

                var query = new SqlCommand(querycom, connection);
                query.Parameters.AddWithValue("@Id", id);

                var reader = query.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        person = new Person
                        {
                            Id = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Age = reader.GetInt32(3)
                        };
                    }

                }

            }
            return person;
        }

        public List<Person> GetPersonsAsync()
        {
            List<Person> persons = new List<Person>();
            using (var connection = new SqlConnection(_connectionstring))
            {
                connection.Open();
                string querycom = "select Id,FirstName,LastName,Age from Person where DeletedDate is NULL";


               var query = new SqlCommand(querycom, connection);
               var reader = query.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        persons.Add(new Person
                        {
                            Id=reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Age = reader.GetInt32(3)
                        });

                    }

                }

            }
            return persons;
        }

        public void UpdatePersonAsync(Person updateperson)
        {
            if (updateperson is null)
            {
                throw new ArgumentNullException(nameof(updateperson));
            }

            var updatePersonEntity = new Entites.Person
            {
                Id = updateperson.Id,
                Age = updateperson.Age,
                FirstName = updateperson.FirstName,
                LastName = updateperson.LastName
            };

            using (var connection = new SqlConnection(_connectionstring))
            {
                connection.Open();
                string querycom = "update Person set Age=@Age,FirstName=@FirstName,LastName=@LastName,UpdatedDate=@UpdatedDate where Id=@Id";

                var query = new SqlCommand(querycom, connection);

                query.Parameters.AddWithValue("@Id", updatePersonEntity.Id);
                query.Parameters.AddWithValue("@Age", updatePersonEntity.Age);
                query.Parameters.AddWithValue("@FirstName", updatePersonEntity.FirstName);
                query.Parameters.AddWithValue("@LastName", updatePersonEntity.LastName);
                query.Parameters.AddWithValue("@UpdatedDate", updatePersonEntity.UpdatedDate);

                query.ExecuteNonQuery();
            }
        }

        public void CleanTable()
        {
            using (var connection = new SqlConnection(_connectionstring))
            {
                connection.Open();

                var command = new SqlCommand(@"
					DELETE FROM Person",
                connection);

                command.ExecuteNonQuery();
            }
        }

    }
}
