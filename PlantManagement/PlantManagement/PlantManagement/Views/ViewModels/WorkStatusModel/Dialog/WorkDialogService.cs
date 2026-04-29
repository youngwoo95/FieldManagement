using System;
using System.Linq;
using System.Windows;
using PlantManagement.Dto.v1.Works;
using PlantManagement.Service.v1.Works;
using PlantManagement.ViewItems;
using PlantManagement.Views.ViewModels.CustomerModel;
using PlantManagement.Views.Views.Dialogs;

namespace PlantManagement.Views.ViewModels.WorkStatusModel.Dialog;

public class WorkDialogService : IWorkDialogService
{
    private readonly IWorkService _workService;

    public WorkDialogService(IWorkService workService)
    {
        _workService = workService;
    }

    public async Task<WorkStatusViewItems?> ShowAddWorkStatusDialogAsync()
    {
        var viewModel = new AddWorkStatusViewModel
        {
            WorkOrderNo = "자동 생성"
        };

        await BindFacilityOptionsAsync(viewModel);
        await BindOrderOptionsAsync(viewModel);

        var dialog = new AddWorkStatusWindow(viewModel)
        {
            Owner = Application.Current?.MainWindow
        };

        if (dialog.ShowDialog() != true)
        {
            return null;
        }

        if (viewModel.SelectedOrderInfo is null)
        {
            return null;
        }

        var facilitySeq = await ResolveFacilitySeqAsync(viewModel.MachineName);
        if (facilitySeq is null)
        {
            return null;
        }

        var startWorkDate = ParseWorkDate(viewModel.WorkDate);
        var dto = BuildAddWorksDto(viewModel, facilitySeq.Value, startWorkDate);
        var saved = await _workService.AddWorksService(dto);
        if (!saved)
        {
            return null;
        }

        return new WorkStatusViewItems
        {
            WorkOrderNo = dto.workSeq,
            MachineName = viewModel.MachineName,
            CustomerName = viewModel.CustomerName,
            Status = string.IsNullOrWhiteSpace(viewModel.Status) ? "InProgress" : viewModel.Status,
            WorkDate = startWorkDate.ToString("yyyy-MM-dd"),
            PdfFileName = string.IsNullOrWhiteSpace(viewModel.AttachmentFilePath)
                ? "pdf1.pdf"
                : viewModel.AttachmentFilePath
        };
    }

    private async Task BindFacilityOptionsAsync(AddWorkStatusViewModel viewModel)
    {
        var rows = await _workService.GetWorkFacilitiesService() ?? [];
        var names = rows
            .Select(x => x.facilityName)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase);

        viewModel.SetFacilityNames(names);
    }

    private async Task BindOrderOptionsAsync(AddWorkStatusViewModel viewModel)
    {
        var rows = await _workService.GetWorkOrderInfosService() ?? [];
        var orderInfos = rows
            .Select(x => new WorkOrderInfoOption
            {
                OrderSeq = x.orderSeq,
                CustomerSeq = x.customerSeq,
                CustomerName = x.customerName ?? string.Empty,
                PdfFileName = x.attach ?? string.Empty
            })
            .OrderByDescending(x => x.OrderSeq)
            .ToList();

        viewModel.SetOrderInfos(orderInfos);
    }

    private async Task<int?> ResolveFacilitySeqAsync(string facilityName)
    {
        var rows = await _workService.GetWorkFacilitiesService() ?? [];
        var facility = rows.FirstOrDefault(x =>
            string.Equals(x.facilityName?.Trim(), facilityName?.Trim(), StringComparison.OrdinalIgnoreCase));

        return facility?.facilitySeq;
    }

    private static AddWorksDto BuildAddWorksDto(
        AddWorkStatusViewModel viewModel,
        int facilitySeq,
        DateTime startWorkDate)
    {
        return new AddWorksDto
        {
            workSeq = GenerateWorkOrderNo(),
            orderSeq = viewModel.SelectedOrderInfo!.OrderSeq,
            facilitySeq = facilitySeq,
            currentQty = 0,
            startWorkDt = startWorkDate,
            endWorkDt = default,
            status = 0
        };
    }

    private static string GenerateWorkOrderNo()
    {
        return $"WORK_{DateTime.Now:yyyyMMddHHmmss}";
    }

    private static DateTime ParseWorkDate(string workDate)
    {
        if (string.IsNullOrWhiteSpace(workDate))
        {
            return DateTime.Today;
        }

        return DateTime.TryParse(workDate, out var date)
            ? date
            : DateTime.Today;
    }

}
