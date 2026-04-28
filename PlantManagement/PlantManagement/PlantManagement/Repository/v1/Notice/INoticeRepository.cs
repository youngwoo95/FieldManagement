using PlantManagement.Dto.v1.Notice;

namespace PlantManagement.Repository.v1.Notice;

public interface INoticeRepository
{
    /// <summary>
    /// 공지사항 조회
    /// </summary>
    public Task<List<GetNoticeDto>?> GetNoticeList();
}