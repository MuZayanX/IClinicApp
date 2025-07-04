﻿using IClinicApp.API.Data;
using IClinicApp.API.Dtos.City;
using IClinicApp.API.Dtos.Doctors;
using IClinicApp.API.Dtos.Governorate;
using IClinicApp.API.Dtos.Specialization;
using IClinicApp.API.Models.Entities;
using IClinicApp.API.Repos.Services;
using Microsoft.AspNetCore.Identity;

namespace IClinicApp.API.Repos.Implementations
{
    public class AdminService(ApplicationDbContext context, IDtoMapper dtoMapper, UserManager<ApplicationUser> userManager) : IAdminService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IDtoMapper _dtoMapper = dtoMapper;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<Doctor> AddDoctorAsync(AddDoctorDto addDoctorDto)
        {
            var doctorUser = await _userManager.FindByEmailAsync(addDoctorDto.Email);
            if (doctorUser != null)
            {
                throw new Exception("A user with this email already exists.");
            }
            var user = new ApplicationUser
            {
                UserName = addDoctorDto.Email,
                Email = addDoctorDto.Email,
                FullName = addDoctorDto.FullName,
                PhoneNumber = addDoctorDto.PhoneNumber,
                EmailConfirmed = true, // Assuming email confirmation is handled elsewhere
            };

            var result = await _userManager.CreateAsync(user, addDoctorDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create user: {errors}");
            }
            await _userManager.AddToRoleAsync(user, "Doctor");

            // Map the DTO to the Doctor entity
            var doctor = _dtoMapper.MapToAddDoctor(addDoctorDto);

            doctor.UserId = user.Id; // Link the doctor to the created user
            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();
            return doctor;
        }
        public async Task<Governorate> AddGovernorateAsync(AddGovernorateDto dto)
        {
            var governorate = new Governorate
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
            };
            await _context.Governorates.AddAsync(governorate);
            await _context.SaveChangesAsync();
            return governorate;
        }
        public async Task<City> AddCityAsync(AddCityDto dto)
        {
            var city = new City
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                GovernorateId = dto.GovernorateId,
            };
            await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();
            return city;
        }
        public async Task<Specialization> AddSpecializationAsync(AddSpecializationDto dto)
        {
            var specialization = new Specialization
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
            };
            await _context.Specializations.AddAsync(specialization);
            await _context.SaveChangesAsync();
            return specialization;
        }

    }
}
