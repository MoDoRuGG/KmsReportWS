using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KmsReportWS
{
    public class MsConnection : IDisposable
    {
        private string _connectionString;

        private SqlConnection _connect;

        private SqlCommand _command;
        public MsConnection(string connectionString)
        {
            _connectionString = connectionString;
            _connect = new SqlConnection(_connectionString);
            _connect.Open();
            
        }

        public DataTable DataTable()
        {
            var dt = new DataTable();

            try
            {
                if (_connect.State != ConnectionState.Open)
                    _connect.Open();

                var dataReader = _command.ExecuteReader();
                dt.Load(dataReader);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;

        }


        public void NewSp(string spName)
        {
            _command = new SqlCommand(spName);
            _command.Connection = _connect;
            _command.CommandType = CommandType.StoredProcedure;
            
        }

        public void AddSpParam(string paramName, object value)
        {
            if (_command == null)
                throw new Exception("Объект команды был равен null. Перед назначение параметра воспользуйтесь методом NewSp");

            _command.Parameters.AddWithValue(paramName, value);
        }

        public void Dispose()
        {
            if (_connect.State == ConnectionState.Open)
                _connect.Close();

            _connect = null;
        }
    }
}