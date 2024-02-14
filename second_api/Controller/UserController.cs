using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using second_api.Data;
using second_api.Models;

namespace second_api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //public static IWebHostEnvironment _WebHostEnvironment;

        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            //_WebHostEnvironment = WebHostEnvironment;
            _context = context;
        }


        [HttpGet("GetAllUser")]
        // public async Task<IEnumerable<User>> Get()
        // => await _context.Users.ToListAsync();

        public async Task<IActionResult> Get()
        {
            // string path = _WebHostEnvironment.WebRootPath + "\\Upload\\";
            // var filePath = path + fileName + ".png";

            // if(System.IO.File.Exists(filePath))
            // {
            //     byte[] b = System.IO.File.ReadAllBytes(filePath);
            //     return new FileContentResult(b, "image/png");
            // }

            var userInfo = (from u in _context.Users
                            join a in _context.Address on u.Id equals a.Id
                            join e in _context.Education on u.Id equals e.Id
                            //join ui in _context.UserImage on u.Id equals ui.Id
                            select new User
                            {
                                Id = u.Id,
                                First_Name = u.First_Name,
                                Last_Name = u.Last_Name,
                                Username = u.Username,
                                Gandder = u.Gandder,
                                Email = u.Email,
                                Password = u.Password,
                                Confirm_Password = u.Confirm_Password,
                                Phone_Number = u.Phone_Number,
                                Address = new Address
                                {
                                    Id = a.Id,
                                    country = a.country,
                                    city = a.city,
                                    street = a.street
                                },
                                Education = new Education
                                {
                                    Id = e.Id,
                                    educationLevel = e.educationLevel,
                                    degree = e.degree,
                                    GeneralAppreciation = e.GeneralAppreciation,
                                    GraduationYear = e.GraduationYear
                                },
                                // UserImage = new UserImage
                                // {
                                //     Id = ui.Id,
                                //     files = ui.files
                                // },
                                Created = u.Created,
                                Completed = u.Completed
                            }).ToList();

            var userDTOs = await _context.Users.ToListAsync();
            var addressDTOs = await _context.Address.ToListAsync();
            var educationDTOs = await _context.Education.ToListAsync();
            // var userImageDTOs = await _context.UserImage.ToListAsync();
            userDTOs.ForEach(u =>
            {
                // userImageDTOs.ForEach(ui =>
                // {
                //     if (ui.Id == u.UserImageId)
                //     {
                //         u.UserImage = ui;
                //     }
                // });
                addressDTOs.ForEach(a =>
                {
                    if (a.Id == u.AddressId)
                    {
                        u.Address = a;
                    }
                });
                educationDTOs.ForEach(e =>
                {
                    if (e.Id == u.EducationId)
                    {
                        u.Education = e;
                    }
                });
            });
            return Ok(userDTOs);
        }

        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return Ok("This User Not Exist");


            var addressDTOs = await _context.Set<Address>().FirstOrDefaultAsync(a => a.Id == user.AddressId);
            user.Address = addressDTOs;

            var EducationDTOs = await _context.Set<Education>().FirstOrDefaultAsync(a => a.Id == user.EducationId);
            user.Education = EducationDTOs;
            return Ok(user);
        }

        [HttpGet("GetUserByUserName/{username}")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
            {
                return Ok("THIS USER IS NOT EXIST");
            }

            var addressDTOs = await _context.Set<Address>().FirstOrDefaultAsync(a => a.Id == user.AddressId);
            user.Address = addressDTOs;

            var EducationDTOs = await _context.Set<Education>().FirstOrDefaultAsync(a => a.Id == user.EducationId);
            user.Education = EducationDTOs;
            return Ok(user);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> Post(User user)
        {
            // public async Task<string> post([FromForm] fileUpload fileUpload)

            // try
            // {
            //     if (user.UserImage.files.Length > 0)
            //     {
            //         string path = _WebHostEnvironment.WebRootPath + "\\Upload\\";
            //         if (!Directory.Exists(path))
            //         {
            //             Directory.CreateDirectory(path);
            //         }
            //         using (FileStream fileStream = System.IO.File.Create(path + user.UserImage.files.FileName))
            //         {
            //             user.UserImage.files.CopyTo(fileStream);
            //             fileStream.Flush();
            //             return "Upload Success" ;
            //         }
            //     }
            //     else
            //     {
            //         return "Upload Failed";
            //     }
            // }
            // catch (Exception ex)
            // {
            //     return ex.Message.ToString();
            // }





            /* excptions*/
            var UserEx = await _context.Users.FirstOrDefaultAsync(x => x.Username == user.Username);
            if (UserEx != null)
            {
                return Ok("This User was Exist");
            }

            var EmailEx = await _context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (EmailEx != null)
            {
                return Ok("This Email was Exist");
            }

            var PhoneEx = await _context.Users.FirstOrDefaultAsync(x => x.Phone_Number == user.Phone_Number);
            if (PhoneEx != null)
            {
                return Ok("This Phone Number was Exist");
            }

            if (!user.Password.Equals(user.Confirm_Password))
            {
                return Ok("The Confirm Password Dose not Equal Password");
            }


            user.Id = Guid.NewGuid();
            user.Address.Id = Guid.NewGuid();
            user.Created = DateTime.UtcNow;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), user, "Seccessfully Created");

        }

        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> Put(Guid id, User user)
        {
            if (id != user.Id)
            {
                return Ok("ID NOT FOUND");
            }
            user.Completed = DateTime.UtcNow;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("Seccessfully Updated");
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userToDelete = await _context.Users.FindAsync(id);
            if (userToDelete == null) return Ok("ID NOT FOUND");

            _context.Users.Remove(userToDelete);
            _context.SaveChanges();
            return Ok("Seccessfully Deleted");
        }
    }
}
