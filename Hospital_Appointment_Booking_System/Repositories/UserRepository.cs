﻿using AutoMapper;
using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Helpers;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace Hospital_Appointment_Booking_System.Repositories
{
    public class UserRepository : IUserRepository
    {
        readonly Master_Hospital_ManagementContext _dbContext = new();
        private readonly IMapper _mapper;

        public UserRepository(Master_Hospital_ManagementContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<bool> AddUser(User user)
        {
            var existingUserWithEmail = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUserWithEmail != null)
            {
                return false; 
            }

            var existingUserWithMobileNumber = await _dbContext.Users.FirstOrDefaultAsync(u => u.MobileNumber == user.MobileNumber);
            if (existingUserWithMobileNumber != null)
            {
                return false; 
            }

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        

        public async Task<User> GetUserById(int userId)
        {
            return await _dbContext.Users.FindAsync(userId);
        }

        public async Task<bool> UpdateUser(User updatedUser)
        {
            // Check for duplicate email and mobile number

            var existingUserWithEmail = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == updatedUser.Email && u.UserId != updatedUser.UserId);
            if (existingUserWithEmail != null)
            {
                return false;
            }

            var existingUserWithMobileNumber = await _dbContext.Users.FirstOrDefaultAsync(u => u.MobileNumber == updatedUser.MobileNumber && u.UserId != updatedUser.UserId);
            if (existingUserWithMobileNumber != null)
            {
                return false;
            }

            // Update the user properties manually
            var existingUser = await _dbContext.Users.FindAsync(updatedUser.UserId);
            
                existingUser.Name = updatedUser.Name;
                existingUser.Email = updatedUser.Email;
                existingUser.Password = updatedUser.Password;
                existingUser.MobileNumber = updatedUser.MobileNumber;
                existingUser.RoleId = updatedUser.RoleId;
                existingUser.SpecializationId = updatedUser.SpecializationId;
                existingUser.HospitalId = updatedUser.HospitalId;

                await _dbContext.SaveChangesAsync();
                return true;
         
        }


        public async Task DeleteUser(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<UserDTO>> GetAllUsers()
        {

            var users = await _dbContext.Users
            .Include(u => u.Role)
            .ToListAsync();
            return _mapper.Map<List<UserDTO>>(users);
        }

       /* public async Task<List<UserDTO>> GetUsersByRoleId(int roleId)
        {
            List<User> users = await _dbContext.Users
                .Where(u => u.RoleId == roleId)
                .ToListAsync();
            List<UserDTO> userDTOs = _mapper.Map<List<UserDTO>>(users);
            return userDTOs;
        }

        public async Task<List<UserDTO>> GetUsersBySpecializationId(int specializationId)
        {
            var users = await _dbContext.Users
                .Include(u => u.Role)
                .Where(u => u.SpecializationId == specializationId)
                .ToListAsync();
            return _mapper.Map<List<UserDTO>>(users);
        }*/
    }
}