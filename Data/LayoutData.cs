using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calypso
{
    internal struct LayoutData
    {
        public float MetadataVSplitter_Ratio { get; set; }
        public float LeftPanelHSplitter_Ratio { get; set; }
        public float RightPanelHSplitter_Ratio { get; set; }
        public bool Metadata_IsOpen { get; set; }
        public bool LeftPanel_IsOpen { get; set; }
        public bool RightPanel_IsOpen { get; set; }

        //public LayoutData(
        //int metadataHSplitter_Distance,
        //int leftPanelVSplitter_Distance,
        //int rightPanelVSplitter_Distance,
        //bool metadata_IsOpen,
        //bool leftPanel_IsOpen,
        //bool rightPanel_IsOpen)
        //{
        //    MetadataVSplitter_Distance = metadataHSplitter_Distance;
        //    LeftPanelHSplitter_Distance = leftPanelVSplitter_Distance;
        //    RightPanelHSplitter_Distance = rightPanelVSplitter_Distance;
        //    Metadata_IsOpen = metadata_IsOpen;
        //    LeftPanel_IsOpen = leftPanel_IsOpen;
        //    RightPanel_IsOpen = rightPanel_IsOpen;
        //}
    }
}
