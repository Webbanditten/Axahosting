using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AxaHosting.Data.Infrastructure;
using AxaHosting.Model;
using MySql.Data.MySqlClient;

namespace AxaHosting.Data.Repositories
{
    public interface IDatabaseRepository : IRepository<Database>
    {
        // Here we define all the custom actions a repository might have. For example filtering for properties.
        IEnumerable<Database> GetDatabasesWithOwner(string owner);
        void CreateMySqlDatabase(Database database);
        void CreateMsSqlDatabase(Database database);
        void DeleteMySqlDatabase(Database database);
        void DeleteMsSqlDatabase(Database database);
        void EditPasswordForMySqlAccount(Database database);
        void EditPasswordForMsSqlAccount(Database database);
    }

    public class DatabaseRepository : RepositoryBase<Database>, IDatabaseRepository
    {
        public DatabaseRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

      
        public IEnumerable<Database> GetDatabasesWithOwner(string owner)
        {
            return this.DbContext.Databases.Where(c => c.Owner == owner);
        }

        public void CreateMySqlDatabase(Database database)
        {
            try
            {
                var connectionString = "Data Source="+database.Server.InternalIp+ ";port=3306;User Id=" + database.Server.Username + ";password=" + database.Server.Password;
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var sqlCommand = "create database `" + database.Name + "`; CREATE USER `" + database.Username + "`@`%` IDENTIFIED BY '" + database.Password + "'; GRANT ALL ON " + database.Name + ".* TO `" + database.Username + @"`@'%'; FLUSH PRIVILEGES;";
                    var cmd = new MySqlCommand(sqlCommand, connection);

                    cmd.ExecuteNonQuery();
                    
                    connection.Close();
                }

            }
            catch
            {
                // ignored
            }
        }

        public void CreateMsSqlDatabase(Database database)
        {
            try
            {
                var connectionString = "Data Source="+database.Server.InternalIp+";User Id=" + database.Server.Username + ";password="+ database.Server.Password;
                using (var connection = new SqlConnection(connectionString))
                {

                    connection.Open();
                    var sqlCommand = "CREATE DATABASE " + database.Name + ";CREATE LOGIN " + database.Username + " WITH PASSWORD = '" + database.Password + "', CHECK_POLICY = OFF;";
                    var cmd = new SqlCommand(sqlCommand, connection);

                    cmd.ExecuteNonQuery();

                    var sqlCommand2 = "Use " + database.Name + "; CREATE USER " + database.Username + " FOR LOGIN " + database.Username + "; EXEC sp_addrolemember 'db_owner', '" + database.Username + "'";
                    var cmd2 = new SqlCommand(sqlCommand2, connection);

                    cmd2.ExecuteNonQuery();

                    connection.Close();
                }

            }
            catch
            {
                // ignored
            }
        }

        public void DeleteMySqlDatabase(Database database)
        {

            try
            {
                var connectionString = "Data Source=" + database.Server.InternalIp + ";port=3306;User Id=" + database.Server.Username + ";password=" + database.Server.Password;
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var sqlCommand = "DROP DATABASE `"+database.Name+ "`; DROP USER `"+database.Username+"`";
                    var cmd = new MySqlCommand(sqlCommand, connection);

                    cmd.ExecuteNonQuery();

                    connection.Close();
                }

            }
            catch
            {
                // ignored
            }
        }

        public void DeleteMsSqlDatabase(Database database)
        {
            try
            {
                var connectionString = "Data Source=" + database.Server.InternalIp + ";User Id=" + database.Server.Username + ";password=" + database.Server.Password;
                using (var connection = new SqlConnection(connectionString))
                {

                    connection.Open();
                    var sqlCommand = "DROP DATABASE " + database.Name + ";DROP LOGIN " + database.Username+";";
                    var cmd = new SqlCommand(sqlCommand, connection);

                    cmd.ExecuteNonQuery();

                    connection.Close();
                }

            }
            catch
            {
                // ignored
            }
        }

        public void EditPasswordForMySqlAccount(Database database)
        {
            try
            {
                var connectionString = "Data Source=" + database.Server.InternalIp + ";port=3306;User Id=" + database.Server.Username + ";password=" + database.Server.Password;
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var sqlCommand = "SET PASSWORD for `" + database.Username + "`@'%' = '" + database.Password + "'";
                    var cmd = new MySqlCommand(sqlCommand, connection);

                    cmd.ExecuteNonQuery();

                    connection.Close();
                }

            }
            catch
            {
                // ignored
            }
        }

        public void EditPasswordForMsSqlAccount(Database database)
        {
            try
            {
                var connectionString = "Data Source=" + database.Server.InternalIp + ";User Id=" + database.Server.Username + ";password=" + database.Server.Password;
                using (var connection = new SqlConnection(connectionString))
                {

                    connection.Open();
                    var sqlCommand = " ALTER LOGIN "+database.Username+" WITH PASSWORD = '"+database.Password+"', CHECK_POLICY = OFF;";
                    var cmd = new SqlCommand(sqlCommand, connection);

                    cmd.ExecuteNonQuery();

                    connection.Close();
                }

            }
            catch
            {
                // ignored
            }
           
        }

        public override void Update(Database entity)
        {
            entity.LastUpdated = DateTime.Now;
            base.Update(entity);
        }
    }

   
}
