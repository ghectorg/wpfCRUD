using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;

namespace webCRUD.Database
{
    public partial class Connection
    {
        public SqlConnection _sqlCon;
        public SqlCommand _sqlCmd;
        private SqlTransaction _sqlTra;
        private SqlDataAdapter _sqlDA;
        private DataSet _ds;
        private List<Parameter> _parametersStoredProcedure = new List<Parameter>();
        //public List<ObjectParameter> _parametersStoredProcedureEF = new List<ObjectParameter>();
        private List<Error> _errors = new List<Error>();
        private Utilities.Utilities _utilities = new Utilities.Utilities();
        private bool _closeConnection = true;


        public Connection()
        {
        }

        public Connection(SqlConnection sqlCon, SqlCommand sqlCmd, bool closeConnection)
        {
            _sqlCon = sqlCon;
            _sqlCmd = sqlCmd;
            _closeConnection = closeConnection;
        }

        public string ConnectionString(string connectionString)
        {
            return ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
        }

        public string ConnectionStringCON_DBCRUD()
        {
            return ConfigurationManager.ConnectionStrings[@"CON_DBCRUD"].ConnectionString;
        }
        

        public void ConnectToDataBase(string connectionString)
        {
            _sqlCon = new SqlConnection();
            if (_sqlCon.State != ConnectionState.Open)
            {
                _sqlCon = new SqlConnection(connectionString);
                _sqlCon.Open();
            }
        }

        public void ConnectToDataBaseCON_DBCRUD()
        {
            ConnectToDataBase(ConnectionStringCON_DBCRUD());
        }

        public List<Parameter> AddParameters(string name, string value, SqlDbType dataType)
        {
            _parametersStoredProcedure.Add(new Parameter { name = name, value = value, dataType = dataType });
            return _parametersStoredProcedure;
        }

        public List<Parameter> AddParameters(string name, string value, SqlDbType dataType, int size)
        {
            _parametersStoredProcedure.Add(new Parameter { name = name, value = value, dataType = dataType, size = size });
            return _parametersStoredProcedure;
        }

        public List<Parameter> AddParameters(string name, string value, SqlDbType dataType, ParameterDirection direction)
        {
            _parametersStoredProcedure.Add(new Parameter { name = name, value = value, dataType = dataType, direction = direction });
            return _parametersStoredProcedure;
        }

        public List<Parameter> AddParameters(string name, string value, SqlDbType dataType, int size, ParameterDirection direction)
        {
            _parametersStoredProcedure.Add(new Parameter { name = name, value = value, dataType = dataType, size = size, direction = direction });
            return _parametersStoredProcedure;
        }

        //public List<ObjectParameter> AddParametersEF(string name, object value, Type dataType)
        //{
        //	if (value == null)
        //		_parametersStoredProcedureEF.Add(new ObjectParameter(name, dataType));
        //	else
        //		_parametersStoredProcedureEF.Add(new ObjectParameter(name, value));

        //	return _parametersStoredProcedureEF;
        //}

        public DataSet ExecuteQueryDS(string sentenceSQL, bool isStoredProcedure, string connectionString)
        {
            try
            {
                if (_sqlCon == null || _sqlCon.State != ConnectionState.Open)
                    ConnectToDataBase(connectionString);

                if (_sqlCmd == null)
                    _sqlCmd = new SqlCommand();

                _sqlCmd.Connection = _sqlCon;
                _sqlCmd.CommandText = sentenceSQL;

                if (isStoredProcedure)
                    _sqlCmd.CommandType = CommandType.StoredProcedure;
                else
                    _sqlCmd.CommandType = CommandType.Text;

                #region parameters
                for (int a = 0; a < _parametersStoredProcedure.Count; a++)
                {
                    SqlParameter parameter = new SqlParameter();
                    parameter.ParameterName = _parametersStoredProcedure[a].name;
                    parameter.SqlDbType = _parametersStoredProcedure[a].dataType;
                    parameter.Direction = _parametersStoredProcedure[a].direction;
                    if (_parametersStoredProcedure[a].size != null)
                        parameter.Size = (int)_parametersStoredProcedure[a].size;

                    if (string.IsNullOrEmpty(_parametersStoredProcedure[a].value) || _parametersStoredProcedure[a].value.Trim().Length == 0)
                        parameter.Value = DBNull.Value;
                    else
                    {
                        switch (parameter.SqlDbType)
                        {
                            case SqlDbType.VarChar:
                                parameter.Value = Convert.ToString(_parametersStoredProcedure[a].value);
                                break;
                            case SqlDbType.Char:
                                parameter.Value = Convert.ToString(_parametersStoredProcedure[a].value);
                                break;
                            case SqlDbType.Text:
                                parameter.Value = Convert.ToString(_parametersStoredProcedure[a].value);
                                break;
                            case SqlDbType.Bit:
                                parameter.Value = Convert.ToBoolean(_parametersStoredProcedure[a].value);
                                break;
                            case SqlDbType.TinyInt:
                                parameter.Value = Convert.ToByte(_parametersStoredProcedure[a].value);
                                break;
                            case SqlDbType.SmallInt:
                                parameter.Value = Convert.ToInt16(_parametersStoredProcedure[a].value);
                                break;
                            case SqlDbType.Int:
                                parameter.Value = Convert.ToInt32(_parametersStoredProcedure[a].value);
                                break;
                            case SqlDbType.BigInt:
                                parameter.Value = Convert.ToInt64(_parametersStoredProcedure[a].value);
                                break;
                            case SqlDbType.Decimal:
                                parameter.Value = Convert.ToDecimal(_parametersStoredProcedure[a].value);
                                break;
                            case SqlDbType.DateTime:
                                parameter.Value = Convert.ToDateTime(_parametersStoredProcedure[a].value);
                                break;
                        }
                    }

                    _sqlCmd.Parameters.Add(parameter);
                }
                #endregion

                _sqlDA = new SqlDataAdapter(_sqlCmd);
                _ds = new DataSet();
                _sqlDA.Fill(_ds);

                _ds = TableName_Set(_ds);

                if (_sqlCmd.Parameters.Contains(@"@messageError") && _sqlCmd.Parameters[@"@messageError"] != null && _sqlCmd.Parameters[@"@messageError"].Value != DBNull.Value && _sqlCmd.Parameters[@"@messageError"].Value != null && _sqlCmd.Parameters[@"@messageError"].Value.ToString().Trim().Length > 0)
                {
                    Error error = new Error();
                    error.number = _sqlCmd.Parameters.Contains(@"@numberError") && _sqlCmd.Parameters[@"@numberError"] != null && _sqlCmd.Parameters[@"@numberError"].Value != DBNull.Value && _sqlCmd.Parameters[@"@numberError"].Value != null && _sqlCmd.Parameters[@"@numberError"].Value.ToString().Trim().Length > 0 ? (int)_sqlCmd.Parameters[@"@numberError"].Value : 0;
                    error.message = (string)_sqlCmd.Parameters[@"@messageError"].Value;
                    error.source = _sqlCmd.Parameters.Contains(@"@procedureError") && _sqlCmd.Parameters[@"@procedureError"] != null && _sqlCmd.Parameters[@"@procedureError"].Value != DBNull.Value && _sqlCmd.Parameters[@"@procedureError"].Value != null && _sqlCmd.Parameters[@"@procedureError"].Value.ToString().Trim().Length > 0 ? (string)_sqlCmd.Parameters[@"@procedureError"].Value : null;
                    _errors.Add(error);
                }
            }
            catch (SqlException sqlEx)
            {
                for (int a = 0; a < sqlEx.Errors.Count; a++)
                {
                    Error error = new Error();
                    error.number = sqlEx.Number;
                    error.message = sqlEx.Message;
                    error.source = sqlEx.Procedure;
                    _errors.Add(error);
                }
            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(true);
                StackTrace stError = new StackTrace(ex, true);
                Error error = new Error();
                error.number = ex.HResult;
                error.message = ex.Message;
                error.source = _utilities.GenerateSourceTrace(st, stError, this.GetType());
                _errors.Add(error);
            }
            finally
            {
                if (_errors != null && _errors.Count > 0)
                {
                    DataTable dt = _utilities.ConvertListToDataTable(_errors);
                    dt.TableName = @"Error";
                    if (_ds == null)
                        _ds = new DataSet();
                    _ds.Tables.Add(dt);
                }

                if (_closeConnection)
                    _sqlCon.Close();
            }

            return _ds;
        }

        private DataSet TableName_Set(DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0)
            {
                for (int a = 0; a < ds.Tables.Count; a++)
                {
                    if (ds.Tables[a].Columns.Contains(@"tableNameDB") && ds.Tables[a].Rows.Count > 0
                        && !string.IsNullOrEmpty(ds.Tables[a].Rows[0][@"tableNameDB"].ToString())
                        && ds.Tables[a].Rows[0][@"tableNameDB"].ToString().Trim().Length > 0)
                    {
                        ds.Tables[a].TableName = ds.Tables[a].Rows[0][@"tableNameDB"].ToString().Trim();
                        ds.Tables[a].Columns.Remove(@"tableNameDB");
                    }
                }
            }

            return ds;
        }
    }
}
