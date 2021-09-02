using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CORE
{
    public interface IPersonRepository
    {
        Person GetPersonByIdAsync(int id);
        List<Person> GetPersonsAsync();
        int CreatePersonAsync(Person person);
        void UpdatePersonAsync(Person person);
        void DeletePersonAsync(int id);
    }
}
