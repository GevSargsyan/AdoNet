using CORE;
using System;
using System.Data.SqlClient;

namespace ADONET
{
    class Program
    {
        private const string connectionstring = "Server=(localdb)\\mssqllocaldb;Database=AdoLesson;Trusted_Connection=True;";
        static void GetCourses()
        {
            using (var connection = new SqlConnection(connectionstring))
            {
                connection.Open();

                var query = new SqlCommand("select * from Course", connection);

                var reader = query.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var coursename = reader.GetString(1);
                        Console.WriteLine($"Id : {id}, Coursename : {coursename}");
                    }

                }
                Console.WriteLine(new string('-', 50));

            }

        }
        static void GetCoursesById(int Id)
        {
            using (var connection = new SqlConnection(connectionstring))
            {
                connection.Open();

                var query = new SqlCommand(
                    @"select * from Course
                    where CourseId=@Id", connection);

                query.Parameters.AddWithValue("@Id", Id);

                var reader = query.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var coursename = reader.GetString(1);
                        Console.WriteLine($"Id : {id}, Coursename : {coursename}");
                    }
                    Console.WriteLine(new string('-', 50));

                }
                else
                {
                    Console.WriteLine("Nothing Founded");
                    Console.WriteLine(new string('-', 50));

                }

            }

        }
        static void GetCoursesByName(string searchname)
        {
            using (var connection = new SqlConnection(connectionstring))
            {
                connection.Open();

                var query = new SqlCommand(
                    @"select * from Course
                    where CourseName Like @name", connection);

                query.Parameters.AddWithValue("@name", searchname);

                var reader = query.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var coursename = reader.GetString(1);
                        Console.WriteLine($"Id : {id}, Coursename : {coursename}");
                    }
                    Console.WriteLine(new string('-', 50));

                }
                else
                {
                    Console.WriteLine("Nothing Founded");
                    Console.WriteLine(new string('-', 50));

                }

            }

        }
        static void InsertCourse(string CourseName)
        {
            using (var connection = new SqlConnection(connectionstring))
            {
                connection.Open();

                var query = new SqlCommand("insert into Course (CourseName) values(@CName)", connection);
                query.Parameters.AddWithValue("@CName", CourseName);
                var reader = query.ExecuteNonQuery();
                Console.WriteLine(new string('-', 50));
                GetCourses();
            }

        }


        static int CreatePersonAsync(Person newPerson)
        {
            if (newPerson is null)
            {
                throw new ArgumentNullException(nameof(newPerson));
            }

            var newPersonEntity = new DAL.Entites.Person
            {

                Age = newPerson.Age,
                FirstName = newPerson.FirstName,
                LastName = newPerson.LastName
            };

            using (var connection = new SqlConnection(connectionstring))
            {
                connection.Open();

                var query = new SqlCommand("insert into Person (Age,FirstName,LastName) Values(@Age,@FirstName,@LastName)", connection);

                query.Parameters.AddWithValue("@Age", newPerson.Age);
                query.Parameters.AddWithValue("@FirstName", newPerson.FirstName);
                query.Parameters.AddWithValue("@LastName", newPerson.LastName);

                var resultParameter = new SqlParameter
                {
                    Direction = System.Data.ParameterDirection.Output,
                    SqlDbType = System.Data.SqlDbType.Int,
                    ParameterName = "@Id"
                };

                query.Parameters.Add(resultParameter);

                var reader = query.ExecuteNonQuery();

                if (query.Parameters["@Id"].Value is int personId)
                {
                    return personId;
                }
                else
                {
                    throw new InvalidOperationException("Value Id cannot be converted");

                }
            }

            
        }
        static void Main()
        {

            //GetCourses();
            //GetCoursesById(5);
            //GetCoursesByName("%eren");
            //InsertCourse("Hayeren");
            Person person = new Person { FirstName = "Gev", LastName = "Sargsyan", Age = 20 };
            CreatePersonAsync(person);
            Console.ReadKey();

        }
    }
}