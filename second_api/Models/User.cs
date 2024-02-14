using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace second_api.Models
{

    
    public class User
    {
        public Guid Id { get; set; }
        public Guid? EducationId { get; set; }
        public Guid? AddressId { get; set; }

        // public List<IFormFile> profilePicture { get; set; }
        //public IFormFile? ImageFile { get; set; }
        // public Guid? UserImageId { get; set; }
        // public UserImage UserImage { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Username { get; set; }
        public string Gandder { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Confirm_Password { get; set; }
        public string Phone_Number { get; set; }
        public Education Education { get; set; }
        public Address Address { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Completed { get; set; }
    }


    public class Address
    {
        public Guid Id { get; set; }
        public string country { get; set; }

        public string city { get; set; }

        public string street { get; set; }
    }

    public class Education
    {
        public Guid Id { get; set; }
        public string educationLevel { get; set; }
        public string degree { get; set; }
        public string GeneralAppreciation { get; set; }
        public string GraduationYear { get; set; }
    }

// public class UserProfilePicture
//     {
//         [Key]
//         public Guid UserProfilePictureId { get; set; }
//         public string UserId { get; set; }
//         public string ImageName { get; set; }
//         public string ImagePath { get; set; }
//     }
    // public class UserImage
    // {
    //     public Guid Id { get; set; }


    //    public byte[] files { get; set; }

    //     // public IFormFile files { get; set; }

    // }
}
