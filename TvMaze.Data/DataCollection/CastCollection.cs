using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Data;

namespace TvMaze.Data
{
    public class CastCollection : List<CastDBParam>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            SqlDataRecord ret = new SqlDataRecord(
                new SqlMetaData("TvMazePersonId", SqlDbType.Int),
                new SqlMetaData("Name", SqlDbType.VarChar, 100)
                );

            foreach (CastDBParam data in this)
            {
                ret.SetInt32(0, data.TvMazePersonId);
                ret.SetString(1, data.Name);
                yield return ret;
            }
        }
    }
}
