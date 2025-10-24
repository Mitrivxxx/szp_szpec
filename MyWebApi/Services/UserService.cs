using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyWebApi.Data;
using MyWebApi.DTOs;
using MyWebApi.Model;
using MyWebApi.Services.Interface;

namespace MyWebApi.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Pobierz wszystkich użytkowników
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.User.ToListAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        // Pobierz użytkownika po ID
        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _context.User.FindAsync(id);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        // Utwórz nowego użytkownika
        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            // Walidacja - czy user już istnieje
            if (await UserExistsAsync(createUserDto.Login))
            {
                throw new InvalidOperationException("Użytkownik o takim loginie już istnieje.");
            }

            // Mapowanie i hashowanie hasła
            var user = _mapper.Map<User>(createUserDto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);
            user.Role = string.IsNullOrEmpty(createUserDto.Role) ? "User" : createUserDto.Role;

            // Zapis do bazy
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        // Aktualizuj użytkownika
        public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
                return null;

            // Aktualizuj tylko te pola, które nie są null
            if (!string.IsNullOrWhiteSpace(updateUserDto.Login))
                user.Login = updateUserDto.Login;

            if (!string.IsNullOrWhiteSpace(updateUserDto.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password);

            if (!string.IsNullOrWhiteSpace(updateUserDto.Role))
                user.Role = updateUserDto.Role;

            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        // Usuń użytkownika
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
                return false;

            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        // Logowanie użytkownika
        public async Task<UserDto?> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Login == loginDto.Login);

            if (user == null)
                return null;

            // Weryfikacja hasła
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
            if (!isPasswordValid)
                return null;

            return _mapper.Map<UserDto>(user);
        }

        // Sprawdź czy użytkownik istnieje
        public async Task<bool> UserExistsAsync(string login)
        {
            return await _context.User.AnyAsync(u => u.Login == login);
        }
    }
}
