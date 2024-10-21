﻿using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace CustomControls.IMControls;

public class IMCircularPictureBox: PictureBox
{
    private int borderSize = 2;
    private Color borderColor = Color.RoyalBlue;
    private Color borderColor2 = Color.HotPink;
    private DashStyle borderLineStyle = DashStyle.Solid;
    private DashCap borderCapStyle = DashCap.Flat;
    private float gradientAngle = 50F;

    public IMCircularPictureBox()
    {
        this.Size = new Size(100, 100);
        this.SizeMode = PictureBoxSizeMode.Zoom;
    }

    #region "Properties"
    public int BorderSize
    {
        get => borderSize;
        set
        {
            borderSize = value;
            this.Invalidate();
        }
    }

    public Color BorderColor
    {
        get => borderColor;
        set
        {
            borderColor = value;
            this.Invalidate();
        }
    }

    public Color BorderColor2
    {
        get => borderColor2;
        set
        {
            borderColor2 = value;
            this.Invalidate();
        }
    }

    public DashStyle BorderLineStyle
    {
        get => borderLineStyle;
        set
        {
            borderLineStyle = value;
            this.Invalidate();
        }
    }

    public DashCap BorderCapStyle
    {
        get => borderCapStyle;
        set
        {
            borderCapStyle = value;
            this.Invalidate();
        }
    }

    public float GradientAngle
    {
        get => gradientAngle;
        set
        {
            gradientAngle = value;
            this.Invalidate();
        }
    }
    #endregion

    #region "Methods"
    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        this.Size = new Size(this.Width, this.Width);
    }

    protected override void OnPaint(PaintEventArgs pe)
    {
        base.OnPaint(pe);

        var graph = pe.Graphics;
        var rectContourSmooth = Rectangle.Inflate(this.ClientRectangle, -1, -1);
        var rectBorder = Rectangle.Inflate(rectContourSmooth, -borderSize, -borderSize);
        var smoothSize = borderSize > 0 ? borderSize * 3 : 1;
        using (var borderGColor = new LinearGradientBrush(rectBorder, borderColor, borderColor2, gradientAngle))
        using (var pathRegion = new GraphicsPath())
        using (var penSmooth = new Pen(this.Parent.BackColor, smoothSize))
        using (var penBorder = new Pen(borderGColor, borderSize))
        {
            graph.SmoothingMode = SmoothingMode.AntiAlias;
            penBorder.DashStyle = borderLineStyle;
            penBorder.DashCap = borderCapStyle;
            pathRegion.AddEllipse(rectContourSmooth);
            this.Region = new Region(pathRegion);

            graph.DrawEllipse(penSmooth, rectContourSmooth);
            if (borderSize > 0) 
                graph.DrawEllipse(penBorder, rectBorder);
        }
    }
    #endregion
}