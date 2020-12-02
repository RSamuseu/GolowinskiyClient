using Dapper;
using GolovinskyAPI.Data.Interfaces;
using GolovinskyAPI.Data.Models.Background;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GolovinskyAPI.Data.Repositories
{
    public class BackgroundRepository : IBackgroundRepository
    {
        private readonly string connectionString;

        public BackgroundRepository(string connection)
        {
            connectionString = connection;
        }

        public async Task<string> Create(Background background)
        {
            string res = "";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var response = await db.QueryAsync<string>("sp_SetUpdateBkgImageMobile", new
                {
                    appcode = background.AppCode,
                    filename = background.FileName,
                    img = background.Img,
                    date = background.Date,
                    orient = background.orient,
                    place = background.place
                },
                    commandType: CommandType.StoredProcedure);
                res = response.First();
            }

            return res;
        }

        public async Task<string> Delete(Background background)
        {
            string res = "";
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var response = await db.QueryAsync<string>("sp_DelBkgImageMobile", new
                {
                    appcode = background.AppCode,
                    filename = background.FileName
                },
                   commandType: CommandType.StoredProcedure);
                res = response.First();
            }

            return res;
        }

        public async Task<List<Background>> GetBackground(Background background)
        {
            List<Background> res = new List<Background>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var response = await db.QueryAsync<Background>("sp_GetBkgImageMobile", new
                {
                    appcode = background.AppCode,
                    date = background.Date,
                    mark = background.Mark,
                    orient = background.orient,
                    place = background.place
                },
                    commandType: CommandType.StoredProcedure);
                res = response.ToList();
            }

            return res;
        }

        public async Task<string> Update(Background background)
        {
            string res = "";
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var response = await db.QueryAsync<string>("sp_SetUpdateBkgImageMobile", new
                {
                    appcode = background.AppCode,
                    filename = background.FileName,
                    img = background.Img,
                    date = background.Date,
                    orient = background.orient,
                    place = background.place
                },
                    commandType: CommandType.StoredProcedure);
                res = response.First();
            }

            return res;
        }
    }
}
