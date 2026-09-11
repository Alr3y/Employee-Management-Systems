using EmployeeManagementSystem.Api.Data;
using EmployeeManagementSystem.Api.DTOs;
using EmployeeManagementSystem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Api.Services;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllAsync(int? departmentId = null, bool? isActive = null);
    Task<EmployeeDto?> GetByIdAsync(int id);
    Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto);
    Task<EmployeeDto?> UpdateAsync(int id, UpdateEmployeeDto dto);
    Task<bool> DeleteAsync(int id);
}

public class EmployeeService : IEmployeeService
{
    private readonly AppDbContext _context;

    public EmployeeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllAsync(int? departmentId = null, bool? isActive = null)
    {
        var query = _context.Employees
            .AsNoTracking()
            .Include(e => e.Department)
            .AsQueryable();

        if (departmentId.HasValue)
            query = query.Where(e => e.DepartmentId == departmentId.Value);

        if (isActive.HasValue)
            query = query.Where(e => e.IsActive == isActive.Value);

        return await query
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .Select(e => MapToDto(e))
            .ToListAsync();
    }

    public async Task<EmployeeDto?> GetByIdAsync(int id)
    {
        var employee = await _context.Employees
            .AsNoTracking()
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id);

        return employee is null ? null : MapToDto(employee);
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
    {
        await EnsureDepartmentExists(dto.DepartmentId);

        var employee = new Employee
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            HireDate = dto.HireDate.ToUniversalTime(),
            Salary = dto.Salary,
            IsActive = dto.IsActive,
            DepartmentId = dto.DepartmentId
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        await _context.Entry(employee).Reference(e => e.Department).LoadAsync();
        return MapToDto(employee);
    }

    public async Task<EmployeeDto?> UpdateAsync(int id, UpdateEmployeeDto dto)
    {
        var employee = await _context.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (employee is null) return null;

        await EnsureDepartmentExists(dto.DepartmentId);

        employee.FirstName = dto.FirstName;
        employee.LastName = dto.LastName;
        employee.Email = dto.Email;
        employee.Phone = dto.Phone;
        employee.HireDate = dto.HireDate.ToUniversalTime();
        employee.Salary = dto.Salary;
        employee.IsActive = dto.IsActive;
        employee.DepartmentId = dto.DepartmentId;
        employee.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        await _context.Entry(employee).Reference(e => e.Department).LoadAsync();

        return MapToDto(employee);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee is null) return false;

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task EnsureDepartmentExists(int departmentId)
    {
        var exists = await _context.Departments.AnyAsync(d => d.Id == departmentId);
        if (!exists)
            throw new InvalidOperationException($"Department with ID {departmentId} does not exist.");
    }

    private static EmployeeDto MapToDto(Employee employee) =>
        new(
            employee.Id,
            employee.FirstName,
            employee.LastName,
            employee.Email,
            employee.Phone,
            employee.HireDate,
            employee.Salary,
            employee.IsActive,
            employee.DepartmentId,
            employee.Department.Name
        );
}
