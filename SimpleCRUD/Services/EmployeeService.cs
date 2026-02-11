using SimpleCRUD.ViewModels;
using SimpleCRUD.DataAccess;
using Microsoft.EntityFrameworkCore;
using SimpleCRUD.DataAccess.Entities;

namespace SimpleCRUD.Services
{
    public class EmployeeService
    {
        private readonly IDbContextFactory<AppDBContext> _factory;

        public EmployeeService(IDbContextFactory<AppDBContext> factory)
        {
            _factory = factory;
        }

        // ================= GET ALL =================
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

        // ================= FIND =================
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

        // ================= TOGGLE STATUS =================
        public async Task ToggleStatus(int id)
        {
            await using var context = await _factory.CreateDbContextAsync();

            var employee = await context.Employees.FindAsync(id);
            if (employee == null) return;

            employee.IsActive = !employee.IsActive;
            await context.SaveChangesAsync();
        }

        // ================= CREATE =================
        public async Task<bool> CreateNewEmployee(EmployeeViewModel model)
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
            return await context.SaveChangesAsync() > 0;
        }

        // ================= UPDATE =================
        public async Task<bool> UpdateEmployee(EmployeeViewModel model)
        {
            await using var context = await _factory.CreateDbContextAsync();

            var employee = await context.Employees.FindAsync(model.EmployeeId);
            if (employee == null) return false;

            employee.FullName = model.FullName;
            employee.Department = model.Department;
            employee.DateOfBirth = model.DateOfBirth;
            employee.Age = model.Age;
            employee.PhoneNumber = model.PhoneNumber;

            return await context.SaveChangesAsync() > 0;
        }

        // ================= DELETE =================
        public async Task<bool> DeleteEmployee(int employeeId)
        {
            await using var context = await _factory.CreateDbContextAsync();

            var employee = await context.Employees.FindAsync(employeeId);
            if (employee == null) return false;

            context.Employees.Remove(employee);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
