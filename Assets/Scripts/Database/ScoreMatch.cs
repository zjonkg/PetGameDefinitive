using UnityEngine;

using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

namespace ShyLaura.Database
{
    public class ScoreMatch : SqliteHelper
    {
        private const string TableName = "score_partidas";

        private const string Key_PlayerId = "player_id";
        private const string Key_PartidaId = "id_partida_jugada";
        private const string Key_Score = "score";
        private const string Key_Coins = "coin_gained";

        public ScoreMatch() : base()
        {
            createTable();
        }

        private void createTable()
        {
            using (IDbCommand dbcmd = getDbCommand())
            {
                dbcmd.CommandText = $"CREATE TABLE IF NOT EXISTS {TableName} (" +
                                    $"{Key_PlayerId} TEXT, " +
                                    $"{Key_PartidaId} TEXT PRIMARY KEY, " +
                                    $"{Key_Score} INTEGER, " +
                                    $"{Key_Coins} INTEGER)";
                dbcmd.ExecuteNonQuery();
            }
        }

        public void insertData(string playerId, string partidaId, int score, int coins)
        {
            using (IDbCommand dbcmd = getDbCommand())
            {
                dbcmd.CommandText = $"INSERT OR REPLACE INTO {TableName} " +
                                    $"({Key_PlayerId}, {Key_PartidaId}, {Key_Score}, {Key_Coins}) " +
                                    $"VALUES (@playerId, @partidaId, @score, @coins)";
                dbcmd.Parameters.Add(new SqliteParameter("@playerId", playerId));
                dbcmd.Parameters.Add(new SqliteParameter("@partidaId", partidaId));
                dbcmd.Parameters.Add(new SqliteParameter("@score", score));
                dbcmd.Parameters.Add(new SqliteParameter("@coins", coins));
                dbcmd.ExecuteNonQuery();
            }
        }

        public override IDataReader getAllData()
        {
            return base.getAllData(TableName);
        }

        public override void deleteAllData()
        {
            base.deleteAllData(TableName);
        }

        public override IDataReader getDataById(int id)
        {
            using (IDbCommand dbcmd = getDbCommand())
            {
                dbcmd.CommandText = $"SELECT * FROM {TableName} WHERE {Key_PartidaId} = @id";
                dbcmd.Parameters.Add(new SqliteParameter("@id", id.ToString()));
                return dbcmd.ExecuteReader();
            }
        }

        public override void deleteDataById(int id)
        {
            using (IDbCommand dbcmd = getDbCommand())
            {
                dbcmd.CommandText = $"DELETE FROM {TableName} WHERE {Key_PartidaId} = @id";
                dbcmd.Parameters.Add(new SqliteParameter("@id", id.ToString()));
                dbcmd.ExecuteNonQuery();
            }
        }

        public override IDataReader getNumOfRows()
        {
            return base.getNumOfRows(TableName);
        }
    }
}
