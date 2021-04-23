using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using log4net;

namespace OPENgovSPORTELLO.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ctxSportelloSQL : DbContext
    {
        public ctxSportelloSQL()
            : base("name=SportelloContext")
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [System.Data.Entity.DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class ctxSportelloMySQL : DbContext
    {
        public ctxSportelloMySQL()
            : base("name=SportelloContext")
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DBModel : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DBModel));
        void IDisposable.Dispose() { }
        public void Dispose() { }
        public DbContext ContextDB { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public DBModel()
        {
            if (RouteConfig.TypeDB== "MySQL")
                ContextDB = new ctxSportelloMySQL();
            else
                ContextDB = new ctxSportelloSQL();
        }
        /// <summary>
        /// 
        /// </summary>
        private static string ToExecSP
        {
            get
            {
                if (RouteConfig.TypeDB == "MySQL")
                    return "CALL ";
                else
                    return "EXEC ";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private static string PrefVarSP
        {
            get
            {
                if (RouteConfig.TypeDB == "MySQL")
                    return "@var";
                else
                    return "@";
            }
        }
        #region "Method"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sSQL"></param>
        /// <param name="myParam"></param>
        /// <returns></returns>
        public string GetSQL(string sSQL, params string[] myParam)
        {
            string sRet = "";
            foreach (string myItem in myParam)
            {
                if (sRet != string.Empty)
                    sRet += ",";
                sRet += PrefVarSP + myItem;
            }

            if (RouteConfig.TypeDB == "MySQL")
                sRet = ToExecSP + sSQL + " (" + sRet + ")";
            else
                sRet = ToExecSP + sSQL + " " + sRet;

            return sRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public object GetParam(string Name, object Value)
        {
            if (RouteConfig.TypeDB == "MySQL")
            {
                MySqlParameter myItem = new MySqlParameter();
                try
                {
                    DateTime d;
                    if (DateTime.TryParse(Value.ToString(), out d))
                    {
                        if (d.ToShortDateString() == DateTime.MaxValue.ToShortDateString())
                            Value = DateTime.MinValue;
                    }
                    myItem = new MySqlParameter(PrefVarSP + Name, Value);
                }
                catch (Exception ex)
                {
                    Log.Debug("DBModel.GetParam.MySqlParameter::errore::", ex);
                    Log.Debug("DBModel.GetParam.MySqlParameter.Name->" + Name + "   Value->" + Value.ToString());
                }
                 return myItem;
            }
            else
            {
                SqlParameter myItem = new SqlParameter();
                try
                {
                    myItem=new SqlParameter(PrefVarSP + Name, Value);
                }
                catch(Exception ex)
                {
                    Log.Debug("DBModel.GetParam.SqlParameter::errore::", ex);
                    Log.Debug("DBModel.GetParam.SqlParameter.Name->" + Name+"   Value->"+Value.ToString());
                }
                return myItem;
            }
        }
        #endregion
    }
}