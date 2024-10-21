using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace CustomControls.IMControls;

public class IMSwitchButton: CheckBox
{
    private Color onBackColor = Color.ForestGreen;
    private Color onToggleColor = Color.WhiteSmoke;
    private Color offBackColor = Color.Gray;
    private Color offToggleColor = Color.White;
    private bool solidStyle = true;

    [Category("Advanced Properties")]
    public IMSwitchButton()
    {
        this.MinimumSize = new Size(30, 20);
        this.AutoSize = false;
        this.Size = new Size(70, 20);
    }

    [Category("Advanced Properties")]
    public Color OnBackColor 
    { 
        get => onBackColor; 
        set 
        { 
            onBackColor = value;
            this.Invalidate();
        } 
    }

    [Category("Advanced Properties")]
    public Color OnToggleColor 
    { 
        get => onToggleColor;
        set 
        { 
            onToggleColor = value;
            this.Invalidate();
        } 
    }

    [Category("Advanced Properties")]
    public Color OffBackColor 
    { 
        get => offBackColor;
        set 
        { 
            offBackColor = value;
            this.Invalidate();
        } 
    }

    [Category("Advanced Properties")]
    public Color OffToggleColor 
    { 
        get => offToggleColor;
        set 
        { 
            offToggleColor = value;
            this.Invalidate();
        } 
    }

    [DefaultValue(true)]
    [Category("Advanced Properties")]
    public bool SolidStyle
    {
        get => solidStyle;
        set
        {
            solidStyle = value;
            this.Invalidate();
        }
    }

    public override string Text 
    { 
        get => base.Text;
        set {} 
    }

    public override bool AutoSize 
    { 
        get => base.AutoSize; 
        set => base.AutoSize = false; 
    }

    private GraphicsPath GetFigurePath()
    {
        int arcSize = this.Height - 1;
        Rectangle leftArc = new Rectangle(0, 0, arcSize, arcSize);
        Rectangle rightArc = new Rectangle(this.Width - arcSize - 2, 0, arcSize, arcSize);

        GraphicsPath path = new GraphicsPath();
        path.StartFigure();
        path.AddArc(leftArc, 90, 180);
        path.AddArc(rightArc, 270, 180);
        path.CloseFigure();

        return path;
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        int toggleSize = this.Height - 5;
        pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        pevent.Graphics.Clear(this.Parent.BackColor);

        if (this.Checked)
        {
            if (solidStyle)
                pevent.Graphics.FillPath(new SolidBrush(onBackColor), GetFigurePath());
            else 
                pevent.Graphics.DrawPath(new Pen(onBackColor, 2), GetFigurePath());

            pevent.Graphics.FillEllipse(new SolidBrush(onToggleColor),
              new Rectangle(this.Width - this.Height + 1, 2, toggleSize, toggleSize));
        } else 
        {
            if (solidStyle)
                pevent.Graphics.FillPath(new SolidBrush(offBackColor), GetFigurePath());
            else 
                pevent.Graphics.DrawPath(new Pen(offBackColor, 2), GetFigurePath());

            pevent.Graphics.FillEllipse(new SolidBrush(offToggleColor),
              new Rectangle(2, 2, toggleSize, toggleSize));
        }
    }
}
