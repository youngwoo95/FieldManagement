using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using PlantManagement.Service.v1.Notice;
using PlantManagement.ViewItems;
using SkiaSharp;

namespace PlantManagement.Views.ViewModels.DashBoardModel;

public partial class DashBoardViewModel : BaseViewModel
{
    private readonly INoticeService _noticeService;
    
    public ISeries[] StackSeries { get; set; } // 스택 막대 그래프
    public Axis[] XAxes { get; set; } // X축
    public Axis[] YAxes { get; set; } // Y축
 
    /// <summary>
    ///  목표량 그래프
    /// </summary>
    public ISeries[] TargetSeries { get; set; }
    
    /// <summary>
    /// 설비별 생산량 그래프
    /// </summary>
    public ISeries[] EquipProductSeries { get; set; }
    
    /// <summary>
    /// 불량률 그래프
    /// </summary>
    public ISeries[] DefectRateSeries { get; set; }
    
    public ObservableCollection<NoticeViewItems> NoticeItems { get; set; } = new();
    
    public DashBoardViewModel(INoticeService noticeService)
    {
        this._noticeService = noticeService;
        
        SetStackSeries();
        SetTargetSeries();
        SetEquipmentProductSeries();
        SetDefectRateSeries();
        
        _ = InitializeAsync(); // 비동기 초기 로딩
    }
    
    private async Task InitializeAsync()
    {
        try
        {
            await SetNotice();
        }
        catch
        {
            // Avoid crashing startup on first dashboard load.
        }
    }

    public void SetDefectRateSeries()
    {
        double value = 98;
        double remain = 100 - value;

        DefectRateSeries = new ISeries[]
        {
            new PieSeries<double>
            {
                Values = new double[] { value },
                Name = "정상",
                InnerRadius = 80
            },
            new PieSeries<double>
            {
                Values = new double[] { remain },
                Name = "불량",
                InnerRadius = 80
            }
        };
    }
    
    /// <summary>
    /// 설비별 생산 그래프
    /// </summary>
    public void SetEquipmentProductSeries()
    {
        EquipProductSeries = new ISeries[]
        {
            new PieSeries<double> { Values = new double[] { 40 }, Name = "A", InnerRadius = 60 },
            new PieSeries<double> { Values = new double[] { 30 }, Name = "B", InnerRadius = 60 },
            new PieSeries<double> { Values = new double[] { 20 }, Name = "C", InnerRadius = 60 },
            new PieSeries<double> { Values = new double[] { 10 }, Name = "D", InnerRadius = 60 }
        };
    }
    
    /// <summary>
    /// 목표량 그래프
    /// </summary>
    public void SetTargetSeries()
    {
        double[] y = { 10, 20, 1, 30, 15,2,3 };
        double[] trend = BuildLinearTrendLine(y);

        TargetSeries = new ISeries[]
        {
            new ColumnSeries<double>
            {
                Name = "실제 생산량",
                Values = y
            },

            new LineSeries<double>
            {
                Name = "기대치",
                Values = new double[] { 4, 6, 5, 3, -3, -1, 2 },
                Fill = null, // 선 아래 채우기 제거
                GeometrySize = 12, // 점 크기
                LineSmoothness = 0.8  // 직선이면 0, 부드러운 곡선이면 1에 가깝게
            },
            
            new LineSeries<double>
            {
                Name = "추세선",
                Values = trend,
                Fill = null,
                GeometrySize = 0,
                LineSmoothness = 1
            }
        };
    }
    // 추세선
    private double[] BuildLinearTrendLine(double[] y)
    {
        int n = y.Length;
        double[] x = Enumerable.Range(0, n).Select(i => (double)i).ToArray();

        double sumX = x.Sum();
        double sumY = y.Sum();
        double sumXY = x.Zip(y, (a, b) => a * b).Sum();
        double sumX2 = x.Sum(v => v * v);

        double slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
        double intercept = (sumY - slope * sumX) / n;

        return x.Select(v => slope * v + intercept).ToArray();
    }
    
    /// <summary>
    /// 공지사항 데이터
    /// </summary>
    public async Task SetNotice()
    {
        var notices = await _noticeService.GetNoticeService();
      
        foreach (var noticeItem in notices)
        {
            NoticeItems.Add(new NoticeViewItems
            {
                No = noticeItem.noticeSeq,
                Title = noticeItem.title ?? string.Empty,
                Description = noticeItem.description ?? string.Empty,
                CreateUser = noticeItem.createUser ?? string.Empty,
                CreateDt = noticeItem.createDt.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }
    }

    public void SetStackSeries()
    {
        StackSeries = new ISeries[]
        {
            new StackedColumnSeries<double>
            {
                Name = "A설비 생산량",
                Values = new double[] { 3, 2, 2.5, 1.5, 2.8, 3.2, 2.4, 3.1, 2.7, 3.8, 4.0, 3.0 },
                Fill = new SolidColorPaint(new SKColor(185, 220, 235))
            },
            new StackedColumnSeries<double>
            {
                Name = "B설비 생산량",
                Values = new double[] { 2, 1.8, 2.0, 1.2, 2.0, 2.5, 1.9, 2.2, 2.1, 2.8, 3.2, 2.3 },
                Fill = new SolidColorPaint(new SKColor(120, 190, 185))
            },
            new StackedColumnSeries<double>
            {
                Name = "C설비 생산량",
                Values = new double[] { 1.2, 1.0, 1.1, 0.8, 1.4, 1.8, 1.2, 1.6, 1.4, 2.0, 2.6, 1.7 },
                Fill = new SolidColorPaint(new SKColor(75, 155, 150))
            }
        };
        XAxes = new Axis[]
        {
            new Axis
            {
                Labels = new[]
                {
                    "1월","2월","3월","4월","5월","6월",
                    "7월","8월","9월","10월","11월","12월"
                },
                LabelsPaint = new SolidColorPaint(new SKColor(229, 231, 235)), // 밝은 글자
                SeparatorsPaint = new SolidColorPaint(new SKColor(75, 85, 99), 1), // 구분선
                TextSize = 12,
                LabelsRotation = 0
            }
        };

        YAxes = new Axis[]
        {
            new Axis
            {
                MinLimit = 0,
                LabelsPaint = new SolidColorPaint(new SKColor(229, 231, 235)), // 밝은 글자
                SeparatorsPaint = new SolidColorPaint(new SKColor(75, 85, 99), 1), // 구분선
                TextSize = 12,
            },
            
        };
    }
    
}
