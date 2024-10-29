using System.Drawing.Drawing2D;
using System.Windows.Forms.VisualStyles;

namespace IMControls.CustomControls;

public class IMCheckBox : CheckBox
{
    private Color checkedColor = Color.MediumSlateBlue;
    private Color unCheckedColor = Color.Gray;

    public IMCheckBox()
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
        float rbCheckSize = 14F;

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
                graphics.DrawRectangle(penBorder, rectRbBorder);   // Rectangle border

                float thickness = (float)rectRbBorder.Width / 11f;
                Color checkColor = Enabled ? checkedColor : SystemColors.ControlDark;
                using (Pen boxPen = new Pen(checkColor, thickness))
                {
                    float tailStartX = (float)rectRbBorder.Left + (float)rectRbBorder.Width * 2f / 11f;
                    float tailStartY = (float)rectRbBorder.Top + (float)rectRbBorder.Height / 2f;
                    float tailEndX = (float)rectRbBorder.Left + (float)rectRbBorder.Width * 4f / 11f;
                    float tailEndY = (float)rectRbBorder.Bottom - (float)rectRbBorder.Height * 3f / 11f;
                    float mainEndX = (float)rectRbBorder.Right - (float)rectRbBorder.Width * 2f / 11f;
                    float mainEndY = (float)rectRbBorder.Top + (float)rectRbBorder.Height * 3f / 11f;

                    double angle = Math.Atan2(tailEndY - tailStartY, tailEndX - tailStartX) * 180 / Math.PI;

                    pevent.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    pevent.Graphics.DrawLine(
                        boxPen, 
                        tailStartX, 
                        tailStartY, 
                        tailEndX + (float)Math.Sin(angle) * thickness / 2, 
                        tailEndY + (float)Math.Cos(angle) * thickness / 2);

                    pevent.Graphics.DrawLine(boxPen, tailEndX, tailEndY, mainEndX, mainEndY);
                }

            } else
            {
                penBorder.Color = unCheckedColor;
                graphics.DrawRectangle(penBorder, rectRbBorder);
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
