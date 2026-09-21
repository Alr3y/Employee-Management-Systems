using EmployeeManagementSystem.Api.Data;
using EmployeeManagementSystem.Api.DTOs;
using EmployeeManagementSystem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Api.Services;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> GetAllAsync();

    Task<DepartmentDto?> GetByIdAsync(int id);

    Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto);

    Task<DepartmentDto?> UpdateAsync(int id, UpdateDepartmentDto dto);

    Task<bool> DeleteAsync(int id);
}

public class DepartmentService : IDepartmentService
{
    private readonly AppDbContext _context;

    public DepartmentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        //return await _context.Departments
        //    .Include(d => d.Employees)
        //    .AsNoTracking()
        //    .Select(d => new DepartmentDto(
        //        d.Id,
        //        d.Name,
        //        d.Description,
        //        d.Employees.Count))
        //    .OrderBy(d => d.Name)
        //    .ToListAsync();
        var departments = await _context.Departments
     .AsNoTracking()
     .Select(d => new
     {
         d.Id,
         d.Name,
         d.Description,
         EmployeeCount = _context.Employees.Count(e => e.DepartmentId == d.Id)
     })
     .OrderBy(x => x.Name)
     .ToListAsync();

        return departments.Select(x => new DepartmentDto(x.Id, x.Name, x.Description, x.EmployeeCount));
    }

    public async Task<DepartmentDto?> GetByIdAsync(int id)
    {
        return await _context.Departments
            .AsNoTracking()
            .Where(d => d.Id == id)
            .Select(d => new DepartmentDto(
                d.Id,
                d.Name,
                d.Description,
                d.Employees.Count))
            .FirstOrDefaultAsync();
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
    {
        var department = new Department
        {
            Name = dto.Name,
            Description = dto.Description
        };

        _context.Departments.Add(department);
        await _context.SaveChangesAsync();

        return new DepartmentDto(department.Id, department.Name, department.Description, 0);
    }

    public async Task<DepartmentDto?> UpdateAsync(int id, UpdateDepartmentDto dto)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department is null) return null;

        department.Name = dto.Name;
        department.Description = dto.Description;

        await _context.SaveChangesAsync();

        var employeeCount = await _context.Employees.CountAsync(e => e.DepartmentId == id);
        return new DepartmentDto(department.Id, department.Name, department.Description, employeeCount);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var department = await _context.Departments
            .Include(d => d.Employees)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (department is null) return false;

        if (department.Employees.Any())
            throw new InvalidOperationException("Cannot delete department that has employees.");

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();
        return true;
    }
}