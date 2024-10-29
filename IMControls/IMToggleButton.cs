
using Microsoft.VisualBasic.Logging;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace IMControls.CustomControls;

public class IMToggleButton: Control
{
    private bool isOn;
    private int radius = 20;

    public IMToggleButton()
    {
        MinimumSize = new Size(80, 20);
        Width = 80;
        Height = 20;
        isOn = false;
        DoubleBuffered = true;
        Font = new Font("Century Gothic", 10f, FontStyle.Bold);
        Click += onToggleButton_Click;
    }

    #region "Methods"
    private void onToggleButton_Click(object? sender, EventArgs e)
    {
        IsOn = !isOn;
        onToggleButtonChanged(EventArgs.Empty);
    }

    protected virtual void onToggleButtonChanged(EventArgs e) => ToggleButtonChanged?.Invoke(this, e);

    public event EventHandler ToggleButtonChanged;

    private GraphicsPath CreateRoundedRectanglePath()
    {
        int arcSize = Height - 1;
        Rectangle leftArc = new Rectangle(0, 0, arcSize, arcSize);
        Rectangle rightArc = new Rectangle(Width - arcSize - 2, 0, arcSize, arcSize);

        GraphicsPath path = new GraphicsPath();
        path.StartFigure();
        path.AddArc(leftArc, 90, 180);
        path.AddArc(rightArc, 270, 180);
        path.CloseFigure();

        return path;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        int toggleSize = Height - 5;
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.Clear(Parent.BackColor);

        if (IsOn)
        {
            e.Graphics.FillPath(new SolidBrush(OnColor), CreateRoundedRectanglePath());
            e.Graphics.FillEllipse(new SolidBrush(SwitchColor),
              new Rectangle(Width - Height + 1, 2, toggleSize, toggleSize));
        }
        else
        {
            e.Graphics.FillPath(new SolidBrush(OffColor), CreateRoundedRectanglePath());
            e.Graphics.FillEllipse(new SolidBrush(SwitchColor),
              new Rectangle(2, 2, toggleSize, toggleSize));
        }

        // Draw Label
        string label = IsOn ? OnLabelText : OffLabelText;
        var labelSize = e.Graphics.MeasureString(label, Font);
        float labelX = IsOn ? 10 : Width - labelSize.Width - 10;
        float labelY = (Height - labelSize.Height) / 2;

        e.Graphics.DrawString(label, Font, new SolidBrush(LabelColor), new PointF(labelX, labelY));
    }

    #endregion

    #region "Properties"
    public string OnLabelText { get; set; } = "On";
    public string OffLabelText { get; set; } = "OFF";
    public Color OnColor { get; set; } = Color.ForestGreen;
    public Color OffColor { get; set; }= Color.Gray;
    public Color SwitchColor { get; set; } = Color.White;
    public Color LabelColor { get; set; } = Color.White;

    public bool IsOn 
    { 
        get { return isOn; }
        set { isOn = value; Invalidate(); }
    }

    public int Radius
    {
        get { return radius; }
        set 
        {
            if (value <=5) 
                radius = 6;
            else if (value > Height) 
                radius = Height;
            else
                radius = value;
            Invalidate();
        }
    }

    public override string Text 
    { 
        get => base.Text; 
        set => base.Text = value; 
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override Color ForeColor 
    { 
        get => base.ForeColor; 
        set => base.ForeColor = value; 
    }

    #endregion
}
