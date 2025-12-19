using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WinForms;

public sealed class VoteChart
{
    private readonly CartesianChart _chart;
    private readonly ColumnSeries<int> _series;

    public Control Control => _chart;

    public VoteChart()
    {
        _series = new ColumnSeries<int> { Values = Array.Empty<int>() };

        _chart = new CartesianChart
        {
            Dock = DockStyle.Fill,
            Series = new ISeries[] { _series },
            XAxes = new[] { new Axis { Labels = Array.Empty<string>() } },
            YAxes = new[] { new Axis { MinLimit = 0 } }
        };
    }

    public void Update(Dictionary<string, int> counts)
    {
        var labels = counts.Keys.ToArray();
        var values = labels.Select(k => counts[k]).ToArray();

        _chart.XAxes = new[] { new Axis { Labels = labels } };
        _series.Values = values;
    }
}
