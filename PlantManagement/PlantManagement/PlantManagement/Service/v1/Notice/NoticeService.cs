using PlantManagement.Comm.Logger;
using PlantManagement.Dto.v1.Notice;
using PlantManagement.Repository.v1.Notice;

namespace PlantManagement.Service.v1.Notice;

public class NoticeService : INoticeService
{
    private readonly ILogService _logService;
    private readonly INoticeRepository _noticeRepository;

    public NoticeService(
        ILogService logService,
        INoticeRepository noticeRepository)
    {
        this._noticeRepository = noticeRepository;
        this._logService = logService;
    }
    
    /// <summary>
    /// 공지사항 조회
    /// </summary>
    public async Task<List<GetNoticeDto>?> GetNoticeService()
    {
        try
        {
            var model = await _noticeRepository.GetNoticeList().ConfigureAwait(false);

            return model;
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return null;
        }
    }
}