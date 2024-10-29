using System.Drawing.Drawing2D;

namespace IMControls.CustomControls;

public class IMRadioButton: RadioButton
{
    private Color checkedColor = Color.MediumSlateBlue;
    private Color unCheckedColor = Color.Gray;

    public IMRadioButton()
    {
        MinimumSize = new Size(0, 21);
        Padding = new Padding(10, 0, 0, 0);
    }

    #region "Properties"
    public Color CheckedColor
    {
        get => checkedColor;
        set
        {
            checkedColor = value;
            Invalidate();
        }
    }

    public Color UnCheckedColor
    {
        get => unCheckedColor;
        set
        {
            unCheckedColor = value;
            Invalidate();
        }
    }
    #endregion

    #region "Methods"
    protected override void OnPaint(PaintEventArgs pevent)
    {
        Graphics graphics = pevent.Graphics;
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        float rbBorderSize = 18F;
        float rbCheckSize = 12F;

        RectangleF rectRbBorder = new RectangleF()
        {
            X = 0.5F,
            Y = (Height - rbBorderSize) / 2,
            Width = rbBorderSize,
            Height = rbBorderSize
        };

        RectangleF rectRbCheck = new RectangleF()
        {
            X = rectRbBorder.X + ((rectRbBorder.Width - rbCheckSize) / 2),
            Y = (Height - rbCheckSize) / 2,
            Width = rbCheckSize,
            Height = rbCheckSize
        };

        using (Pen penBorder = new Pen(checkedColor, 1.6F))
        using (SolidBrush brushRbCheck = new SolidBrush(checkedColor))
        using (SolidBrush brushText = new SolidBrush(ForeColor))
        {
            graphics.Clear(BackColor);

            if (Checked)
            {
                graphics.DrawEllipse(penBorder, rectRbBorder);   // Circle border
                graphics.FillEllipse(brushRbCheck, rectRbCheck); // Circle Radio Check
            } else
            {
                penBorder.Color = unCheckedColor;
                graphics.DrawEllipse(penBorder, rectRbBorder); // Circle border
            }
            graphics.DrawString
                (Text, 
                Font, 
                brushText,
                rbBorderSize + 8, 
                (Height - TextRenderer.MeasureText(Text, Font).Height) / 2);
        }
    }
    #endregion
}
