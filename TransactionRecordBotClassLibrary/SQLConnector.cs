using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionRecordBotClassLibrary.Models;
using Dapper;
using System.Data;
using System.Configuration;

namespace TransactionRecordBotClassLibrary
{
    public static class SQLConnector
    {
        public static string LocalDB = "TransactionsBot";
        public static string AzureDB = "TransactionBotAzure";
        public static int SaveTransaction(WorkflowModel workflow)
        {
            int returnval;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.SQLconnectionstring))
            {
                var p = new DynamicParameters();
                p.Add("@amount", workflow.transaction.amount);
                p.Add("@category", workflow.transaction.category.ToString());
                p.Add("@placename", workflow.transaction.place.Name);
                p.Add("@latitude", workflow.transaction.place.Latitude);
                p.Add("@longitude", workflow.transaction.place.Longitude);
                p.Add("@userid", workflow.UserId);
                p.Add("@returnval",-1, direction: ParameterDirection.ReturnValue);
                connection.Execute("dbo.spSaveTransaction", p, commandType: CommandType.StoredProcedure);
                returnval = p.Get<int>("@returnval");
            }
            return returnval;
        }
        
        public static int SaveUser(UserModel user)
        {
            int returnval;
            using(IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.SQLconnectionstring))
            {
                var p = new DynamicParameters();
                p.Add("@id",user.Id);
                p.Add("@firstname",user.FirstName);
                p.Add("@lastname", user.LastName);
                p.Add("@email", user.Email);
                p.Add("@returnval", 1, direction: ParameterDirection.ReturnValue);
                connection.Execute("dbo.spRegisterUser", p, commandType: CommandType.StoredProcedure);
                returnval = p.Get<int>("@returnval");
            }
            return returnval;
        }

        public static int UpdateEmail(long id, string email)
        {
            int returnval;
            using(IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.SQLconnectionstring))
            {
                var p = new DynamicParameters();
                p.Add("@id", id);
                p.Add("@email", email);
                p.Add("@returnval",1, direction: ParameterDirection.ReturnValue);
                connection.Execute("dbo.spUpdateemail", p, commandType: CommandType.StoredProcedure);
                returnval = p.Get<int>("@returnval");
            }
            return returnval;
        }
        public static bool CheckUserExists(long id)
        {
            bool flag;
            int returnval;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.SQLconnectionstring))
            {
                var p = new DynamicParameters();
                p.Add("@id", id);
                p.Add("@returnval", 1, direction: ParameterDirection.ReturnValue);
                List<dynamic> users = connection.Query("dbo.spGetUser", p, commandType: CommandType.StoredProcedure).ToList();
                returnval = p.Get<int>("@returnval");
                if (users.Count == 0 || returnval != 0)
                {
                    flag = false;
                }
                else
                {
                    flag = true;
                }
            }
            return flag;
        }
        public static List<TransactionModel> GetTransactionsByUserid(long userid)
        {
            List<TransactionModel> transactions = new List<TransactionModel>();
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.SQLconnectionstring))
            {
                var p = new DynamicParameters();
                p.Add("@userid", userid);
                p.Add("@returnval", 1, direction: ParameterDirection.ReturnValue);
                // transactions = connection.Query<TransactionModel>("dbo.spGetTransactionsByUserid",param: p, commandType: CommandType.StoredProcedure).ToList();
                transactions = connection.Query<TransactionModel, PlaceModel, TransactionModel>("dbo.spGetTransactionsByUserid", (tr, pl) =>
                {
                    tr.place = pl;
                    return tr;
                },
                param: p, splitOn: "placename", commandType: CommandType.StoredProcedure).ToList();
            }
            return transactions;
        }
    }
}
