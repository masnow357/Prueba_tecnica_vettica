using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaTecnica.Models
{
    [Index(nameof(Email), nameof(Document), nameof(CardSerial), IsUnique = true)]
    public class User
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
		public string Name { get; set; }
        public string Email { get; set; }
		public int DocumentTypeID { get; set; }
        public string Document { get; set; }
        public string CardSerial { get; set; }
		public string Password { get; set; }
		public int RoleID { get; set; }

        [ForeignKey("RoleID")]
        public Role? Role { get; set; }

        [ForeignKey("DocumentTypeID")]
        public DocumentType? DocumentType { get; set; }

        public User(
            string name, 
            string email, 
            string document,
            string cardSerial,
            string password,
            int documentTypeID = 1,
            int roleID = 2
            )
        {
            Name = name;
            Email = email;
            DocumentTypeID = documentTypeID;
            Document = document;
            CardSerial = cardSerial;
            Password = password;
            RoleID = roleID;
        }
    }
}

