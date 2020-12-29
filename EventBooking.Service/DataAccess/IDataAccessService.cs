using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace EventBooking.Service
{
    public interface IDataAccessService
    {

        /// <summary>
        /// User data of authenticated Person in current scope. Has personId and role property
        /// </summary>
        //MvUserContext CurrentUser { get; set; }

        /// <summary>
        /// Unique Id of authenticated User in current scope
        /// </summary>
        int CurrentPersonId { get; set; }

        /// <summary>
        /// Role of authenticated User in current scope
        /// </summary>
        string Role { get; set; }

        /// <summary>
        /// User Data of authenticated User in current scope
        /// </summary>
        string UserContext { get; set; }

        /// <summary>
        /// Sets User context in database and returns Database Connection 
        /// </summary>
        Task<IDbConnection> GetConnection();
        //IDbConnection Connection { get; }

        /// <summary>
        /// Call insert and update procedures with dynamic parameters. Does not return output
        /// </summary>
        /// <param name="storedProcedure">Name of stored procedure</param>
        /// <param name="dynamicParameters">SP parameters of c# type DynamicParameters</param>
        Task ActionProcedure(string storedProcedure, DynamicParameters dynamicParameters);

        /// <summary>
        /// Call insert and update procedures with dynamic parameters. Does not return output
        /// </summary>
        /// <param name="storedProcedure">Name of stored procedure</param>
        /// <param name="dynamicParameters">SP parameters of c# type DynamicParameters</param>
        /// <returns>Executed dynamic parameters</returns>
        //Task<DynamicParameters> ActionProcedure(string storedProcedure, DynamicParameters dynamicParameters);

        /// <summary>
        /// Call insert and update procedures. Doesnot return output
        /// </summary>
        /// <param name="storedProcedure">Name of stored procedure</param>
        /// <param name="parameters">Sp Parameters in an object</param>
        Task ActionProcedure(string storedProcedure, object parameters);

        /// <summary>
        /// Call insert and update procedures with dynamic parameters.
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="storedProcedure">Name of stored procedure</param>
        /// <param name="dynamicParameters">SP parameters of c# type DynamicParameters</param>
        /// <returns>Returns output of specified type</returns>
        //Task<T> ActionProcedure<T>(string storedProcedure, DynamicParameters dynamicParameters);

        /// <summary>
        /// Call insert and update procedures.
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="storedProcedure">Name of stored procedure</param>
        /// <param name="parameters">Sp Parameters in an object</param>
        /// <returns>Returns output of specified type</returns>
        Task<T> ActionProcedure<T>(string storedProcedure, object parameters);

        /// <summary>
        /// Call data select procedures
        /// </summary>
        /// <typeparam name="T">Return type for IEnumerable</typeparam>
        /// <param name="storedProcedure">Name of stored procedure</param>
        /// <param name="dynamicParameters">SP parameters of c# type DynamicParameters</param>
        /// <returns>Returns IEnumerable output of specified type</returns>
        Task<IEnumerable<T>> RetrievalProcedure<T>(string storedProcedure, DynamicParameters dynamicParameters);

        /// <summary>
        /// Call data select procedures
        /// </summary>
        /// <typeparam name="T">Return type for IEnumerable</typeparam>
        /// <param name="storedProcedure">Name of stored procedure</param>
        /// <param name="parameters">Sp Parameters in an object</param>
        /// <returns>Returns IEnumerable output of specified type</returns>
        Task<IEnumerable<T>> RetrievalProcedure<T>(string storedProcedure, object parameters);

        /// <summary>
        /// Call data select procedures
        /// </summary>
        /// <typeparam name="T">Return type for IEnumerable</typeparam>
        /// <param name="storedProcedure">Name of stored procedure</param>
        /// <param name="parameters">Sp Parameters in an object: will be stringified</param>
        /// <returns>Returns IEnumerable output of specified type</returns>
        Task<IEnumerable<T>> RetrievalProcedureWithJsonInput<T>(string storedProcedure, object parameters);

        /// <summary>
        /// Call data select procedures
        /// </summary>
        /// <typeparam name="T">Return type for object</typeparam>
        /// <param name="storedProcedure">Name of stored procedure</param>
        /// <param name="dynamicParameters">SP parameters of c# type DynamicParameters</param>
        /// <returns>Returns single output of specified type</returns>
        Task<T> RetrievalProcedureSingle<T>(string storedProcedure, DynamicParameters dynamicParameters);

        /// <summary>
        /// Call data select procedures
        /// </summary>
        /// <typeparam name="T">Return type for IEnumerable</typeparam>
        /// <param name="storedProcedure">Name of stored procedure</param>
        /// <param name="parameters">Sp Parameters in an object</param>
        /// <returns>Returns single output of specified type</returns>
        Task<T> RetrievalProcedureSingle<T>(string storedProcedure, object parameters);

        /// <summary>
        /// Call data select procedures
        /// </summary>
        /// <typeparam name="T">Return type for IEnumerable</typeparam>
        /// <param name="storedProcedure">Name of stored procedure</param>
        /// <param name="parameters">Sp Parameters in an object : will be stringified</param>
        /// <returns>Returns single output of specified type</returns>
        Task<T> RetrievalProcedureSingleWithJsonInput<T>(string storedProcedure, object parameters);

        /// <summary>
        /// Call data select procedures
        /// </summary>
        /// <typeparam name="T">Return type for Scalar values like int, string, bool, etc</typeparam>
        /// <param name="storedProcedure">Name of stored procedure to execute for scalar</param>
        /// <param name="parameters">Sp Parameters in an object</param>
        /// <returns>Returns scalar output of specified type</returns>
        Task<T> RetrievalProcedureScalar<T>(string storedProcedure, object parameters);
    }
}