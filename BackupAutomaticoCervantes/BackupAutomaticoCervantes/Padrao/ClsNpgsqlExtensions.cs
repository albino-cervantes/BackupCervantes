using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BackupAutomaticoCervantes.Padrao
{
    public static class ClsNpgsqlExtensions
    {
        public static NpgsqlParameter AddWithNullableValue(this NpgsqlParameterCollection collection, string parameterName, object value)
        {
            if (value == null)
                return collection.AddWithValue(parameterName, DBNull.Value);
            else
                return collection.AddWithValue(parameterName, value);
        }

        /// <summary>
        /// Robson: Escreve os SQLs no console. Utilizar para ver o SQL completo do NpgsqlCommand, sem precisar executar no banco
        /// </summary>
        /// <param name="pCommand"></param>
        private static void PrintSQL(this NpgsqlCommand cmm)
        {
            Console.WriteLine(cmm.GetSqlWithParameters());
        }

        /// <summary>
        /// Robson: Obtém o SQL completo do NpgsqlCommand, sem precisar executar no banco
        /// </summary>
        /// <param name="pCommand"></param>
        public static string GetSqlWithParameters(this NpgsqlCommand cmm)
        {
            string sqlWithParameters = cmm.CommandText;

            // Substitui cada parâmetro na consulta pelos seus valores
            foreach (var parameter in cmm.Parameters.GetInternalList())
            {
                string formattedValue;

                if (parameter.Value is string)
                    formattedValue = $"'{parameter.Value}'";
                else if (parameter.Value == null)
                    formattedValue = "NULL";
                else
                    formattedValue = parameter.Value.ToString();

                sqlWithParameters = sqlWithParameters.Replace(parameter.ParameterName, formattedValue);
            }

            return sqlWithParameters;
        }

        /// <summary>
        /// Robson: Obtém a lista de parametros interna do NpgsqlCommand utilizando reflection
        /// </summary>
        /// <param name="pCommand"></param>
        public static List<NpgsqlParameter> GetInternalList(this NpgsqlParameterCollection collection)
        {
            FieldInfo internalListField = typeof(NpgsqlParameterCollection).GetField("InternalList", BindingFlags.Instance | BindingFlags.NonPublic);

            // Verifica se o campo foi encontrado via reflection
            if (internalListField != null)
            {
                // Obtenha o valor do campo para o objeto atual
                var internalList = (List<NpgsqlParameter>)internalListField.GetValue(collection);

                return internalList;
            }
            else
            {
                throw new InvalidOperationException("O campo 'InternalList' não foi encontrado em NpgsqlParameterCollection.");
            }
        }

        public static T Get<T>(this NpgsqlDataReader dr, string name, T defaultValue = default(T))
            => dr.IsNull(name) ? defaultValue : (T)dr.GetValue(dr.GetOrdinal(name));

        public static bool IsNull(this NpgsqlDataReader dr, string name)
            => dr.IsDBNull(dr.GetOrdinal(name));

        /// <summary>
        /// Robson
        /// </summary>
        /// <param name="cmm">se a conexão do command já estiver aberta, deverá ser tratada por fora</param>
        /// <param name="action"></param>
        public static void ExecutaLeitura(this NpgsqlCommand cmm, Action<NpgsqlDataReader> action)
        {
            if (cmm.Connection == null)
                throw new Exception("A conexão do command (cmm) deve ser preenchida.");

            bool controlaConexao = cmm.Connection.State == System.Data.ConnectionState.Closed;

            NpgsqlDataReader dr = null;

            try
            {
                if (controlaConexao)
                    cmm.Connection.Open();

                dr = cmm.ExecuteReader();

                while (dr.Read())
                    action.Invoke(dr);
            }
            finally
            {
                if (dr != null && !dr.IsClosed)
                    dr.Close();

                if (controlaConexao && cmm.Connection.State != System.Data.ConnectionState.Closed)
                    cmm.Connection.Close();
            }
        }


        public static NpgsqlConnection OpenAndReturn(this NpgsqlConnection conn)
        {
            conn.Open();

            return conn;
        }
    }
}
