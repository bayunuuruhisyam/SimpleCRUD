namespace SimpleCRUD.Core.Constants
{
    public static class AppConstants
    {
        public static class Messages
        {
            // Success Messages
            public const string EmployeeAddedSuccess = "Employee added successfully!";
            public const string EmployeeUpdatedSuccess = "Employee updated successfully!";
            public const string EmployeeDeletedSuccess = "Employee deleted successfully!";

            // Error Messages
            public const string EmployeeNotFound = "Employee not found!";
            public const string ErrorSaving = "Error saving employee.";
            public const string ErrorDeleting = "Failed deleting employee.";
        }

        public static class Pagination
        {
            public const int DefaultPageSize = 10;
            public static readonly int[] PageSizeOptions = new int[] { 5, 10, 20 };
        }
    }
}