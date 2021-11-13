using System.Windows.Forms;

namespace School.PresentationLayer.Tools
{
    public static class PanelExtensions
    {
        public static void OpenPanel(this Panel panel)
        {
            panel.Dock = DockStyle.Fill;
            panel.Visible = true;
        }

        public static void ClosePanel(this Panel panel)
        {
            panel.Dock = DockStyle.None;
            panel.Visible = false;
        }
    }
}
