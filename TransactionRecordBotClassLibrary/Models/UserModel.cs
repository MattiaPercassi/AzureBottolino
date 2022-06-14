using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionRecordBotClassLibrary.Models
{
    public class UserModel
    {
        public long Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string FullInfo
        {
            get { return $"{FirstName} {LastName}\nid: {Id}\nemail: {Email}"; }
        }

        public UserModel()
        {

        }
        public UserModel(long id, string firstname, string lastname, string email)
        {
            Id = id;
            FirstName = firstname;
            LastName = lastname;
            Email = email;
        }
    }
}
