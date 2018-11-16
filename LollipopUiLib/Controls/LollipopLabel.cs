using System.Drawing;
using System.Windows.Forms;

public class LollipopLabel : Label
{
    FontManager font = new FontManager();
    public LollipopLabel()
    {
        this.Font = font.Roboto_Medium10;
        ForeColor = ColorTranslator.FromHtml("#999999");
        BackColor = Color.Transparent;
    }
}
