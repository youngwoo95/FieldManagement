using PlantManagement.Dto.v1.Notice;

namespace PlantManagement.Service.v1.Notice;

public interface INoticeService
{
    /// <summary>
    /// 공지사항 조회
    /// </summary>
    public Task<List<GetNoticeDto>?> GetNoticeService();
}