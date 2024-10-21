using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CustomControls.IMControls;

public class IMEllipseControl : Component
{
    [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
    private static extern IntPtr CreateRoundRectRgn
        (
           int nLeftRect,
           int nTopRect,
           int nRightRect,
           int nBottomRect,
           int nWidthEllipse,
           int nHeightEllipse
        );

    private Control control;
    private int cornerRadius = 20;

    public Control TargetControl
    {
        get { return control; }
        set
        {
            control = value;
            control.SizeChanged += (sender, eventArgs) =>
                control.Region = Region.FromHrgn(
                    CreateRoundRectRgn(0, 0, control.Width, control.Height, cornerRadius, cornerRadius));
        }
    }

    public int CornerRadius
    {
        get { return cornerRadius; }
        set
        {
            cornerRadius = value;
            if (control != null)
                control.Region = Region.FromHrgn(
                    CreateRoundRectRgn(0, 0, control.Width, control.Height, cornerRadius, cornerRadius));
        }
    }
}