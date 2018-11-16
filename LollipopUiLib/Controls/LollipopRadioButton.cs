using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class LollipopRadioButton : RadioButton
{
    #region Variables

    string HexColor = "#508ef5";

    Color EnabledCheckedColor;
    Color EnabledUnCheckedColor = ColorTranslator.FromHtml("#9c9ea1");

    Color DisabledColor = ColorTranslator.FromHtml("#c4c6ca");
    Color EnabledStringColor = ColorTranslator.FromHtml("#929292");
    Color DisabledStringColor = ColorTranslator.FromHtml("#babbbd");

    Timer AlphaAnimationTimer = new Timer { Interval = 16 };
    Timer SizeAnimationTimer = new Timer { Interval = 35 };

    FontManager font = new FontManager();

    int SizeAnimationNum = 0;
    int PointAnimationNum = 9;
    int Alpha = 0;

    #endregion
    #region  Properties

    [Category("Appearance")]
    public string CheckColor
    {
        get { return HexColor; }
        set
        {
            HexColor = value;
            Invalidate();
        }
    }

    #endregion
    #region Events

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        AlphaAnimationTimer.Start();
        SizeAnimationTimer.Start();
    }
    protected override void OnResize(EventArgs e)
    {
        Height = 19;
        Width = 19 + (int)CreateGraphics().MeasureString(Text, font.Roboto_Medium10).Width;
    }

    #endregion
    public LollipopRadioButton()
    {        
        AlphaAnimationTimer.Tick += new EventHandler(AlphaAnimationTick);
        SizeAnimationTimer.Tick += new EventHandler(SizeAnimationTick);
        
        DoubleBuffered = true;
    }
    protected override void OnPaint(PaintEventArgs pevent)
    {
        var g = pevent.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Parent.BackColor);

        Rectangle BGEllipse = new Rectangle(0, 0, 18, 18);

        EnabledCheckedColor = ColorTranslator.FromHtml(HexColor);
        SolidBrush BG = new SolidBrush(Enabled ? Checked ? EnabledCheckedColor : EnabledUnCheckedColor : DisabledColor);

        //RadioButton BG
        if (Checked)
        {
            g.FillEllipse(new SolidBrush(Color.FromArgb(Alpha, BG.Color)), BGEllipse);
            g.FillEllipse(new SolidBrush(Color.White), new Rectangle(2, 2, 14, 14));
        }
        else
        {
            g.FillEllipse(BG, BGEllipse);
            g.FillEllipse(new SolidBrush(Color.White), new Rectangle(2, 2, 14, 14));
        }       

        g.FillEllipse(BG, new Rectangle(PointAnimationNum, PointAnimationNum, SizeAnimationNum, SizeAnimationNum));       

        //RadioButton Text
        g.DrawString(Text, font.Roboto_Medium10, new SolidBrush(Enabled ? EnabledStringColor : DisabledStringColor), 20, 0);
    }

    private void AlphaAnimationTick(object sender, EventArgs e)
    {
        if (Checked)
        {
            if (Alpha < 250)
            {
                Alpha += 25;
                this.Invalidate();
            }
        }
        else if (Alpha > 0)
        {
            Alpha -= 25;
            this.Invalidate();
        }
    }
    private void SizeAnimationTick(object sender, EventArgs e)
    {
        if (Checked)
        {
            if (SizeAnimationNum < 8)
            {
                SizeAnimationNum += 2;
                this.Invalidate();

                if (PointAnimationNum > 5)
                {
                    PointAnimationNum -= 1;
                    this.Invalidate();
                }
            }
        }
        else if (SizeAnimationNum > 0)
        {
            SizeAnimationNum -= 2;
            this.Invalidate();

            if (PointAnimationNum < 9)
            {
                PointAnimationNum += 1;
                this.Invalidate();
            }
        }
    }
}
