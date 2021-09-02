using AutoFixture;
using CORE;
using DAL;
using System;
using Xunit;

namespace Ado.DataAccess.Tests
{
    public class UnitTest1 : IDisposable
    {
       private PersonRepository _personRepository;
        public UnitTest1()
        {
            string connectionstring = "Server=(localdb)\\mssqllocaldb;Database=AdoLessonTest;Trusted_Connection=True;";

            _personRepository = new PersonRepository(connectionstring);


        }

        public void Dispose()
        {
            _personRepository.CleanTable();
        }

        [Fact]
        public void Test1()
        {

            //Arrange
            var fixture = new Fixture();
            var person = fixture.Create<Person>();
          
            //Act
            var personid=_personRepository.CreatePersonAsync(person);

            //Assert
            Assert.True(personid > 0);
        }
    }
}
