namespace FieldManagement.Models;

/// <summary>
/// 작업현황 모델
/// </summary>
public class WorkerModel
{
    public string WorkOrderNo { get; init; } = string.Empty;
    public string MachineName { get; init; } = string.Empty;
    public string CustomerName { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string WorkDate { get; init; } = string.Empty;
}