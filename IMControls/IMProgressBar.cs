using System.ComponentModel;

namespace IMControls.CustomControls;

public enum TextPosition
{
    Left,
    Right,
    Center,
    Sliding,
    None
}

public class IMProgressBar: ProgressBar
{
    private Color channelColor = Color.LightSteelBlue;
    private Color sliderColor = Color.RoyalBlue;
    private Color foreBackColor = Color.RoyalBlue;
    private int channelHeight = 6;
    private int sliderHeight = 6;
    private TextPosition showValue = TextPosition.Center;
    private string symbolBefore = "";
    private string symbolAfter = "";
    private bool showMaximun = false;
    private bool paintedBack = false;
    private bool stopPainting = false;

    public IMProgressBar()
    {
        SetStyle(ControlStyles.UserPaint, true);
        ForeColor = Color.White;
    }

    #region "Properties"
    public Color ChannelColor
    {
        get => channelColor;
        set
        {
            channelColor = value;
            Invalidate();
        }
    }

    public Color SliderColor
    {
        get => sliderColor;
        set
        {
            sliderColor = value;
            Invalidate();
        }
    }

    public Color ForeBackColor
    {
        get => foreBackColor;
        set
        {
            foreBackColor = value;
            Invalidate();
        }
    }

    public int ChannelHeight
    {
        get => channelHeight;
        set
        {
            channelHeight = value;
            Invalidate();
        }
    }

    public int SliderHeight
    {
        get => sliderHeight;
        set
        {
            sliderHeight = value;
            Invalidate();
        }
    }

    public TextPosition ShowValue
    {
        get => showValue;
        set
        {
            showValue = value;
            Invalidate();
        }
    }

    public string SymbolBefore
    {
        get => symbolBefore;
        set
        {
            symbolBefore = value;
            Invalidate();
        }
    }

    public string SymbolAfter
    {
        get => symbolAfter;
        set
        {
            symbolAfter = value;
            Invalidate();
        }
    }

    public bool ShowMaximun
    {
        get => showMaximun;
        set
        {
            showMaximun = value;
            Invalidate();
        }
    }

    [Browsable(true)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public override Font Font
    {
        get => base.Font;
        set
        {
            base.Font = value;
        }
    }

    public override Color ForeColor
    {
        get => base.ForeColor;
        set
        {
            base.ForeColor = value;
        }
    }

    #endregion

    #region "Methods"
    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        if (stopPainting == false)
        {
            if (paintedBack == false)
            {
                Graphics graph = pevent.Graphics;
                Rectangle rectChannel = new Rectangle(0, 0, Width, ChannelHeight);
                using (var brushChannel = new SolidBrush(channelColor))
                {
                    if (channelHeight >= sliderHeight)
                        rectChannel.Y = Height - channelHeight;
                    else 
                        rectChannel.Y = Height - ((channelHeight + sliderHeight) / 2);

                    graph.Clear(Parent.BackColor);
                    graph.FillRectangle(brushChannel, rectChannel);

                    if (DesignMode == false)
                        paintedBack = true;
                }
            }

            if (Value == Maximum || Value == Minimum)
                paintedBack = false;
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        if (stopPainting == false)
        {
            Graphics graph = e.Graphics;
            double scaleFactor = (((double)Value - Minimum) / ((double)Maximum - Minimum));
            int sliderWidth = (int)(Width * scaleFactor);
            Rectangle rectSlider = new Rectangle(0, 0, sliderWidth, sliderHeight);
            using (var brushSlider = new SolidBrush(sliderColor))
            {
                if (sliderHeight >= channelHeight)
                    rectSlider.Y = Height - sliderHeight;
                else 
                    rectSlider.Y = Height - ((sliderHeight + channelHeight) / 2);

                if (sliderWidth > 1)
                    graph.FillRectangle(brushSlider, rectSlider);

                if (showValue != TextPosition.None)
                    DrawValueText(graph, sliderWidth, rectSlider);
            }
        }

        if (Value == Maximum) 
            stopPainting = true;
        else 
            stopPainting = false;
    }

    private void DrawValueText(Graphics graph, int sliderWidth, Rectangle rectSlider)
    {
        string text = symbolBefore + Value.ToString() + symbolAfter;
        if (showMaximun) 
            text = text + "/" + symbolBefore + Maximum.ToString() + symbolAfter;

        var textSize = TextRenderer.MeasureText(text, Font);
        var rectText = new Rectangle(0, 0, textSize.Width, textSize.Height + 2);

        using (var brushText = new SolidBrush(ForeColor))
        using (var brushTextBack = new SolidBrush(foreBackColor))
        using (var textFormat = new StringFormat())
        {
            switch (showValue)
            {
                case TextPosition.Left:
                    rectText.X = 0;
                    textFormat.Alignment = StringAlignment.Near;
                    break;

                case TextPosition.Right:
                    rectText.X = Width - textSize.Width;
                    textFormat.Alignment = StringAlignment.Far;
                    break;

                case TextPosition.Center:
                    rectText.X = (Width - textSize.Width) / 2;
                    textFormat.Alignment = StringAlignment.Center;
                    break;

                case TextPosition.Sliding:
                    rectText.X = sliderWidth - textSize.Width;
                    textFormat.Alignment = StringAlignment.Center;
                    using (var brushClear = new SolidBrush(Parent.BackColor))
                    {
                        var rect = rectSlider;
                        rect.Y = rectText.Y;
                        rect.Height = rectText.Height;
                        graph.FillRectangle(brushClear, rect);
                    }
                    break;
            }
            graph.FillRectangle(brushTextBack, rectText);
            graph.DrawString(text, Font, brushText, rectText, textFormat);
        }
    }
    #endregion
}
