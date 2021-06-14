using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Financing.Transactions;

namespace Financing
{
    class SQLFunctions
    {
        // Refrence Connection.
        private static SqlConnection sqlcon = new SqlConnection(
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        private static DataTable Select(string Columns, string Table)
        {
            // Select query string.
            string query = "Select " + Columns + " from [Financing].[dbo]." + Table;

            // Begin sql query.
            try
            {
                sqlcon.Open();

                // Bind string to sql command.
                SqlCommand cmd = sqlcon.CreateCommand();
                cmd.CommandText = query;

                // Gets returned data and inserts into datatable.
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable(Table);
                sda.Fill(dt);

                sqlcon.Close();
                return dt;
            }
            catch(SqlException e)
            {
                // Outputs error and returns nothing.
                MessageBox.Show("Failed to connect to the database! " + e);
                DataTable failed = new DataTable();

                sqlcon.Close();
                return failed;
            }
        }

        public static void Update(string Columns, string Table, string Args)
        {
            // Update string format.
            string query = "UPDATE [" + Table + "] SET " + Columns + " WHERE " + Args;

            sqlcon.Open();

            // Bind string to sql command.
            SqlCommand cmd = sqlcon.CreateCommand();
            cmd.CommandText = query;

            // Execute sql command and return nothing.
            cmd.ExecuteNonQuery();
            sqlcon.Close();
        }

        public static void RemoveData(string Table, string Args)
        {
            // Update string format.
            string query = "DELETE FROM [" + Table + "] WHERE " + Args;

            sqlcon.Open();

            // Bind string to sql command.
            SqlCommand cmd = sqlcon.CreateCommand();
            cmd.CommandText = query;

            // Execute sql command and return nothing.
            cmd.ExecuteNonQuery();

            sqlcon.Close();
        }

        public static void updateTransaction(Transaction original, Transaction changed)
        {
            string change = "[Value] = " + returnDecimalAmount(changed.Amount) + ", [Type] = '" + changed.Type + "', [Category] = '" + changed.Category + "', [Date] = '" + changed.Date + "', [Description] = '" + changed.Description + "', [Merchant] = '" + changed.Merchant + "'";
            string args = "[AccID] = " + original.AccountID + " AND [Value] = " + returnDecimalAmount(original.Amount) + " AND [Type] = '" + original.Type + "' AND [Category] = '" + original.Category + "' AND [Date] = '" + original.Date + "' AND [Description] = '" + original.Description + "' AND [Merchant] = '" + original.Merchant + "'";

            Update(change,"Transactions",args);
        }

        public static void removeTransaction(Transaction Remove)
        {
            string toDelete = "[AccID] = " + Remove.AccountID + " AND [Value] = " + returnDecimalAmount(Remove.Amount) + " AND [Type] = '" + Remove.Type + "' AND [Category] = '" + Remove.Category + "' AND [Date] = '" + Remove.Date + "' AND [Description] = '" + Remove.Description + "' AND [Merchant] = '" + Remove.Merchant + "'";
            RemoveData("Transactions",toDelete);
        }

        private static string returnDecimalAmount(Decimal decNum)
        {
            return decNum.ToString().Replace(',', '.');
        }

        public static void CreateProfile(string Username, string Password)
        {
            // Uses Parameters to avoid sql injection attacks.
            string query = "INSERT INTO [Profiles] ([Username], [Pass]) " +
                "VALUES (@Username, @Pass)";

            // Initialize command.
            SqlCommand cmd = new SqlCommand(query, sqlcon);

            // Bind parameters.
            cmd.Parameters.AddWithValue("@Username", Username);
            cmd.Parameters.AddWithValue("@Pass", Password);

            // Execute command.
            sqlcon.Open();
            cmd.ExecuteNonQuery();
            sqlcon.Close();
        }

        public static void CreateBankAccount(int userID, string accountName, string accountType)
        {
            // Uses Parameters to avoid sql injection attacks.
            string query = "INSERT INTO [Accounts] ([UserID], [AccName], [AccType], [Balance]) " +
                "VALUES (@UserID, @AccName, @AccType, @Balance)";

            // Initialize command.
            SqlCommand cmd = new SqlCommand(query, sqlcon);

            // Bind parameters.
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@AccName", accountName);
            cmd.Parameters.AddWithValue("@AccType", accountType);
            cmd.Parameters.AddWithValue("@Balance", "100");

            // Execute command.
            sqlcon.Open();
            cmd.ExecuteNonQuery();
            sqlcon.Close();
        }

        // Tests login credentials by selecting the user in the SQL table.
        public static int ValidateLogin(string Username, string Password)
        {
            DataTable dt = Select(
                "[ID]", "[Profiles] WHERE [Username]='" 
                + Username + "'" + "AND [Pass]='" + Password + "'");

            // Return true if the UserID is returned. Otherwise returns false.
           try
            {
                return (int)dt.Rows[0][0];
            }
            catch(IndexOutOfRangeException)
            {
                return -1;
            }
        }

        // Queries database for all accounts belonging to specified user.
        public static DataTable GetAccounts(int UserID)
        {
            DataTable dt = Select(
                "[ID], [AccName], [AccType], [Balance]",
                "[Accounts] WHERE [UserID]= " + UserID);

            return dt;
        }

        // Queries database for all transactions belonging to specified account.
        public static DataTable GetTransactions(int AccID)
        {
            DataTable dt = Select(
                "[ID], [AccID], [Merchant], [Value], [Type], [Category], [Date], [Description]", 
                "[Transactions] WHERE [AccID]= " + AccID + " ORDER BY [Date] DESC");

            return dt;
        }

        public static DataTable GetCategories()
        {
            DataTable dt = Select(
                "[CategoryName]",
                "[Categories]");

            return dt;
        }

        public static void AddTransaction(Transaction Parameters)
        {
            // Uses Parameters to avoid sql injection attacks.
            string query = "INSERT INTO [Transactions] ([AccID], [Merchant], [Value], [Type], [Category], [Date], [Description]) " +
                "VALUES (@AccID, @Merchant, @Value, @Type, @Category, @Date, @Description)";

            // Initialize command.
            SqlCommand cmd = new SqlCommand(query, sqlcon);

            // Bind parameters.
            cmd.Parameters.AddWithValue("@AccID", Parameters.AccountID);
            cmd.Parameters.AddWithValue("@Merchant", Parameters.Merchant);
            cmd.Parameters.AddWithValue("@Value", Parameters.Amount);
            cmd.Parameters.AddWithValue("@Type", Parameters.Type);
            cmd.Parameters.AddWithValue("@Category", Parameters.Category);
            cmd.Parameters.AddWithValue("@Date", Parameters.Date);
            cmd.Parameters.AddWithValue("@Description", Parameters.Description);

            // Execute command.
            sqlcon.Open();
            cmd.ExecuteNonQuery();
            sqlcon.Close();
        }
    }
}