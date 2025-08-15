using Npgsql;


namespace MigracaoPostgreSQL.Utils
{
    public static class ClsNpgsqlExtensions
    {
        public static T Get<T>(this NpgsqlDataReader dr, string name, T defaultValue = default(T))
            => dr.IsNull(name) ? defaultValue : (T)dr.GetValue(dr.GetOrdinal(name));

        public static bool IsNull(this NpgsqlDataReader dr, string name)
            => dr.IsDBNull(dr.GetOrdinal(name));
    }
}
