using SimpleCRUD.Features.Employees.Models;

namespace SimpleCRUD.Core.Extensions
{
    public static class EmployeeFilterExtensions
    {
        public static IEnumerable<EmployeeViewModel> SearchBy(
            this IEnumerable<EmployeeViewModel> employees,
            string? searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return employees;

            return employees.Where(e =>
                e.FullName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                e.EmployeeIdView.Contains(searchText, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<EmployeeViewModel> InDepartment(
            this IEnumerable<EmployeeViewModel> employees,
            string? department)
        {
            if (string.IsNullOrWhiteSpace(department))
                return employees;

            return employees.Where(e =>
                string.Equals(e.Department, department, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<EmployeeViewModel> WithAgeRange(
            this IEnumerable<EmployeeViewModel> employees,
            int? minAge,
            int? maxAge)
        {
            var filtered = employees;

            if (minAge.HasValue)
                filtered = filtered.Where(e => e.Age >= minAge.Value);

            if (maxAge.HasValue)
                filtered = filtered.Where(e => e.Age <= maxAge.Value);

            return filtered;
        }
    }
}