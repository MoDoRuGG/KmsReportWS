using System;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Properties;
using NLog;

namespace KmsReportWS.Service
{
    public class FilialService
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly string ConnStr = Settings.Default.ConnStr;

        public void SaveUser(string filialCode, string fio, string email, string phone, bool isAddUser, int idUser)
        {
            try
            {
                using var db = new LinqToSqlKmsReportDataContext(ConnStr);
                var employee = CreateFioEmployee(fio);

                if (isAddUser)
                {
                    var newEmployee = new Employee {
                        Surname = employee.Surname,
                        Name = employee.Name,
                        MiddleName = employee.MiddleName,
                        Login = employee.Login,
                        Email = email,
                        Phone = phone,
                        Password = phone,
                        IsActive = true,
                        Region = filialCode
                    };
                    Log.Debug($"Save new user = {newEmployee.Surname} {newEmployee.Name} {newEmployee.MiddleName}");
                    db.Employee.InsertOnSubmit(newEmployee);
                    db.SubmitChanges();
                }
                else
                {
                    var editEmployee = db.Employee.SingleOrDefault(e => e.Id == idUser);
                    if (editEmployee == null)
                    {
                        Log.Error($"Error saving user with id = {idUser} because user not found");
                        return;
                    }

                    editEmployee.Surname = employee.Surname;
                    editEmployee.Name = employee.Name;
                    editEmployee.MiddleName = employee.MiddleName;
                    editEmployee.Login = employee.Login;
                    editEmployee.Email = email;
                    editEmployee.Phone = phone;
                    editEmployee.Password = phone;
                    editEmployee.Region = filialCode;

                    Log.Debug($"Edit user = {editEmployee}");
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error saving user");
                throw;
            }
        }
        
        public void EditFilial(string filialCode, string filialName, string fio, string position, string phone)
        {
            try
            {
                using var db = new LinqToSqlKmsReportDataContext(Settings.Default.ConnStr);
                var filial = db.Region.Single(r => r.id == filialCode);
                filial.name_devision = filialName;
                filial.position = position;
                filial.name_head = fio;
                filial.phone = phone;

                db.SubmitChanges();
            }
            catch (Exception e)
            {
                Log.Error(e, "Error saving information about filial");
                throw;
            }
        }

        private Employee CreateFioEmployee(string fio)
        {
            var parts = fio.Split(' ');
            var surname = parts[0];
            var name = parts[1];
            var middlename = "";
            
            if (parts.Length > 2)
            {
                var middleNameList = parts.ToList().GetRange(2, parts.Length - 2);
                middlename = string.Join(" ", middleNameList);
            }

            var login = surname + name[0];
            login += middlename.Length > 0 ? middlename[0].ToString() : "";
            
            return new Employee {
                Surname = surname, Name = name, MiddleName = middlename, Login = login,
            };
        }
    }
}