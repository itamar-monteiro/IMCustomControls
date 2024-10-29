using System.Drawing.Drawing2D;

namespace IMControls.CustomControls;

public class IMGradientPanel: Panel
{
    private int borderRadius = 0;
    private float gradientAngle = 90F;
    private Color gradientTopColor = ColorTranslator.FromHtml("#2C43D4");
    private Color gradientBottomColor = ColorTranslator.FromHtml("#4C8BFF");

    public IMGradientPanel()
    {
        BackColor = Color.White;
        ForeColor = Color.Black;
        Size = new Size(300, 50);
    }

    #region "Properties"
    public int BorderRadius 
    { 
        get => borderRadius;
        set { borderRadius = value; Invalidate(); }
    }

    public float GradientAngle 
    { 
        get => gradientAngle;
        set { gradientAngle = value; Invalidate(); } 
    }

    public Color GradientTopColor
    { 
        get => gradientTopColor;
        set { gradientTopColor = value; Invalidate(); }
    }

    public Color GradientBottomColor
    { 
        get => gradientBottomColor;
        set { gradientBottomColor = value; Invalidate(); }
    }
    #endregion

    #region "Methods"
    private GraphicsPath GetFigurePath(RectangleF rectangle, float radius)
    {
        GraphicsPath path = new GraphicsPath();
        path.StartFigure();
        path.AddArc(rectangle.Width - radius, rectangle.Height - radius, radius, radius, 0, 90);
        path.AddArc(rectangle.X, rectangle.Height - radius, radius, radius, 90, 90);
        path.AddArc(rectangle.X, rectangle.Y, radius, radius, 180, 90);
        path.AddArc(rectangle.Width - radius, rectangle.Y, radius, radius, 270, 90);
        path.CloseFigure();
        return path;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        LinearGradientBrush gradientBrush = new LinearGradientBrush
            (ClientRectangle, GradientTopColor, GradientBottomColor, GradientAngle);
        Graphics graphics = e.Graphics;

        graphics.FillRectangle(gradientBrush, ClientRectangle);

        RectangleF rectangleF = new RectangleF(0, 0, Width, Height);

        if (borderRadius > 2)
        {
            using (GraphicsPath graphicsPath = GetFigurePath(rectangleF, borderRadius))
            using (Pen pen = new Pen(Parent.BackColor, 2))
            {
                Region = new Region(graphicsPath);
                e.Graphics.DrawPath(pen, graphicsPath);
            }
        } else 
            Region = new Region(rectangleF);
    }
    #endregion
}
