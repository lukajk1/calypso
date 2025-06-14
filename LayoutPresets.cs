using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calypso
{
    internal class LayoutPresets
    {

        public static LayoutData DefaultLayout = new LayoutData()
        {
            MetadataVSplitter_Ratio = 0.67f,
            LeftPanelHSplitter_Ratio = 0.35f,
            RightPanelHSplitter_Ratio = 0.67f,
            Metadata_IsOpen = true,
            LeftPanel_IsOpen = true,
            RightPanel_IsOpen = true
        };

        public static LayoutData LargeWindow =
            new LayoutData()
            {
                MetadataVSplitter_Ratio = 0.74f,
                LeftPanelHSplitter_Ratio = 0.5f,
                RightPanelHSplitter_Ratio = 0.5f,
                Metadata_IsOpen = false,
                LeftPanel_IsOpen = false,
                RightPanel_IsOpen = true
            };
    }
}
