using Microsoft.EntityFrameworkCore;
using SimpleCRUD.Core.Common;
using SimpleCRUD.Infrastructure.Data;
using SimpleCRUD.Features.Employees.Models;
using SimpleCRUD.Features.Employees.Entities;

namespace SimpleCRUD.Features.Employees.Services
{
    public class EmployeeService
    {
        private readonly IDbContextFactory<AppDBContext> _factory;

        public EmployeeService(IDbContextFactory<AppDBContext> factory)
        {
            _factory = factory;
        }

        // GET ALL
        public async Task<List<EmployeeViewModel>> GetAllEmployees()
        {
            await using var context = await _factory.CreateDbContextAsync();

            return await context.Employees
                .AsNoTracking()
                .OrderBy(x => x.FullName)
                .Select(x => new EmployeeViewModel
                {
                    EmployeeId = x.EmployeeId,
                    FullName = x.FullName,
                    Department = x.Department,
                    DateOfBirth = x.DateOfBirth,
                    Age = x.Age,
                    PhoneNumber = x.PhoneNumber,
                    IsActive = x.IsActive
                })
                .ToListAsync();
        }

        // FIND
        public async Task<EmployeeViewModel?> FindEmployee(int employeeId)
        {
            await using var context = await _factory.CreateDbContextAsync();

            var employee = await context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            if (employee == null)
                return null;

            return new EmployeeViewModel
            {
                EmployeeId = employee.EmployeeId,
                FullName = employee.FullName,
                Department = employee.Department,
                DateOfBirth = employee.DateOfBirth,
                Age = employee.Age,
                PhoneNumber = employee.PhoneNumber,
                IsActive = employee.IsActive
            };
        }

        // TOGGLE STATUS
        public async Task ToggleStatus(int   id)
        {
            await using var context = await _factory.CreateDbContextAsync();

            var employee = await context.Employees.FindAsync(id);
            if (employee == null) return;

            employee.IsActive = !employee.IsActive;
            await context.SaveChangesAsync();
        }

        // ================= CREATE =================
        public async Task<Result<int>> CreateNewEmployee(EmployeeViewModel model)
        {
            try
            {
                await using var context = await _factory.CreateDbContextAsync();

                var employee = new Employee
                {
                    FullName = model.FullName,
                    Department = model.Department,
                    DateOfBirth = model.DateOfBirth,
                    Age = model.Age,
                    PhoneNumber = model.PhoneNumber,
                    IsActive = true
                };

                context.Employees.Add(employee);
                var saved = await context.SaveChangesAsync() > 0;

                if (saved)
                    return Result<int>.Success(employee.EmployeeId, "Employee created successfully");
                else
                    return Result<int>.Failure("Failed to save employee to database");
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Error: {ex.Message}");
            }
        }

        // ================= UPDATE =================
        public async Task<Result<bool>> UpdateEmployee(EmployeeViewModel model)
        {
            try
            {
                await using var context = await _factory.CreateDbContextAsync();

                var employee = await context.Employees.FindAsync(model.EmployeeId);
                if (employee == null)
                    return Result<bool>.Failure("Employee not found");

                employee.FullName = model.FullName;
                employee.Department = model.Department;
                employee.DateOfBirth = model.DateOfBirth;
                employee.Age = model.Age;
                employee.PhoneNumber = model.PhoneNumber;

                var saved = await context.SaveChangesAsync() > 0;

                if (saved)
                    return Result<bool>.Success(true, "Employee updated successfully");
                else
                    return Result<bool>.Failure("Failed to update employee");
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error: {ex.Message}");
            }
        }

        // ================= DELETE =================
        public async Task<Result<bool>> DeleteEmployee(int employeeId)
        {
            try
            {
                await using var context = await _factory.CreateDbContextAsync();

                var employee = await context.Employees.FindAsync(employeeId);
                if (employee == null)
                    return Result<bool>.Failure("Employee not found");

                context.Employees.Remove(employee);
                var deleted = await context.SaveChangesAsync() > 0;

                if (deleted)
                    return Result<bool>.Success(true, "Employee deleted successfully");
                else
                    return Result<bool>.Failure("Failed to delete employee");
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error: {ex.Message}");
            }
        }
    }
}
