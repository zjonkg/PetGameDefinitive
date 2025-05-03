using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;

namespace ShyLaura.Database
{
    public class SqliteHelper : IDisposable
    {
        private const string Tag = "Shylaura:\t";
        private const string DatabaseName = "shylaura";

        public string db_connection_string;
        public IDbConnection db_connection;

        public SqliteHelper()
        {
            db_connection_string = "URI=file:" + Application.persistentDataPath + "/" + DatabaseName;
            Debug.Log("db_connection_string: " + db_connection_string);
            db_connection = new SqliteConnection(db_connection_string);
            db_connection.Open();
        }

        // Método explícito para cerrar conexión correctamente
        public void Dispose()
        {
            if (db_connection != null)
            {
                db_connection.Close();
                db_connection = null;
                Debug.Log(Tag + "Conexión cerrada correctamente.");
            }
        }

        // Métodos virtuales
        public virtual IDataReader getDataById(int id)
        {
            Debug.LogWarning(Tag + "getDataById no está implementado.");
            throw new NotImplementedException("getDataById no está implementado.");
        }

        public virtual IDataReader getDataByString(string str)
        {
            Debug.LogWarning(Tag + "getDataByString no está implementado.");
            throw new NotImplementedException("getDataByString no está implementado.");
        }

        public virtual void deleteDataById(int id)
        {
            Debug.LogWarning(Tag + "deleteDataById no está implementado.");
            throw new NotImplementedException("deleteDataById no está implementado.");
        }

        public virtual void deleteDataByString(string str)
        {
            Debug.LogWarning(Tag + "deleteDataByString no está implementado.");
            throw new NotImplementedException("deleteDataByString no está implementado.");
        }

        public virtual IDataReader getAllData()
        {
            Debug.LogWarning(Tag + "getAllData no está implementado.");
            throw new NotImplementedException("getAllData no está implementado.");
        }

        public virtual void deleteAllData()
        {
            Debug.LogWarning(Tag + "deleteAllData no está implementado.");
            throw new NotImplementedException("deleteAllData no está implementado.");
        }

        public virtual IDataReader getNumOfRows()
        {
            Debug.LogWarning(Tag + "getNumOfRows no está implementado.");
            throw new NotImplementedException("getNumOfRows no está implementado.");
        }

        // Helper para obtener un comando SQL
        public IDbCommand getDbCommand()
        {
            return db_connection.CreateCommand();
        }

        // Obtiene todos los datos de una tabla
        public IDataReader getAllData(string table_name)
        {
            using (IDbCommand dbcmd = db_connection.CreateCommand())
            {
                dbcmd.CommandText = $"SELECT * FROM {table_name}";
                return dbcmd.ExecuteReader(); // El lector se cierra fuera
            }
        }

        // Elimina todas las filas (no la tabla)
        public void deleteAllData(string table_name)
        {
            using (IDbCommand dbcmd = db_connection.CreateCommand())
            {
                dbcmd.CommandText = $"DELETE FROM {table_name}";
                dbcmd.ExecuteNonQuery();
            }
        }

        // Borra por completo la tabla (estructura incluida)
        public void dropTable(string table_name)
        {
            using (IDbCommand dbcmd = db_connection.CreateCommand())
            {
                dbcmd.CommandText = $"DROP TABLE IF EXISTS {table_name}";
                dbcmd.ExecuteNonQuery();
            }
        }

        // Obtiene el siguiente ID disponible (si usas autoincremento manual)
        public IDataReader getNumOfRows(string table_name)
        {
            using (IDbCommand dbcmd = db_connection.CreateCommand())
            {
                dbcmd.CommandText = $"SELECT COALESCE(MAX(id)+1, 0) FROM {table_name}";
                return dbcmd.ExecuteReader(); // Cuidado: recuerda cerrarlo después de usar
            }
        }

        // Cierra la conexión manualmente (alternativo a Dispose)
        public void close()
        {
            Dispose();
        }
    }
}
