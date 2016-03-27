using System;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace Iris.DataLayer
{
    public class SqlServerExecutionStrategy : DbExecutionStrategy
    {
        public SqlServerExecutionStrategy()
        { }

        public SqlServerExecutionStrategy(int maxRetryCount, TimeSpan maxDelay)
            : base(maxRetryCount, maxDelay)
        { }

        protected override bool ShouldRetryOn(Exception ex)
        {
            var sqlException = ex as SqlException;
            if (sqlException == null)
                return false; // don't retry

            foreach (var error in sqlException.Errors.Cast<SqlError>())
            {
                switch (error.Number)
                {
                    case 1205: // Deadlock
                    case -1: // Timeout
                    case -2: // Timeout
                        return true; // retry
                }
            }

            return false;
        }
    }
}
