using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using FieldManagement.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace FieldManagement.Views;

public partial class MainBoardView : UserControl
{
    public ISeries[] PieSeries { get; set; }
    public ISeries[] Series { get; set; }
    
    public ISeries[] PieSeries2 { get; set; }
    public ObservableCollection<NoticeItem> Notices { get; set; } = new();
    
    public ISeries[] StackedSeries { get; set; }
    public Axis[] XAxes { get; set; }
    public Axis[] YAxes { get; set; }
    
    public MainBoardView()
    {
        InitializeComponent();
        
        double[] y = { 10, 20, 1, 30, 15,2,3 };
        double[] trend = BuildLinearTrendLine(y);

        Series = new ISeries[]
        {
            new ColumnSeries<double>
            {
                Name = "실데이터",
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
        
        PieSeries = new ISeries[]
        {
            new PieSeries<double> { Values = new double[] { 40 }, Name = "A", InnerRadius = 60 },
            new PieSeries<double> { Values = new double[] { 30 }, Name = "B", InnerRadius = 60 },
            new PieSeries<double> { Values = new double[] { 20 }, Name = "C", InnerRadius = 60 },
            new PieSeries<double> { Values = new double[] { 10 }, Name = "D", InnerRadius = 60 }
        };
        
        double value = 98;
        double remain = 100 - value;

        PieSeries2 = new ISeries[]
        {
            new PieSeries<double>
            {
                Values = new double[] { value },
                Name = "완료",
                InnerRadius = 80
            },
            new PieSeries<double>
            {
                Values = new double[] { remain },
                Name = "남음",
                InnerRadius = 80
            }
        };

        Notices = new ObservableCollection<NoticeItem>
        {
            new() { No = 1, Title = "System Check", Description = "Daily health check completed.", CreateUser = "Admin", CreateDt = "2026-04-20 10:00:00" },
            new() { No = 2, Title = "Sensor Alert", Description = "Zone B temperature exceeded threshold.", CreateUser = "System", CreateDt = "2026-04-20 11:25:00" },
            new() { No = 3, Title = "Maintenance", Description = "Inspection scheduled for tomorrow.", CreateUser = "Manager", CreateDt = "2026-04-20 14:30:00" },
            new() { No = 4, Title = "테스트 공지", Description = "Inspection scheduled for tomorrow.", CreateUser = "Manager", CreateDt = "2026-04-20 14:30:00" },
            new() { No = 5, Title = "Maintenance", Description = "Inspection scheduled for tomorrow.", CreateUser = "Manager", CreateDt = "2026-04-20 14:30:00" },
            new() { No = 6, Title = "Maintenance", Description = "Inspection scheduled for tomorrow.", CreateUser = "Manager", CreateDt = "2026-04-20 14:30:00" }
        };

        StackedSeries = new ISeries[]
        {
            new StackedColumnSeries<double>
            {
                Name = "하단",
                Values = new double[] { 3, 2, 2.5, 1.5, 2.8, 3.2, 2.4, 3.1, 2.7, 3.8, 4.0, 3.0 },
                Fill = new SolidColorPaint(new SKColor(185, 220, 235))
            },
            new StackedColumnSeries<double>
            {
                Name = "중단",
                Values = new double[] { 2, 1.8, 2.0, 1.2, 2.0, 2.5, 1.9, 2.2, 2.1, 2.8, 3.2, 2.3 },
                Fill = new SolidColorPaint(new SKColor(120, 190, 185))
            },
            new StackedColumnSeries<double>
            {
                Name = "상단",
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
        
        DataContext = this;
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
}
