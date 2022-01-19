namespace Domain.Common
{
    public static class ResponseMessages
    {
        public const string DuplicateEmail = "Email already exist";
        public const string RetrievalSuccessResponse = "Data retrieved successfully";
        public const string CreationSuccessResponse = "Data created successfully";
        public const string LoginSuccessResponse = "Login successful"; 
        public const string WrongEmailOrPassword = "Wrong email or password provided";
        public const string UserNotFound = "User not found";
        public const string InvalidToken = "Invalid token";
        public const string MissingClaim = "MissingClaim:";
        public const string PasswordCannotBeEmpty = "Password cannot be empty";
        public const string staffIdExist = "StaffId already exist";
        public const string AgeLimitError = "Age must not be less than 16";
        public const string EmployeeNotFound = "Employee not found";
        public const string DeleteEmployeeIdResponse = " has been deleted from the records";
        public const string UpdateEmployeeByIdResponse = " has been updated in the records";
        public const string DepartmentExist = "Department already exist";
        public const string DepartmentNotExist = "Department does not exist";
        public const string DeleteDepartmentByIdResponse = " has been deleted from the records";
        public const string DepartmentCreationSuccessResponse = " Department created successfully";
    }
}