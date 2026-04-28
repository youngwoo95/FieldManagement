using Dapper;
using PlantManagement.Dto.v1.Notice;

namespace PlantManagement.Repository.v1.Notice;

public partial class NoticeRepository
{
    /// <summary>
    /// 공지사항 리스트 조회
    /// </summary>
    public async Task<List<GetNoticeDto>?> GetNoticeList()
    {
        try
        {
            const string query = @"
                SELECT 
                    t.noticeSeq AS noticeSeq,
                    t.title AS title,
                    t.`description` AS `description`,
                    t.createUser AS createUser,
                    t.createDt AS createDt
                     FROM NoticeTb AS t
            ";
            
            var rows = await _dapper.QueryAsync<GetNoticeDto>(query);

            return rows.ToList();
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return null;
        }
    }
}